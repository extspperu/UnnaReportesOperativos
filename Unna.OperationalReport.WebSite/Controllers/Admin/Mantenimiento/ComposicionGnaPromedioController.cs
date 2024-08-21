using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Propiedades.ComposicionGnaPromedios.Dtos;
using Unna.OperationalReport.Service.Propiedades.ComposicionGnaPromedios.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Mantenimiento
{
    [Route("api/admin/mantenimiento/[controller]")]
    [ApiController]
    public class ComposicionGnaPromedioController : ControladorBase
    {

        private readonly IComposicionGnaPromedioServicio _composicionGnaPromedioServicio;
        public ComposicionGnaPromedioController(IComposicionGnaPromedioServicio composicionGnaPromedioServicio)
        {
            _composicionGnaPromedioServicio = composicionGnaPromedioServicio;
        }

        [HttpGet("ListarPorFechaYLote/{idLote}/{fecha}")]
        [RequiereAcceso()]
        public async Task<List<ComposicionGnaPromedioDto>?> ListarPorIdLoteYFechaAsync(string? idLote, DateTime? fecha)
        {
            var operacion = await _composicionGnaPromedioServicio.ListarPorIdLoteYFechaAsync(fecha, idLote);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("Guardar/{idLote}/{fecha}")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(List<ComposicionGnaPromedioDto>? peticion, string? idLote, DateTime? fecha)
        {
            var operacion = await _composicionGnaPromedioServicio.GuardarAsync(peticion,fecha, idLote,ObtenerIdUsuarioActual());
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
