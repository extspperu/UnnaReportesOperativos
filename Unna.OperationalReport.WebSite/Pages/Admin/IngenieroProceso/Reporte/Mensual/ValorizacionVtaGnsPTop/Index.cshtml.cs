using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.ValorizacionVtaGnsPTop.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.ValorizacionVtaGnsPTop.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.ValorizacionVtaGnsPTop
{
    public class IndexModel : PageModel
    {
        public ValorizacionVtaGnsPTopDto? Dato { get; set; }

        private readonly IValorizacionVtaGnsPTopServicio _ValorizacionVtaGnsPTopServicio;
        public IndexModel(IValorizacionVtaGnsPTopServicio ComposicionGnaLIVPTopServicio)
        {
            _ValorizacionVtaGnsPTopServicio = ComposicionGnaLIVPTopServicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _ValorizacionVtaGnsPTopServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}

