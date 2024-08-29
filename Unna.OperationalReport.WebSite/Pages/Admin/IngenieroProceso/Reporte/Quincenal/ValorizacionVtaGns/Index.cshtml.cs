using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Quincenal.ValorizacionVtaGns
{
    public class IndexModel : PageModel
    {
        public ValorizacionVtaGnsDto? Dato { get; set; }
        public string? Grupo { get; set; }

        private readonly IValorizacionVtaGnsServicio _ValorizacionVtaGnsServicio;
        private readonly IConfiguration _configuration;

        public IndexModel(IValorizacionVtaGnsServicio valorizacionVtaGnsServicio, IConfiguration configuration)
        {
            _ValorizacionVtaGnsServicio = valorizacionVtaGnsServicio;
            _configuration = configuration;
        }

        public async Task OnGet(string? Id)
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
            var operacion = await _ValorizacionVtaGnsServicio.ObtenerAsync(idUsuario, Id);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
            Grupo = Id;
        }
    }
}
