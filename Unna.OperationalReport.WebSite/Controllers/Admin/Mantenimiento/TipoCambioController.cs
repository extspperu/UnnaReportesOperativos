using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Dtos;
using Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Mantenimiento
{
    [Route("api/mantenimiento/[controller]")]
    [ApiController]
    public class TipoCambioController : ControladorBase
    {
        private readonly ITipoCambioServicio _tipoCambioServicio;
        public TipoCambioController(ITipoCambioServicio tipoCambioServicio)
        {
            _tipoCambioServicio = tipoCambioServicio;
        }

        [HttpGet("ListarDelMes")]
        [RequiereAcceso()]
        public async Task<List<TipoCambioDto>?> ListarDelMesAync()
        {
            var operacion = await _tipoCambioServicio.ListarDelMesAync(FechasUtilitario.ObtenerDiaOperativo(), (int)TiposMonedas.Soles);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("ProcesarArchivo")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>> ProcesarArchivoAsync(IFormFile file)
        {
            var operacion = await _tipoCambioServicio.ProcesarArchivoAsync(file,(int)TiposMonedas.Soles);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
