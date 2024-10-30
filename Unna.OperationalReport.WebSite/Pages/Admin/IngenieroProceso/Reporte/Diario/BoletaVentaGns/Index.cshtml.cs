using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Diario.BoletaVentaGns
{
    public class IndexModel : PageModel
    {
        public BoletaVentaGnsDto? Dato { get; set; }
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        private readonly IBoletaVentaGnsServicio _boletaVentaGnsServicio;
        public IndexModel(IUsuarioRepositorio usuarioRepositorio, IBoletaVentaGnsServicio boletaVentaGnsServicio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _boletaVentaGnsServicio = boletaVentaGnsServicio;
        }
                

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
            var operacion = await _boletaVentaGnsServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }
    }
}
