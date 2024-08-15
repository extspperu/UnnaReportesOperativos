using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Threading.Tasks;

namespace Unna.OperationalReport.WebSite.Pages.Admin.DashBoard
{
    public class IndexModel : PageModel
    {
        private readonly ITokenAcquisition _tokenAcquisition;

        public string EmbedToken { get; private set; }
        public string EmbedUrl { get; private set; }
        public string ReportId { get; private set; }

        public IndexModel(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
        }

        public async Task OnGetAsync()
        {
            string[] scopes = new string[] { "https://analysis.windows.net/powerbi/api/.default" };
            EmbedToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

            ReportId = "1970263a-9366-4fb9-b07b-fb71e7a00ff6"; // Obtén esto desde appsettings.json
            EmbedUrl = $"https://app.powerbi.com/reportEmbed?reportId={ReportId}&groupId=449eb480-9feb-47b2-917e-156297e4dfd6";
        }
    }
}
