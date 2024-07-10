using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Laboratorio.GLP.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Service.Laboratorio.CGN.Servicios.Abstracciones;


namespace Unna.OperationalReport.WebSite.Controllers.Admin.Laboratorio.CGN
{
    [Route("api/admin/laboratorio/cgn/[controller]")]
    [ApiController]
    public class LaboratorioCGNController : ControllerBase
    {
        private readonly ICGN _iCGN;
        public LaboratorioCGNController(ICGN CGN)
        {
            _iCGN = CGN;
        }
        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public IActionResult ObtenerAsync()
        {
            var operacion = _iCGN.ObtenerAsync();
            return Ok(operacion);
        }
    }
}
