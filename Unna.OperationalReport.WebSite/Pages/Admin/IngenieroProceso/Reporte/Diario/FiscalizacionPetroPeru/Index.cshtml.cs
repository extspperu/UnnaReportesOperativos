using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Abstracciones;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.Reporte.Diario.FiscalizacionPetroPeru
{
    public class IndexModel : PageModel
    {
        public FiscalizacionPetroPeruDto? Dato { get; set; }
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IFiscalizacionPetroPeruServicio _fiscalizacionPetroPeruServicio;
        public IndexModel(IUsuarioRepositorio usuarioRepositorio, IFiscalizacionPetroPeruServicio fiscalizacionPetroPeruServicio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _fiscalizacionPetroPeruServicio = fiscalizacionPetroPeruServicio;
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
            var operacion = await _fiscalizacionPetroPeruServicio.ObtenerAsync(idUsuario);
            if (!operacion.Completado)
            {
                return RedirectToPage("/Admin/IngenieriaProceso/Reporte/Diario/Index");
            }
            Dato = operacion.Resultado;

            return Page();
        }
    }
}
