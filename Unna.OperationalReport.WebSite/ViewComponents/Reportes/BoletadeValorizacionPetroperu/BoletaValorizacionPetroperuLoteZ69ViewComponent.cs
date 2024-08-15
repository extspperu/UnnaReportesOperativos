using Microsoft.AspNetCore.Mvc;

namespace Unna.OperationalReport.WebSite.ViewComponents.Reportes.BoletadeValorizacionPetroperu
{
    public class BoletaValorizacionPetroperuLoteZ69ViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Delay(0);
            return View("Default");
        }
    }
}
