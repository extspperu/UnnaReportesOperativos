using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Auth.Repositorios.Implementaciones;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Dtos;
using Unna.OperationalReport.Service.Configuraciones.MenuUrls.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.ViewComponents.Configuracion
{
    public class ConfiguracionMenuUrlViewComponent : ViewComponent
    {
        public readonly IMenuUrlServicio _menuUrlServicio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public ConfiguracionMenuUrlViewComponent(IUsuarioRepositorio usuarioRepositorio,
        IMenuUrlServicio menuUrlServicio
            )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _menuUrlServicio = menuUrlServicio;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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
            lista = lista.Where(e => e.IdMenuUrlPadre == null).ToList();
            return View(lista);
        }
    }
}
