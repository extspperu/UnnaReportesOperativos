using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Diario.ReporteExistencias
{
    public class IndexModel : PageModel
    {
        private readonly IBoletaVentaGnsServicio _boletaVentaGnsServicio;
        public IndexModel(IBoletaVentaGnsServicio boletaVentaGnsServicio)
        {
            _boletaVentaGnsServicio = boletaVentaGnsServicio;
        }
        public BoletaVentaGnsDto? Data { get; set; }
        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _boletaVentaGnsServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Data = operacion.Resultado;
            }

        }
    }
}
