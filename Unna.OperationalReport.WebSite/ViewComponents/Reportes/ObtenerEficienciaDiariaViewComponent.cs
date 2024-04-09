using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.ViewComponents.Reportes
{
    public class ObtenerEficienciaDiariaViewComponent: ViewComponent
    {

        private readonly IBoletaEnelRepositorio _boletaEnelRepositorio;
        public ObtenerEficienciaDiariaViewComponent(IBoletaEnelRepositorio boletaEnelRepositorio)
        {
            _boletaEnelRepositorio = boletaEnelRepositorio;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string? eficiencia = default(string?);
            var pgtVolumenEntidad = await _boletaEnelRepositorio.ObtenerPgtVolumen(FechasUtilitario.ObtenerDiaOperativo());
            if (pgtVolumenEntidad != null)
            {
                eficiencia = pgtVolumenEntidad?.Eficiencia?.ToString();
            }
            return View("Default", eficiencia);
        }

    }

  
}
