using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ResDiarioFiscalProd.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ResDiarioFiscalProd.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Diario.ResDiarioFiscalProd
{
    public class IndexModel : PageModel
    {
        public ResDiarioFiscalProdDto? Dato { get; set; }

        private readonly IResDiarioFiscalProdServicio _ResDiarioFiscalProdServicio;
        public IndexModel(IResDiarioFiscalProdServicio ResDiarioFiscalProdServicio)
        {
            _ResDiarioFiscalProdServicio = ResDiarioFiscalProdServicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _ResDiarioFiscalProdServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}

