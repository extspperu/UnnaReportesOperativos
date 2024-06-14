using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Cartas.ResumenVentaCliente.Dtos;
using Unna.OperationalReport.Service.Cartas.ResumenVentaCliente.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Cartas
{
    [Route("api/cartas/[controller]")]
    [ApiController]
    public class ResumenVentaClienteController : ControladorBase
    {
        private readonly IResumenVentaClienteServicio _resumenVentaClienteServicio;
        public ResumenVentaClienteController(IResumenVentaClienteServicio resumenVentaClienteServicio)
        {
            _resumenVentaClienteServicio = resumenVentaClienteServicio;
        }

        [HttpPost("ProcesarArchivo")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> ProcesarArchivoAsync([FromBody] ProcesarArchivoCartaDto peticion)
        {
            peticion.IdUsuario = ObtenerIdUsuarioActual();
            var operacion = await _resumenVentaClienteServicio.ProcesarArchivoAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
