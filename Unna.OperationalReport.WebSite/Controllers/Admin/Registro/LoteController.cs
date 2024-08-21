using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;
using Unna.OperationalReport.Service.Registros.Lotes.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Registro
{
    [Route("api/admin/registro/[controller]")]
    [ApiController]
    public class LoteController : ControladorBase
    {
        private readonly ILoteServicio _loteServicio;
        public LoteController(ILoteServicio loteServicio)
        {
            _loteServicio = loteServicio;
        }

        [HttpGet("Listar")]
        [RequiereAcceso()]
        public async Task<List<LoteDto>?> ListarTodosAsync()
        {
            var operacion = await _loteServicio.ListarTodosAsync();
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
