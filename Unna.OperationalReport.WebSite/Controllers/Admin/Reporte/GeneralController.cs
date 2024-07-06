using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Reporte
{
    [Route("api/admin/reporte/[controller]")]
    [ApiController]
    public class GeneralController : ControladorBase
    {

        private readonly IReporteServicio _reporteServicio;
        public GeneralController(IReporteServicio reporteServicio)
        {
            _reporteServicio = reporteServicio;
        }

        [HttpGet("Listar")]
        [RequiereAcceso()]
        public async Task<List<ReporteDto>?> ListarAsync()
        {            
            var operacion = await _reporteServicio.ListarAsync();
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
        
        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(ReporteDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            var operacion = await _reporteServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("Obtener/{idReporte}")]
        [RequiereAcceso()]
        public async Task<ReporteDto?> ObtenerAsync(string? idReporte)
        {
            var operacion = await _reporteServicio.ObtenerAsync(idReporte);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
