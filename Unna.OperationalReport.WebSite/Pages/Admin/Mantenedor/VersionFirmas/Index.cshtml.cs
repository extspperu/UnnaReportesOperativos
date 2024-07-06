using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.Mantenedor.VersionFirmas
{
    public class IndexModel : PageModel
    {
        private readonly IReporteServicio _reporteServicio;
        public IndexModel(IReporteServicio reporteServicio)
        {
            _reporteServicio = reporteServicio;
        }
        public List<ReporteDto> reportes { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var operacion = await _reporteServicio.ListarAsync();
            if (!operacion.Completado || operacion.Resultado == null)
            {
                return NotFound();
            }
            reportes = operacion.Resultado;

            return Page();
        }

    }
}
