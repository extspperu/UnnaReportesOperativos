using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivoController : ControladorBase
    {

        private readonly IArchivoServicio _archivoServicio;
        public ArchivoController(IArchivoServicio archivoServicio)
        {
            _archivoServicio = archivoServicio;
        }

      

        [HttpGet("{idArchivo}")]
        public async Task<FileContentResult> DescargarAsync(string idArchivo)
        {
            var operacion = await _archivoServicio.ObtenerAsync(idArchivo);
            if (!operacion.Completado)
            {
                return File(new byte[0], "application/octet-stream");
            }
            return File(operacion.Resultado.Contenido, operacion.Resultado.TipoMime, operacion.Resultado.Nombre);
        }


        [HttpPost("SubirArchivo")]
        [RequiereAcceso()]
        public async Task<ArchivoRespuestaDto?> SubirArchivoAsync(IFormFile file)
        {
            var operacion = await _archivoServicio.SubirArchivoAsync(file);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
