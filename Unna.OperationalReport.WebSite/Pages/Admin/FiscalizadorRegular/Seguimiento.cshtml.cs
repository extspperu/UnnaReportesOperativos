using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.Pages.Admin.FiscalizadorRegular
{
    public class SeguimientoModel : PageModel
    {

        private readonly IDiaOperativoServicio _diaOperativoServicio;
        public SeguimientoModel(IDiaOperativoServicio diaOperativoServicio)
        {
            _diaOperativoServicio = diaOperativoServicio;
        }

        public DiaOperativoDto? DiaOperativo { get; set; }

        public async Task OnGet()
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }
            var operacion = await _diaOperativoServicio.ObtenerPorIdUsuarioYFechaAsync(idUsuario, FechasUtilitario.ObtenerDiaOperativo(), (int)TipoGrupos.FiscalizadorRegular, null);
            
            if (operacion != null || operacion.Resultado != null || operacion.Completado)
            {
                DiaOperativo = operacion.Resultado;
            }
            



        }
    }
}
