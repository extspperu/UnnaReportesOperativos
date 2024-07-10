
//using Aspose.Cells;
using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class FiscalizacionPetroPeruController : ControladorBaseWeb
    {
        private readonly IFiscalizacionPetroPeruServicio _fiscalizacionPetroPeruServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly IImpresionServicio _impresionServicio;
        public FiscalizacionPetroPeruController(
            IFiscalizacionPetroPeruServicio fiscalizacionPetroPeruServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IImpresionServicio impresionServicio
            )
        {
            _fiscalizacionPetroPeruServicio = fiscalizacionPetroPeruServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _impresionServicio = impresionServicio;
        }

        [HttpGet("GenerarExcel")]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            var operativo = await _fiscalizacionPetroPeruServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }
            var dato = operativo.Resultado;
            var factorAsignacionLiquidoGasNatural = new
            {
                Items = dato.FactorAsignacionLiquidoGasNatural
            };
            var distribucionGasNaturalSeco = new
            {
                Items = dato.DistribucionGasNaturalSeco
            };
            var volumenTransferidoRefineriaPorLote = new
            {
                Items = dato.VolumenTransferidoRefineriaPorLote
            };
            var complexData = new
            {
                DiaOperativo = dato.Fecha,
                Compania = dato?.General?.Nombre,
                VersionFecha = $"{dato?.General?.Version} / {dato?.General?.Fecha}",
                PreparadoPor = $"{dato?.General?.PreparadoPor}",
                AprobadoPor = $"{dato?.General?.AprobadoPor}",
                VolumenTotalProduccion = dato?.VolumenTotalProduccion,
                ContenidoLgn = dato?.ContenidoLgn,
                Eficiencia = dato?.Eficiencia,
                FactorAsignacionLiquidoGasNatural = factorAsignacionLiquidoGasNatural,
                FactorConversionZ69 = dato?.FactorConversionZ69,
                FactorConversionVi = dato?.FactorConversionVi,
                FactorConversionI = dato?.FactorConversionI,
                DistribucionGasNaturalSeco = distribucionGasNaturalSeco,
                VolumenTotalGns = dato?.VolumenTotalGns,
                VolumenTransferidoRefineriaPorLote = volumenTransferidoRefineriaPorLote,
                VolumenTotalGnsFlare = dato?.VolumenTotalGnsFlare
            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaDiariaDeFiscalizacionPetroperu.xlsx"))
            {
                if (!string.IsNullOrWhiteSpace(dato?.General?.RutaFirma))
                {
                    using (var stream = new FileStream(dato.General.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("G52")).WithSize(220, 110);
                    }
                }
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            //System.IO.File.Delete(tempFilePath);
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.BoletaDiariaFiscalizacionPetroPeru,
                RutaExcel = tempFilePath,
            });

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BoletaDiariaDeFiscalizacionPetroperu-{dato.Fecha.Replace("/", "-")}.xlsx");
        }

        [HttpGet("GenerarPdf")]
        public async Task<IActionResult> GenerarPdfAsync()
        {
            var operativo = await _fiscalizacionPetroPeruServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }
            var dato = operativo.Resultado;

            if (dato.FactorAsignacionLiquidoGasNatural != null)
            {
                dato.FactorAsignacionLiquidoGasNatural.ForEach(e => e.Factor = (e.Factor / 100));
            }

            var factorAsignacionLiquidoGasNatural = new
            {
                Items = dato.FactorAsignacionLiquidoGasNatural
            };
            var distribucionGasNaturalSeco = new
            {
                Items = dato.DistribucionGasNaturalSeco
            };
            var volumenTransferidoRefineriaPorLote = new
            {
                Items = dato.VolumenTransferidoRefineriaPorLote
            };
            var complexData = new
            {
                DiaOperativo = dato.Fecha,
                Compania = dato?.General?.Nombre,
                VersionFecha = $"{dato?.General?.Version} / {dato?.General?.Fecha}",
                PreparadoPor = $"{dato?.General?.PreparadoPor}",
                AprobadoPor = $"{dato?.General?.AprobadoPor}",
                VolumenTotalProduccion = dato?.VolumenTotalProduccion,
                ContenidoLgn = dato?.ContenidoLgn,
                Eficiencia = dato?.Eficiencia,
                FactorAsignacionLiquidoGasNatural = factorAsignacionLiquidoGasNatural,
                FactorConversionZ69 = dato?.FactorConversionZ69,
                FactorConversionVi = dato?.FactorConversionVi,
                FactorConversionI = dato?.FactorConversionI,
                DistribucionGasNaturalSeco = distribucionGasNaturalSeco,
                VolumenTotalGns = dato?.VolumenTotalGns,
                VolumenTransferidoRefineriaPorLote = volumenTransferidoRefineriaPorLote,
                VolumenTotalGnsFlare = dato?.VolumenTotalGnsFlare
            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaDiariaDeFiscalizacionPetroperu.xlsx"))
            {
                if (!string.IsNullOrWhiteSpace(dato?.General?.RutaFirma))
                {
                    using (var stream = new FileStream(dato.General.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("G52")).WithSize(220, 110);
                    }
                }
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }

            var tempFilePathPdf = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";
            //var workbook = new Workbook(tempFilePath);
            //workbook.Save(tempFilePathPdf);
            //var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);


            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            string excelFilePath = tempFilePath;
            string pdfFilePath = tempFilePathPdf;

            using (var excelPackage = new OfficeOpenXml.ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelFile workbook = ExcelFile.Load(excelFilePath);
                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);

            System.IO.File.Delete(tempFilePath);
            //System.IO.File.Delete(tempFilePathPdf);

            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.BoletaDiariaFiscalizacionPetroPeru,
                RutaPdf = tempFilePathPdf,
            });

            return File(bytes, "application/pdf", $"BoletaDiariaDeFiscalizacionPetroperu-{dato.Fecha.Replace("/", "-")}.pdf");

        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<FiscalizacionPetroPeruDto?> ObtenerAsync()
        {
            var operacion = await _fiscalizacionPetroPeruServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(FiscalizacionPetroPeruDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _fiscalizacionPetroPeruServicio.GuardarAsync(peticion,true);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


    }
}
