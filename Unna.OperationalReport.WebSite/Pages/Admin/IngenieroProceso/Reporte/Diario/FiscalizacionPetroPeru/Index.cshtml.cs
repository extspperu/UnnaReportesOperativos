using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Diario.FiscalizacionPetroPeru
{
    public class IndexModel : PageModel
    {
        public FiscalizacionPetroPeruDto? Dato { get; set; }
        public void OnGet()
        {
        }
    }
}
