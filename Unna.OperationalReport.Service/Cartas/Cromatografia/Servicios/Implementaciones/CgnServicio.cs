﻿using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Service.Cartas.Cromatografia.Dtos;
using Unna.OperationalReport.Service.Cartas.Cromatografia.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Cartas.Cromatografia.Servicios.Implementaciones
{
    public class CgnServicio : ICgnServicio
    {
        private readonly IRegistroCromatografiaRepositorio _registroCromatografiaRepositorio;
        private readonly IRegistroAnalisiDespachoCgnRepositorio _registroAnalisiDespachoCgnRepositorio;
        public CgnServicio(
            IRegistroCromatografiaRepositorio registroCromatografiaRepositorio,
            IRegistroAnalisiDespachoCgnRepositorio registroAnalisiDespachoCgnRepositorio
            )
        {
            _registroCromatografiaRepositorio = registroCromatografiaRepositorio;
            _registroAnalisiDespachoCgnRepositorio = registroAnalisiDespachoCgnRepositorio;
        }

        public async Task<OperacionDto<RegistroCromatografiaDto>> ObtenerAsync()
        {
            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();

            DateTime periodo = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
            var registro = await _registroCromatografiaRepositorio.BuscarPorPeriodoTipoYTanqueAsync(periodo, TiposProducto.CGN, TiposTanques.T_4601);
            if (registro == null)
            {
                var datos = new RegistroCromatografiaDto
                {
                    Periodo = diaOperativo,
                    Tipo = TiposProducto.CGN,
                    Tanque = TiposTanques.T_4601
                };
                var day = DateTime.DaysInMonth(periodo.Year, periodo.Month);

                List<RegistroAnalisiDespachoCgnDto> cgnDetalle = new List<RegistroAnalisiDespachoCgnDto>();
                for (var i = 0; i < day; i++)
                {
                    var a = new RegistroAnalisiDespachoCgnDto
                    {
                        Fecha = periodo.AddDays(i),
                    };
                    a.Day = a.Fecha.HasValue ? a.Fecha.Value.Day : null;
                    cgnDetalle.Add(a);
                }
                datos.Cgn = cgnDetalle;
                return new OperacionDto<RegistroCromatografiaDto>(datos);
            }
            var dto = new RegistroCromatografiaDto
            {
                Periodo = registro.Periodo,
                Tipo = registro.Tipo,
                Tanque = registro.Tanque
            };

            var lista = await _registroAnalisiDespachoCgnRepositorio.ListarPorIdRegistroCromatografiaAsync(registro.Id);
            var cgn = lista.Select(e => new RegistroAnalisiDespachoCgnDto
            {
                Api = e.Api,
                Fecha = e.Fecha,
                Gesp = e.Gesp,
                IdRegistroCromatografia = RijndaelUtilitario.EncryptRijndaelToUrl(registro.Id),
                NroDespacho = e.NroDespacho,
                P5 = e.P5,
                P10 = e.P10,
                P30 = e.P30,
                P50 = e.P50,
                P70 = e.P70,
                P90 = e.P90,
                P95 = e.P95,
                Pfin = e.Pfin,
                Pinic = e.Pinic,
                Rvp = e.Rvp,
                Day = e.Fecha.Day
            }).ToList();
            dto.Cgn = cgn;
            if (cgn.Count == 0)
            {
                var day = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                List<RegistroAnalisiDespachoCgnDto> listaFechas = new List<RegistroAnalisiDespachoCgnDto>();
                for (var i = 0; i < day; i++)
                {
                    var a = new RegistroAnalisiDespachoCgnDto
                    {
                        Fecha = periodo.AddDays(i),
                    };
                    a.Day = a.Fecha.HasValue ? a.Fecha.Value.Day : null;
                    listaFechas.Add(a);
                }
                dto.Cgn = listaFechas;
            }
            return new OperacionDto<RegistroCromatografiaDto>(dto);

        }


        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(RegistroCromatografiaDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            if (string.IsNullOrEmpty(peticion.Tipo) || string.IsNullOrEmpty(peticion.Tanque))
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Campo Tipo y Tanque son requeridos");
            }

            if (peticion.Cgn == null || peticion.Cgn.Count == 0)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Registro por días no puede ser vacío");
            }

            if (!peticion.Periodo.HasValue)
            {
                peticion.Periodo = FechasUtilitario.ObtenerDiaOperativo();
            }

            DateTime periodo = new DateTime(peticion.Periodo.Value.Year, peticion.Periodo.Value.Month, 1);

            var registro = await _registroCromatografiaRepositorio.BuscarPorPeriodoTipoYTanqueAsync(periodo, peticion.Tipo, peticion.Tanque);
            if (registro == null)
            {
                registro = new RegistroCromatografia();
            }
            registro.Actualizado = DateTime.UtcNow;
            registro.Periodo = periodo;
            registro.Tipo = peticion.Tipo;
            registro.Tanque = peticion.Tanque;
            registro.IdUsuario = peticion.IdUsuario;

            if (registro.Id > 0)
            {
                await _registroCromatografiaRepositorio.EditarAsync(registro);
            }
            else
            {
                await _registroCromatografiaRepositorio.InsertarAsync(registro);
            }

            foreach (var item in peticion.Cgn)
            {
                if (!item.Day.HasValue)
                {
                    continue;
                }
                var entidad = new RegistroAnalisiDespachoCgn
                {
                    Actualizado = DateTime.UtcNow,
                    Api = item.Api,
                    Fecha = item.Fecha.HasValue ? item.Fecha.Value : new DateTime(periodo.Year, periodo.Month, item.Day.Value),
                    Creado = DateTime.UtcNow,
                    Gesp = item.Gesp,
                    IdRegistroCromatografia = registro.Id,
                    IdUsuario = peticion.IdUsuario,
                    NroDespacho = item.NroDespacho,
                    P5 = item.P5,
                    P10 = item.P10,
                    P30 = item.P30,
                    P50 = item.P50,
                    P70 = item.P70,
                    P90 = item.P90,
                    P95 = item.P95,
                    Pfin = item.Pfin,
                    Pinic = item.Pinic,
                    Rvp = item.Rvp
                };
                await _registroAnalisiDespachoCgnRepositorio.InsertarAsync(entidad);

            }

            return new OperacionDto<RespuestaSimpleDto<bool>>(
                new RespuestaSimpleDto<bool>()
                {
                    Id = true,
                    Mensaje = "Se guardó correctamente"
                }
                );

        }

    }
}
