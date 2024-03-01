using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.FacturacionGnsLIV
{
    public class IndexModel : PageModel
    {
        public FacturacionGnsLIVDto? Dato { get; set; }

        private readonly IFacturacionGnsLIVServicio _FacturacionGnsLIVServicio;
        public IndexModel(IFacturacionGnsLIVServicio FacturacionGnsLIVServicio)
        {
            _FacturacionGnsLIVServicio = FacturacionGnsLIVServicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _FacturacionGnsLIVServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}

