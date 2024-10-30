using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Servicios.Abstracciones;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.BoletaProcesamientoGnaLoteIv
{
    public class IndexModel : PageModel
    {

        public BoletaProcesamientoGnaLoteIvDto? Dato { get; set; }
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IBoletaProcesamientoGnaLoteIvServicio _boletaProcesamientoGnaLoteIvServicio;
        public IndexModel(IUsuarioRepositorio usuarioRepositorio, IBoletaProcesamientoGnaLoteIvServicio boletaProcesamientoGnaLoteIvServicio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _boletaProcesamientoGnaLoteIvServicio = boletaProcesamientoGnaLoteIvServicio;
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
            var operacion = await _boletaProcesamientoGnaLoteIvServicio.ObtenerAsync(idUsuario);
            if (!operacion.Completado)
            {
                return RedirectToPage("/Admin/IngenieroProceso/Reporte/Mensual/Index");
            }

            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
            return Page();
        }

    }
}
