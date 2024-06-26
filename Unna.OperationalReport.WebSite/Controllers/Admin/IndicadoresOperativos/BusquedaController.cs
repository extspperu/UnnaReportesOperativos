using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.IndicadoresOperativos.Dtos;
using Unna.OperationalReport.Service.IndicadoresOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IndicadoresOperativos
{
    [Route("api/admin/indicadoresOperativos/[controller]")]
    [ApiController]
    public class BusquedaController : ControladorBase
    {
        private readonly IIndicadoresOperativosServicio _indicadoresOperativosServicio;
        public BusquedaController(IIndicadoresOperativosServicio indicadoresOperativosServicio)
        {
            _indicadoresOperativosServicio = indicadoresOperativosServicio;
        }

        [HttpGet("ListarPeriodos")]
        [RequiereAcceso()]
        public List<PeriodoIndicadoresDto>? ListarPeriodosAsync()
        {
            var operacion = _indicadoresOperativosServicio.ListarPeriodosAsync();
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("Indicadores/{periodo}")]
        [RequiereAcceso()]
        public async Task<List<IndicadoresOperativosDto>?> IndicadoresAsync(string periodo)
        {
            var operacion = await _indicadoresOperativosServicio.BusquedaIndicadoresAsync(periodo);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
