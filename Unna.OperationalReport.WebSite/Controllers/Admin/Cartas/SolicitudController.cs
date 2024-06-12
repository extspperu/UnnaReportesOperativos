using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            string? url = await GenerarAsync(operativo.Resultado);
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
            return File(bytes, "application/pdf", $"{operativo.Resultado?.Solicitud?.NombreArchivo}.pdf");
        }

        private async Task<string?> GenerarAsync(CartaDto entidad)
        {
            await Task.Delay(0);
                 
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\cartas\\solicitud.xlsx"))
            {
                template.AddVariable(entidad.Solicitud);
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


        //[HttpPost("Guardar")]
        //[RequiereAcceso()]
        //public async Task<RespuestaSimpleDto<string>?> GuardarAsync(FacturacionGnsLIVDto peticion)
        //{
        //    VerificarIfEsBuenJson(peticion);
        //    peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
        //    var operacion = await _cartaDghServicio.GuardarAsync(peticion);
        //    return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        //}
    }
}
