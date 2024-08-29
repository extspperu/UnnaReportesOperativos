using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.BoletadeValorizacionPetroperuLoteI
{
    public class IndexModel : PageModel
    {

        public BoletadeValorizacionPetroperuLoteIDto? Dato { get; set; }

        private readonly IBoletadeValorizacionPetroperuLoteIServicio _boletadeValorizacionPetroperuLoteIServicio;
        public IndexModel(IBoletadeValorizacionPetroperuLoteIServicio boletadeValorizacionPetroperuLoteIServicio)
        {
            _boletadeValorizacionPetroperuLoteIServicio = boletadeValorizacionPetroperuLoteIServicio;
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
            var operacion = await _boletadeValorizacionPetroperuLoteIServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }



    }
}
