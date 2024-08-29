using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Servicios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Diario.ReporteDiarioPgt
{
    public class IndexModel : PageModel
    {
        public ReporteDiarioDto? Dato { get; set; }

        private readonly IReporteDiarioServicio _reporteDiarioServicio;
        public IndexModel(IReporteDiarioServicio reporteDiarioServicio)
        {
            _reporteDiarioServicio = reporteDiarioServicio;
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
            var operacion = await _reporteDiarioServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}

