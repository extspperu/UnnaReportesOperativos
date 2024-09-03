using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.FiscalizadorEnel
{
    public class IndexModel : PageModel
    {
        public DatosFiscalizadorEnelDto? Datos { get; set; }

        private readonly IDiaOperativoServicio _diaOperativoServicio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public IndexModel(IUsuarioRepositorio usuarioRepositorio, IDiaOperativoServicio diaOperativoServicio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _diaOperativoServicio = diaOperativoServicio;
        }

        public async Task<IActionResult> OnGet(string? Id, string? edicion)
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
                        }
                    }
                }

            }
            var operacion = await _diaOperativoServicio.ObtenerPermisosFiscalizadorEnelAsync(idUsuario,edicion, Id);
            if (operacion == null || !operacion.Completado)
            {
                return RedirectToPage("/Admin/Error");
            }
            Datos = operacion.Resultado;
            return Page();
        }
    }
}
