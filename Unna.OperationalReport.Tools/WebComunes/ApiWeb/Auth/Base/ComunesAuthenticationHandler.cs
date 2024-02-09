using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Seguridad.Servicios.Auth.Servicios.Abstracciones;

namespace Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Base
{
    public class ComunesAuthenticationHandler : AuthenticationHandler<ComunesAuthenticationOptions>
    {
        private readonly ITokenAccesoServicio _tokenAccesoServicio;

        public ComunesAuthenticationHandler(
            IOptionsMonitor<ComunesAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ITokenAccesoServicio tokenAccesoServicio

            ) : base(options, logger, encoder, clock)
        {
            _tokenAccesoServicio = tokenAccesoServicio;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.TokenHeaderName) || string.IsNullOrWhiteSpace(Request.Headers[Options.TokenHeaderName]))
            {
                return await Task.FromResult(AuthenticateResult.Fail($"No existe token: {Options.TokenHeaderName}"));
            }

            var token = Request.Headers[Options.TokenHeaderName];
            var operacionToken = _tokenAccesoServicio.ObtenerTokenAccesoDeCadena(token);

            if (!operacionToken.Completado)
            {
                return await Task.FromResult(AuthenticateResult.Fail($"Token Inválido: {Options.TokenHeaderName}"));
            }


            if (operacionToken.Resultado == null)
            {
                return await Task.FromResult(AuthenticateResult.Fail($"Token Inválido: {Options.TokenHeaderName}"));
            }

            var username = operacionToken.Resultado.IdUsuario.ToString();


            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Name, username),
            // add other claims/roles as you like
            };


            var id = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(id);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return await Task.FromResult(AuthenticateResult.Success(ticket));

        }
    }
}
