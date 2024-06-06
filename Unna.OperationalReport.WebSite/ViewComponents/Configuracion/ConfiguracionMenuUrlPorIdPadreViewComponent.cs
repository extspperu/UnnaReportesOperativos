using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Dtos;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.ViewComponents.Configuracion
{
    public class ConfiguracionMenuUrlPorIdPadreViewComponent : ViewComponent
    {
        public readonly IMenuUrlServicio _menuUrlServicio;
        public ConfiguracionMenuUrlPorIdPadreViewComponent(
            IMenuUrlServicio menuUrlServicio
            )
        {
            _menuUrlServicio = menuUrlServicio;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? Id)
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
            string aaa = RijndaelUtilitario.EncryptRijndaelToUrl(15);
            int IdMenuUrlPadre = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(Id);
            var menu = lista.Where(e => e.IdMenuUrl == IdMenuUrlPadre).FirstOrDefault();
            var dto = new ConfiguracionMenuUrlPorIdPadreViewComponentDto
            {
                Menus = lista.Where(e=>e.IdMenuUrlPadre == IdMenuUrlPadre).ToList(),
                Menu = menu
            };
            return View(dto);
        }

        public class ConfiguracionMenuUrlPorIdPadreViewComponentDto
        {
            public MenuUrlAdminDto? Menu { get; set; }
            public List<MenuUrlAdminDto>? Menus { get; set; }
        }
    }
}
