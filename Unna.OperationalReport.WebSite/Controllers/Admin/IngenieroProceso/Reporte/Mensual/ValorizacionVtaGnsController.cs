using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Mensual
{
    [Route("api/admin/ingenieroProceso/reporte/mensuales/[controller]")]
    [ApiController]
    public class ValorizacionVtaGnsController : ControladorBaseWeb
    {
        private readonly GeneralDto _general;
        private readonly IValorizacionVtaGnsServicio _valorizacionVtaGnsServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public ValorizacionVtaGnsController(
            IValorizacionVtaGnsServicio valorizacionVtaGnsServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IConfiguration configuration)
        {
            _valorizacionVtaGnsServicio = valorizacionVtaGnsServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _configuration = configuration;
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

            string fechaEmisionArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Valorización quincenal de venta de GNS LOTE IV  - {fechaEmisionArchivo}.xlsx");

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

            var workbook = ExcelFile.Load(excelFilePath);
            foreach (var worksheet in workbook.Worksheets)
            {

                worksheet.PrintOptions.PaperType = PaperType.A4;
                worksheet.PrintOptions.Portrait = true;

                worksheet.PrintOptions.FitWorksheetWidthToPages = 1;
                worksheet.PrintOptions.FitWorksheetHeightToPages = 0;

            }

            var pdfSaveOptions = new PdfSaveOptions()
            {
                SelectionType = SelectionType.EntireFile
            };
            workbook.Save(pdfFilePath, pdfSaveOptions);

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
            System.IO.File.Delete(url);
            System.IO.File.Delete(tempFilePathPdf);
            string fechaEmisionArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"Valorización quincenal de venta de GNS LOTE IV - {fechaEmisionArchivo}.pdf");
        }
        private async Task<string?> GenerarAsync()
        {
            string someSetting = _configuration["general:diaOperativo"];

            var operativo = await _valorizacionVtaGnsServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, someSetting, 2);
            if (operativo.Resultado is null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            var resBalanceEnergLIVDetMedGas = new
            {
                Items = dato.Comentario
            };
            var resBalanceEnergLIVDetGnaFisc = new
            {
                Items = dato.TotalFact
            };

            foreach (var item in operativo.Resultado.ValorizacionVtaGnsDet)
            {
                if (item.Fecha != "Total")
                {
                    DateTime date = DateTime.ParseExact(item.Fecha, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.Fecha = date.ToString("d-MMM-yy", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"));
                }
            }

            var complexData = new
            {
                dataResult = operativo.Resultado.ValorizacionVtaGnsDet,
                ResBalanceEnergLIVDetMedGas = resBalanceEnergLIVDetMedGas,
                ResBalanceEnergLIVDetGnaFisc = resBalanceEnergLIVDetGnaFisc,
                Mes = DateTime.UtcNow.ToString("MMMM", new CultureInfo("es-ES")),
                Anio = DateTime.UtcNow.Year,
                dataResultResumen = operativo.Resultado,
                Total1 = operativo.Resultado.ValorizacionVtaGnsDet?.Sum(item => (decimal?)item.Volumen) ?? 0,
                Total2 = operativo.Resultado.ValorizacionVtaGnsDet?.Average(item => (decimal?)item.PoderCal) ?? 0,
                Total3 = operativo.Resultado.ValorizacionVtaGnsDet?.Sum(item => (decimal?)item.Energia) ?? 0,
                Total4 = operativo.Resultado.ValorizacionVtaGnsDet?.Average(item => (decimal?)item.Precio) ?? 0,
                Total5 = operativo.Resultado.ValorizacionVtaGnsDet?.Sum(item => (decimal?)item.Costo) ?? 0,
            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\quincenal\\ValorizacionVtaGnsLIV.xlsx"))
            {
                //if (!string.IsNullOrWhiteSpace(operativo.Resultado?.RutaFirma))
                //{
                //    using (var stream = new FileStream(operativo.Resultado?.General?.RutaFirma, FileMode.Open))
                //    {
                //        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                //        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("H29")).WithSize(120, 70);
                //    }
                //}
                template.AddVariable(complexData);
                template.Generate();

                string imagePath = $"{_hostingEnvironment.WebRootPath}\\images\\firmas\\FIRMA RSC.jpeg";

                var workbook = template.Workbook;
                var worksheet = workbook.Worksheets.First();

                var imageCell = worksheet.Cell("Firma");

                imageCell.Value = string.Empty;

                var image = worksheet.AddPicture(imagePath)
                                     .MoveTo(imageCell)
                                     .Scale(0.3);

                workbook.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(ValorizacionVtaGnsPost valorizacionVtaGnsPost)
        {
            Console.WriteLine("JSON recibido:");
            Console.WriteLine(valorizacionVtaGnsPost);

            VerificarIfEsBuenJson(valorizacionVtaGnsPost);
            valorizacionVtaGnsPost.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _valorizacionVtaGnsServicio.GuardarAsync(valorizacionVtaGnsPost);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
