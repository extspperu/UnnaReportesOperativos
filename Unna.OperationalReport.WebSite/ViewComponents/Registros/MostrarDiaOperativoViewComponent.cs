using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.General.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.ViewComponents.Registros
{
    public class MostrarDiaOperativoViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Delay(0);
            return View("Default", FechasUtilitario.ObtenerDiaOperativo().ToString("dd/MM/yyyy"));
        }
    }
}
