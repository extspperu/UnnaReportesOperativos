using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Cartas.ResumenVentaCliente.Dtos;
using Unna.OperationalReport.Service.Cartas.ResumenVentaCliente.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Cartas
{
    [Route("api/admin/cartas/[controller]")]
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
        public async Task<RespuestaSimpleDto<bool>?> ProcesarArchivoAsync([FromBody] ProcesarArchivoBase64CartaDto peticion)
        {
            if (peticion.File != null)
            {
                byte[] fileBytes = Convert.FromBase64String(peticion.File);
                var stream = new MemoryStream(fileBytes);
                var formFile = new FormFile(stream, 0, fileBytes.Length, "file", "upload.xlsx");

                var dto = new ProcesarArchivoCartaDto
                {
                    File = formFile,
                    Producto = peticion.Producto,
                    Tipo = peticion.Tipo,
                    IdUsuario = ObtenerIdUsuarioActual()
                };

                var operacion = await _resumenVentaClienteServicio.ProcesarArchivoAsync(dto);
                return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
            }

            return null;
        }
    }
}
