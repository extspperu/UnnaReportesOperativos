using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaValorizacionProcesamientoGNALoteIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaValorizacionProcesamientoGNALoteIV.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.BoletaValorizacionProcesamientoGNALoteIV
{
    public class IndexModel : PageModel
    {

        public BoletaValorizacionProcesamientoGNALoteIVDto? Dato { get; set; }

        private readonly IBoletaValorizacionProcesamientoGNALoteIVServicio _boletaValorizacionProcesamientoGNALoteIVServicio;
        public IndexModel(IBoletaValorizacionProcesamientoGNALoteIVServicio boletaValorizacionProcesamientoGNALoteIVServicio)
        {
            _boletaValorizacionProcesamientoGNALoteIVServicio = boletaValorizacionProcesamientoGNALoteIVServicio;
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
            var operacion = await _boletaValorizacionProcesamientoGNALoteIVServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }



    }
}
