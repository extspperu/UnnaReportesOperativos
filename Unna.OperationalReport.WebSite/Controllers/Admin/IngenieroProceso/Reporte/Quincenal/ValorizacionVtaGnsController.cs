using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;



using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.AspNetCore.Http;

using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Abstracciones;

using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Implementaciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using System.Globalization;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Quincenal
{
    [Route("api/admin/ingenieroProceso/reporte/quincenal/[controller]")]
    [ApiController]
    public class ValorizacionVtaGnsController : ControladorBaseWeb
    {
        private readonly GeneralDto _general;
        private readonly IValorizacionVtaGnsServicio _valorizacionVtaGnsServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ValorizacionVtaGnsController(
            IValorizacionVtaGnsServicio valorizacionVtaGnsServicio, 
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general)
        {
            _valorizacionVtaGnsServicio = valorizacionVtaGnsServicio;
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
            using (var excelPackage = new OfficeOpenXml.ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelFile workbook = ExcelFile.Load(excelFilePath);
                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
            System.IO.File.Delete(url);
            System.IO.File.Delete(tempFilePathPdf);
            string fechaEmisionArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"Valorización quincenal de venta de GNS LOTE IV - {fechaEmisionArchivo}.pdf");
        }
        private async Task<string?> GenerarAsync()
        {
            var operativo = await _valorizacionVtaGnsServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
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
                if (item.Fecha !="Total")
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
                dataResultResumen = operativo.Resultado
            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\quincenal\\ValorizacionVtaGnsLIV.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
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
