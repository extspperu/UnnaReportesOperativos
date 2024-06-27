using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Report;
using GemBox.Spreadsheet;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Calculos.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class BoletaDeterminacionVolumenController : ControladorBaseWeb
    {
        private readonly IBoletaDeterminacionVolumenGnaServicio _boletaDeterminacionVolumenGnaServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly ICalculoServicio _calculoServicio;
        public BoletaDeterminacionVolumenController(
        IBoletaDeterminacionVolumenGnaServicio boletaDeterminacionVolumenGnaServicio,
        IWebHostEnvironment hostingEnvironment,
        GeneralDto general,
        ICalculoServicio calculoServicio
        )
        {
            _boletaDeterminacionVolumenGnaServicio = boletaDeterminacionVolumenGnaServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _calculoServicio = calculoServicio;
        }
        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<BoletaDeterminacionVolumenGnaDto?> ObtenerAsync()
        {
            var operacion = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(BoletaDeterminacionVolumenGnaDto boletaDeterminacionVolumenGna)
        {
            VerificarIfEsBuenJson(boletaDeterminacionVolumenGna);
            boletaDeterminacionVolumenGna.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaDeterminacionVolumenGnaServicio.GuardarAsync(boletaDeterminacionVolumenGna);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("GenerarExcel")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            string? url = await GenerarAsync(1);
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);
            System.IO.File.Delete(url);

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BOLETA DE DETERMINACION DE VOLUMEN DE GNA FISCALIZADO - {nombreArchivo}.xlsx");
        }

        [HttpGet("GenerarPdf")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarPdfAsync()
        {
            string? url = await GenerarAsync(2);
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
            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"BOLETA DE DETERMINACION DE VOLUMEN DE GNA FISCALIZADO - {nombreArchivo}.pdf");
        }

        private async Task<string?> GenerarAsync(int tipoReporte)
        {
            var operativo = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            var propiedades = await _calculoServicio.ObtenerPropiedadesFisicasAsync(DateTime.UtcNow);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;
            if (dato.FactoresAsignacionGasCombustible != null)
            {
                dato.FactoresAsignacionGasCombustible.ForEach(e => e.FactorAsignacion = (e.FactorAsignacion / 100));
            }
            if (dato.FactorAsignacionGns != null)
            {
                dato.FactorAsignacionGns.ForEach(e => e.FactorAsignacion = (e.FactorAsignacion / 100));
            }

            if (dato.FactorAsignacionLiquidosGasNatural != null)
            {
                dato.FactorAsignacionLiquidosGasNatural.ForEach(e => e.FactorAsignacion = (e.FactorAsignacion / 100));
            }


            var factoresAsignacionGasCombustible = new
            {
                Items = dato.FactoresAsignacionGasCombustible
            };

            var factorAsignacionGns = new
            {
                Items = dato.FactorAsignacionGns
            };

            var factorAsignacionLiquidosGasNatural = new
            {
                Items = dato.FactorAsignacionLiquidosGasNatural
            };

            var complexData = new
            {
                Compania = dato?.General?.Nombre,
                PreparadoPör = $"{dato?.General?.PreparadoPör}",
                AprobadoPor = $"{dato?.General?.AprobadoPor}",
                VersionFecha = $"{dato?.General?.Version} / {dato?.General?.Fecha}",

                DiaOperativo = dato?.Fecha,
                VolumenTotalGasCombustible = dato?.VolumenTotalGasCombustible,
                VolumenTotalGns = dato?.VolumenTotalGns,

                FactoresAsignacionGasCombustible = factoresAsignacionGasCombustible,
                FactorAsignacionGns = factorAsignacionGns,
                VolumenProduccionTotalGlp = dato?.VolumenProduccionTotalGlp,
                VolumenProduccionTotalCgn = dato?.VolumenProduccionTotalCgn,
                VolumenProduccionTotalLgn = dato?.VolumenProduccionTotalLgn,
                FactorAsignacionLiquidosGasNatural = factorAsignacionLiquidosGasNatural,
                DistribucionGasNaturalAsociado = dato?.DistribucionGasNaturalAsociado,
                VolumenProduccionTotalGlpLoteIv = dato?.VolumenProduccionTotalGlpLoteIv,
                VolumenProduccionTotalCgnLoteIv = dato?.VolumenProduccionTotalCgnLoteIv,
                FactorCoversion = dato?.FactorCoversion,

                VolumenGnsVentaVgnsvTotal = dato?.VolumenGnsVentaVgnsvTotal ?? 0,
                VolumenGnsVentaVgnsvEnel = dato?.VolumenGnsVentaVgnsvEnel ?? 0,
                VolumenGnsVentaVgnsvGasnorp = dato?.VolumenGnsVentaVgnsvGasnorp ?? 0,
                VolumenGnsVentaVgnsvLimagas = dato?.VolumenGnsVentaVgnsvLimagas ?? 0,
                VolumenGnsFlareVgnsrf = dato?.VolumenGnsFlareVgnsrf ?? 0,
                SumaVolumenGasCombustibleVolumen = double.IsNaN(dato?.SumaVolumenGasCombustibleVolumen ?? 0) ? 0 : dato.SumaVolumenGasCombustibleVolumen,
                VolumenGnaFiscalizado = dato?.VolumenGnaFiscalizado ?? 0,

                PropiedadesGpa = propiedades.Resultado.PropiedadesGpa,
                PropiedadesGpsa = propiedades.Resultado.PropiedadesGpsa,
                IngresarComposicion = propiedades.Resultado.ComponsicionGnaEntrada,
                CantidadCalidad = propiedades.Resultado.CantidadCalidad,
                DeterminacionFactorConvertirVolumenLgn = propiedades.Resultado.DeterminacionFactorConvertirVolumenLgn

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            string plantillaExcel = string.Empty;
            if (tipoReporte == 1)
            {
                plantillaExcel = $"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaDeterminacionVolGNA.xlsx";
            }
            else
            {
                plantillaExcel = $"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaDeterminacionVolGNAPDF.xlsx";
            }

            using (var template = new XLTemplate(plantillaExcel))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }
 
    }
}
