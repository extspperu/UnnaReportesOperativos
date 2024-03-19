using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;
using Aspose.Cells;
using ClosedXML.Report;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class BoletaDeterminacionVolumenController : ControladorBaseWeb
    {
        private readonly IBoletaDeterminacionVolumenGnaServicio _boletaDeterminacionVolumenGnaServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public BoletaDeterminacionVolumenController(
        IBoletaDeterminacionVolumenGnaServicio boletaDeterminacionVolumenGnaServicio,
        IWebHostEnvironment hostingEnvironment,
        GeneralDto general
        )
        {
            _boletaDeterminacionVolumenGnaServicio = boletaDeterminacionVolumenGnaServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
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
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(BoletaDeterminacionVolumenGnaDto boletaDeterminacionVolumenGna)
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
            var operativo = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }
            var dato = operativo.Resultado;
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
                PreparadoPör = $"Preparado por: {dato?.General?.PreparadoPör}",
                AprobadoPor = $"Aprobado por: {dato?.General?.AprobadoPor}",

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

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaDeterminacionVolGNA.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BoletaDeterminacionVolGNA-{dato.Fecha.Replace("/", "-")}.xlsx");
        }
    }
}
