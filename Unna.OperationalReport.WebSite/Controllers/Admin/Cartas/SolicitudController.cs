using ClosedXML.Report;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Unna.OperationalReport.Service.Cartas.Dgh.Dtos;
using Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Cartas
{
    [Route("api/admin/cartas/[controller]")]
    [ApiController]
    public class SolicitudController : ControladorBaseWeb
    {

        private readonly ICartaDghServicio _cartaDghServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public SolicitudController(
            ICartaDghServicio cartaDghServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _cartaDghServicio = cartaDghServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }


        [HttpGet("GenerarPdf/{idCarta}")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarPdfAsync(string idCarta)
        {
            var operativo = await _cartaDghServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, FechasUtilitario.ObtenerDiaOperativo(), idCarta);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }

            // Generar el primer archivo Excel y convertirlo a PDF
            string? url1 = await GenerarAsync(operativo.Resultado);
            if (string.IsNullOrWhiteSpace(url1))
            {
                return File(new byte[0], "application/octet-stream");
            }

            var tempFilePathPdf1 = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";
            ConvertExcelToPdf(url1, tempFilePathPdf1);

            // Generar el segundo archivo Excel y convertirlo a PDF
            string? url2 = await GenerarAsyncSegundo(operativo.Resultado); // Método para generar el segundo Excel
            if (string.IsNullOrWhiteSpace(url2))
            {
                return File(new byte[0], "application/octet-stream");
            }

            var tempFilePathPdf2 = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";
            ConvertExcelToPdf(url2, tempFilePathPdf2);

            // Leer ambos archivos PDF y combinarlos
            List<PdfDocument> list = new List<PdfDocument>
    {
        PdfReader.Open(tempFilePathPdf1, PdfDocumentOpenMode.Import),
        PdfReader.Open(tempFilePathPdf2, PdfDocumentOpenMode.Import)
    };

            using (PdfDocument outPdf = new PdfDocument())
            {
                foreach (PdfDocument pdf in list)
                {
                    foreach (PdfPage page in pdf.Pages)
                    {
                        outPdf.AddPage(page);
                    }
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    outPdf.Save(stream, false);
                    byte[] bytes = stream.ToArray();

                    System.IO.File.Delete(tempFilePathPdf1);
                    System.IO.File.Delete(tempFilePathPdf2);
                    System.IO.File.Delete(url1);
                    System.IO.File.Delete(url2);
                    string fechaEmisionArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
                    return File(bytes, "application/pdf", $"Resumen Balance Energético UNNA Lote IV - {fechaEmisionArchivo}.pdf");
                }
            }
        }

        private void ConvertExcelToPdf(string excelFilePath, string pdfFilePath)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = ExcelFile.Load(excelFilePath);

            foreach (var worksheet in workbook.Worksheets)
            {
                worksheet.PrintOptions.PaperType = PaperType.A4;
                worksheet.PrintOptions.Portrait = true;
                worksheet.PrintOptions.FitWorksheetWidthToPages = 1;
                worksheet.PrintOptions.FitWorksheetHeightToPages = 1;
            }

            var pdfSaveOptions = new PdfSaveOptions()
            {
                SelectionType = SelectionType.EntireFile
            };
            workbook.Save(pdfFilePath, pdfSaveOptions);
        }

        private async Task<string?> GenerarAsync(CartaDto entidad)
        {
            await Task.Delay(0);

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\cartas\\solicitud.xlsx"))
            {
                template.AddVariable(entidad);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }
        private async Task<string?> GenerarAsyncSegundo(CartaDto entidad)
        {
            await Task.Delay(0);

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\cartas\\solicitud_segundo.xlsx"))
            {
                template.AddVariable(entidad);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }


        [HttpGet("Obtener/{idCarta}")]
        [RequiereAcceso()]
        public async Task<CartaDto?> ObtenerAsync(string idCarta)
        {
            var operacion = await _cartaDghServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, FechasUtilitario.ObtenerDiaOperativo(), idCarta);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(CartaDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _cartaDghServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
