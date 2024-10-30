using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Unna.OperationalReport.WebSite.Pages.Admin.FiscalizadorRegular
{
    public class SeguimientoModel : PageModel
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IDiaOperativoServicio _diaOperativoServicio;
        public SeguimientoModel(IUsuarioRepositorio usuarioRepositorio, IDiaOperativoServicio diaOperativoServicio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _diaOperativoServicio = diaOperativoServicio;
        }

        public DiaOperativoDto? DiaOperativo { get; set; }

        public async Task OnGet()
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

            var operacion = await _diaOperativoServicio.ObtenerPorIdUsuarioYFechaAsync(idUsuario, FechasUtilitario.ObtenerDiaOperativo(), (int)TipoGrupos.FiscalizadorRegular, null);

            if (operacion != null || operacion.Resultado != null || operacion.Completado)
            {
                DiaOperativo = operacion.Resultado;
            }
        }

    }
}
