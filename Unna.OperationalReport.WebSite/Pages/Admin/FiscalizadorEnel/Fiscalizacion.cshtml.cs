using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Service.Registros.Datos.Dtos;
using Unna.OperationalReport.Service.Registros.Datos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.Pages.Admin.FiscalizadorEnel
{
    public class FiscalizacionModel : PageModel
    {
        public List<DiaOperativoDto>? DiaOperativo { get; set; }
        public List<DatoDto>? Datos { get; set; }
        public string? Fecha { get; set; }

        private readonly IDiaOperativoServicio _diaOperativoServicio;
        private readonly IDatoServicio _datoServicio;
        public FiscalizacionModel(
            IDiaOperativoServicio diaOperativoServicio,
            IDatoServicio datoServicio
            )
        {
            _diaOperativoServicio = diaOperativoServicio;
            _datoServicio = datoServicio;
        }

        public async Task<IActionResult> OnGet()
        {
            var operacion = await _diaOperativoServicio.ListarRegistrosFiscalizadorRegularAsync();
            if (operacion == null || !operacion.Completado)
            {
                return RedirectToPage("/Admin/Error");
            }
            DiaOperativo = operacion.Resultado;

            var operacionDato = await _datoServicio.ListarPorTipoAsync(TiposFiscalizadores.Regular);
            if (operacionDato != null || operacionDato.Resultado != null)
            {
                Datos = operacionDato.Completado ? operacionDato.Resultado : new List<DatoDto>();
            }
            Fecha = FechasUtilitario.ObtenerDiaOperativo().ToString("dd/MM/yyyy");
            return Page();
        }
    }
}
