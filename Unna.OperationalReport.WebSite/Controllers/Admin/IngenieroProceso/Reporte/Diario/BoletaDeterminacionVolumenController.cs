using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;
using Aspose.Cells;
using ClosedXML.Report;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class BoletaDeterminacionVolumenController : ControladorBaseWeb
    {
        private readonly IBoletaDeterminacionVolumenGnaServicio _boletaDeterminacionVolumenGnaServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public BoletaDeterminacionVolumenController(
        IBoletaDeterminacionVolumenGnaServicio boletaDeterminacionVolumenGnaServicio,
        IWebHostEnvironment hostingEnvironment,
        GeneralDto general
        )
        {
            _boletaDeterminacionVolumenGnaServicio = boletaDeterminacionVolumenGnaServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }
        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<BoletaDeterminacionVolumenGnaDto?> ObtenerAsync()
        {
            var operacion = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
