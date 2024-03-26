using ClosedXML.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class ReporteDiarioPgtController : ControladorBaseWeb
    {
        private readonly IReporteDiarioServicio _reporteDiarioServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;

        public ReporteDiarioPgtController(
            IReporteDiarioServicio reporteDiarioServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _reporteDiarioServicio = reporteDiarioServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }

        [HttpGet("GenerarExcel")]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            var operativo = await _reporteDiarioServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }

            var dato = operativo.Resultado;

            var gasNaturalAsociado = new
            {
                Items = dato.GasNaturalAsociado
            };

            var gasNaturalSeco = new
            {
                Items = dato.GasNaturalSeco
            };

            var liquidosGasNaturalProduccionVentas = new
            {
                Items = dato.LiquidosGasNaturalProduccionVentas
            };

            var volumenDespachoGlp = new
            {
                Items = dato.VolumenDespachoGlp
            };

            var volumenDespachoCgn = new
            {
                Items = dato.VolumenDespachoCgn
            };

            var volumenProduccionLoteXGnaTotalCnpc = new
            {
                Items = dato.VolumenProduccionLoteXGnaTotalCnpc
            };

            var volumenProduccionLoteXLiquidoGasNatural = new
            {
                Items = dato.VolumenProduccionLoteXLiquidoGasNatural
            };

            var volumenProduccionEnel = new
            {
                Items = dato.VolumenProduccionEnel
            };

            var volumenProduccionGasNaturalEnel = new
            {
                Items = dato.VolumenProduccionGasNaturalEnel
            };

            var volumenProduccionPetroperu = new
            {
                Items = dato.VolumenProduccionPetroperu
            };

            var volumenProduccionLiquidoGasNatural = new
            {
                Items = dato.VolumenProduccionLiquidoGasNatural
            };

            var volumenProduccionLoteIvUnnaEnegia = new
            {
                Items = dato.VolumenProduccionLoteIvUnnaEnegia
            };

            var volumenProduccionLoteIvLiquidoGasNatural = new
            {
                Items = dato.VolumenProduccionLoteIvLiquidoGasNatural
            };

            var complexData = new
            {
                Fecha = dato?.Fecha,
                GasProcesado = dato?.GasProcesado,
                GasNoProcesado = dato?.GasNoProcesado,
                UtilizacionPlantaParinias = dato?.UtilizacionPlantaParinias,
                HoraPlantaFs = dato?.HoraPlantaFs,
                EficienciaRecuperacionLgn = dato?.EficienciaRecuperacionLgn,
                Comentario = dato?.Comentario,
                GasNaturalAsociado = gasNaturalAsociado,
                GasNaturalSeco = gasNaturalSeco,
                LiquidosGasNaturalProduccionVentas = liquidosGasNaturalProduccionVentas,
                VolumenDespachoGlp = volumenDespachoGlp,
                VolumenDespachoCgn = volumenDespachoCgn,
                VolumenProduccionLoteXGnaTotalCnpc = volumenProduccionLoteXGnaTotalCnpc,
                VolumenProduccionLoteXLiquidoGasNatural = volumenProduccionLoteXLiquidoGasNatural,
                VolumenProduccionEnel = volumenProduccionEnel,
                VolumenProduccionGasNaturalEnel = volumenProduccionGasNaturalEnel,
                VolumenProduccionPetroperu = volumenProduccionPetroperu,
                VolumenProduccionLiquidoGasNatural = volumenProduccionLiquidoGasNatural,
                VolumenProduccionLoteIvUnnaEnegia = volumenProduccionLoteIvUnnaEnegia,
                VolumenProduccionLoteIvLiquidoGasNatural = volumenProduccionLoteIvLiquidoGasNatural
            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaReporteDiario.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BoletaReporteDiario-{dato?.Fecha?.Replace("/", "-")}.xlsx");
        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<ReporteDiarioDto?> ObtenerAsync()
        {
            var operacion = await _reporteDiarioServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(ReporteDiarioDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _reporteDiarioServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
