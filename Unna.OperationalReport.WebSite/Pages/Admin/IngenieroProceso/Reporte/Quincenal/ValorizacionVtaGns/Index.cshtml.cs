using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Auth.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Quincenal.ValorizacionVtaGns
{
    public class IndexModel : PageModel
    {
        public ValorizacionVtaGnsDto? Dato { get; set; }
        public string? Grupo { get; set; }
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IValorizacionVtaGnsServicio _ValorizacionVtaGnsServicio;
        private readonly IConfiguration _configuration;

        public IndexModel(IUsuarioRepositorio usuarioRepositorio, IValorizacionVtaGnsServicio valorizacionVtaGnsServicio, IConfiguration configuration)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _ValorizacionVtaGnsServicio = valorizacionVtaGnsServicio;
            _configuration = configuration;
        }

        public async Task OnGet(string? Id)
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
            var operacion = await _ValorizacionVtaGnsServicio.ObtenerAsync(idUsuario, Id);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
            Grupo = Id;
        }
    }
}
