using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Servicios.Abstracciones;


namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Quincenal.ResBalanceEnergLgnLIV_2
{
    public class IndexModel : PageModel
    {
        public ResBalanceEnergLgnLIV_2Dto? Dato { get; set; }

        private readonly IResBalanceEnergLgnLIV_2Servicio _ResBalanceEnergLgnLIV_2Servicio;
        public IndexModel(IResBalanceEnergLgnLIV_2Servicio ResBalanceEnergLgnLIV_2Servicio)
        {
            _ResBalanceEnergLgnLIV_2Servicio = ResBalanceEnergLgnLIV_2Servicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _ResBalanceEnergLgnLIV_2Servicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}

