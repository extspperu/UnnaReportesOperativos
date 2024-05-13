using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Abstracciones;
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
        public ComposicionGnaLIVController(
        IComposicionGnaLIVServicio composicionGnaLIVServicio,
        IWebHostEnvironment hostingEnvironment,
        GeneralDto general
        )
        {
            _composicionGnaLIVServicio = composicionGnaLIVServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
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
            System.IO.File.Delete(url);

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Composición quincenal GNA Lote IV - {nombreArchivo}.xlsx");

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
                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
            System.IO.File.Delete(url);
            System.IO.File.Delete(tempFilePathPdf);
            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"Composición quincenal GNA Lote IV - {nombreArchivo}.pdf");
        }

        private async Task<string?> GenerarAsync()
        {
            var operativo = await _composicionGnaLIVServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;           

            var componente = new
            {
                Items = dato.Componente
            };

            var composicion = new
            {
                Items = dato.Composicion
            };

            var complexData = new
            {
                //Compania = dato?.General?.Nombre,
                //PreparadoPör = $"{dato?.General?.PreparadoPör}",
                //AprobadoPor = $"{dato?.General?.AprobadoPor}",
                //VersionFecha = $"{dato?.General?.Version} / {dato?.General?.Fecha}",

                Composicion = composicion,
                Componente = componente,

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\quincenal\\ComposicionQuincenalGNALoteIV.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }
    }
}
