using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.BoletaSuministroGNSdelLoteIVaEnel
{
    public class IndexModel : PageModel
    {

        public BoletaSuministroGNSdelLoteIVaEnelDto? Dato { get; set; }

        private readonly IBoletaSuministroGNSdelLoteIVaEnelServicio _boletaSuministroGNSdelLoteIVaEnelServicio;
        public IndexModel(IBoletaSuministroGNSdelLoteIVaEnelServicio boletaSuministroGNSdelLoteIVaEnelServicio)
        {
            _boletaSuministroGNSdelLoteIVaEnelServicio = boletaSuministroGNSdelLoteIVaEnelServicio;
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
            var operacion = await _boletaSuministroGNSdelLoteIVaEnelServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }



    }
}
