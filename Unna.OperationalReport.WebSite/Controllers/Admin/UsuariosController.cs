using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Unna.OperationalReport.Service.Auth.Dtos;
using Unna.OperationalReport.Service.Auth.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Auth;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UsuariosController : ControladorAuth
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public UsuariosController(IUsuarioRepositorio usuarioRepositorio, IOptionsMonitor<CookieAuthenticationOptions> optionsMonitor)
            : base(optionsMonitor)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [AllowAnonymous]
        [HttpGet("LoginExterno")]
        public IActionResult LoginExterno(string proveedor = "Microsoft", string? urlRetorno = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                var claims = User.Claims;
                foreach (var claim in claims)
                {
                    Console.WriteLine($"Tipo: {claim.Type}, Valor: {claim.Value}");
                }

                var urlRedireccion = "/";
                var propiedades = new AuthenticationProperties { RedirectUri = urlRedireccion };
                return new ChallengeResult(proveedor, propiedades);
            }

            var authenticatedClaims = User.Claims;
            foreach (var claim in authenticatedClaims)
            {
                Console.WriteLine($"Tipo: {claim.Type}, Valor: {claim.Value}");
            }

            return RedirectToAction(nameof(ProcesarLoginExterno), new { urlRetorno });
        }


        [AllowAnonymous]
        [HttpGet("ProcesarLoginExterno")]
        public async Task<LoginFormRespuestaDto?> ProcesarLoginExterno(string? urlRetorno = null)
        {
            var info = await HttpContext.AuthenticateAsync();
            if (info.Principal == null)
            {
                return new LoginFormRespuestaDto
                {
                    Suceso = false,
                    Mensaje = "Error loading external login information."
                };
            }

            var claims = info.Principal.Claims.ToList();
            string email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "";

            if (string.IsNullOrEmpty(email))
            {
                return new LoginFormRespuestaDto
                {
                    Suceso = false,
                    Mensaje = "Error reading the email from the external provider."
                };
            }


            var formLoginService = (ILoginServicio)HttpContext.RequestServices.GetService(typeof(ILoginServicio));

            var operacion = await formLoginService.LoginAsync(new LoginPeticionDto()
            {
                Username = email,
                Password = "" 
            });

            var resultado = await _usuarioRepositorio.VerificarUsuarioAsync(email);


            var username = email;

            var customClaims = new[] {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Sid, resultado.IdUsuario.ToString())
            };
            var id = new ClaimsIdentity(customClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(id);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties()
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });

            return new LoginFormRespuestaDto()
            {
                Suceso = true,
                Mensaje = "operacion.Resultado.Mensaje"
            };
        }



    }
}
