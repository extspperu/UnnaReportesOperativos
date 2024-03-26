using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV_2.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV_2.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Quincenal.ResBalanceEnergLIV_2
{
    public class IndexModel : PageModel
    {
        public ResBalanceEnergLIV_2Dto? Dato { get; set; }

        private readonly IResBalanceEnergLIV_2Servicio _ResBalanceEnergLIV_2Servicio;
        public IndexModel(IResBalanceEnergLIV_2Servicio ResBalanceEnergLIV_2Servicio)
        {
            _ResBalanceEnergLIV_2Servicio = ResBalanceEnergLIV_2Servicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _ResBalanceEnergLIV_2Servicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}

