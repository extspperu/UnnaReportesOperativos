using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Unna.OperationalReport.WebSite.Pages.Admin.Cartas
{
    public class IndexModel : PageModel
    {
        public string? IdCarta { get; set; }

        public void OnGet(string Id)
        {
            IdCarta = Id;
        }
    }
}
