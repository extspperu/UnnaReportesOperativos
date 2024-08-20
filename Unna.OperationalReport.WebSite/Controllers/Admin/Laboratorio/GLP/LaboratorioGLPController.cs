using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Cartas.Cromatografia.Dtos;
using Unna.OperationalReport.Service.Cartas.Cromatografia.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Laboratorio.GLP.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Laboratorio.GLP
{
    [Route("api/admin/laboratorio/glp/[controller]")]
    [ApiController]
    public class LaboratorioGLPController : ControladorBase
    {
        private readonly IGLP _iGLP;
        private readonly IGlpServicio _glpServicio;

        public LaboratorioGLPController(IGLP iGLP)
        {
            _iGLP = iGLP;
        }
        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public IActionResult ObtenerAsync()
        {
            var operacion = _iGLP.ObtenerAsync();
            return Ok(operacion);
        }
        [HttpGet("Obtener2")]
        [RequiereAcceso()]
        public async Task<RegistroCromatografiaDto?> ObtenerAsync2()
        {
            var operacion = await _glpServicio.ObtenerAsync();
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
