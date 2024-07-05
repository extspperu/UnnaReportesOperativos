using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Dtos;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Implementaciones
{
    public class ValoresDefectoReporteServicio: IValoresDefectoReporteServicio
    {

        private readonly IValoresDefectoReporteRepositorio _valoresDefectoReporteRepositorio;
        public ValoresDefectoReporteServicio(IValoresDefectoReporteRepositorio valoresDefectoReporteRepositorio)
        {
            _valoresDefectoReporteRepositorio = valoresDefectoReporteRepositorio;
        }

        public async Task<double?> ObtenerValorAsync(string? llave)
        {
            if (string.IsNullOrWhiteSpace(llave))
            {
                return new double?();
            }
            var entidad = await _valoresDefectoReporteRepositorio.BuscarPorLlaveAsync(llave);
            if (entidad == null)
            {
                return new double?();
            }

            return entidad.Valor;
        }


        public async Task<OperacionDto<List<ValoresDefectoReporteDto>>> ListarAsync()
        {

            var entidad = await _valoresDefectoReporteRepositorio.ListarAsync();

            var dto = entidad?.Select(e=> new ValoresDefectoReporteDto
            {
                Id = RijndaelUtilitario.EncryptRijndaelToUrl(e.Llave),
                Llave = e.Llave,
                Valor = e.Valor,
                Comentario = e.Comentario,
                EstaHabilitado = e.EstaHabilitado,
                Creado = FechasUtilitario.ObtenerFechaSegunZonaHoraria(e.Creado).ToString("dd/MM/yyyy HH:mm:ss"),
                Actualizado = FechasUtilitario.ObtenerFechaSegunZonaHoraria(e.Creado).ToString("dd/MM/yyyy HH:mm:ss")
            }).ToList();
            return new OperacionDto<List<ValoresDefectoReporteDto>>(dto);

        }

        public async Task<OperacionDto<ValoresDefectoReporteDto>> ObtenerAsync(string id)
        {
            var llave = RijndaelUtilitario.DecryptRijndaelFromUrl<string>(id);
            var entidad = await _valoresDefectoReporteRepositorio.BuscarPorLlaveAsync(llave);
            if (entidad == null)
            {
                return new OperacionDto<ValoresDefectoReporteDto>(CodigosOperacionDto.NoExiste, "No existe registro");
            }
            var dto = new ValoresDefectoReporteDto
            {
                Id = RijndaelUtilitario.EncryptRijndaelToUrl(entidad.Llave),
                Llave = entidad.Llave,
                Valor = entidad.Valor,
                Comentario = entidad.Comentario,
                EstaHabilitado = entidad.EstaHabilitado,
                Creado = FechasUtilitario.ObtenerFechaSegunZonaHoraria(entidad.Creado).ToString("dd/MM/yyyy HH:mm:ss"),
                Actualizado = FechasUtilitario.ObtenerFechaSegunZonaHoraria(entidad.Creado).ToString("dd/MM/yyyy HH:mm:ss")
            };
            return new OperacionDto<ValoresDefectoReporteDto>(dto);

        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(ValoresDefectoReporteDto peticion)
        {
         
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }


            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id=true, Mensaje = "Se guardó correctamente"});

        }


    }
}
