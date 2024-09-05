using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Auth.Repositorios.Implementaciones;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Dtos;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.ViewComponents.Configuracion
{
    public class ConfiguracionMenuUrlPorIdPadreViewComponent : ViewComponent
    {
        public readonly IMenuUrlServicio _menuUrlServicio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public ConfiguracionMenuUrlPorIdPadreViewComponent(IUsuarioRepositorio usuarioRepositorio,
        IMenuUrlServicio menuUrlServicio
            )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _menuUrlServicio = menuUrlServicio;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? Id)
        {
            var lista = new List<MenuUrlAdminDto>();
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                if (!long.TryParse(claim.Value, out idUsuario) && claim?.Subject?.Claims != null)
                {
                    var emailClaim = claim.Subject.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                    if (emailClaim != null)
                    {
                        string email = emailClaim.Value;

                        var resultado = await _usuarioRepositorio.VerificarUsuarioAsync(email);

                        if (resultado.Existe)
                        {
                            idUsuario = resultado.IdUsuario ?? 0;
                            if (idUsuario > 0)
                            {
                                var claimsIdentity = (ClaimsIdentity)User.Identity;

                                var existingClaim = claimsIdentity.FindFirst("IdUsuario");

                                if (existingClaim == null)
                                {
                                    claimsIdentity.AddClaim(new Claim("IdUsuario", idUsuario.ToString()));

                                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                                }
                            }
                        }
                    }
                }
            }
            var operacion = await _menuUrlServicio.ObtenerListaMenuUrl(idUsuario);

            if (operacion.Completado)
            {
                lista = operacion.Resultado;
            }
            //string aaa = RijndaelUtilitario.EncryptRijndaelToUrl(16);
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
