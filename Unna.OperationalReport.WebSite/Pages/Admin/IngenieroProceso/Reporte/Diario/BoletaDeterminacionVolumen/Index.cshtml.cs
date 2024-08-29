using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Servicios.Implementaciones;


namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Diario.BoletaDeterminacionVolumen
{
    public class IndexModel : PageModel
    {
        public BoletaDeterminacionVolumenGnaDto? Dato { get; set; }
        private readonly IBoletaDeterminacionVolumenGnaServicio _boletaDeterminacionVolumenGnaServicio;
        public IndexModel(IBoletaDeterminacionVolumenGnaServicio ReporteDiarioDeterminacionVolumenGnaServicio)
        {
            _boletaDeterminacionVolumenGnaServicio = ReporteDiarioDeterminacionVolumenGnaServicio;
        }
        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                if (long.TryParse(claim.Value, out idUsuario))
                {
                }
                else
                {
                    idUsuario = 16;
                }
            }
            var operacion = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}
