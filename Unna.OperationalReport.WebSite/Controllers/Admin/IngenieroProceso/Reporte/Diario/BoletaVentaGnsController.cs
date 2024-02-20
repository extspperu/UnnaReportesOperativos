using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class BoletaVentaGnsController : ControladorBaseWeb
    {

        private readonly IBoletaVentaGnsServicio _boletaVentaGnsServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BoletaVentaGnsController(
            IBoletaVentaGnsServicio boletaVentaGnsServicio,
            IWebHostEnvironment hostingEnvironment
            )
        {
            _boletaVentaGnsServicio = boletaVentaGnsServicio;
            _hostingEnvironment = hostingEnvironment;
        }



        [HttpGet("Pdf")]
        public async Task<IActionResult> Pdf()
        {
            var dto = new BoletaVentaGnsDto();
            var operacion = await _boletaVentaGnsServicio.ObtenerAsync(ObtenerIdUsuarioActual()??0);
            if (operacion.Completado)
            {
                dto = operacion.Resultado;
            }
            return new ViewAsPdf("/Pages/Admin/IngenieroProceso/Reporte/Diario/ReporteExistencias/Pdf.cshtml")
            {
                FileName = "BoletaVentaDeGasNatural.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                Model = dto
            };
        }

    }
}
