using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Cartas.Cromatografia.Dtos;
using Unna.OperationalReport.Service.Cartas.Cromatografia.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Laboratorio
{
    [Route("api/admin/laboratorio/[controller]")]
    [ApiController]
    public class GnaGnsController : ControladorBase
    {
        private readonly IGnaGnsServicio _gnaGnsServicio;
        public GnaGnsController(IGnaGnsServicio gnaGnsServicio)
        {
            _gnaGnsServicio = gnaGnsServicio;
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(RegistroCromatografiaDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual();
            peticion.Periodo = FechasUtilitario.ObtenerDiaOperativo();
            var operacion = await _gnaGnsServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Obtener")]
        [RequiereAcceso()]
        public async Task<RegistroCromatografiaDto?> ObtenerAsync(BuscarGnaGnsDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.Periodo = FechasUtilitario.ObtenerDiaOperativo();
            var operacion = await _gnaGnsServicio.ObtenerAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
