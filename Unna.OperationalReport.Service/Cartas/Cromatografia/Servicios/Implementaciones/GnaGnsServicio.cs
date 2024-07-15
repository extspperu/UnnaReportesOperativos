using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Cartas.Cromatografia.Dtos;
using Unna.OperationalReport.Service.Cartas.Cromatografia.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Cartas.Cromatografia.Servicios.Implementaciones
{
    public class GnaGnsServicio : IGnaGnsServicio
    {

        private readonly IRegistroCromatografiaRepositorio _registroCromatografiaRepositorio;
        private readonly ILoteRepositorio _loteRepositorio;
        public GnaGnsServicio(
            IRegistroCromatografiaRepositorio registroCromatografiaRepositorio,
            ILoteRepositorio loteRepositorio
            )
        {
            _registroCromatografiaRepositorio = registroCromatografiaRepositorio;
            _loteRepositorio = loteRepositorio;
        }

        public async Task<OperacionDto<RegistroCromatografiaDto>> ObtenerAsync(BuscarGnaGnsDto peticion)
        {
            if (!peticion.Periodo.HasValue)
            {
                return new OperacionDto<RegistroCromatografiaDto>(CodigosOperacionDto.NoExiste, "Periodo es requerido");
            }

            if (string.IsNullOrEmpty(peticion.Tipo) || !peticion.IdLote.HasValue)
            {
                return new OperacionDto<RegistroCromatografiaDto>(CodigosOperacionDto.NoExiste, "Campo Tipo y Lote son requeridos");
            }

            var lote = await _loteRepositorio.BuscarPorIdAsync(peticion.IdLote ?? 0);
            if (lote == null)
            {
                return new OperacionDto<RegistroCromatografiaDto>(CodigosOperacionDto.NoExiste, "Lote no existe");
            }

            DateTime periodo = new DateTime(peticion.Periodo.Value.Year, peticion.Periodo.Value.Month, 1);
            var registro = await _registroCromatografiaRepositorio.BuscarPorPeriodoTipoYIdLoteAsync(periodo, peticion.Tipo, peticion.IdLote);
            if (registro == null)
            {
                var datos = new RegistroCromatografiaDto
                {
                    Periodo = registro.Periodo,
                    Tipo = registro.Tipo,
                    Tanque = registro.Tanque,
                    IdLote = lote.IdLote,
                    Lote = lote.Nombre
                };
                var day = DateTime.DaysInMonth(periodo.Year, periodo.Month);

                List<RegistroGnaGnsDto> cgnDetalle = new List<RegistroGnaGnsDto>();
                for (var i = 0; i < day; i++)
                {
                    var a = new RegistroGnaGnsDto
                    {
                        Fecha = periodo.AddDays(i),
                    };
                    cgnDetalle.Add(a);
                }
                datos.GnaGns = cgnDetalle;
                return new OperacionDto<RegistroCromatografiaDto>(datos);
            }
            var dto = new RegistroCromatografiaDto
            {
                Periodo = registro.Periodo,
                Tipo = registro.Tipo,
                Tanque = registro.Tanque
            };

            //var lista = await _registroAnalisiDespachoCgnRepositorio.ListarPorIdRegistroCromatografiaAsync(registro.Id);
            //var cgn = lista.Select(e => new RegistroAnalisiDespachoCgnDto
            //{
            //    Api = e.Api,
            //    Fecha = e.Fecha,
            //    Gesp = e.Gesp,
            //    IdRegistroCromatografia = RijndaelUtilitario.EncryptRijndaelToUrl(registro.Id),
            //    NroDespacho = e.NroDespacho,
            //    P5 = e.P5,
            //    P10 = e.P10,
            //    P30 = e.P30,
            //    P50 = e.P50,
            //    P70 = e.P70,
            //    P90 = e.P90,
            //    P95 = e.P95,
            //    Pfin = e.Pfin,
            //    Pinic = e.Pinic,
            //    Rvp = e.Rvp
            //}).ToList();

            dto.GnaGns = new List<RegistroGnaGnsDto>();

            return new OperacionDto<RegistroCromatografiaDto>(dto);

        }



        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(RegistroCromatografiaDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            if (string.IsNullOrEmpty(peticion.Tipo) || !peticion.IdLote.HasValue)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Campo Tipo y Lote son requeridos");
            }

            if (peticion.GnaGns == null || peticion.GnaGns.Count == 0)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Registro por días no puede ser vacío");
            }

            if (!peticion.Periodo.HasValue)
            {
                peticion.Periodo = FechasUtilitario.ObtenerDiaOperativo();
            }

            DateTime periodo = new DateTime(peticion.Periodo.Value.Year, peticion.Periodo.Value.Month, 1);

            var registro = await _registroCromatografiaRepositorio.BuscarPorPeriodoTipoYIdLoteAsync(periodo, peticion.Tipo, peticion.IdLote);
            if (registro == null)
            {
                registro = new RegistroCromatografia();
            }
            registro.Actualizado = DateTime.UtcNow;
            registro.Periodo = periodo;
            registro.Tipo = peticion.Tipo;
            registro.Tanque = peticion.Tanque;
            registro.IdUsuario = peticion.IdUsuario;

            //if (registro.Id > 0)
            //{
            //    await _registroCromatografiaRepositorio.EditarAsync(registro);
            //}
            //else
            //{
            //    await _registroCromatografiaRepositorio.InsertarAsync(registro);
            //}

            //foreach (var item in peticion.Cgn)
            //{
            //    if (!item.Fecha.HasValue)
            //    {
            //        continue;
            //    }
            //    var entidad = new RegistroAnalisiDespachoCgn
            //    {
            //        Actualizado = DateTime.UtcNow,
            //        Api = item.Api,
            //        Fecha = item.Fecha.Value,
            //        Creado = DateTime.UtcNow,
            //        Gesp = item.Gesp,
            //        IdRegistroCromatografia = registro.Id,
            //        IdUsuario = peticion.IdUsuario,
            //        NroDespacho = item.NroDespacho,
            //        P5 = item.P5,
            //        P10 = item.P10,
            //        P30 = item.P30,
            //        P50 = item.P50,
            //        P70 = item.P70,
            //        P90 = item.P90,
            //        P95 = item.P95,
            //        Pfin = item.Pfin,
            //        Pinic = item.Pinic,
            //        Rvp = item.Rvp
            //    };
            //    await _registroAnalisiDespachoCgnRepositorio.InsertarAsync(entidad);

            //}

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
