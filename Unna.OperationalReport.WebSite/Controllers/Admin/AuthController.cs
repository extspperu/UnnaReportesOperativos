using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Unna.OperationalReport.Service.Auth.Dtos;
using Unna.OperationalReport.Service.Auth.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Auth;

namespace Unna.OperationalReport.WebSite.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AuthController: ControladorAuth
    {

        private readonly UrlConfiguracionDto _urlConfiguracionDto;
        private readonly SeguridadConfiguracionDto _seguridadConfiguracionDto;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILoginServicio _loginServicio;
        public AuthController(IOptionsMonitor<CookieAuthenticationOptions> optionsMonitor,
                             UrlConfiguracionDto urlConfiguracionDto,
                             SeguridadConfiguracionDto seguridadConfiguracionDto,
                             IWebHostEnvironment hostingEnvironment,
                             ILoginServicio loginServicio
            ) : base(optionsMonitor)
        {
            _urlConfiguracionDto = urlConfiguracionDto;
            _seguridadConfiguracionDto = seguridadConfiguracionDto;
            _hostingEnvironment = hostingEnvironment;
            _loginServicio = loginServicio;
        }

        [HttpPost("LoginUser")]
        public async Task<LoginFormRespuestaDto?> LoginUserAsync([FromBody] LoginPeticionDto peticion)
        {

            VerificarIfEsBuenJson(peticion);
            var operacionModelo = ValidacionUtilitario.ValidarModelo<LoginFormRespuestaDto>(peticion);
            if (!operacionModelo.Completado)
            {
                return ObtenerResultadoOGenerarErrorDeOperacion(operacionModelo);
            }

            var formLoginService = (ILoginServicio)HttpContext.RequestServices.GetService(typeof(ILoginServicio));

            var operacion = await formLoginService.LoginAsync(new LoginPeticionDto()
            {
                Password = peticion.Password,
                Username = peticion.Username
            });

            if (!operacion.Completado)
            {
                return ObtenerResultadoOGenerarErrorDeOperacion(new OperacionDto<LoginFormRespuestaDto>(operacion.Codigo, operacion.Mensajes));
            }


            //await _usuarioIbrokerServicio.ActualizarUltimoLoginAsync(operacion.Resultado.IdUsuario);

            var username = operacion.Resultado.IdUsuario.ToString();

            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Name, username),
            };
            var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
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
                Mensaje = operacion.Resultado.Mensaje
            };

        }

    }
}
