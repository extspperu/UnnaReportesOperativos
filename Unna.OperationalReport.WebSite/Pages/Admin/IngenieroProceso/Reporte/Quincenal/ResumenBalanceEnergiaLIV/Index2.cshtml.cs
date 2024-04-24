using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Quincenal.ResBalanceEnergLgnLIV
{
    public class IndexModel2 : PageModel
    {
        public ResBalanceEnergLgnLIVDto? Dato { get; set; }

        private readonly IResBalanceEnergLgnLIVServicio _ResBalanceEnergLgnLIVServicio;
        public IndexModel2(IResBalanceEnergLgnLIVServicio ResBalanceEnergLgnLIVServicio)
        {
            _ResBalanceEnergLgnLIVServicio = ResBalanceEnergLgnLIVServicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _ResBalanceEnergLgnLIVServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}

