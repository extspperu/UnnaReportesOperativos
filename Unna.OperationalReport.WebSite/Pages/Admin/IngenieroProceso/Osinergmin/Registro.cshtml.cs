using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Osinergmin
{
    public class RegistroModel : PageModel
    {

        public string? Fecha { get; set; } 
        public void OnGet()
        {
            Fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd/MM/yyyy");
        }
    }
}
