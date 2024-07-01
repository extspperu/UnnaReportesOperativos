using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.Pages.Admin.IngenieroProceso.BalanceDiario
{
    public class ValidarDatosModel : PageModel
    {

        public DatosFiscalizadorEnelDto? Datos { get; set; }
        public bool TieneVariosRegistros { get; set; }
        public int? Numero { get; set; }
        public string? IdLote { get; set; }

        private readonly IDiaOperativoServicio _diaOperativoServicio;
        public ValidarDatosModel(IDiaOperativoServicio diaOperativoServicio)
        {
            _diaOperativoServicio = diaOperativoServicio;
        }

        public async Task OnGet(string Id,int? numero)
        {
            IdLote = Id;
            int id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(Id);
            if (id == (int)TiposLote.LoteX)
            {
                TieneVariosRegistros = true;
                numero = !numero.HasValue ? (int)TiposNumeroRegistro.PrimeroRegistro: numero;
            }
            Numero = numero;
            var operacion = await _diaOperativoServicio.ObtenerValidarDatosAsync(Id, FechasUtilitario.ObtenerDiaOperativo(), numero);
            if (operacion != null && operacion.Completado)
            {
                Datos = operacion.Resultado;
            }            
        }
    }
}
