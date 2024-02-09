using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Service.Registros.Datos.Dtos;
using Unna.OperationalReport.Service.Registros.Datos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.Pages.Admin.FiscalizadorRegular
{
    public class IndexModel : PageModel
    {
        public List<DatoDto>? Datos { get; set; }
        public string? IdGrupo { get; set; }
        public bool PermitirEditar { get; set; }
        public string? Titulo { get; set; }

        private readonly IDatoServicio _datoServicio;
        private readonly IDiaOperativoServicio _diaOperativoServicio;
        public IndexModel(
            IDatoServicio datoServicio,
            IDiaOperativoServicio diaOperativoServicio
            )
        {
            _datoServicio = datoServicio;
            _diaOperativoServicio = diaOperativoServicio;
        }

        public async Task<IActionResult> OnGet(string Id)
        {
            var claim = HttpContext.User.Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            long idUsuario = 0;
            if (claim != null)
            {
                idUsuario = Convert.ToInt64(claim.Value);
            }

            var operacion = await _datoServicio.ListarPorTipoAsync(TiposFiscalizadores.Regular);
            if (operacion != null || operacion.Resultado != null)
            {
                Datos = operacion.Completado ? operacion.Resultado : new List<DatoDto>();
            }
            IdGrupo = RijndaelUtilitario.EncryptRijndaelToUrl((int)TipoGrupos.FiscalizadorRegular);


            bool permitirEditar = true;
            switch (Id)
            {
                case TiposAcciones.Registro:
                    Titulo = $"REGISTRO DE DATOS";
                    var operacionExisteRegistro = await _diaOperativoServicio.ObtenerPorIdUsuarioYFechaAsync(idUsuario, FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1)), (int)TipoGrupos.FiscalizadorRegular, null);
                    if (operacionExisteRegistro.Completado)
                    {
                        permitirEditar = false;
                    }

                    break;
                case TiposAcciones.Editar:
                    Titulo = $"EDICIÓN DE DATOS";
                    permitirEditar = await _diaOperativoServicio.ExisteParaEdicionDatosAsync(idUsuario, (int)TipoGrupos.FiscalizadorRegular, null);
                    break;
                default:
                    return RedirectToPage("/Admin/Error");
            }
            PermitirEditar = permitirEditar;
            return Page();
            
        }
    }
}
