using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Propiedad.Entidades;
using Unna.OperationalReport.Data.Propiedad.Procedimientos;
using Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Propiedades.ComposicionGnaPromedios.Dtos;
using Unna.OperationalReport.Service.Propiedades.ComposicionGnaPromedios.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Propiedades.ComposicionGnaPromedios.Servicios.Implementaciones
{
    public class ComposicionGnaPromedioServicio : IComposicionGnaPromedioServicio
    {
        private readonly IComposicionGnaPromedioRepositorio _composicionGnaPromedioRepositorio;
        public ComposicionGnaPromedioServicio(IComposicionGnaPromedioRepositorio composicionGnaPromedioRepositorio)
        {
            _composicionGnaPromedioRepositorio = composicionGnaPromedioRepositorio;
        }

        public async Task<OperacionDto<List<ComposicionGnaPromedioDto>>> ListarPorIdLoteYFechaAsync(DateTime? fecha, string? idLote)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idLote);
            var entidad = await _composicionGnaPromedioRepositorio.ListarPorIdLoteYFechaAsync(fecha, id);

            var dto = entidad.Select(e => new ComposicionGnaPromedioDto
            {
                Lote = e.Lote,
                Fecha = e.Fecha,
                Porcentaje = e.Porcentaje,
                IdSuministrador = e.IdSuministrador,
                Suministrador = e.Suministrador
            }).ToList();

            return new OperacionDto<List<ComposicionGnaPromedioDto>>(dto);

        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(List<ComposicionGnaPromedioDto> peticion, DateTime? fecha, string? idLote, long? idUsuario)
        {

            if (peticion == null || peticion.Count == 0)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Debe ingresar la lista componentes");
            }

            if(peticion.Where(e=>e.Porcentaje == null).Count() == peticion.Count())
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Todos los componentes no pueden tener valores nulos");
            }

            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idLote);

            if (!fecha.HasValue)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Fecha es requerido");
            }

            await _composicionGnaPromedioRepositorio.EliminarAsync(new ComposicionGnaPromedio
            {
                Fecha = fecha.Value,
                IdLote = id,
            });


            foreach (var item in peticion)
            {
                if (!item.IdSuministrador.HasValue)
                {
                    continue;
                }
                await _composicionGnaPromedioRepositorio.InsertarAsync(new ComposicionGnaPromedio
                {
                    Fecha = fecha.Value,
                    IdLote = id,
                    IdUsuario = idUsuario,
                    Porcentaje = item.Porcentaje,
                    IdSuministradorComponente = item.IdSuministrador.Value,
                });
            }
            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true, Mensaje = "Se guardó correctamente" });
        }


    }
}
