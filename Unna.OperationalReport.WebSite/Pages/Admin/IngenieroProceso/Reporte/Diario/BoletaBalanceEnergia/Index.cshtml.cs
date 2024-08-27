using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

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

        public async Task<IActionResult> OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _BoletaBalanceEnergiaServicio.ObtenerAsync(idUsuario, FechasUtilitario.ObtenerDiaOperativo());
            if (!operacion.Completado)
            {
                return RedirectToPage("/Admin/IngenieriaProceso/Reporte/Diario/Index");
            }
            Dato = operacion.Resultado;

            return Page();
        }
    }
}
