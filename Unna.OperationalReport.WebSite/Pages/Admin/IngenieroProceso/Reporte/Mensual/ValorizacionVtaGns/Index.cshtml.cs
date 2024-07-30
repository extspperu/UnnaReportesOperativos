using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.ValorizacionVtaGns
{
    public class IndexModel : PageModel
    {
        public ValorizacionVtaGnsDto? Dato { get; set; }

        private readonly IValorizacionVtaGnsServicio _ValorizacionVtaGnsServicio;
        private readonly IConfiguration _configuration;
        public IndexModel(IValorizacionVtaGnsServicio valorizacionVtaGnsServicio, IConfiguration configuration)
        {
            _ValorizacionVtaGnsServicio = valorizacionVtaGnsServicio;
            _configuration = configuration;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            string someSetting = _configuration["general:diaOperativo"];
            var operacion = await _ValorizacionVtaGnsServicio.ObtenerAsync(idUsuario, someSetting, 2);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}
