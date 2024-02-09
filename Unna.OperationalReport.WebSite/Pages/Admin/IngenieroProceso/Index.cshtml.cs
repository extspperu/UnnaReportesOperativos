using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso
{
    public class IndexModel : PageModel
    {

        public string? IdGrupo { get; set; }
        public void OnGet(string? Id)
        {
            IdGrupo = Id;
        }
    }
}
