using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.BalanceDiario.SupervisorPgt
{
    public class IndexModel : PageModel
    {

        public CargaSupervisorPgtDto? Dato { get; set; }
        private readonly ICargaSupervisorPgtServicio _cargaSupervisorPgtServicio;
        public IndexModel(ICargaSupervisorPgtServicio cargaSupervisorPgtServicio)
        {
            _cargaSupervisorPgtServicio = cargaSupervisorPgtServicio;
        }

        public async Task OnGet()
        {
            var operacion = await _cargaSupervisorPgtServicio.ObtenerAsync();
            if (operacion.Completado)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}
