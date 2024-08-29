using ClosedXML.Report;
using DocumentFormat.OpenXml.Wordprocessing;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Quincenal
{
    [Route("api/admin/ingenieroProceso/reporte/quincenal/[controller]")]
    [ApiController]
    public class ComposicionGnaLIVController : ControladorBaseWeb
    {
        private readonly IComposicionGnaLIVServicio _composicionGnaLIVServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly IImpresionServicio _impresionServicio;
        public ComposicionGnaLIVController(
        IComposicionGnaLIVServicio composicionGnaLIVServicio,
        IWebHostEnvironment hostingEnvironment,
        GeneralDto general,
        IImpresionServicio impresionServicio
        )
        {
            _composicionGnaLIVServicio = composicionGnaLIVServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _impresionServicio = impresionServicio;
        }

        [HttpGet("GenerarExcel/{grupo}")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarExcelAsync(string? grupo)
        {
            string? url = await GenerarAsync(grupo);
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);

            int? idReporte = default(int?);
            switch (grupo)
            {
                case GruposReportes.Quincenal:
                    idReporte = (int)TiposReportes.ComposicionQuincenalGNALoteIV;
                    break;
                case GruposReportes.Mensual:
                    idReporte = (int)TiposReportes.ComposicionMensualGNALoteIV;
                    break;
                default:
                    return File(new byte[0], "application/octet-stream");
            }

            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = idReporte ?? 0,
                RutaExcel = url,
            });
            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Composición quincenal GNA Lote IV - {nombreArchivo}.xlsx");

        }

        [HttpGet("GenerarPdf/{grupo}")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarPdfAsync(string? grupo)
        {
            string? url = await GenerarAsync(grupo);
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
                workbook.Worksheets[0].PrintOptions.PaperType = PaperType.A3;
                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
            System.IO.File.Delete(url);

            int? idReporte = default(int?);
            switch (grupo)
            {
                case GruposReportes.Quincenal:
                    idReporte = (int)TiposReportes.ComposicionQuincenalGNALoteIV;
                    break;
                case GruposReportes.Mensual:
                    idReporte = (int)TiposReportes.ComposicionMensualGNALoteIV;
                    break;
                default:
                    return File(new byte[0], "application/octet-stream");
            }

            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = idReporte ?? 0,
                RutaPdf = tempFilePathPdf,
            });
            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"Composición quincenal GNA Lote IV - {nombreArchivo}.pdf");
        }

        private async Task<string?> GenerarAsync(string? grupo)
        {
            var operativo = await _composicionGnaLIVServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, grupo);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            var componente = new
            {
                Items = dato.ComposicionGnaLIVDetComponente
            };

            var composicion = new
            {
                Items = dato.ComposicionGnaLIVDetComposicion
            };

            var complexData = new
            {
                NombreReporte = dato.NombreReporte,
                Composicion = composicion,
                Componente = componente,


            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\quincenal\\ComposicionQuincenalGNALoteIV.xlsx"))
            {
                if (!string.IsNullOrWhiteSpace(operativo.Resultado?.RutaFirma))
                {
                    using (var stream = new FileStream(operativo.Resultado?.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("H29")).WithSize(120, 70);
                    }
                }
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }

        [HttpGet("Obtener/{grupo}")]
        [RequiereAcceso()]
        public async Task<ComposicionGnaLIVDto?> ObtenerAsync(string? grupo)
        {
            var operacion = await _composicionGnaLIVServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, grupo);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(ComposicionGnaLIVDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _composicionGnaLIVServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}