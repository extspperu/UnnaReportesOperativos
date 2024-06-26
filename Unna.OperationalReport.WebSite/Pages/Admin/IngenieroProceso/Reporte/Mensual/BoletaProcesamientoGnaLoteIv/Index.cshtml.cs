using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.BoletaProcesamientoGnaLoteIv
{
    public class IndexModel : PageModel
    {

        public BoletaProcesamientoGnaLoteIvDto? Dato { get; set; }

        private readonly IBoletaProcesamientoGnaLoteIvServicio _boletaProcesamientoGnaLoteIvServicio;
        public IndexModel(IBoletaProcesamientoGnaLoteIvServicio boletaProcesamientoGnaLoteIvServicio)
        {
            _boletaProcesamientoGnaLoteIvServicio = boletaProcesamientoGnaLoteIvServicio;
        }

        public async Task<IActionResult> OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _boletaProcesamientoGnaLoteIvServicio.ObtenerAsync(idUsuario);
            if (!operacion.Completado)
            {
                return RedirectToPage("/Admin/IngenieroProceso/Reporte/Mensual/Index");
            }

            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
            return Page();
        }

    }
}
