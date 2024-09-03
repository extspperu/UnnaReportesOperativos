using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Auth.Repositorios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Mensual.BoletaVolumenesUNNAEnergiaCNPC
{
    public class IndexModel : PageModel
    {

        public BoletaVolumenesUNNAEnergiaCNPCDto? Dato { get; set; }
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        private readonly IBoletaVolumenesUNNAEnergiaCNPCServicio _boletaVolumenesUNNAEnergiaCNPCServicio;
        public IndexModel(IUsuarioRepositorio usuarioRepositorio, IBoletaVolumenesUNNAEnergiaCNPCServicio boletaVolumenesUNNAEnergiaCNPCServicio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _boletaVolumenesUNNAEnergiaCNPCServicio = boletaVolumenesUNNAEnergiaCNPCServicio;
        }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
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
            }
            var operacion = await _boletaVolumenesUNNAEnergiaCNPCServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Dato = operacion.Resultado;
            }
        }



    }
}
