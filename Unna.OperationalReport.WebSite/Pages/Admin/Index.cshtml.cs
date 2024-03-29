using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;

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
                idUsuario = Convert.ToInt64(claim.Value);
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

            return Page();
        }
    }
}
