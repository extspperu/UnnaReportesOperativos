using System;
using System.Collections.Generic;
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
    public class GlpServicio: IGlpServicio
    {

        private readonly IRegistroCromatografiaRepositorio _registroCromatografiaRepositorio;
        private readonly IRegistroAnalisiDespachoCgnRepositorio _registroAnalisiDespachoCgnRepositorio;
        public GlpServicio(
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
            var registro = await _registroCromatografiaRepositorio.BuscarPorPeriodoYTipoAsync(periodo, TiposProducto.GLP);
            if (registro == null)
            {
                var datos = new RegistroCromatografiaDto
                {
                    Periodo = registro.Periodo,
                    Tipo = registro.Tipo,
                };

                var day = DateTime.DaysInMonth(periodo.Year, periodo.Month);

                List<RegistroGlpDto> glp = new List<RegistroGlpDto>();
                for (var i = 0; i < day; i++)
                {
                    var a = new RegistroGlpDto
                    {
                        Fecha = periodo.AddDays(i),
                    };
                    glp.Add(a);
                }
                datos.Glp = glp;
                return new OperacionDto<RegistroCromatografiaDto>(datos);
            }
            var dto = new RegistroCromatografiaDto
            {
                Periodo = registro.Periodo,
                Tipo = registro.Tipo
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
                Rvp = e.Rvp
            }).ToList();


            return new OperacionDto<RegistroCromatografiaDto>(dto);

        }


        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(RegistroCromatografiaDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            if (string.IsNullOrEmpty(peticion.Tipo))
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Campo Tipo es requerido");
            }

            if (peticion.Glp == null || peticion.Glp.Count == 0)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Registro por días no puede ser vacío");
            }

            if (!peticion.Periodo.HasValue)
            {
                peticion.Periodo = FechasUtilitario.ObtenerDiaOperativo();
            }

            DateTime periodo = new DateTime(peticion.Periodo.Value.Year, peticion.Periodo.Value.Month, 1);

            var registro = await _registroCromatografiaRepositorio.BuscarPorPeriodoYTipoAsync(periodo, peticion.Tipo);
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
