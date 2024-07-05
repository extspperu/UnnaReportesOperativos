using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Dtos;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Mantenimiento
{
    [Route("api/admin/mantenimiento/[controller]")]
    [ApiController]
    public class ValoresDefectoController : ControladorBase
    {
        private readonly IValoresDefectoReporteServicio _valoresDefectoReporteServicio;
        public ValoresDefectoController(IValoresDefectoReporteServicio valoresDefectoReporteServicio)
        {
            _valoresDefectoReporteServicio = valoresDefectoReporteServicio;
        }

        [HttpGet("Listar")]
        [RequiereAcceso()]
        public async Task<List<ValoresDefectoReporteDto>?> ListarAsync()
        {
            var operacion = await _valoresDefectoReporteServicio.ListarAsync();
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
        
        
        [HttpGet("Obtener/{id}")]
        [RequiereAcceso()]
        public async Task<ValoresDefectoReporteDto?> ObtenerAsync(string id)
        {
            var operacion = await _valoresDefectoReporteServicio.ObtenerAsync(id);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }



    }
}
