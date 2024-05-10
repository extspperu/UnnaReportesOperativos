using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Implementaciones
{
    public class ImpresionServicio: IImpresionServicio
    {
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly IMapper _mapper;
        public ImpresionServicio(
            IImprimirRepositorio imprimirRepositorio,
            IMapper mapper
            )
        {
            _imprimirRepositorio = imprimirRepositorio;
            _mapper = mapper;
        }

        public async Task<OperacionDto<ImpresionDto>> ObtenerAsync(int idReporte, DateTime fecha)
        {
            var entidad = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(idReporte,fecha);
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
           
            try
            {
                if (entidad.IdImprimir > 0)
                {
                    _imprimirRepositorio.Editar(entidad);
                }
                else
                {
                    _imprimirRepositorio.Insertar(entidad);
                }
                await _imprimirRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();
            }
            catch (Exception ex)
            {

            }
            return new OperacionDto<RespuestaSimpleDto<string>>(new RespuestaSimpleDto<string>() { Id= RijndaelUtilitario.EncryptRijndaelToUrl(entidad.IdImprimir), Mensaje ="Se guardó correctamente" });
        }
    }
}
