using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.ViewComponents.Reportes
{
    public class ObtenerEficienciaDiariaViewComponent: ViewComponent
    {

        private readonly IRegistroRepositorio _registroRepositorio;
        public ObtenerEficienciaDiariaViewComponent(IRegistroRepositorio registroRepositorio)
        {
            _registroRepositorio = registroRepositorio;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            double? eficiencia = default(double?);         
            var pgtVolumenEntidad = await _registroRepositorio.ObtenerValorAsync((int)TiposDatos.EficienciaProduccion, (int)TiposLote.LoteX, FechasUtilitario.ObtenerDiaOperativo(), (int)TiposNumeroRegistro.SegundoRegistro);
            if (pgtVolumenEntidad != null)
            {
                eficiencia = pgtVolumenEntidad.Valor.HasValue ? pgtVolumenEntidad.Valor.Value : null;
            }

            return View("Default", eficiencia);
        }

    }

  
}
