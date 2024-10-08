﻿using AutoMapper;
using DocumentFormat.OpenXml.Office2019.Excel.ThreadedComments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.Datos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.Lotes.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Implementaciones
{
    public class DiaOperativoServicio : IDiaOperativoServicio
    {
        DateTime FechaDiaOperativo = FechasUtilitario.ObtenerDiaOperativo();

        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IDiaOperativoRepositorio _diaOperativoRepositorio;
        private readonly IUsuarioLoteServicio _usuarioLoteServicio;
        private readonly IMapper _mapper;
        private readonly IDatoServicio _datoServicio;
        private readonly ICargaSupervisorPgtServicio _cargaSupervisorPgtServicio;

        public DiaOperativoServicio(
            IRegistroRepositorio registroRepositorio,
            IDiaOperativoRepositorio diaOperativoRepositorio,
            IUsuarioLoteServicio usuarioLoteServicio,
            IMapper mapper,
            IDatoServicio datoServicio,
            ICargaSupervisorPgtServicio cargaSupervisorPgtServicio
            )
        {
            _registroRepositorio = registroRepositorio;
            _diaOperativoRepositorio = diaOperativoRepositorio;
            _usuarioLoteServicio = usuarioLoteServicio;
            _mapper = mapper;
            _datoServicio = datoServicio;
            _cargaSupervisorPgtServicio = cargaSupervisorPgtServicio;
        }


        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(DiaOperativoDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            
            var operacion = await _usuarioLoteServicio.ObtenerIdLotePorIdUsuarioAsync(peticion.IdUsuario ?? 0);
            if (operacion == null || !operacion.Completado || operacion.Resultado == null)
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.NoExiste, operacion.Mensajes);
            }
            if (operacion.Resultado.Id != (int)TiposLote.LoteX)
            {
                peticion.NumeroRegistro = new int?();
            }

            int idGrupo = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(peticion.IdGrupo);
            var diaOperativo = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync(operacion.Resultado.Id, peticion.Fecha, idGrupo, peticion.NumeroRegistro);
            if (diaOperativo == null)
            {
                diaOperativo = new Data.Registro.Entidades.DiaOperativo();
                diaOperativo.IdUsuario = peticion.IdUsuario;
            }
            diaOperativo.IdLote = operacion.Resultado.Id;
            diaOperativo.Fecha = peticion.Fecha ?? FechaDiaOperativo;
            diaOperativo.NumeroRegistro = peticion.NumeroRegistro;
            diaOperativo.Adjuntos = peticion.Adjuntos;
            diaOperativo.Comentario = peticion.Comentario;
            diaOperativo.Actualizado = DateTime.UtcNow;
            diaOperativo.IdGrupo = idGrupo;


            if (diaOperativo.EsObservado == true)
            {
                diaOperativo.EsObservado = false;
            }
            if (diaOperativo.IdDiaOperativo > 0)
            {
                _diaOperativoRepositorio.Editar(diaOperativo);
            }
            else
            {
                _diaOperativoRepositorio.Insertar(diaOperativo);
            }
            await _diaOperativoRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();

            if (peticion.Registros != null && peticion.Registros.Count > 0)
            {
                foreach (var item in peticion.Registros)
                {
                    var registro = await _registroRepositorio.ObtenerPorIdDatoYDiaOperativoAsync(item.IdDato ?? 0, diaOperativo.IdDiaOperativo);
                    if (registro == null)
                    {
                        registro = new Data.Registro.Entidades.Registro();
                        registro.IdUsuario = peticion.IdUsuario;
                    }
                    registro.IdDato = item.IdDato ?? 0;
                    registro.Valor = item.Valor;
                    registro.EsConciliado = item.EsConciliado;
                    registro.IdDiaOperativo = diaOperativo.IdDiaOperativo;
                    registro.Actualizado = DateTime.UtcNow;
                    if (registro.EsDevuelto == true && registro.EsValido == false)
                    {
                        registro.EsValido = new bool?();
                    }
                    if (registro.IdRegistro > 0)
                    {
                        _registroRepositorio.Editar(registro);
                    }
                    else
                    {
                        _registroRepositorio.Insertar(registro);
                    }
                    await _registroRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();


                }
            }

            if (diaOperativo.IdLote == (int)TiposLote.LoteIv)
            {

                List<ArchivoRespuestaDto>  rpta = JsonConvert.DeserializeObject<List<ArchivoRespuestaDto>>(diaOperativo.Adjuntos);
                foreach (var item in rpta)
                {
                    long idDiaOperativoArchivo = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(item.Id);

                    var test =  await _cargaSupervisorPgtServicio.ProcesarDocumentoTxtAsync(idDiaOperativoArchivo, diaOperativo.IdDiaOperativo);
                } 
            }


            return new OperacionDto<RespuestaSimpleDto<string>>(
                new RespuestaSimpleDto<string>()
                {
                    Id = RijndaelUtilitario.EncryptRijndaelToUrl(diaOperativo.IdDiaOperativo),
                    Mensaje = "Se guardo correctamente"
                }
                );

        }


        public async Task<OperacionDto<DiaOperativoDto>> ObtenerPorIdUsuarioYFechaAsync(long idUsuario, DateTime fecha, int idGrupo, int? numero)
        {

            var operacion = await _usuarioLoteServicio.ObtenerIdLotePorIdUsuarioAsync(idUsuario);
            if (operacion == null || !operacion.Completado || operacion.Resultado == null)
            {
                return new OperacionDto<DiaOperativoDto>(CodigosOperacionDto.Invalido, operacion.Mensajes);
            }
            var diaOperativo = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync(operacion.Resultado.Id, fecha, idGrupo, numero);
            if (diaOperativo == null)
            {
                return new OperacionDto<DiaOperativoDto>(CodigosOperacionDto.NoExiste, "No existe registro de día");
            }

            return await ObtenerAsync(diaOperativo.IdDiaOperativo);

        }

        public async Task<OperacionDto<DiaOperativoDto>> ObtenerAsync(string idDiaOperativo)
        {
            long id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(idDiaOperativo);
            return await ObtenerAsync(id);
        }

        public async Task<OperacionDto<DiaOperativoDto>> ObtenerAsync(long idDiaOperativo)
        {
            var diaOperativo = await _diaOperativoRepositorio.BuscarPorIdYNoBorradoAsync(idDiaOperativo);
            if (diaOperativo == null)
            {
                return new OperacionDto<DiaOperativoDto>(CodigosOperacionDto.Invalido, "No existe registro");
            }
            await _diaOperativoRepositorio.UnidadDeTrabajo.Entry(diaOperativo).Collection(e => e.Registros).LoadAsync();

            var dto = _mapper.Map<DiaOperativoDto>(diaOperativo);

            return new OperacionDto<DiaOperativoDto>(dto);

        }


        public async Task<bool> ExisteParaEdicionDatosAsync(long idUsuario, int idGrupo, int? numero)
        {
            var operacion = await _usuarioLoteServicio.ObtenerIdLotePorIdUsuarioAsync(idUsuario);
            if (operacion == null || operacion.Resultado == null || !operacion.Completado)
            {
                return false;
            }
            var diaOperativo = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync(operacion.Resultado.Id, FechaDiaOperativo, idGrupo, numero);
            if (diaOperativo == null || !diaOperativo.EsObservado.HasValue)
            {
                return false;
            }
            return diaOperativo.EsObservado.Value;
        }

        public async Task<List<int>> ListarNumeroEdicionFiscalizadorEnelAsync(long idUsuario, int idGrupo, int? numero)
        {
            List<int> numeros = new List<int>();
            var operacion = await _usuarioLoteServicio.ObtenerIdLotePorIdUsuarioAsync(idUsuario);
            if (operacion == null || operacion.Resultado == null || !operacion.Completado)
            {
                return numeros;
            }
            var diasOperativos = await _diaOperativoRepositorio.ListarPorIdLoteYFechaAsync(operacion.Resultado.Id, FechaDiaOperativo, idGrupo, numero);
            if (diasOperativos == null || diasOperativos.Count == 0)
            {
                return numeros;
            }
            var numeroDias = diasOperativos.Where(e => e.EsObservado == true).Select(e => e.NumeroRegistro).ToList();
            return numeroDias.Select(e => e.Value).ToList();
        }



        public async Task<OperacionDto<DatosFiscalizadorEnelDto>> ObtenerPermisosFiscalizadorEnelAsync(long idUsuario, string? grupo, string? edicion)
        {
            var dto = new DatosFiscalizadorEnelDto()
            {
                IdGrupo = RijndaelUtilitario.EncryptRijndaelToUrl((int)TipoGrupos.FiscalizadorEnel)
            };
            string? tipo = TiposFiscalizadores.Regular;
            string? tituloNumeroCabecera = default(string);
            switch (grupo)
            {
                case TiposNumeroRegistroDescripcion.Primero:
                    dto.NumeroRegistro = (int)TiposNumeroRegistro.PrimeroRegistro;
                    tituloNumeroCabecera = "PRIMER";
                    dto.TieneDatoConsiliado = true;
                    break;
                case TiposNumeroRegistroDescripcion.Segundo:
                    dto.NumeroRegistro = (int)TiposNumeroRegistro.SegundoRegistro;
                    tituloNumeroCabecera = "SEGUNDO";
                    tipo = TiposFiscalizadores.SegundoRegistroEnnel;
                    dto.TieneDatoConsiliado = false;
                    break;
                default:
                    return new OperacionDto<DatosFiscalizadorEnelDto>(CodigosOperacionDto.Invalido, "No existe la pagina");

            }


            var operacion = await _datoServicio.ListarPorTipoAsync(tipo);
            if (operacion == null || operacion.Resultado == null || !operacion.Completado)
            {
                return new OperacionDto<DatosFiscalizadorEnelDto>(CodigosOperacionDto.Invalido, "No existe datos configurados");
            }
            dto.Datos = operacion.Resultado;

            switch (edicion)
            {
                case TiposAcciones.Registro:
                    dto.Titulo = $"{tituloNumeroCabecera} REGISTRO DE DATOS";
                    break;
                case TiposAcciones.Editar:
                    dto.Titulo = $"EDICIÓN DE {tituloNumeroCabecera} REGISTRO DE DATOS";
                    break;
                default:
                    return new OperacionDto<DatosFiscalizadorEnelDto>(CodigosOperacionDto.Invalido, "Tipo de acción no valida");
            }
            var operacionExisteRegistro = await ObtenerPorIdUsuarioYFechaAsync(idUsuario, FechasUtilitario.ObtenerDiaOperativo(), (int)TipoGrupos.FiscalizadorRegular, null);
            if (operacionExisteRegistro == null || !operacionExisteRegistro.Completado || operacionExisteRegistro.Resultado == null)
            {
                dto.PermitirEditar  = true;
            }
            else
            {
                dto.PermitirEditar = operacionExisteRegistro.Resultado.DatoValidado == true ? false : true;
            }
            return new OperacionDto<DatosFiscalizadorEnelDto>(dto);

        }


        public async Task<OperacionDto<SeguimientoFiscalizadorEnelDto>> SeguimientoFiscalizadorEnelAsync(long idUsuario)
        {
            var operacion = await _usuarioLoteServicio.ObtenerIdLotePorIdUsuarioAsync(idUsuario);
            if (operacion == null || !operacion.Completado || operacion.Resultado == null)
            {
                return new OperacionDto<SeguimientoFiscalizadorEnelDto>(CodigosOperacionDto.Invalido, operacion.Mensajes);
            }
            var dto = new SeguimientoFiscalizadorEnelDto()
            {
                PrimerRegistro = "rojo",
                SegundoRegistro = "rojo",
                DatosValidados = "rojo"
            };
            var entidades = await _diaOperativoRepositorio.ListarPorIdLoteYFechaAsync(operacion.Resultado.Id, FechaDiaOperativo, (int)TipoGrupos.FiscalizadorEnel, null);
            if (entidades == null || entidades.Count == 0)
            {
                return new OperacionDto<SeguimientoFiscalizadorEnelDto>(dto);
            }

            var primerRegistro = entidades.Where(e => e.NumeroRegistro == (int)TiposNumeroRegistro.PrimeroRegistro).FirstOrDefault();
            if (primerRegistro != null)
            {
                if (primerRegistro.EsObservado.HasValue)
                {
                    dto.PrimerRegistro = primerRegistro.EsObservado.Value ? "rojo" : "verde";
                }
                else
                {
                    dto.PrimerRegistro = "verde";
                }
            }
            var segundoRegistro = entidades.Where(e => e.NumeroRegistro == (int)TiposNumeroRegistro.SegundoRegistro).FirstOrDefault();
            if (segundoRegistro != null)
            {
                if (segundoRegistro.EsObservado.HasValue)
                {
                    dto.SegundoRegistro = segundoRegistro.EsObservado.Value ? "rojo" : "verde";
                }
                else
                {
                    dto.SegundoRegistro = "verde";
                }
            }
            if (entidades.Where(e => e.DatoValidado == true).ToList().Count() == entidades.Count())
            {
                dto.DatosValidados = "verde";
            }
            return new OperacionDto<SeguimientoFiscalizadorEnelDto>(dto);

        }

        public async Task<OperacionDto<List<DiaOperativoDto>>> ListarRegistrosFiscalizadorRegularAsync()
        {
            DateTime fecha = new DateTime(FechaDiaOperativo.Year, FechaDiaOperativo.Month, FechaDiaOperativo.Day);
            var diaOperativo = await _diaOperativoRepositorio.ListarPorFechaYIdGrupoAsync(fecha, (int)TipoGrupos.FiscalizadorRegular);
            if (diaOperativo == null || diaOperativo.Count == 0)
            {
                return new OperacionDto<List<DiaOperativoDto>>(CodigosOperacionDto.NoExiste, "No existe registro para el dia");
            }

            var usuarioLotes = await _usuarioLoteServicio.ListarPorIdGrupoAsync((int)TipoGrupos.FiscalizadorRegular);
            if (!usuarioLotes.Completado || usuarioLotes.Resultado == null || diaOperativo.Count != usuarioLotes.Resultado.Count)
            {
                return new OperacionDto<List<DiaOperativoDto>>(CodigosOperacionDto.NoExiste, "Aun no se completa los registros de datos");
            }
            var dto = _mapper.Map<List<DiaOperativoDto>>(diaOperativo);
            return new OperacionDto<List<DiaOperativoDto>>(dto);

        }



        public async Task<OperacionDto<DatosFiscalizadorEnelDto>> ObtenerValidarDatosAsync(string idLote, DateTime fecha,int? numeroRegistro)
        {
            int id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idLote);           
            int? numero = default(int?);
            if (id == (int)TiposLote.LoteX)
            {
                numero = numeroRegistro == (int)TiposNumeroRegistro.SegundoRegistro ? numeroRegistro : (int)TiposNumeroRegistro.PrimeroRegistro;
            }
            
            var diaOperativo = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync(id, fecha, null, numero);
            if (diaOperativo == null)
            {
                return new OperacionDto<DatosFiscalizadorEnelDto>(CodigosOperacionDto.NoExiste, "No existe registro de día");
            }

            var registros = await _registroRepositorio.BuscarPorIdDiaOperativoAsync(diaOperativo.IdDiaOperativo);
            if (registros == null || registros.Count == 0)
            {
                return new OperacionDto<DatosFiscalizadorEnelDto>(CodigosOperacionDto.NoExiste, "No existe registro de día");
            }
            string? tipo = registros?.FirstOrDefault()?.Dato != null ? registros?.FirstOrDefault()?.Dato?.Tipo : default(string);


            var dto = new DatosFiscalizadorEnelDto();
            var operacion = await _datoServicio.ListarPorTipoAsync(tipo);
            if (operacion.Completado)
            {
                dto.Datos = operacion.Resultado;
            }
            dto.IdDiaOperativo = RijndaelUtilitario.EncryptRijndaelToUrl(diaOperativo.IdDiaOperativo);
            return new OperacionDto<DatosFiscalizadorEnelDto>(dto);

        }


    }
}
