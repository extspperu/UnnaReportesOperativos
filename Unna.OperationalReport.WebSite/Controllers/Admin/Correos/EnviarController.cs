using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Correos.Dtos;
using Unna.OperationalReport.Service.Correos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Correos
{
    [Route("api/admin/correos/[controller]")]
    [ApiController]
    public class EnviarController : ControladorBase
    {

        private readonly IEnviarCorreoServicio _enviarCorreoServicio;
        public EnviarController(IEnviarCorreoServicio enviarCorreoServicio)
        {
            _enviarCorreoServicio = enviarCorreoServicio;
        }

        [HttpGet("Obtener/{idReporte}/{diaOperativo}")]
        [RequiereAcceso()]
        public async Task<ConsultaEnvioReporteDto?> ObtenerAsync(string idReporte, DateTime diaOperativo)
        {
            var operacion = await _enviarCorreoServicio.ObtenerAsync(idReporte, diaOperativo);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("EnviarCorreo")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> EnviarCorreoAsync(EnviarCorreoDto peticion)
        {
            peticion.IdUsuario = ObtenerIdUsuarioActual();
            var operacion = await _enviarCorreoServicio.EnviarCorreoAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("ListarCorreosEnviados")]
        [RequiereAcceso()]
        public async Task<List<ListarCorreosEnviadosDto>?> ListarCorreosEnviadoAsync(BuscarCorreosEnviadosDto peticion)
        {
            var operacion = await _enviarCorreoServicio.ListarCorreosEnviadoAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpGet("DescargarDocumento/{tipo}/{IdReporte}")]
        [RequiereAcceso()]
        public async Task<IActionResult> DescargarDocumentoAsync(string tipo, string IdReporte)
        {
            var operacion = await _enviarCorreoServicio.DescargarDocumentoAsync(tipo, IdReporte);
            if (!operacion.Completado || operacion.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }
            return File(operacion?.Resultado?.Contenido, operacion.Resultado?.TipoMime, operacion.Resultado.Nombre);
        }


    }
}
