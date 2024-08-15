using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Dtos;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Quincenal
{
    [Route("api/ingenieroProceso/reporte/seguimiento/quincenal/[controller]")]
    [ApiController]
    public class SeguimientoController : ControllerBase
    {
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;
        public SeguimientoController(ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio)
        {
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
        }
        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<List<ColumnaDto>?> ObtenerAsync()
        {
            var operacion = await _seguimientoBalanceDiarioServicio.ObtenerDatosSeguimiento(3);
            return operacion;
        }
    }
}
