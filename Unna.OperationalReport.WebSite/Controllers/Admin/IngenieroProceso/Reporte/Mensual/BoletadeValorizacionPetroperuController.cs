using ClosedXML.Report;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Drawing;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Registros.Datos.Servicios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Servicios.Abstracciones;
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
        private readonly IBoletadeValorizacionPetroperuLoteIServicio _boletadeValorizacionPetroperuLoteIServicio;
        private readonly IBoletadeValorizacionPetroperuLoteVIServicio _boletadeValorizacionPetroperuLoteVIServicio;
        private readonly IBoletadeValorizacionPetroperuLoteZ69Servicio _boletadeValorizacionPetroperuLoteZ69Servicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;

        public BoletadeValorizacionPetroperuController(
            IBoletadeValorizacionPetroperuServicio boletadeValorizacionPetroperuServicio,
            IBoletadeValorizacionPetroperuLoteIServicio boletadeValorizacionPetroperuLoteIServicio,
            IBoletadeValorizacionPetroperuLoteVIServicio boletadeValorizacionPetroperuLoteVIServicio,
            IBoletadeValorizacionPetroperuLoteZ69Servicio boletadeValorizacionPetroperuLoteZ69Servicio,
            IWebHostEnvironment hostingEnvironment, 
            GeneralDto general
            )
        {
            _boletadeValorizacionPetroperuServicio = boletadeValorizacionPetroperuServicio;
            _boletadeValorizacionPetroperuLoteIServicio = boletadeValorizacionPetroperuLoteIServicio;
            _boletadeValorizacionPetroperuLoteVIServicio = boletadeValorizacionPetroperuLoteVIServicio;
            _boletadeValorizacionPetroperuLoteZ69Servicio = boletadeValorizacionPetroperuLoteZ69Servicio;
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
                workbook.Worksheets[0].PrintOptions.LeftMargin = Length.From(0.002, LengthUnit.Inch);
                workbook.Worksheets[0].PrintOptions.RightMargin = Length.From(0.002, LengthUnit.Inch);
                workbook.Worksheets[0].PrintOptions.TopMargin = Length.From(0.002, LengthUnit.Inch);
                workbook.Worksheets[0].PrintOptions.BottomMargin = Length.From(0.002, LengthUnit.Inch);
                workbook.Worksheets[0].PrintOptions.PaperType = PaperType.A2;
                workbook.Worksheets[0].PrintOptions.Portrait = false;
                workbook.Worksheets[0].PrintOptions.FitWorksheetWidthToPages = 1;
                workbook.Worksheets[0].PrintOptions.FitWorksheetHeightToPages = 1;
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
            var operativoI = await _boletadeValorizacionPetroperuLoteIServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            var operativoVI = await _boletadeValorizacionPetroperuLoteVIServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            var operativoZ69 = await _boletadeValorizacionPetroperuLoteZ69Servicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            if (!operativoI.Completado || operativoI.Resultado == null)
            {
                return null;
            }
            if (!operativoVI.Completado || operativoVI.Resultado == null)
            {
                return null;
            }
            if (!operativoZ69.Completado || operativoZ69.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;
            var datoI = operativoI.Resultado;
            var datoVI = operativoVI.Resultado;
            var datoZ69 = operativoZ69.Resultado;

            var composicion = new
            {
                Items = dato.BoletadeValorizacionPetroperuDet,
                
            };
            var composicionLI = new
            {
                Items = datoI.BoletadeValorizacionPetroperuLoteIDet,
                
            };
            var composicionLVI = new
            {
                Items = datoVI.BoletadeValorizacionPetroperuLoteVIDet,
                
            };
            var composicionLZ69 = new
            {
                Items = datoZ69.BoletadeValorizacionPetroperuLoteZ69Det,
                
            };

            var complexData = new
            {

                
                Fecha = dato?.Fecha,
                Composicion = composicion,
                ComposicionLI = composicionLI,
                ComposicionLVI = composicionLVI,
                ComposicionLZ69 = composicionLZ69,
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
                Observacion3 = dato?.Observacion3,
                Observacion4 = dato?.Observacion4,

                TotalGasSecoMS9215EnergiaMMBTU_LI = datoI?.TotalGasSecoMS9215EnergiaMMBTU,
                TotalGasSecoMS9215EnergiaMMBTU_LVI = datoVI?.TotalGasSecoMS9215EnergiaMMBTU,
                TotalGasSecoMS9215EnergiaMMBTU_LZ69 = datoZ69?.TotalGasSecoMS9215EnergiaMMBTU,
                TotalValorLiquidosUS_LI = datoI?.TotalValorLiquidosUS,
                TotalValorLiquidosUS_LVI = datoVI?.TotalValorLiquidosUS,
                TotalValorLiquidosUS_LZ69 = datoZ69?.TotalValorLiquidosUS,
                TotalCostoMaquilaUS_LI = datoI?.TotalCostoMaquilaUS,
                TotalCostoMaquilaUS_LVI = datoVI?.TotalCostoMaquilaUS,
                TotalCostoMaquilaUS_LZ69 = datoZ69?.TotalCostoMaquilaUS,
                TotalMontoFacturarporUnnaE_LI = datoI?.TotalMontoFacturarporUnnaE,
                TotalMontoFacturarporPetroperu_LI = datoI?.TotalMontoFacturarporPetroperu,
                TotalMontoFacturarporUnnaE_LVI = datoVI?.TotalMontoFacturarporUnnaE,
                TotalMontoFacturarporPetroperu_LVI = datoVI?.TotalMontoFacturarporPetroperu,
                TotalMontoFacturarporUnnaE_LZ69 = datoZ69?.TotalMontoFacturarporUnnaE,
                TotalMontoFacturarporPetroperu_LZ69 = datoZ69?.TotalMontoFacturarporPetroperu,

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
