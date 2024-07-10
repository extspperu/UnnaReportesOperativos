using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Laboratorio.GLP.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Laboratorio.GLP
{
    [Route("api/admin/laboratorio/glp/[controller]")]
    [ApiController]
    public class LaboratorioGLPController : ControllerBase
    {
        private readonly IGLP _iGLP;
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
    }
}
