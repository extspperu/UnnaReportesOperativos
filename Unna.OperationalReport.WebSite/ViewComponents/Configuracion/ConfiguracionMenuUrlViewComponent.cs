using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Dtos;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.ViewComponents.Configuracion
{
    public class ConfiguracionMenuUrlViewComponent : ViewComponent
    {
        public readonly IMenuUrlServicio _menuUrlServicio;
        public ConfiguracionMenuUrlViewComponent(
            IMenuUrlServicio menuUrlServicio
            )
        {
            _menuUrlServicio = menuUrlServicio;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lista = new List<MenuUrlAdminDto>();
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _menuUrlServicio.ObtenerListaMenuUrl(idUsuario);

            if (operacion.Completado)
            {
                lista = operacion.Resultado;
            }
            lista = lista.Where(e => e.IdMenuUrlPadre == null).ToList();
            return View(lista);
        }
    }
}
