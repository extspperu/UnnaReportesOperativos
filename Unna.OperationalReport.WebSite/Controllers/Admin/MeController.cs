using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Usuarios.Dtos;
using Unna.OperationalReport.Service.Usuarios.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Auth;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class MeController : ControladorBase
    {

        private readonly IUsuarioServicio _usuarioServicio;
        public MeController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<UsuarioDto?> ObtenerAsync()
        {
            var operacion = await _usuarioServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("ActualizarFirma/{idFirma}")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> ActualizarFirmaAsync(string idFirma)
        {
            var operacion = await _usuarioServicio.ActualizarFirmaAsync(ObtenerIdUsuarioActual() ?? 0, idFirma);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
        
        [HttpPost("ActualizarDatos")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> ActualizarDatosAsync(ActualizarDatosUsuarioDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _usuarioServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpGet("Obtener/{idUsuario}")]
        [RequiereAcceso()]
        public async Task<UsuarioDto?> ObtenerAsync(string idUsuario)
        {
            var operacion = await _usuarioServicio.ObtenerAsync(idUsuario);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("ListarUsuarios")]
        [RequiereAcceso()]
        public async Task<List<ListarUsuariosDto>?> ListarUsuariosAsync()
        {
            var operacion = await _usuarioServicio.ListarUsuariosAsync();
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
        
        [HttpPost("CrearActualizar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> CrearActualizarAsync(CrearActualizarUsuarioDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuarioAdmin = ObtenerIdUsuarioActual();
            var operacion = await _usuarioServicio.CrearActualizarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
