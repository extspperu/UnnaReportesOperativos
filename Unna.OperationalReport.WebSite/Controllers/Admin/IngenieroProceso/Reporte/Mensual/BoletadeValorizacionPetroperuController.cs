using ClosedXML.Report;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Drawing;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Data.Reporte.Enums;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Unna.OperationalReport.Service.General.Extensiones;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Mensual
{
    [Route("api/admin/ingenieroProceso/reporte/mensual/[controller]")]
    [ApiController]
    public class BoletadeValorizacionPetroperuController : ControladorBaseWeb
    {

        private readonly IBoletadeValorizacionPetroperuServicio _boletadeValorizacionPetroperuServicio;
        private readonly IBoletadeValorizacionPetroperuLoteIServicio _boletadeValorizacionPetroperuLoteIServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly IImpresionServicio _impresionServicio;

        public BoletadeValorizacionPetroperuController(
            IBoletadeValorizacionPetroperuServicio boletadeValorizacionPetroperuServicio,
            IBoletadeValorizacionPetroperuLoteIServicio boletadeValorizacionPetroperuLoteIServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IImpresionServicio impresionServicio
            )
        {
            _boletadeValorizacionPetroperuServicio = boletadeValorizacionPetroperuServicio;
            _boletadeValorizacionPetroperuLoteIServicio = boletadeValorizacionPetroperuLoteIServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _impresionServicio = impresionServicio;
        }

        [HttpGet("GenerarExcel")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            string? url = await GenerarAsync("Excel");
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);

            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.BoletadeValorizacionPetroperu,
                RutaExcel = url,
            });

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Boleta de Valorizacion Petroperu {nombreArchivo}.xlsx");

        }

        [HttpGet("GenerarPdf")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarPdfAsync()
        {
            string? url = await GenerarAsync("Pdf");
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var tempFilePathPdf = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            string excelFilePath = url;
            string pdfFilePath = tempFilePathPdf;

            using (var excelPackage = new OfficeOpenXml.ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelFile workbook = ExcelFile.Load(excelFilePath);
                workbook.Worksheets[0].PrintOptions.LeftMargin = Length.From(0.002, LengthUnit.Inch);
                workbook.Worksheets[0].PrintOptions.RightMargin = Length.From(0.002, LengthUnit.Inch);
                workbook.Worksheets[0].PrintOptions.TopMargin = Length.From(0.002, LengthUnit.Inch);
                workbook.Worksheets[0].PrintOptions.BottomMargin = Length.From(0.002, LengthUnit.Inch);
                workbook.Worksheets[0].PrintOptions.PaperType = PaperType.A2;
                workbook.Worksheets[0].PrintOptions.Portrait = false;
                workbook.Worksheets[0].PrintOptions.FitWorksheetWidthToPages = 1;
                workbook.Worksheets[0].PrintOptions.FitWorksheetHeightToPages = 1;
                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);

            System.IO.File.Delete(url);
            //System.IO.File.Delete(tempFilePathPdf);

            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.BoletadeValorizacionPetroperu,
                RutaPdf = tempFilePathPdf,
            });

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"Boleta de Valorizacion Petroperu {nombreArchivo}.pdf");
        }

        private async Task<string?> GenerarAsync(string tipo)
        {
            BoletadeValorizacionPetroperuDto? dato = HttpContext.Session.GetObjectFromJson<BoletadeValorizacionPetroperuDto?>("ReporteBoleta");
            if (dato == null)
            {
                var operativo = await _boletadeValorizacionPetroperuServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
                if (!operativo.Completado || operativo.Resultado == null)
                {
                    return null;
                }
                dato = operativo.Resultado;
            }

            var boletadeValorizacionPetroperu = new
            {
                Items = dato.BoletadeValorizacionPetroperu,

            };

            var complexData = new
            {
                Periodo = dato?.Periodo,
                BoletadeValorizacionPetroperu = boletadeValorizacionPetroperu,
                BoletadeValorizacionLoteI = boletadeValorizacionPetroperu,
                BoletadeValorizacionLoteVi = boletadeValorizacionPetroperu,
                BoletadeValorizacionLoteZ69 = boletadeValorizacionPetroperu,
                GnaLoteI = dato?.GnaLoteI,
                EnergiaLoteI = dato?.EnergiaLoteI,
                LgnRecuperadosLoteI = dato?.LgnRecuperadosLoteI,
                GnaLoteVi = dato?.GnaLoteVi,
                EnergiaLoteVi = dato?.EnergiaLoteVi,
                LgnRecuperadosLoteVi = dato?.LgnRecuperadosLoteVi,
                GnaLoteZ69 = dato?.GnaLoteZ69,
                EnergiaLoteZ69 = dato?.EnergiaLoteZ69,
                LgnRecuperadosLoteZ69 = dato?.LgnRecuperadosLoteZ69,
                TotalGna = dato?.TotalGna,
                Eficiencia = dato?.Eficiencia,
                LiquidosRecuperados = dato?.LiquidosRecuperados,
                GnsLoteI = dato?.GnsLoteI,
                GnsLoteVi = dato?.GnsLoteVi,
                GnsLoteZ69 = dato?.GnsLoteZ69,
                GnsTotal = dato?.GnsTotal,
                EnergiaMmbtu = dato?.EnergiaMmbtu,
                ValorLiquidosUs = dato?.ValorLiquidosUs,
                CostoUnitMaquilaUsMmbtu = dato?.CostoUnitMaquilaUsMmbtu,
                CostoMaquilaUs = dato?.CostoMaquilaUs,
                DensidadGlp = dato?.DensidadGlp,
                MontoFacturarUnna = dato?.MontoFacturarUnna,
                MontoFacturarPetroperu = dato?.MontoFacturarPetroperu,
                NombreReporte = dato?.NombreReporte,
                VersionReporte = $"Versión {dato?.VersionReporte}",
                CompaniaReporte = dato?.CompaniaReporte,
                dato.EnergiaMmbtuLoteI,
                dato.EnergiaMmbtuLoteVi,
                dato.EnergiaMmbtuLoteZ69,
                dato.ValorLiquidosLoteI,
                dato.ValorLiquidosLoteVi,
                dato.ValorLiquidosLoteZ69,
                dato.CostoMaquillaLoteI,
                dato.CostoMaquillaLoteVi,
                dato.CostoMaquillaLoteZ69,
                dato.MontoFacturarLoteI,
                dato.MontoFacturarLoteVi,
                dato.MontoFacturarLoteZ69,
                dato?.Observacion,
                dato?.ObservacionLoteI,
                dato?.ObservacionLoteVi,
                dato?.ObservacionLoteZ69,
            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            string rutaPlantilla = $"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\BoletadeValorizacionPetroperu.xlsx";
            if (tipo.Equals("Pdf"))
            {
                rutaPlantilla = $"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\BoletadeValorizacionPetroperuPdf.xlsx";
            }

            using (var template = new XLTemplate(rutaPlantilla))
            {
                if (!string.IsNullOrWhiteSpace(dato?.RutaFirma))
                {
                    using (var stream = new FileStream(dato.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("D40")).WithSize(120, 70);
                        if (tipo.Equals("Excel"))
                        {
                            var worksheetLote1 = template.Workbook.Worksheets.Worksheet(2);
                            worksheetLote1.AddPicture(stream).MoveTo(worksheetLote1.Cell("D40")).WithSize(120, 70);

                            var worksheetLoteVi = template.Workbook.Worksheets.Worksheet(3);
                            worksheetLoteVi.AddPicture(stream).MoveTo(worksheetLoteVi.Cell("D40")).WithSize(120, 70);

                            var worksheetLoteZ69 = template.Workbook.Worksheets.Worksheet(4);
                            worksheetLoteZ69.AddPicture(stream).MoveTo(worksheetLoteZ69.Cell("D40")).WithSize(120, 70);
                        }


                    }
                }
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<BoletadeValorizacionPetroperuDto?> ObtenerAsync()
        {
            var operacion = await _boletadeValorizacionPetroperuServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (operacion.Completado && operacion.Resultado != null)
            {
                HttpContext.Session.SetObjectAsJson("ReporteBoleta", operacion.Resultado);
            }
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(BoletadeValorizacionPetroperuDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            HttpContext.Session.SetObjectAsJson("ReporteBoleta", peticion);
            var operacion = await _boletadeValorizacionPetroperuServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
