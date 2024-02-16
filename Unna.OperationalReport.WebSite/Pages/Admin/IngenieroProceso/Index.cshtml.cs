using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso
{
    public class IndexModel : PageModel
    {

        public string? NombreMenu { get; set; }

        private readonly IMenuUrlServicio _menuUrlServicio;
        public IndexModel(IMenuUrlServicio menuUrlServicio)
        {
            _menuUrlServicio = menuUrlServicio;
        }

        public string? IdGrupo { get; set; }
        public async Task OnGet(string? Id)
        {
            IdGrupo = Id;

            var operacion = await _menuUrlServicio.ObtenerListaMenuUrl(Id);
            if (operacion != null && operacion.Completado && operacion.Resultado != null)
            {
                NombreMenu = operacion.Resultado.Nombre;
            }
        }
    }
}
