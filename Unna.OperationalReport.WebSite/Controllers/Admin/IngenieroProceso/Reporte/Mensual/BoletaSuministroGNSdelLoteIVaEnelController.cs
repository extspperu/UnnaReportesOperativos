using ClosedXML.Report;
using DocumentFormat.OpenXml.Spreadsheet;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Mensual
{
    [Route("api/admin/ingenieroProceso/reporte/mensual/[controller]")]
    [ApiController]
    public class BoletaSuministroGNSdelLoteIVaEnelController : ControladorBaseWeb
    {


        private readonly IBoletaSuministroGNSdelLoteIVaEnelServicio _boletaSuministroGNSdelLoteIVaEnelServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly IImpresionServicio _impresionServicio;
        public BoletaSuministroGNSdelLoteIVaEnelController(
            IBoletaSuministroGNSdelLoteIVaEnelServicio boletaIBoletaSuministroGNSdelLoteIVaEnelServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IImpresionServicio impresionServicio
            )
        {
            _boletaSuministroGNSdelLoteIVaEnelServicio = boletaIBoletaSuministroGNSdelLoteIVaEnelServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _impresionServicio = impresionServicio;
        }

        [HttpGet("GenerarExcel")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            string? url = await GenerarAsync();
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);

            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.BoletaSuministroGNSdelLoteIVaEnel,
                RutaExcel = url,
            });

            var fecha = FechasUtilitario.ObtenerDiaOperativo().AddDays(1).AddMonths(1);
            string? mes = FechasUtilitario.ObtenerNombreMes(fecha);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fecha.Month}. Boleta Mensual de Suministro de GNS del LOTE IV a Enel {mes} {fecha.Year}.xlsx");

        }

        [HttpGet("GenerarPdf")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarPdfAsync()
        {
            string? url = await GenerarAsync();
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
                workbook.Worksheets[0].PrintOptions.PaperType = PaperType.A4;
                workbook.Worksheets[0].PrintOptions.Portrait = true;
                workbook.Worksheets[0].PrintOptions.LeftMargin = 2;
                workbook.Worksheets[0].PrintOptions.RightMargin = 1;
                workbook.Worksheets[0].PrintOptions.TopMargin = 0.80;
                workbook.Worksheets[0].PrintOptions.BottomMargin = 1;
                workbook.Worksheets[0].PrintOptions.FitWorksheetWidthToPages = 1;
                workbook.Worksheets[0].PrintOptions.FitWorksheetHeightToPages = 1;

                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
            System.IO.File.Delete(url);
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.BoletaSuministroGNSdelLoteIVaEnel,
                RutaPdf = tempFilePathPdf,
            });
            var fecha = FechasUtilitario.ObtenerDiaOperativo().AddDays(1).AddMonths(1);
            string? mes = FechasUtilitario.ObtenerNombreMes(fecha);
            return File(bytes, "application/pdf", $"{fecha.Month}. Boleta Mensual de Suministro de GNS del LOTE IV a Enel {mes} {fecha.Year}.pdf");
        }



        private async Task<string?> GenerarAsync()
        {
            var operativo = await _boletaSuministroGNSdelLoteIVaEnelServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            var composicion = new
            {
                Items = dato.BoletaSuministroGNSdelLoteIVaEnelDet
            };

            var complexData = new
            {

                NombreReporte = $"{dato?.NombreReporte}",
                Periodo = dato?.Periodo,
                Composicion = composicion,
                TotalVolumenMPC = dato?.TotalVolumen,
                TotalPCBTUPC = dato?.TotalPoderCalorifico,
                TotalEnergiaMMBTU = dato?.TotalEnergia,
                TotalEnergiaVolTransferidoMMBTU = dato?.TotalEnergiaTransferido,
                Comentarios = dato?.Comentarios
            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\BoletaMensualdeSuministrodeGNSdelLIVaEnel.xlsx"))
            {
                if (!string.IsNullOrWhiteSpace(dato?.RutaFirma))
                {
                    using (var stream = new FileStream(dato.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("C30")).WithSize(120, 70);
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
        public async Task<BoletaSuministroGNSdelLoteIVaEnelDto?> ObtenerAsync()
        {
            var operacion = await _boletaSuministroGNSdelLoteIVaEnelServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(BoletaSuministroGNSdelLoteIVaEnelDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaSuministroGNSdelLoteIVaEnelServicio.GuardarAsync(peticion, true);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
