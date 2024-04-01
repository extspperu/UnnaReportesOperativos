using ClosedXML.Report;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class BoletaBalanceEnergiaController : ControladorBaseWeb
    {
        private readonly IBoletaBalanceEnergiaServicio _boletaBalanceEnergiaServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;

        public BoletaBalanceEnergiaController(
            IBoletaBalanceEnergiaServicio boletaBalanceEnergiaServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _boletaBalanceEnergiaServicio = boletaBalanceEnergiaServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(BoletaBalanceEnergiaDto boletaBalanceEnergia)
        {
            VerificarIfEsBuenJson(boletaBalanceEnergia);
            boletaBalanceEnergia.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaBalanceEnergiaServicio.GuardarAsync(boletaBalanceEnergia);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("GenerarPdf")]
        public async Task<IActionResult> GenerarExcelAsync()
        {

            var operativo = await _boletaBalanceEnergiaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }
            var dato = operativo.Resultado;

            var liquidosBarriles = new
            {
                Items = dato.LiquidosBarriles
            };

            var gnsAEnel = new
            {
                Items = dato.GnsAEnel
            };

            var consumoPropio = new
            {
                Items = dato.ConsumoPropio
            };

            var consumoPropioGnsVendioEnel = new
            {
                Items = dato.ConsumoPropioGnsVendioEnel
            };

            var complexData = new
            {
                Fecha = dato?.Fecha,

                Entrega = dato?.GnaEntregaUnna?.Entrega,
                Volumen = dato?.GnaEntregaUnna?.Volumen,
                PoderCalorifico = dato?.GnaEntregaUnna?.PoderCalorifico,
                Energia = dato?.GnaEntregaUnna?.Energia,
                Riqueza = dato?.GnaEntregaUnna?.Riqueza,
               
                LiquidosBarriles = liquidosBarriles,
                
                ComPesadosGna = dato?.ComPesadosGna,
                PorcentajeEficiencia = dato?.PorcentajeEficiencia,

                //Contenido Calórico promedio del  LGN
                ContenidoCalorificoPromLgn = dato?.ContenidoCalorificoPromLgn,

                GnsAEnel = gnsAEnel,
                ConsumoPropio = consumoPropio,
                ConsumoPropioGnsVendioEnel = consumoPropioGnsVendioEnel,
               
                EntregaGna = dato?.EntregaGna?.Mpcsd,
                EntregaGnaEnergia = dato?.EntregaGna?.Energia,
                GnsRestituido = dato?.GnsRestituido?.Mpcsd,
                GnsRestituidoEnergia = dato?.GnsRestituido?.Energia,
                GnsConsumoPropio = dato?.GnsConsumoPropio?.Mpcsd,
                GnsConsumoPropioEnergia = dato?.GnsConsumoPropio?.Energia,
                Recuperacion = dato?.Recuperacion?.Barriles,

                DiferenciaEnergetica = dato?.DiferenciaEnergetica,
                ExesoConsumoPropio = dato?.ExesoConsumoPropio,
                Comentario = dato?.Comentario
            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaBalanceEnergia.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BoletaBalanceEnergia-{dato?.Fecha?.Replace("/", "-")}.xlsx");
        }
    }
}
