using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Laboratorio.GNSGNA.Dtos;
using Unna.OperationalReport.Service.Laboratorio.GNSGNA.Servicios.Abstracciones;

using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Laboratorio.GNSGNA
{
    [Route("api/admin/laboratorio/gnsgna/[controller]")]
    [ApiController]
    public class LaboratorioGNSGNAController : ControladorBaseWeb
    {
        private readonly IGNSGNA _IGNSGNA;
        public LaboratorioGNSGNAController(IGNSGNA IGNSGNA)
        {
            _IGNSGNA = IGNSGNA;
        }
        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public IActionResult ObtenerAsync()
        {
            var operacion = _IGNSGNA.ObtenerAsync();
            return Ok(operacion);
        }
        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(GNSGNADto datos)
        {
            var operacion = await _IGNSGNA.GuardarAsync(datos);

            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
