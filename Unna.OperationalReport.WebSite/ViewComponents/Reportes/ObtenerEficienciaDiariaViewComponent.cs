using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.ViewComponents.Reportes
{
    public class ObtenerEficienciaDiariaViewComponent: ViewComponent
    {

        private readonly IFiscalizacionProductoProduccionRepositorio _fiscalizacionProductoProduccionRepositorio;
        public ObtenerEficienciaDiariaViewComponent(IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio)
        {
            _fiscalizacionProductoProduccionRepositorio = fiscalizacionProductoProduccionRepositorio;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            double? eficiencia = await _fiscalizacionProductoProduccionRepositorio.ObtenerEficienciaPlantaAsync(FechasUtilitario.ObtenerDiaOperativo());
            
            return View("Default", eficiencia);
        }

    }

  
}
