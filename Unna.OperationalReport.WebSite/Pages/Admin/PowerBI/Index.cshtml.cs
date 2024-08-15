using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Unna.OperationalReport.WebSite.Pages.Admin.PowerBI
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public string PowerBIUrl { get; set; }
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
            var workspaceId = _configuration["PowerBI:WorkspaceId"];
            var reportId = _configuration["PowerBI:ReportId"];
            var tenantId = _configuration["AzureAd:TenantId"];

            PowerBIUrl = $"https://app.powerbi.com/reportEmbed?reportId={reportId}&groupId={workspaceId}&autoAuth=true&ctid={tenantId}";
        }
    }
}
