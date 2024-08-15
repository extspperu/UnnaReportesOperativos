using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Carta.Enums;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
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
        private readonly IRegistroPuntoFiscalizacionGnaGnsRepositorio _registroPuntoFiscalizacionGnaGnsRepositorio;
        public GnaGnsServicio(
            IRegistroCromatografiaRepositorio registroCromatografiaRepositorio,
            ILoteRepositorio loteRepositorio,
            IRegistroPuntoFiscalizacionGnaGnsRepositorio registroPuntoFiscalizacionGnaGnsRepositorio
            )
        {
            _registroCromatografiaRepositorio = registroCromatografiaRepositorio;
            _loteRepositorio = loteRepositorio;
            _registroPuntoFiscalizacionGnaGnsRepositorio = registroPuntoFiscalizacionGnaGnsRepositorio;
        }

        public async Task<OperacionDto<RegistroCromatografiaDto>> ObtenerAsync(BuscarGnaGnsDto peticion)
        {
            if (!peticion.Periodo.HasValue)
            {
                return new OperacionDto<RegistroCromatografiaDto>(CodigosOperacionDto.NoExiste, "Periodo es requerido");
            }

            if (string.IsNullOrEmpty(peticion.Tipo))
            {
                return new OperacionDto<RegistroCromatografiaDto>(CodigosOperacionDto.NoExiste, "Campo tipo es requerido");
            }

            Lote? lote = default(Lote?);
            if (peticion.Tipo.Equals(TiposRegistroGna.GNA))
            {
                lote = await _loteRepositorio.BuscarPorIdAsync(peticion.IdLote ?? 0);
                if (lote == null)
                {
                    return new OperacionDto<RegistroCromatografiaDto>(CodigosOperacionDto.NoExiste, $"Para el tipo Lote {peticion.Tipo}, el lote es requerido");
                }
            }
            else
            {
                peticion.IdLote = new int?();
            }
           

            DateTime periodo = new DateTime(peticion.Periodo.Value.Year, peticion.Periodo.Value.Month, 1);
            var registro = await _registroCromatografiaRepositorio.BuscarPorPeriodoTipoYIdLoteAsync(periodo, peticion.Tipo, peticion.IdLote);
            if (registro == null)
            {
                var datos = new RegistroCromatografiaDto
                {
                    Periodo = periodo,
                    Tipo = peticion.Tipo,
                    IdLote = lote?.IdLote,
                    Lote = lote?.Nombre,
                    
                };
                var day = DateTime.DaysInMonth(periodo.Year, periodo.Month);

                List<RegistroGnaGnsDto> cgnDetalle = new List<RegistroGnaGnsDto>();
                for (var i = 0; i < day; i++)
                {
                    var a = new RegistroGnaGnsDto
                    {
                        Fecha = periodo.AddDays(i),                        
                    };
                    a.Day = a.Fecha.HasValue ? a.Fecha.Value.Day : null;
                    cgnDetalle.Add(a);
                }
                datos.GnaGns = cgnDetalle;
                return new OperacionDto<RegistroCromatografiaDto>(datos);
            }
            var dto = new RegistroCromatografiaDto
            {
                Periodo = registro.Periodo,
                Tipo = registro.Tipo,
                Tanque = registro.Tanque,
                IdLote = registro.IdLote,
            };

            var lista = await _registroPuntoFiscalizacionGnaGnsRepositorio.ListarPorIdRegistroCromatografiaAsync(registro.Id);
            var gnaGns = lista.Select(e => new RegistroGnaGnsDto
            {
                Fecha = e.Fecha,
                C6 = e.C6,
                C3 = e.C3,
                Ic4 = e.Ic4,
                Nc4 = e.Nc4,
                NeoC5 = e.NeoC5,
                Ic5 = e.Ic5,
                Nc5 = e.Nc5,
                Nitrog = e.Nitrog,
                C1 = e.C1,
                Co2 = e.Co2,
                C2 = e.C2,
                O2 = e.O2,
                Grav = e.Grav,
                Btu = e.Btu,
                Lgn = e.Lgn,
                LgnRpte = e.LgnRpte,
                Conciliado = e.Conciliado,
                Comentario = e.Comentario,
            }).ToList();
            dto.GnaGns = gnaGns;
            if (gnaGns.Count == 0)
            {
                var day = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                List<RegistroGnaGnsDto> listaFechas = new List<RegistroGnaGnsDto>();
                for (var i = 0; i < day; i++)
                {
                    var a = new RegistroGnaGnsDto
                    {
                        Fecha = periodo.AddDays(i),
                    };
                    a.Day = a.Fecha.HasValue ? a.Fecha.Value.Day : null;
                    listaFechas.Add(a);
                }
                dto.GnaGns = listaFechas;
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

            if (peticion.Tipo.Equals(TiposRegistroGna.GNA))
            {
                if (!peticion.IdLote.HasValue)
                {
                    return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, $"Para el tipo ${peticion.Tipo}, lote es requerido");
                }                
            }
            else
            {
                peticion.IdLote = new int?();
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
            registro.IdLote = peticion.IdLote;
            registro.Tanque = peticion.Tanque;
            registro.IdUsuario = peticion.IdUsuario;            
            registro.HoraMuestreo = peticion.HoraMuestreo;

            if (registro.Id > 0)
            {
                await _registroCromatografiaRepositorio.EditarAsync(registro);
            }
            else
            {
                await _registroCromatografiaRepositorio.InsertarAsync(registro);
            }

            await _registroPuntoFiscalizacionGnaGnsRepositorio.EliminarAsync(new RegistroPuntoFiscalizacionGnaGns
            {
                IdRegistroCromatografia = registro.Id
            });

            foreach (var item in peticion.GnaGns)
            {
                if (!item.Day.HasValue)
                {
                    continue;
                }
                var entidad = new RegistroPuntoFiscalizacionGnaGns
                {
                    Fecha = item.Fecha.HasValue ? item.Fecha.Value : new DateTime(periodo.Year, periodo.Month, item.Day.Value),
                    C6 = item.C6,
                    C3 = item.C3,
                    Ic4 = item.Ic4,
                    Nc4 = item.Nc4,
                    IdRegistroCromatografia = registro.Id,
                    IdUsuario = peticion.IdUsuario,
                    NeoC5 = item.NeoC5,
                    Ic5 = item.Ic5,
                    Nc5 = item.Nc5,
                    Nitrog = item.Nitrog,
                    C1 = item.C1,
                    Co2 = item.Co2,
                    C2 = item.C2,
                    O2 = item.O2,
                    Grav = item.Grav,
                    Btu = item.Btu,
                    Lgn = item.Lgn,
                    LgnRpte = item.LgnRpte,
                    Conciliado = item.Conciliado,
                    Comentario = item.Comentario,
                    Creado = DateTime.UtcNow,
                    Actualizado = DateTime.UtcNow                    
                };
                await _registroPuntoFiscalizacionGnaGnsRepositorio.InsertarAsync(entidad);

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
