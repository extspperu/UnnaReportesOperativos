using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Implementaciones
{
    public class ImpresionServicio : IImpresionServicio
    {
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly IMapper _mapper;
        private readonly IConfiguracionRepositorio _configuracionRepositorio;
        public ImpresionServicio(
            IImprimirRepositorio imprimirRepositorio,
            IConfiguracionRepositorio configuracionRepositorio,
            IMapper mapper
            )
        {
            _imprimirRepositorio = imprimirRepositorio;
            _configuracionRepositorio = configuracionRepositorio;
            _mapper = mapper;
        }

        public async Task<OperacionDto<ImpresionDto>> ObtenerAsync(int idReporte, DateTime fecha)
        {
            var entidad = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(idReporte, fecha);
            if (entidad == null)
            {
                return new OperacionDto<ImpresionDto>(CodigosOperacionDto.NoExiste, "No existe dato");
            }
            var dto = _mapper.Map<ImpresionDto>(entidad);
            return new OperacionDto<ImpresionDto>(dto);
        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ImpresionDto peticion)
        {
            var idReporte = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(peticion.IdConfiguracion);
            var entidad = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(idReporte, peticion.Fecha);
            if (entidad == null)
            {
                entidad = new Imprimir();
            }
            entidad.IdConfiguracion = idReporte;
            entidad.Datos = peticion.Datos;
            entidad.Actualizado = DateTime.UtcNow;
            entidad.IdUsuario = peticion.IdUsuario;
            entidad.Fecha = peticion.Fecha;
            entidad.Comentario = peticion.Comentario;
            entidad.EsEditado = peticion.EsEditado;

            try
            {
                if (entidad.IdImprimir > 0)
                {
                   await _imprimirRepositorio.EditarAsync(entidad);
                }
                else
                {
                    await _imprimirRepositorio.InsertarAsync(entidad);
                    //await _imprimirRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();
                }                
            }
            catch (Exception ex)
            {

            }
            return new OperacionDto<RespuestaSimpleDto<string>>(new RespuestaSimpleDto<string>() { Id = RijndaelUtilitario.EncryptRijndaelToUrl(entidad.IdImprimir), Mensaje = "Se guardó correctamente" });
        }


        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarRutaArchivosAsync(GuardarRutaArchivosDto peticion)
        {
            var reporte = await _configuracionRepositorio.BuscarPorIdYNoBorradoAsync(peticion.IdReporte);
            if (reporte == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Reporte no existe");
            }
            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            switch (reporte.Grupo)
            {
                case TiposGruposReportes.Mensual:
                case TiposGruposReportes.Quincenal:
                    DateTime fecha = diaOperativo.AddDays(1).AddMonths(-1);
                    diaOperativo = new DateTime(fecha.Year, fecha.Month, 1);
                    break;
            }


            var entidad = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(peticion.IdReporte, diaOperativo);
            if (entidad == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "No existe registro");
            }
            entidad.RutaArchivoExcel = peticion.RutaExcel;
            entidad.RutaArchivoPdf = peticion.RutaPdf;
            entidad.Actualizado = DateTime.UtcNow;
            await _imprimirRepositorio.ActualizarRutaArchivosAsync(entidad);

            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true, Mensaje = "Se actuazó correctamente" });
        }


    }
}
