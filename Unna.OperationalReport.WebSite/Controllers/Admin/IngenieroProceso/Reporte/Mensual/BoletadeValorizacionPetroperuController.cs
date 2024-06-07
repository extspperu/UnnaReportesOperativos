using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Registros.Datos.Servicios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Mensual
{
    [Route("api/admin/ingenieroProceso/reporte/mensual/[controller]")]
    [ApiController]
    public class BoletadeValorizacionPetroperuController : ControladorBaseWeb
    {

        private readonly IBoletadeValorizacionPetroperuServicio _boletadeValorizacionPetroperuServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;

        public BoletadeValorizacionPetroperuController(
            IBoletadeValorizacionPetroperuServicio boletadeValorizacionPetroperuServicio, 
            IWebHostEnvironment hostingEnvironment, 
            GeneralDto general
            )
        {
            _boletadeValorizacionPetroperuServicio = boletadeValorizacionPetroperuServicio;
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
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Boleta de Valorizacion Petroperu {nombreArchivo}.xlsx");

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
            return File(bytes, "application/pdf", $"Boleta de Valorizacion Petroperu {nombreArchivo}.pdf");
        }

        private async Task<string?> GenerarAsync()
        {
            var operativo = await _boletadeValorizacionPetroperuServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            var composicion = new
            {
                Items = dato.BoletadeValorizacionPetroperuDet
            };

            var complexData = new
            {

                //NombreReporte = $"{dato?.NombreReporte}",
                Fecha = dato?.Fecha,
                Composicion = composicion,
                TotalGasNaturalLoteIGNAMPCSD = dato?.TotalGasNaturalLoteIGNAMPCSD,
                TotalGasNaturalLoteIEnergiaMMBTU = dato?.TotalGasNaturalLoteIEnergiaMMBTU,
                TotalGasNaturalLoteILGNRecupBBL = dato?.TotalGasNaturalLoteILGNRecupBBL,
                TotalGasNaturalLoteVIGNAMPCSD = dato?.TotalGasNaturalLoteVIGNAMPCSD,
                TotalGasNaturalLoteVIEnergiaMMBTU = dato?.TotalGasNaturalLoteVIEnergiaMMBTU,
                TotalGasNaturalLoteVILGNRecupBBL = dato?.TotalGasNaturalLoteVILGNRecupBBL,
                TotalGasNaturalLoteZ69GNAMPCSD = dato?.TotalGasNaturalLoteZ69GNAMPCSD,
                TotalGasNaturalLoteZ69EnergiaMMBTU = dato?.TotalGasNaturalLoteZ69EnergiaMMBTU,
                TotalGasNaturalLoteZ69LGNRecupBBL = dato?.TotalGasNaturalLoteZ69LGNRecupBBL,
                TotalGasNaturalTotalGNA= dato?.TotalGasNaturalTotalGNA,
                TotalGasNaturalEficienciaPGT = dato?.TotalGasNaturalEficienciaPGT,
                TotalGasNaturalLiquidosRecupTotales = dato?.TotalGasNaturalLiquidosRecupTotales,
                TotalGasSecoMS9215GNSLoteIMCSD = dato?.TotalGasSecoMS9215GNSLoteIMCSD,
                TotalGasSecoMS9215GNSLoteVIMCSD = dato?.TotalGasSecoMS9215GNSLoteVIMCSD,
                TotalGasSecoMS9215GNSLoteZ69MCSD = dato?.TotalGasSecoMS9215GNSLoteZ69MCSD,
                TotalGasSecoMS9215GNSTotalMCSD = dato?.TotalGasSecoMS9215GNSTotalMCSD,
                TotalGasSecoMS9215EnergiaMMBTU = dato?.TotalGasSecoMS9215EnergiaMMBTU,
                TotalValorLiquidosUS = dato?.TotalValorLiquidosUS,
                TotalCostoUnitMaquilaUSMMBTU = dato?.TotalCostoUnitMaquilaUSMMBTU,
                TotalCostoMaquilaUS = dato?.TotalCostoMaquilaUS,
                TotalDensidadGLPPromMesAnt = dato?.TotalDensidadGLPPromMesAnt,
                TotalMontoFacturarporUnnaE = dato?.TotalMontoFacturarporUnnaE,
                TotalMontoFacturarporPetroperu = dato?.TotalMontoFacturarporPetroperu,
                Observacion1 = dato?.Observacion1,
                Observacion2 = dato?.Observacion2,
                Observacion3 = dato?.Observacion3

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\BoletadeValorizacionPetroperu.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<BoletadeValorizacionPetroperuDto?> ObtenerAsync()
        {
            var operacion = await _boletadeValorizacionPetroperuServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(BoletadeValorizacionPetroperuDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletadeValorizacionPetroperuServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
