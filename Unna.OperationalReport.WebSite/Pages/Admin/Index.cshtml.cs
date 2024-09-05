using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Auth.Entidades;

namespace Unna.OperationalReport.WebSite.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IGrupoRepositorio _grupoRepositorio;

        public IndexModel(
            IUsuarioRepositorio usuarioRepositorio,
            IGrupoRepositorio grupoRepositorio
            )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _grupoRepositorio = grupoRepositorio;
        }

        public async Task<IActionResult> OnGet()
        {
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

            var usuario = await _usuarioRepositorio.BuscarPorIdYNoBorradoAsync(idUsuario);
            if (usuario != null && usuario.IdGrupo.HasValue)
            {
                var grupo = await _grupoRepositorio.BuscarPorIdGrupoAsync(usuario.IdGrupo??0);
                if (grupo != null && !string.IsNullOrWhiteSpace(grupo.UrlDefecto))
                {
                    return RedirectToPage(grupo.UrlDefecto);
                }
            }
            else
            {
                return RedirectToPage("/Admin/Login");
            }

            return Page();
        }
    }
}
