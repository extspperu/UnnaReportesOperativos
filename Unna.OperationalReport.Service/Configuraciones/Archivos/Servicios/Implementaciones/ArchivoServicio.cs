using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Configuraciones.Archivos.Servicios.Implementaciones
{
    public class ArchivoServicio: IArchivoServicio
    {

        private readonly IArchivoRepositorio _archivoRepositorio;
        private readonly UrlConfiguracionDto _urlConfiguracion;
        private readonly ITipoArchivoRepositorio _tipoArchivoRepositorio;
        private readonly GeneralDto _general;
        public ArchivoServicio(
            IArchivoRepositorio archivoRepositorio,
            UrlConfiguracionDto urlConfiguracion,
            ITipoArchivoRepositorio tipoArchivoRepositorio,
            GeneralDto general
            )
        {
            _archivoRepositorio = archivoRepositorio;
            _urlConfiguracion = urlConfiguracion;
            _tipoArchivoRepositorio = tipoArchivoRepositorio;
            _general = general;
        }

        public async Task<OperacionDto<ArchivoDto>> ObtenerAsync(long id)
        {
            var archivo = await _archivoRepositorio.BuscarPorIdAsync(id);
            if (archivo == null)
            {
                return new OperacionDto<ArchivoDto>(CodigosOperacionDto.NoExiste, "No existe Archivo");
            }

            await _archivoRepositorio.UnidadDeTrabajo.Entry(archivo).Reference(e => e.TipoArchivo).LoadAsync();

            if (archivo.TipoArchivo == null)
            {
                return new OperacionDto<ArchivoDto>(CodigosOperacionDto.NoExiste, "Archivo No existe");
            }

            var rutaFisica = archivo.RutaArchivo;
            //rutaFisica = "G:\\SPP_Propuesta\\Rutas\\24 Hour_Jun_03_2024_06_03_14.txt";
            byte[] imageByteData = File.ReadAllBytes(rutaFisica);

            var dto = new ArchivoDto()
            {
                Contenido = imageByteData,
                Nombre = archivo.NombreArchivoOriginal,
                TipoMime = archivo.TipoArchivo.TypeMime,
                Ruta = rutaFisica,
                Id = RijndaelUtilitario.EncryptRijndaelToUrl(id)
            };
            return new OperacionDto<ArchivoDto>(dto);
        }

        public async Task<OperacionDto<ArchivoDto>> ObtenerAsync(string idCifrado)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(idCifrado);
            return await ObtenerAsync(id);
        }

        public async Task<OperacionDto<RespuestaSimpleDto<long>>> GuardarArchivoBase64Async(string base64Imagen, string extension, string folder)
        {
            var nombreArchivo = $"{Guid.NewGuid()}.{extension}";
            var ruta = $"{folder}{nombreArchivo}";

            File.WriteAllBytes(ruta, Convert.FromBase64String(base64Imagen));

            var tipoArchivo = await _tipoArchivoRepositorio.BuscarPorExtensionAsync($".{extension}");
            if (tipoArchivo == null)
            {
                return new OperacionDto<RespuestaSimpleDto<long>>(CodigosOperacionDto.NoExiste, "Tipo de archivo seleccionado no existe");
            }
            var archivo = new Archivo()
            {
                IdTipoArchivo = tipoArchivo.Id,
                NombreArchivo = nombreArchivo,
                NombreArchivoOriginal = nombreArchivo,
                RutaArchivo = ruta

            };
            _archivoRepositorio.Insertar(archivo);
            await _archivoRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();

            return new OperacionDto<RespuestaSimpleDto<long>>(
                new RespuestaSimpleDto<long>()
                {
                    Id = archivo.Id
                }
                );

        }

        public async Task<OperacionDto<RespuestaSimpleDto<long>>> GuardarArchivoAsync(string? ruta, string? folder, string? nombreArchivoOriginal)
        {
            if (string.IsNullOrWhiteSpace(ruta))
            {
                return new OperacionDto<RespuestaSimpleDto<long>>(CodigosOperacionDto.Invalido, "Ruta del archivo no existe");
            }

            var extension = Path.GetExtension(ruta).Replace(".", "");

            var nombreArchivo = $"{Path.GetFileName(ruta)}.{extension}";


            var nuevaRuta = $"{folder}{nombreArchivo}";

            var tipoArchivo = await _tipoArchivoRepositorio.BuscarPorExtensionAsync($".{extension}");

            if (tipoArchivo == null)
            {
                return new OperacionDto<RespuestaSimpleDto<long>>(CodigosOperacionDto.Invalido, "No existe soporte para ese tipo de archivo");
            }


            File.WriteAllBytes(nuevaRuta, File.ReadAllBytes(ruta));

            var archivo = new Archivo()
            {
                IdTipoArchivo = tipoArchivo.Id,
                NombreArchivo = Path.GetFileName(ruta),
                NombreArchivoOriginal = nombreArchivoOriginal,
                RutaArchivo = nuevaRuta
            };
            _archivoRepositorio.Insertar(archivo);
            await _archivoRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();

            return new OperacionDto<RespuestaSimpleDto<long>>(
                new RespuestaSimpleDto<long>()
                {
                    Id = archivo.Id
                }
              );
        }
        
        
        public async Task<OperacionDto<ArchivoRespuestaDto>> SubirArchivoAsync(IFormFile file)
        {
            if (file == null)
            {
                return new OperacionDto<ArchivoRespuestaDto>(CodigosOperacionDto.Invalido, "Seleccione un archivo para subir");
            }

            
            var extension = Path.GetExtension(file.FileName);

            var nameArchivo = $"{Guid.NewGuid()}{extension}";

            var rutaArchivo = $"{_general.RutaArchivos}{nameArchivo}";

            using (FileStream filestream = System.IO.File.Create($"{rutaArchivo}"))
            {
                await file.CopyToAsync(filestream);
                filestream.Flush();                
            }
            var operacion = await GuardarArchivoAsync(rutaArchivo, _general.RutaArchivos, file.FileName);
            if (!operacion.Completado)
            {
                return new OperacionDto<ArchivoRespuestaDto>(CodigosOperacionDto.Invalido, operacion.Mensajes);
            }


            return new OperacionDto<ArchivoRespuestaDto>(new ArchivoRespuestaDto()
            {
                Nombre = file.FileName,
                Url = $"{_urlConfiguracion.UrlBase}{"api/Archivo/"}{RijndaelUtilitario.EncryptRijndaelToUrl(operacion.Resultado.Id)}",
                Id = RijndaelUtilitario.EncryptRijndaelToUrl(operacion.Resultado.Id)
            });

        }


        public async Task<OperacionDto<ArchivoRespuestaDto>> ObtenerResumenArchivoAsync(string idCifrado)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(idCifrado);          
            return await ObtenerResumenArchivoAsync(id);

        }
        public async Task<OperacionDto<ArchivoRespuestaDto>> ObtenerResumenArchivoAsync(long id)
        {
            var archivo = await _archivoRepositorio.BuscarPorIdAsync(id);
            if (archivo == null)
            {
                return new OperacionDto<ArchivoRespuestaDto>(CodigosOperacionDto.NoExiste, "No existe Archivo");
            }
            return new OperacionDto<ArchivoRespuestaDto>(new ArchivoRespuestaDto()
            {
                Nombre = archivo.NombreArchivoOriginal,
                Url = $"{_urlConfiguracion.UrlBase}{"api/Archivo/"}{RijndaelUtilitario.EncryptRijndaelToUrl(id)}",
                Id = RijndaelUtilitario.EncryptRijndaelToUrl(id)
            });

        }


    }
}
