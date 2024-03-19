//using Aspose.Cells;
using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class BoletaVentaGnsController : ControladorBaseWeb
    {

        private readonly IBoletaVentaGnsServicio _boletaVentaGnsServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;

        public BoletaVentaGnsController(
            IBoletaVentaGnsServicio boletaVentaGnsServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _boletaVentaGnsServicio = boletaVentaGnsServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<BoletaVentaGnsDto?> ObtenerAsync()
        {
            var operacion = await _boletaVentaGnsServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(BoletaVentaGnsDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaVentaGnsServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
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

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy HH:mm:ss tt");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BOLETA DE VENTA DEL GAS NATURAL SECO DE UNNA LOTE IV A ENEL - {nombreArchivo}.xlsx");
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




            
            //var workbook = new Workbook(url);
            //workbook.Save(tempFilePathPdf);
            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);

            System.IO.File.Delete(url);
            System.IO.File.Delete(tempFilePathPdf);
            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy HH:mm:ss tt");

            return File(bytes, "application/pdf", $"BOLETA DE VENTA DEL GAS NATURAL SECO DE UNNA LOTE IV A ENEL - {nombreArchivo}.pdf");
        }
        
        
        
        //[HttpGet("GenerarPdf")]
        //[RequiereAcceso()]
        //public async Task<IActionResult> GenerarPdfAsync()
        //{
        //    string? url = await GenerarAsync();
        //    if (string.IsNullOrWhiteSpace(url))
        //    {
        //        return File(new byte[0], "application/octet-stream");
        //    }
        //    var tempFilePathPdf = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";
        //    var workbook = new Workbook(url);
        //    workbook.Save(tempFilePathPdf);
        //    var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
        //    System.IO.File.Delete(url);
        //    System.IO.File.Delete(tempFilePathPdf);
        //    string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy HH:mm:ss tt");

        //    return File(bytes, "application/pdf", $"BOLETA DE VENTA DEL GAS NATURAL SECO DE UNNA LOTE IV A ENEL - {nombreArchivo}.pdf");
        //}


        private async Task<string?> GenerarAsync()
        {
            var operativo = await _boletaVentaGnsServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null || operativo.Resultado.General == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            object cuerpo = new
            {
                NombreReporte = dato.General.NombreReporte,
                Compania = dato.General.Nombre,
                Version = $"Versión {dato.General.Version}",
                Fecha = dato.Fecha,
                Mpcs = dato.Mpcs,
                BtuPcs = dato.BtuPcs,
                Mmbtu = dato.Mmbtu,
                RazonSocial = dato.Empresa,
                Abreviatura = dato.Abreviatura,
            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaDeVentaGns.xlsx"))
            {
                template.AddVariable(cuerpo);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }


        [HttpGet("Pdf")]
        public async Task<IActionResult> Pdf()
        {
            var dto = new BoletaVentaGnsDto();
            var operacion = await _boletaVentaGnsServicio.ObtenerAsync(ObtenerIdUsuarioActual()??0);
            if (operacion.Completado)
            {
                dto = operacion.Resultado;
            }
            return new ViewAsPdf("/Pages/Admin/IngenieroProceso/Reporte/Diario/ReporteExistencias/Pdf.cshtml")
            {
                FileName = "BoletaVentaDeGasNatural.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                Model = dto
            };
        }

    }
}
