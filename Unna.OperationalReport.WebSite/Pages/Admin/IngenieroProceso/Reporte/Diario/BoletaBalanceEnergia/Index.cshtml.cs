using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Diario.BoletaBalanceEnergia
{
    public class IndexModel : PageModel
    {
        public BoletaBalanceEnergiaDto? Dato { get; set; }
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IBoletaBalanceEnergiaServicio _BoletaBalanceEnergiaServicio;
        public IndexModel(IUsuarioRepositorio usuarioRepositorio, IBoletaBalanceEnergiaServicio BoletaBalanceEnergiaServicio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _BoletaBalanceEnergiaServicio = BoletaBalanceEnergiaServicio;
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
                        }
                    }
                }
            }
            var operacion = await _BoletaBalanceEnergiaServicio.ObtenerAsync(idUsuario, FechasUtilitario.ObtenerDiaOperativo());
            if (!operacion.Completado)
            {
                return RedirectToPage("/Admin/IngenieriaProceso/Reporte/Diario/Index");
            }
            Dato = operacion.Resultado;

            return Page();
        }
    }
}
