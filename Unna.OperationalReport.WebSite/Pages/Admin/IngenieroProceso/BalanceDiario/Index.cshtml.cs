using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;
using Unna.OperationalReport.Service.Registros.Lotes.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.BalanceDiario
{
    public class IndexModel : PageModel
    {

        public List<LoteDto>? Lotes { get;set; }


        private readonly ILoteServicio _loteServicio;
        public IndexModel(ILoteServicio loteServicio)
        {
            _loteServicio = loteServicio;
        }

        public async Task OnGet()
        {
            var operacion = await _loteServicio.ListarTodosAsync();
            if (operacion.Completado)
            {
                Lotes = operacion.Resultado;
            }
        }
    }
}
