using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Service.Cartas.Cromatografia.Dtos;
using Unna.OperationalReport.Service.Cartas.Cromatografia.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Laboratorio
{
    [Route("api/admin/laboratorio/[controller]")]
    [ApiController]
    public class CgnController : ControladorBase
    {
        private readonly ICgnServicio _cgnServicio;
        public CgnController(ICgnServicio cgnServicio)
        {
            _cgnServicio = cgnServicio;
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(RegistroCromatografiaDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual();
            peticion.Periodo = FechasUtilitario.ObtenerDiaOperativo();
            var operacion = await _cgnServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<RegistroCromatografiaDto?> ObtenerAsync()
        {
            var operacion = await _cgnServicio.ObtenerAsync();
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


    }
}
