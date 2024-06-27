using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Registros.Osinergmin.Dtos;
using Unna.OperationalReport.Service.Registros.Osinergmin.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Osinergmin
{
    [Route("api/ingenieroProceso/osinergmin/[controller]")]
    [ApiController]
    public class RegistroController : ControladorBase
    {

        private readonly IOsinergminServicio _osinergminServicio;
        public RegistroController(IOsinergminServicio osinergminServicio)
        {
            _osinergminServicio = osinergminServicio;
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(OsinergminDto parametros)
        {
            VerificarIfEsBuenJson(parametros);
            parametros.IdUsuario = ObtenerIdUsuarioActual();
            parametros.Fecha = DateTime.UtcNow;
            var operacion = await _osinergminServicio.GuardarAsync(parametros);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<OsinergminDto?> ObtenerAsync()
        {
            var operacion = await _osinergminServicio.ObtenerAsync(DateTime.UtcNow);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
