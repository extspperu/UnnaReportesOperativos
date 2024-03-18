using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.BoletadeValorizacionPetroperu
{
    public class IndexModel : PageModel
    {

        public BoletadeValorizacionPetroperuDto? Dato { get; set; }

        private readonly IBoletadeValorizacionPetroperuServicio _boletadeValorizacionPetroperuServicio;
        public IndexModel(IBoletadeValorizacionPetroperuServicio boletadeValorizacionPetroperuServicio)
        {
            _boletadeValorizacionPetroperuServicio = boletadeValorizacionPetroperuServicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _boletadeValorizacionPetroperuServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }



    }
}
