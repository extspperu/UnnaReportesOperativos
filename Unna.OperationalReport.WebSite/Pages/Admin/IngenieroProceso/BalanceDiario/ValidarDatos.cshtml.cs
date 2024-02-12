using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.BalanceDiario
{
    public class ValidarDatosModel : PageModel
    {

        public DatosFiscalizadorEnelDto? Datos { get; set; }

        private readonly IDiaOperativoServicio _diaOperativoServicio;
        public ValidarDatosModel(IDiaOperativoServicio diaOperativoServicio)
        {
            _diaOperativoServicio = diaOperativoServicio;
        }

        public async Task OnGet(string Id)
        {
            var fecha = FechasUtilitario.ObtenerDiaOperativo();            
            var operacion = await _diaOperativoServicio.ObtenerValidarDatosAsync(Id, fecha);
            if (operacion != null && operacion.Completado)
            {
                Datos = operacion.Resultado;
            }            
        }
    }
}
