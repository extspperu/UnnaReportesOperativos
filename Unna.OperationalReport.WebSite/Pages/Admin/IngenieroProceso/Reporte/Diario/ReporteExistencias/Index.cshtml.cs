using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Servicios.Abstracciones;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Diario.ReporteExistencias
{
    public class IndexModel : PageModel
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IReporteExistenciaServicio _reporteExistenciaServicio;
        public IndexModel(IUsuarioRepositorio usuarioRepositorio, IReporteExistenciaServicio reporteExistenciaServicio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _reporteExistenciaServicio = reporteExistenciaServicio;
        }
        public ReporteExistenciaDto? Data { get; set; }
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
                        }
                    }
                }
            }
            var operacion = await _reporteExistenciaServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                Data = operacion.Resultado;
            }

        }
    }
}
