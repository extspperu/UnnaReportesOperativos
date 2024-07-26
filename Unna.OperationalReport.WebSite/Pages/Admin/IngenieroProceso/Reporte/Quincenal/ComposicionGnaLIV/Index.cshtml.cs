using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Quincenal.ComposicionGnaLIV
{
    public class IndexModel : PageModel
    {
        public ComposicionGnaLIVDto? Dato { get; set; }

        private readonly IComposicionGnaLIVServicio _ComposicionGnaLIVServicio;
        public IndexModel(IComposicionGnaLIVServicio ComposicionGnaLIVServicio)
        {
            _ComposicionGnaLIVServicio = ComposicionGnaLIVServicio;
        }

        public async Task OnGet(string? Id)
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _ComposicionGnaLIVServicio.ObtenerAsync(idUsuario, Id);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}

