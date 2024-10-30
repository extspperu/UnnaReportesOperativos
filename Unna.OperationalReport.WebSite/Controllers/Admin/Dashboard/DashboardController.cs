using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;
using DocumentFormat.OpenXml.Bibliography;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Dashboard
{
    [Authorize] 
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
            string[] scopes = new string[] { "https://analysis.windows.net/powerbi/api/.default" };

            string accessToken;

            var result = new
            {
                AccessToken = "",

            };
            return Ok(result);
        }
    }
}
