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
            //try
            //{
            //    accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            //}
            //catch (Exception ex)
            //{
            //    // Manejo de errores para el caso donde no se pueda obtener el token
            //    return Unauthorized(new { error = "Unable to obtain access token.", detail = ex.Message });
            //}

            //// Datos del informe de Power BI (puedes parametrizar estos valores según tu caso)
            //var reportId = "1970263a-9366-4fb9-b07b-fb71e7a00ff6";
            //var embedUrl = $"https://app.powerbi.com/reportEmbed?reportId={reportId}";
            //var clientId = _configuration["AzureAd:ClientId"];
            //var tenantId = _configuration["AzureAd:TenantId"];

            //var result = new
            //{
            //    AccessToken = accessToken,
            //    EmbedUrl = embedUrl,
            //    ReportId = reportId,
            //    ClientId = clientId,
            //    TenantId = tenantId
            //};
            var result = new
            {
                AccessToken = "",

            };
            return Ok(result);
        }
    }
}
