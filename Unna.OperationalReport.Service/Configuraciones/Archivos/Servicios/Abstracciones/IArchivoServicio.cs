using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Configuraciones.Archivos.Servicios.Abstracciones
{
    public interface IArchivoServicio
    {
        Task<OperacionDto<ArchivoDto>> ObtenerAsync(long id);
        Task<OperacionDto<ArchivoDto>> ObtenerAsync(string idCifrado);
        Task<OperacionDto<RespuestaSimpleDto<long>>> GuardarArchivoBase64Async(string base64Imagen, string extension, string folder);
        Task<OperacionDto<RespuestaSimpleDto<long>>> GuardarArchivoAsync(string ruta, string folder, string? nombreArchivoOriginal);
        Task<OperacionDto<ArchivoRespuestaDto>> SubirArchivoAsync(IFormFile file);
        Task<OperacionDto<ArchivoRespuestaDto>> ObtenerResumenArchivoAsync(long id);
        Task<OperacionDto<ArchivoRespuestaDto>> ObtenerResumenArchivoAsync(string id);
    }
}
