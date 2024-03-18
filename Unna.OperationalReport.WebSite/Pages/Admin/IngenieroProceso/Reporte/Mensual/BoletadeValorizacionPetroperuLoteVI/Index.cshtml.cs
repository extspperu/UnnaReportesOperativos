using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.BoletadeValorizacionPetroperuLoteVI
{
    public class IndexModel : PageModel
    {

        public BoletadeValorizacionPetroperuLoteVIDto? Dato { get; set; }

        private readonly IBoletadeValorizacionPetroperuLoteVIServicio _boletadeValorizacionPetroperuLoteVIServicio;
        public IndexModel(IBoletadeValorizacionPetroperuLoteVIServicio boletadeValorizacionPetroperuLoteVIServicio)
        {
            _boletadeValorizacionPetroperuLoteVIServicio = boletadeValorizacionPetroperuLoteVIServicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _boletadeValorizacionPetroperuLoteVIServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }



    }
}
