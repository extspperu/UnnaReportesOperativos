using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns_2.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns_2.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Quincenal.ValorizacionVtaGns_2
{
    public class IndexModel : PageModel
    {
        public ValorizacionVtaGns_2Dto? Dato { get; set; }

        private readonly IValorizacionVtaGns_2Servicio _ValorizacionVtaGns_2Servicio;
        public IndexModel(IValorizacionVtaGns_2Servicio ComposicionGnaLIV_2Servicio)
        {
            _ValorizacionVtaGns_2Servicio = ComposicionGnaLIV_2Servicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _ValorizacionVtaGns_2Servicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}

