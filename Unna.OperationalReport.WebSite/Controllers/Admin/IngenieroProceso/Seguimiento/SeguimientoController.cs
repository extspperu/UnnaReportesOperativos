using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Cartas.Dgh.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Dtos;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Seguimiento
{
    [Route("api/ingenieroProceso/balanceDiario/[controller]")]
    [ApiController]
    public class SeguimientoController : ControladorBase
    {

        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;
        public SeguimientoController(ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio)
        {
            _seguimientoBalanceDiarioServicio =seguimientoBalanceDiarioServicio;
        }
        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<List<ColumnaDto>?> ObtenerAsync()
        {
            var operacion = await _seguimientoBalanceDiarioServicio.ObtenerDatosSeguimiento(1);
            return operacion;
        }

        [HttpGet("ActualizarEstado")]
        [RequiereAcceso()]
        public async Task<bool> ActualizarEstadoAsync(int idSeguimientoDiario, int idEstadoColor)
        {
            var operacion = await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(idSeguimientoDiario, idEstadoColor);
            return operacion;
        }
    }
}
