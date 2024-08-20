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

namespace Unna.OperationalReport.WebSite.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UsuariosController : ControladorAuth
    {
        public UsuariosController(IOptionsMonitor<CookieAuthenticationOptions> optionsMonitor)
            : base(optionsMonitor)
        {
        }

        [AllowAnonymous]
        [HttpGet("LoginExterno")]
        public IActionResult LoginExterno(string proveedor = "Microsoft", string? urlRetorno = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Inicia el challenge para autenticación con Microsoft
                var urlRedireccion = "/";
                var propiedades = new AuthenticationProperties { RedirectUri = urlRedireccion };
                return new ChallengeResult(proveedor, propiedades); // Retorna el challenge para que se inicie la autenticación
            }

            // Si ya está autenticado, el flujo continúa aquí (no debería estar null si el challenge se completó)
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

            // Aquí podrías llamar a algún servicio que valide o cree un usuario en tu sistema
            // por ejemplo, usando `ILoginServicio` como en `LoginUserAsync`
            var formLoginService = (ILoginServicio)HttpContext.RequestServices.GetService(typeof(ILoginServicio));

            var operacion = await formLoginService.LoginAsync(new LoginPeticionDto()
            {
                Username = email,
                Password = "" // O algún valor placeholder, dependiendo de cómo manejes la autenticación externa
            });

            //if (!operacion.Completado)
            //{
            //    return ObtenerResultadoOGenerarErrorDeOperacion(new OperacionDto<LoginFormRespuestaDto>(operacion.Codigo, operacion.Mensajes));
            //}

            var username = email;

            var customClaims = new[] {
        new Claim(ClaimTypes.NameIdentifier, username),
        new Claim(ClaimTypes.Name, username),
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
