using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Servicios.Implementaciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Mensual
{
    [Route("api/admin/ingenieroProceso/reporte/mensual/[controller]")]
    [ApiController]
    public class BoletaVolumenUnnaEnergiaCnpcController : ControladorBaseWeb
    {
        private readonly IBoletaVolumenesUNNAEnergiaCNPCServicio _boletaVolumenesUNNAEnergiaCNPCServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public BoletaVolumenUnnaEnergiaCnpcController(
            IBoletaVolumenesUNNAEnergiaCNPCServicio boletaCnpcServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _boletaVolumenesUNNAEnergiaCNPCServicio = boletaCnpcServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }

        [HttpGet("GenerarExcel")]
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
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BoletaMensualVolumenesUNNAENERGIA_CNPC.xlsx");

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
            return File(bytes, "application/pdf", $"BoletaMensualVolumenesUNNAENERGIA_CNPC.pdf");
        }



        private async Task<string?> GenerarAsync()
        {
            var operativo = await _boletaVolumenesUNNAEnergiaCNPCServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            var volumenGna = new
            {
                Items = dato.VolumenGna
            };

            var complexData = new
            {
                NombreReporte = $"{dato?.General?.NombreReporte}",
                DiaOperativo = dato?.DiaOperativo,
                Anio = dato?.Anio,
                Mes = dato?.Mes,
                TotalGasMpcd = dato?.TotalGasMpcd,
                TotalGlpBls = dato?.TotalGlpBls,
                TotalCgnBls = dato?.TotalCgnBls,
                TotalGnsMpc = dato?.TotalGnsMpc,
                TotalGcMpc = dato?.TotalGcMpc,
                GravedadEspacificoGlp = dato?.GravedadEspacificoGlp,
                Nota = dato?.Nota,
                VolumenGna = volumenGna,

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\BoletaMensualVolumenesUNNAENERGIA_CNPC.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }



        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<BoletaVolumenesUNNAEnergiaCNPCDto?> ObtenerAsync()
        {
            var operacion = await _boletaVolumenesUNNAEnergiaCNPCServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(BoletaVolumenesUNNAEnergiaCNPCDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaVolumenesUNNAEnergiaCNPCServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
