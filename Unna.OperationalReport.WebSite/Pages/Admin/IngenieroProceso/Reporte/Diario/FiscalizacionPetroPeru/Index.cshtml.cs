using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Diario.FiscalizacionPetroPeru
{
    public class IndexModel : PageModel
    {
        public FiscalizacionPetroPeruDto? Dato { get; set; }

        private readonly IFiscalizacionPetroPeruServicio _fiscalizacionPetroPeruServicio;
        public IndexModel(IFiscalizacionPetroPeruServicio fiscalizacionPetroPeruServicio)
        {
            _fiscalizacionPetroPeruServicio = fiscalizacionPetroPeruServicio;
        }

        public async void OnGet()
        {

            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _fiscalizacionPetroPeruServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}
