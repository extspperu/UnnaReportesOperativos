using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;

namespace Unna.OperationalReport.WebSite.Pages.Admin.FiscalizadorEnel
{
    public class IndexModel : PageModel
    {
        public DatosFiscalizadorEnelDto? Datos { get; set; }

        private readonly IDiaOperativoServicio _diaOperativoServicio;
        public IndexModel(IDiaOperativoServicio diaOperativoServicio)
        {
            _diaOperativoServicio = diaOperativoServicio;
        }

        public async Task<IActionResult> OnGet(string? Id, string? edicion)
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _diaOperativoServicio.ObtenerPermisosFiscalizadorEnelAsync(idUsuario,edicion, Id);
            if (operacion == null || !operacion.Completado)
            {
                return RedirectToPage("/Admin/Error");
            }
            Datos = operacion.Resultado;
            return Page();
        }
    }
}
