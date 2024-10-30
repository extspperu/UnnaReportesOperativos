
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Correos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Correos
{
    [Route("api/admin/correos/[controller]")]
    [ApiController]
    public class AdjuntoController : ControladorBase
    {

        private readonly IEnviarCorreoServicio _enviarCorreoServicio;

        public AdjuntoController(IEnviarCorreoServicio enviarCorreoServicio)
        {
            _enviarCorreoServicio = enviarCorreoServicio;
        }

        [HttpGet("Descargar/{idImprimir}/{tipo}")]
        public async Task<FileContentResult> DescargarAsync(string idImprimir, string tipo)
        {
            var operacion = await _enviarCorreoServicio.DescargarDocumentoAsync(tipo, idImprimir);
            if (operacion == null || !operacion.Completado || operacion.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }
            return File(operacion.Resultado?.Contenido, operacion.Resultado?.TipoMime, operacion.Resultado.Nombre);
        }
    }
}
