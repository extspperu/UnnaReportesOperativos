using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.BalanceDiario
{
    [Route("api/ingenieroProceso/balanceDiario/[controller]")]
    [ApiController]
    public class RegistroController : ControladorBase
    {

        private readonly IRegistroServicio _registroServicio;
        public RegistroController(IRegistroServicio registroServicio)
        {
            _registroServicio = registroServicio;
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(List<RegistroDto> peticion)
        {
            VerificarIfEsBuenJson(peticion);
            var operacion = await _registroServicio.GuardarAsync(peticion, ObtenerIdUsuarioActual(),null);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


		[HttpPost("GuardarEditado")]
		[RequiereAcceso()]
		public async Task<RespuestaSimpleDto<bool>?> GuardarEditadoAsync(List<RegistroDto> peticion)
		{
			VerificarIfEsBuenJson(peticion);
			var operacion = await _registroServicio.GuardarAsync(peticion, ObtenerIdUsuarioActual(), true);
			return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
		}

	}
}
