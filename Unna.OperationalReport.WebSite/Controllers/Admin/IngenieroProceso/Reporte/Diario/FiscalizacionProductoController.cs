using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class FiscalizacionProductoController : ControladorBaseWeb
    {
        private readonly IFiscalizacionProductosServicio _fiscalizacionProductosServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public FiscalizacionProductoController(
            IFiscalizacionProductosServicio fiscalizacionProductosServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _fiscalizacionProductosServicio = fiscalizacionProductosServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }

        [HttpGet("GenerarPdf")]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            var operativo = await _fiscalizacionProductosServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }
            var dato = operativo.Resultado;
            var productoParaReproceso = new
            {
                Items = dato.ProductoParaReproceso
            };
            var productoGlp = new
            {
                Items = dato.ProductoGlp
            };
            var productoCgn = new
            {
                Items = dato.ProductoCgn
            };
            var productoGlpCgn = new
            {
                Items = dato.ProductoGlpCgn
            };
            var complexData = new
            {
                DiaOperativo = dato.Fecha,
                Compania = dato?.General?.Nombre,
                VersionFecha = $"{dato?.General?.Version} / {dato?.General?.Fecha}",
                PreparadoPor = $"{dato?.General?.PreparadoPör}",
                AprobadoPor = $"{dato?.General?.AprobadoPor}",
                ProductoParaReproceso = productoParaReproceso,
                ProductoGlp = productoGlp,
                productoCgn = productoCgn,
                ProductoGlpCgn = productoGlpCgn,
                Observacion = dato.Observacion
            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\FisProductos.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }

            var tempFilePathPdf = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";
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
            System.IO.File.Delete(tempFilePathPdf);

            return File(bytes, "application/pdf", $"{dato.General.NombreReporte}-{dato.Fecha.Replace("/", "-")}.pdf");
        }


        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<FiscalizacionProductosDto?> ObtenerAsync()
        {
            var operacion = await _fiscalizacionProductosServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(FiscalizacionProductosDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _fiscalizacionProductosServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
