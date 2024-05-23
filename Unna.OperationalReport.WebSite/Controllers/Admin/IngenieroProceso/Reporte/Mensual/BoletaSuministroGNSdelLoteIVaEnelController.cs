using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
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
        public BoletaSuministroGNSdelLoteIVaEnelController(
            IBoletaSuministroGNSdelLoteIVaEnelServicio boletaIBoletaSuministroGNSdelLoteIVaEnelServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _boletaSuministroGNSdelLoteIVaEnelServicio = boletaIBoletaSuministroGNSdelLoteIVaEnelServicio;
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

            DateTime fecha = DateTime.UtcNow.AddDays(-1);
            string? mes = FechasUtilitario.ObtenerNombreMes(fecha);
            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Boleta Mensual de Suministro de GNS del LOTE IV a Enel {nombreArchivo}.xlsx");

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
                workbook.Worksheets[0].PrintOptions.PaperType = PaperType.A2;
                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);

            System.IO.File.Delete(url);
            System.IO.File.Delete(tempFilePathPdf);
            DateTime fecha = DateTime.UtcNow.AddDays(-1);
            string? mes = FechasUtilitario.ObtenerNombreMes(fecha);
            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"Boleta Mensual de Suministro de GNS del LOTE IV a Enel {nombreArchivo}.pdf");
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
                
                //NombreReporte = $"{dato?.NombreReporte}",
                Periodo = dato?.Periodo,
                Composicion = composicion,
                TotalVolumenMPC = dato?.TotalVolumenMPC,
                TotalPCBTUPC = dato?.TotalPCBTUPC,
                TotalEnergiaMMBTU = dato?.TotalEnergiaMMBTU,
                TotalEnergiaVolTransferidoMMBTU = dato?.TotalEnergiaVolTransferidoMMBTU,
                Comentarios = dato?.Comentarios


            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\BoletaMensualdeSuministrodeGNSdelLIVaEnel.xlsx"))
            {
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
            var operacion = await _boletaSuministroGNSdelLoteIVaEnelServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
