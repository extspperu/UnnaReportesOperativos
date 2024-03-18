using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.BoletadeValorizacionPetroperuLoteZ69
{
    public class IndexModel : PageModel
    {

        public BoletadeValorizacionPetroperuLoteZ69Dto? Dato { get; set; }

        private readonly IBoletadeValorizacionPetroperuLoteZ69Servicio _boletadeValorizacionPetroperuLoteZ69Servicio;
        public IndexModel(IBoletadeValorizacionPetroperuLoteZ69Servicio boletadeValorizacionPetroperuLoteZ69Servicio)
        {
            _boletadeValorizacionPetroperuLoteZ69Servicio = boletadeValorizacionPetroperuLoteZ69Servicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _boletadeValorizacionPetroperuLoteZ69Servicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }



    }
}
