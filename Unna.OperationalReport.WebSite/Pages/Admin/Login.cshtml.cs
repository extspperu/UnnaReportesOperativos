using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Unna.OperationalReport.WebSite.Pages.Admin
{
    public class LoginModel : PageModel
    {
        public string? UrlDireccionar { get; set; }
        public void OnGet(string ReturnUrl)
        {
            UrlDireccionar = ReturnUrl;
        }
    }
}
