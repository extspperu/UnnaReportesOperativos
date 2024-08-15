using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;
using DocumentFormat.OpenXml.Bibliography;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Dashboard
{
    [Authorize] // Asegura que sólo usuarios autenticados puedan acceder
    [Route("api/admin/[controller]")]
    [ApiController]
    public class DashboardController : ControladorBaseWeb
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IConfiguration _configuration;

        public DashboardController(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            _tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
        }

        [HttpGet("PowerBI")]
        public async Task<IActionResult> GetPowerBIEmbedToken()
        {
            // Escopos necesarios para acceder a la API de Power BI
            string[] scopes = new string[] { "https://analysis.windows.net/powerbi/api/.default" };

            // Obtener el token de acceso para el usuario autenticado
            string accessToken;

            var result = new
            {
                AccessToken = "",

            };
            return Ok(result);
        }
    }
}
