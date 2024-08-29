using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.BoletaVolumenesUNNAEnergiaCNPC
{
    public class IndexModel : PageModel
    {

        public BoletaVolumenesUNNAEnergiaCNPCDto? Dato { get; set; }

        private readonly IBoletaVolumenesUNNAEnergiaCNPCServicio _boletaVolumenesUNNAEnergiaCNPCServicio;
        public IndexModel(IBoletaVolumenesUNNAEnergiaCNPCServicio boletaVolumenesUNNAEnergiaCNPCServicio)
        {
            _boletaVolumenesUNNAEnergiaCNPCServicio = boletaVolumenesUNNAEnergiaCNPCServicio;
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
            var operacion = await _boletaVolumenesUNNAEnergiaCNPCServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }



    }
}
