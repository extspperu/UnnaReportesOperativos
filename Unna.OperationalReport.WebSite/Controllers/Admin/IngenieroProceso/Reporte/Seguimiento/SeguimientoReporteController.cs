using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Dtos;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Seguimiento
{
    [Route("api/ingenieroProceso/reporte/[controller]")]
    [ApiController]
    public class SeguimientoReporteController : ControladorBase
    {
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;
        public SeguimientoReporteController(ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio)
        {
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<List<ColumnaDto>?> ObtenerAsync()
        {
            var operacion = await _seguimientoBalanceDiarioServicio.ObtenerDatosSeguimiento(2);
            return operacion;
        }
    }
}
