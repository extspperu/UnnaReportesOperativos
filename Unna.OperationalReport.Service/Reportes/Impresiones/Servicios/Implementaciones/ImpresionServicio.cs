using AutoMapper;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Respaldo.Dtos;
using Unna.OperationalReport.Service.Respaldo.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Implementaciones
{
    public class ImpresionServicio : IImpresionServicio
    {
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly IMapper _mapper;
        private readonly IConfiguracionRepositorio _configuracionRepositorio;
        private readonly IRespaldoServicio _respaldoServicio;
        public ImpresionServicio(
            IImprimirRepositorio imprimirRepositorio,
            IConfiguracionRepositorio configuracionRepositorio,
            IMapper mapper,
            IRespaldoServicio respaldoServicio
            )
        {
            _imprimirRepositorio = imprimirRepositorio;
            _configuracionRepositorio = configuracionRepositorio;
            _mapper = mapper;
            _respaldoServicio = respaldoServicio;
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

            if (entidad.IdImprimir > 0)
            {
                await _imprimirRepositorio.EditarAsync(entidad);
            }
            else
            {
                await _imprimirRepositorio.InsertarAsync(entidad);
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
                case GruposReportes.Quincenal:
                    if (diaOperativo.Day < 16) diaOperativo = diaOperativo.AddMonths(-1);
                    diaOperativo = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
                    break;
                case GruposReportes.Mensual:
                    DateTime mensual = diaOperativo.AddMonths(-1);
                    diaOperativo = new DateTime(mensual.Year, mensual.Month, 16);
                    break;
            }


            var entidad = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(peticion.IdReporte, diaOperativo);
            if (entidad == null)
            {
                entidad = new Imprimir();
            }
            entidad.IdConfiguracion = peticion.IdReporte;
            if (!string.IsNullOrWhiteSpace(peticion.RutaExcel))
            {
                entidad.RutaArchivoExcel = peticion.RutaExcel;
            }
            if (!string.IsNullOrWhiteSpace(peticion.RutaPdf))
            {
                entidad.RutaArchivoPdf = peticion.RutaPdf;
            }
            entidad.Actualizado = DateTime.UtcNow;
            if (entidad.IdImprimir > 0)
            {
                await _imprimirRepositorio.ActualizarRutaArchivosAsync(entidad);
            }
            else
            {
                entidad.Fecha = diaOperativo;
                entidad.Creado = DateTime.UtcNow; 
                await _imprimirRepositorio.InsertarAsync(entidad);
            }

            //await _respaldoServicio.EnviarAsync(new RespaldoDto
            //{
            //    FilePath = entidad.RutaArchivoPdf,
            //    Nombre = $"/Reporte"
            //});

            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true, Mensaje = "Se actuazó correctamente" });
        }


    }
}
