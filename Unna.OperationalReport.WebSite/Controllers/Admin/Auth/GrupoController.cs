using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Auth.Grupos.Dtos;
using Unna.OperationalReport.Service.Auth.Grupos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Auth
{
    [Route("api/admin/auth/[controller]")]
    [ApiController]
    public class GrupoController : ControladorBase
    {
        private readonly IGrupoServicio _grupoServicio;
        public GrupoController(IGrupoServicio grupoServicio)
        {
            _grupoServicio = grupoServicio;
        }

        [HttpGet("Listar")]
        [RequiereAcceso()]
        public async Task<List<GrupoDto>?> ListarAsync()
        {
            var operacion = await _grupoServicio.ListarAsync();
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
