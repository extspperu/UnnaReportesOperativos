using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Quincenal.ResumenBalanceEnergiaLIV
{
    public class IndexModel : PageModel
    {
        public ResBalanceEnergLIVDto? Dato { get; set; }

        private readonly IResBalanceEnergLIVServicio _ResBalanceEnergLIVServicio;
        private readonly IConfiguration _configuration;

        public IndexModel(IResBalanceEnergLIVServicio ResBalanceEnergLIVServicio, IConfiguration configuration)
        {
            _ResBalanceEnergLIVServicio = ResBalanceEnergLIVServicio;
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

            var operacion = await _ResBalanceEnergLIVServicio.ObtenerAsync(idUsuario, someSetting);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}

