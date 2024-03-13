using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Quincenal.ValorizacionVtaGns
{
    public class IndexModel : PageModel
    {
        public ValorizacionVtaGnsDto? Dato { get; set; }

        private readonly IValorizacionVtaGnsServicio _ValorizacionVtaGnsServicio;
        public IndexModel(IValorizacionVtaGnsServicio ComposicionGnaLIVServicio)
        {
            _ValorizacionVtaGnsServicio = ComposicionGnaLIVServicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _ValorizacionVtaGnsServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}

