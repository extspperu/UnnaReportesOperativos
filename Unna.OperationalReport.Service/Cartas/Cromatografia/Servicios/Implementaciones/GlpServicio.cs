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
        private readonly IRegistroCromatografiaGlpRepositorio _registroCromatografiaGlpRepositorio;
        public GlpServicio(
            IRegistroCromatografiaRepositorio registroCromatografiaRepositorio,
            IRegistroCromatografiaGlpRepositorio registroCromatografiaGlpRepositorio
            )
        {
            _registroCromatografiaRepositorio = registroCromatografiaRepositorio;
            _registroCromatografiaGlpRepositorio = registroCromatografiaGlpRepositorio;
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
                    a.Day = a.Fecha.HasValue ? a.Fecha.Value.Day : null;
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

            var lista = await _registroCromatografiaGlpRepositorio.ListarPorIdRegistroCromatografiaAsync(registro.Id);
            var glpLista = lista.Select(e => new RegistroGlpDto
            {
                Fecha = e.Fecha,
                C1 = e.C1,
                C2 = e.C2,
                C3 = e.C3,
                IC4 = e.Ic4,
                NC4 = e.Nc4,
                NeoC5 = e.NeoC5,
                IC5 = e.Ic5,
                NC5 = e.Nc5,
                C6Plus = e.C6,
                DRel = e.Drel,
                PresionVapor = e.PresionVapor,
                T95 = e.T95,
                PorcentajeMolarTotal = e.MolarTotal,
                TK = e.Tk,
                Despachos = e.NroDespacho  
            }).ToList();
            dto.Glp = glpLista;
            if (glpLista.Count == 0)
            {
                var day = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                List<RegistroGlpDto> listaFechas = new List<RegistroGlpDto>();
                for (var i = 0; i < day; i++)
                {
                    var a = new RegistroGlpDto
                    {
                        Fecha = periodo.AddDays(i),
                    };
                    a.Day = a.Fecha.HasValue ? a.Fecha.Value.Day : null;
                    listaFechas.Add(a);
                }
                dto.Glp = listaFechas;
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

            await _registroCromatografiaGlpRepositorio.EliminarAsync(new RegistroCromatografiaGlp
            {
                IdRegistroCromatografia = registro.Id
            });

            foreach (var item in peticion.Glp)
            {
                if (!item.Day.HasValue)
                {
                    continue;
                }
                var entidad = new RegistroCromatografiaGlp
                {
                    Fecha = item.Fecha.HasValue ? item.Fecha.Value : new DateTime(periodo.Year, periodo.Month, item.Day.Value),
                    C1 = item.C1,
                    C2 = item.C2,
                    C3 = item.C3,
                    Ic4 = item.IC4,
                    Nc4 = item.NC4,
                    NeoC5 = item.NeoC5,
                    Ic5 = item.IC5,
                    Nc5 = item.NC5,
                    C6 = item.C6Plus,
                    Drel = item.DRel,
                    PresionVapor = item.PresionVapor,
                    T95 = item.T95,
                    MolarTotal = item.PorcentajeMolarTotal,
                    Tk = item.TK,
                    NroDespacho = item.Despachos,
                    Creado = DateTime.UtcNow,
                    Actualizado = DateTime.UtcNow,
                    IdRegistroCromatografia = registro.Id,
                    IdUsuario = peticion.IdUsuario
                };
                await _registroCromatografiaGlpRepositorio.InsertarAsync(entidad);

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
