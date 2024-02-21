using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Diario.BoletaBalanceEnergia
{
    public class IndexModel : PageModel
    {
        public BoletaBalanceEnergiaDto? Dato { get; set; }

        private readonly IBoletaBalanceEnergiaServicio _BoletaBalanceEnergiaServicio;
        public IndexModel(IBoletaBalanceEnergiaServicio BoletaBalanceEnergiaServicio)
        {
            _BoletaBalanceEnergiaServicio = BoletaBalanceEnergiaServicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _BoletaBalanceEnergiaServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}
