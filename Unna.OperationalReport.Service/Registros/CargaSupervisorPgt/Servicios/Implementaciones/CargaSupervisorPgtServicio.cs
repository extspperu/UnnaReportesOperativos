using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Servicios.Implementaciones
{
    public class CargaSupervisorPgtServicio: ICargaSupervisorPgtServicio
    {
        private readonly IArchivoServicio _archivoServicio;
        private readonly IDatoDeltaVRepositorio _datoDeltaVRepositorio;
        public CargaSupervisorPgtServicio(
            IArchivoServicio archivoServicio,
            IDatoDeltaVRepositorio datoDeltaVRepositorio
            )
        {
            _archivoServicio = archivoServicio;
            _datoDeltaVRepositorio = datoDeltaVRepositorio;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> ProcesarDocuemtoAsync(long idArchivo)
        {
            var operacion = await _archivoServicio.ObtenerAsync(idArchivo);
            if (!operacion.Completado || operacion.Resultado == null || string.IsNullOrWhiteSpace(operacion.Resultado.Ruta))
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, operacion.Mensajes);
            }

            string rutaArchivo = operacion.Resultado.Ruta;


            //await _datoDeltaVRepositorio.GuardarDeltaVAsync();




            return new OperacionDto<RespuestaSimpleDto<bool>>(
                new RespuestaSimpleDto<bool>()
                {
                    Id = true,
                    Mensaje = "Se guardo correctamente"
                }
                );

        }

    }
}
