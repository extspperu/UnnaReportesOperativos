using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Service.Auth.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Auth;

namespace Unna.OperationalReport.WebSite.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UsuariosController : Controller
    {
        //private readonly UrlConfiguracionDto _urlConfiguracionDto;
        //private readonly SeguridadConfiguracionDto _seguridadConfiguracionDto;
        //private readonly IWebHostEnvironment _hostingEnvironment;
        //private readonly ILoginServicio _loginServicio;
        //private readonly SignInManager<IdentityUser> signInManager;
        //private readonly UserManager<IdentityUser> userManager;
        //public LoginMailController(IOptionsMonitor<CookieAuthenticationOptions> optionsMonitor,
        //                     UrlConfiguracionDto urlConfiguracionDto,
        //                     SeguridadConfiguracionDto seguridadConfiguracionDto,
        //                     IWebHostEnvironment hostingEnvironment,
        //                     ILoginServicio loginServicio,
        //                     SignInManager<IdentityUser> signInManager,
        //                     UserManager<IdentityUser> userManager
        //    ) : base(optionsMonitor)
        //{
        //    _urlConfiguracionDto = urlConfiguracionDto;
        //    _seguridadConfiguracionDto = seguridadConfiguracionDto;
        //    _hostingEnvironment = hostingEnvironment;
        //    _loginServicio = loginServicio;
        //    this.signInManager = signInManager;
        //    this.userManager = userManager;
        //}

        private readonly SignInManager<IdentityUser> signInManager;
        //private readonly UserManager<IdentityUser> userManager;

        public UsuariosController(
            SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }



        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult LoginExterno(string proveedor, string? urlRetorno = null)
        {
            proveedor = "Microsoft";
            urlRetorno = "/Admin/Index";
            var urlRedireccion = Url.Action("RegistrarUsuarioExterno", values: new { urlRetorno });
            var propiedades = signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);
            return new ChallengeResult(proveedor, propiedades);
        }

        //[AllowAnonymous]
        public async Task<IActionResult> RegistrarUsuarioExterno(string? urlRetorno = null,
        string? remoteError = null)
        {
            urlRetorno = urlRetorno ?? Url.Content("~/");
            var mensaje = "";

            if (remoteError != null)
            {
                mensaje = $"Error from external provider: {remoteError}";
                return RedirectToAction("login", routeValues: new { mensaje });
                //return RedirectToPage(grupo.UrlDefecto);
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                mensaje = "Error loading external login information.";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var resultadoLoginExterno = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            // Ya la cuenta existe
            if (resultadoLoginExterno.Succeeded)
            {
                return LocalRedirect(urlRetorno);
            }

            string email = "";

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            }
            else
            {
                mensaje = "Error leyendo el email del usuario del proveedor.";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var usuario = new IdentityUser() { Email = email, UserName = email };

            //var resultadoCrearUsuario = await userManager.CreateAsync(usuario);
            //if (!resultadoCrearUsuario.Succeeded)
            //{
            //    mensaje = resultadoCrearUsuario.Errors.First().Description;
            //    return RedirectToAction("login", routeValues: new { mensaje });
            //}

            //var resultadoAgregarLogin = await userManager.AddLoginAsync(usuario, info);

            //if (resultadoAgregarLogin.Succeeded)
            //{
            //    await signInManager.SignInAsync(usuario, isPersistent: false, info.LoginProvider);
            //    return LocalRedirect(urlRetorno);
            //}

            mensaje = "Ha ocurrido un error agregando el login.";
            return RedirectToAction("login", routeValues: new { mensaje });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }


    }
}
