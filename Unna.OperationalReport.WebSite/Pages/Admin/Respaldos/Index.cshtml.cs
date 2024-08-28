using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Unna.OperationalReport.Service.Respaldo.Dtos;
using Unna.OperationalReport.Service.Respaldo.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.Respaldos
{
    public class IndexModel : PageModel
    {

        private readonly IRespaldoServicio _respaldoServicio;
        public IndexModel(IRespaldoServicio respaldoServicio)
        {
            _respaldoServicio = respaldoServicio;
        }

        public async Task<IActionResult> OnGet()
        {
            await _respaldoServicio.EnviarAsync(new RespaldoDto { Nombre = "" });

            return Page();
        }
    }
}
