using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Dtos;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Dtos;
using Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Servicios.Implementaciones
{
    public class RegistroSupervisorServicio : IRegistroSupervisorServicio
    {

        private readonly IRegistroSupervisorRepositorio _registroSupervisorRepositorio;
        private readonly IAdjuntoSupervisorRepositorio _adjuntoSupervisorRepositorio;
        private readonly IArchivoServicio _archivoServicio;
        private readonly IAdjuntoServicio _adjuntoServicio;
        private readonly IMapper _mapper;
        private readonly ICargaSupervisorPgtServicio _cargaSupervisorPgtServicio;
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;

        public RegistroSupervisorServicio(
            IRegistroSupervisorRepositorio registroSupervisorRepositorio,
            IAdjuntoSupervisorRepositorio adjuntoSupervisorRepositorio,
            IArchivoServicio archivoServicio,
            IAdjuntoServicio adjuntoServicio,
            IMapper mapper,
            ICargaSupervisorPgtServicio cargaSupervisorPgtServicio,
            ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio
            )
        {
            _registroSupervisorRepositorio = registroSupervisorRepositorio;
            _adjuntoSupervisorRepositorio = adjuntoSupervisorRepositorio;
            _archivoServicio = archivoServicio;
            _adjuntoServicio = adjuntoServicio;
            _mapper = mapper;
            _cargaSupervisorPgtServicio = cargaSupervisorPgtServicio;
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(string accion, RegistroSupervisorDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            if (peticion.Fecha == null)
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.NoExiste, "Campo fecha es requerido");
            }

            if (accion == "CargarArchivo")
            {
                var operacionAdjuntos = await _adjuntoServicio.ListarPorGrupoAsync(TipoGrupoAdjuntos.SupervisorPgt);
                if (operacionAdjuntos.Completado || operacionAdjuntos.Resultado != null)
                {
                    if (operacionAdjuntos.Resultado?.Count > peticion.Adjuntos?.Count)
                    {
                        return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.NoExiste, $"Usted a cargado {peticion.Adjuntos.Count} documentos de {operacionAdjuntos.Resultado.Count} por favor cargar {(operacionAdjuntos.Resultado.Count - peticion.Adjuntos.Count)} documentos restantes");
                    }

                }
            }
            bool procesarArchivoExcel = false;
            var registroSupervisor = await _registroSupervisorRepositorio.BuscarPorFechaAsync(peticion.Fecha.Value);
            if (registroSupervisor == null)
            {
                registroSupervisor = new Data.Reporte.Entidades.RegistroSupervisor();
            }
            registroSupervisor.Fecha = peticion.Fecha.Value;
            registroSupervisor.Comentario = peticion.Comentario;
            if (!string.IsNullOrWhiteSpace(peticion.IdArchivo))
            {
                registroSupervisor.IdArchivo = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(peticion.IdArchivo);
                procesarArchivoExcel = true;
            }
            registroSupervisor.Actualizado = DateTime.UtcNow;
            registroSupervisor.IdUsuario = peticion.IdUsuario;
            if (registroSupervisor.IdRegistroSupervisor > 0)
            {
                _registroSupervisorRepositorio.Editar(registroSupervisor);
            }
            else
            {
                _registroSupervisorRepositorio.Insertar(registroSupervisor);
            }
            await _registroSupervisorRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();

            if (peticion.Adjuntos != null && peticion.Adjuntos.Count > 0)
            {
                foreach (var item in peticion.Adjuntos)
                {
                    var registro = await _adjuntoSupervisorRepositorio.BuscarPorIdRegistroSupervisorYIdAdjuntoAsync(registroSupervisor.IdRegistroSupervisor, item.IdAdjunto);
                    if (registro == null)
                    {
                        registro = new Data.Reporte.Entidades.AdjuntoSupervisor();
                    }
                    registro.IdRegistroSupervisor = registroSupervisor.IdRegistroSupervisor;
                    registro.EsConciliado = item.EsConciliado;
                    if (item.EsConciliado == false || !item.EsConciliado.HasValue)
                    {
                        if (!string.IsNullOrWhiteSpace(item.IdArchivo))
                        {
                            registro.IdArchivo = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(item.IdArchivo);
                        }
                        
                    }
                    else
                    {
                        registro.IdArchivo = new long?();
                    }
                    registro.IdAdjunto = item.IdAdjunto;
                    registro.Actualizado = DateTime.UtcNow;
                    if (registro.IdAdjuntoSupervisor > 0)
                    {
                        _adjuntoSupervisorRepositorio.Editar(registro);
                    }
                    else
                    {
                        _adjuntoSupervisorRepositorio.Insertar(registro);
                    }
                    await _adjuntoSupervisorRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();

                }
            }


            if (procesarArchivoExcel)
            {
                await _cargaSupervisorPgtServicio.ProcesarDocuemtoAsync(registroSupervisor.IdArchivo ?? 0, registroSupervisor.IdRegistroSupervisor);
            }


            return new OperacionDto<RespuestaSimpleDto<string>>(
                new RespuestaSimpleDto<string>()
                {
                    Id = RijndaelUtilitario.EncryptRijndaelToUrl(registroSupervisor.IdRegistroSupervisor),
                    Mensaje = "Se guardo correctamente"
                }
                );

        }


        public async Task<OperacionDto<RegistroSupervisorDto>> ObtenerPorFechaAsync(DateTime fecha)
        {
            var entidad = await _registroSupervisorRepositorio.BuscarPorFechaAsync(fecha);
            if (entidad == null)
            {
                return new OperacionDto<RegistroSupervisorDto>(CodigosOperacionDto.NoExiste, "No existe registro para el día seleccionado");
            }
            //            await _registroSupervisorRepositorio.UnidadDeTrabajo.Entry(entidad).Reference(e => e.Archivo).LoadAsync();            
            //await _registroSupervisorRepositorio.UnidadDeTrabajo.Entry(entidad).Collection(e => e.AdjuntoSupervisores).LoadAsync();

            var dto = _mapper.Map<RegistroSupervisorDto>(entidad);
            if (!string.IsNullOrWhiteSpace(dto.IdArchivo))
            {
                var operacionArchivo = await _archivoServicio.ObtenerResumenArchivoAsync(dto.IdArchivo);
                if (operacionArchivo.Completado)
                {
                    dto.Archivo = operacionArchivo.Resultado;
                }
            }

            var adjuntoSupervisor = await _adjuntoSupervisorRepositorio.LstarPorIdRegistroSupervisorAsync(entidad.IdRegistroSupervisor);
            dto.Adjuntos = _mapper.Map<List<AdjuntoSupervisorDto>>(adjuntoSupervisor);
            foreach (var item in dto.Adjuntos)
            {
                if (string.IsNullOrWhiteSpace(item.IdArchivo))
                {
                    continue;
                }
                var operacionArchivo = await _archivoServicio.ObtenerResumenArchivoAsync(item.IdArchivo);
                if (operacionArchivo.Completado)
                {
                    item.Archivo = operacionArchivo.Resultado;
                }
            }

            return new OperacionDto<RegistroSupervisorDto>(dto);

        }



        public async Task<OperacionDto<List<AdjuntoSupervisorDto>?>> ValidarArhivosAsync(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return new OperacionDto<List<AdjuntoSupervisorDto>?>(CodigosOperacionDto.NoExiste, "Debe cargar archivos");
            }
            var operacionAdjuntos = await _adjuntoServicio.ListarPorGrupoAsync(TipoGrupoAdjuntos.SupervisorPgt);
            if (!operacionAdjuntos.Completado || operacionAdjuntos.Resultado == null)
            {
                return new OperacionDto<List<AdjuntoSupervisorDto>?>(CodigosOperacionDto.NoExiste, "No existe archivo configurados para cargar");
            }
            List<AdjuntoDto> adjuntos = operacionAdjuntos.Resultado;
            //string formatoFecha = FechaRegistro.ToString("dd-MM-yyyy");
            //adjuntos.ForEach(e => e.Nomenclatura = e.Nomenclatura?.Replace("FECHA", formatoFecha));

            var dto = new List<AdjuntoSupervisorDto>();
            foreach (var item in files)
            {
                var extension = Path.GetExtension(item.FileName);
                var nombreArchivo = item.FileName.Replace(extension, "");

                var existeNomenclatura = adjuntos.Where(e => e.Nomenclatura.Equals(nombreArchivo)).FirstOrDefault();
                if (existeNomenclatura == null)
                {
                    continue;
                }
                var operacionArchivo = await _archivoServicio.SubirArchivoAsync(item);
                if (operacionArchivo.Completado && operacionArchivo.Resultado != null)
                {
                    dto.Add(new AdjuntoSupervisorDto()
                    {
                        IdAdjunto = existeNomenclatura.Id,
                        IdArchivo = operacionArchivo.Resultado.Id,
                        Color = "rojo",
                        Archivo = operacionArchivo.Resultado,
                        Adjunto = existeNomenclatura,
                    });

                }
            }
            if (dto.Count == 0)
            {
                return new OperacionDto<List<AdjuntoSupervisorDto>?>(CodigosOperacionDto.NoExiste, "Los archivos cargados no tiene la nomenclatura correcta");
            }
            return new OperacionDto<List<AdjuntoSupervisorDto>?>(dto);

        }



        public async Task<OperacionDto<RespuestaSimpleDto<string>>> ValidarRegistroAsync(long? idUsuario, string? idRegistroSupervisor)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(idRegistroSupervisor);
            var registroSupervisor = await _registroSupervisorRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (registroSupervisor == null)
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.NoExiste, "No existe registro de supervisor para el dia operativo");
            }
            if (registroSupervisor.EsValidado == true)
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.NoExiste, "El registro ya se encuentra validado");
            }
            registroSupervisor.FechaValidado = DateTime.UtcNow;
            registroSupervisor.EsValidado = true;
            registroSupervisor.Actualizado = DateTime.UtcNow;
            registroSupervisor.IdUsuarioValidado = idUsuario;
            registroSupervisor.EsObservado = registroSupervisor.EsObservado == true ? false : registroSupervisor.EsObservado;
            _registroSupervisorRepositorio.Editar(registroSupervisor);
            await _registroSupervisorRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();
            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(6, 1);
            return new OperacionDto<RespuestaSimpleDto<string>>(
                new RespuestaSimpleDto<string>()
                {
                    Id = RijndaelUtilitario.EncryptRijndaelToUrl(registroSupervisor.IdRegistroSupervisor),
                    Mensaje = "Se guardo correctamente"
                }
            );

        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> ObservarRegistroAsync(long? idUsuario, string? idRegistroSupervisor)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(idRegistroSupervisor);
            var registroSupervisor = await _registroSupervisorRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (registroSupervisor == null)
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.NoExiste, "No existe registro de supervisor para el dia operativo");
            }
            if (registroSupervisor.EsObservado == true)
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.NoExiste, "El registro ya se encuentra observado");
            }
            registroSupervisor.FechaObservado = DateTime.UtcNow;
            registroSupervisor.EsObservado = true;
            registroSupervisor.Actualizado = DateTime.UtcNow;
            registroSupervisor.IdUsuarioObservado = idUsuario;
            registroSupervisor.EsValidado = registroSupervisor.EsValidado == true ? false : registroSupervisor.EsValidado;
            _registroSupervisorRepositorio.Editar(registroSupervisor);
            await _registroSupervisorRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();
            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(6, 3);

            return new OperacionDto<RespuestaSimpleDto<string>>(
                new RespuestaSimpleDto<string>()
                {
                    Id = RijndaelUtilitario.EncryptRijndaelToUrl(registroSupervisor.IdRegistroSupervisor),
                    Mensaje = "Se guardo correctamente"
                }
                );

        }


    }
}
