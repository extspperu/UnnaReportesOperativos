using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.Osinergmin.Dtos;
using Unna.OperationalReport.Service.Registros.Osinergmin.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Registros.Osinergmin.Servicios.Implementaciones
{
    public class OsinergminServicio : IOsinergminServicio
    {
        private readonly IOsinergminRepositorio _osinergminRepositorio;
        private readonly IArchivoServicio _archivoServicio;
        public OsinergminServicio(
            IOsinergminRepositorio osinergminRepositorio,
            IArchivoServicio archivoServicio
            )
        {
            _osinergminRepositorio = osinergminRepositorio;
            _archivoServicio = archivoServicio;
        }


        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(OsinergminDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            long? idArchivo = default(long?);
            if (peticion.Archivo != null)
            {
                if (!string.IsNullOrWhiteSpace(peticion.Archivo.Id))
                {
                    idArchivo = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(peticion.Archivo.Id);
                }
                
            }

            var registro = await _osinergminRepositorio.BuscarPorIdAsync(peticion.Fecha.Value);
            if (registro == null)
            {
                registro = new Data.Registro.Entidades.Osinergmin();
            }
            registro.Actualizado = DateTime.UtcNow;
            registro.Username = peticion.Username;
            registro.Password = peticion.Password;
            registro.NacionalizadaM3 = peticion.NacionalizadaM3;
            registro.NacionalizadaTn = peticion.NacionalizadaTn;
            registro.NacionalizadaBbl = peticion.NacionalizadaBbl;
            registro.IdArchivo = idArchivo;
            registro.IdUsuario = peticion.IdUsuario;

            if (registro.Fecha.HasValue)
            {
                await _osinergminRepositorio.EditarAsync(registro);
            }
            else
            {
                registro.Fecha = peticion.Fecha.Value;
                await _osinergminRepositorio.InsertarAsync(registro);
            }

            return new OperacionDto<RespuestaSimpleDto<bool>>(
                new RespuestaSimpleDto<bool>()
                {
                    Id = true,
                    Mensaje = "Se guardo correctamente"
                }
                );

        }

        public async Task<OperacionDto<OsinergminDto?>> ObtenerAsync(DateTime fecha)
        {
            var registro = await _osinergminRepositorio.BuscarPorIdAsync(fecha);
            if (registro == null)
            {
                return new OperacionDto<OsinergminDto?>(CodigosOperacionDto.NoExiste, "No existe registro para el día");
            }

            var dto = new OsinergminDto
            {
                Fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(registro.Fecha ?? DateTime.MinValue),
                Username = registro.Username,
                Password = registro.Password,
                NacionalizadaBbl = registro.NacionalizadaBbl,
                NacionalizadaM3 = registro.NacionalizadaM3,
                NacionalizadaTn = registro.NacionalizadaTn,
                Creado = registro.Creado,
            };
            if (registro.IdArchivo.HasValue)
            {
                var operacion = await _archivoServicio.ObtenerResumenArchivoAsync(registro.IdArchivo ?? 0);
                if (operacion.Completado)
                {
                    dto.Archivo = operacion.Resultado;
                }
            }

            return new OperacionDto<OsinergminDto?>(dto);

        }

    }
}
