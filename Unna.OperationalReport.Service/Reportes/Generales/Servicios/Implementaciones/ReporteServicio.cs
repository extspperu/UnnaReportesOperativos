using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Usuarios.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.Generales.Servicios.Implementaciones
{
    public class ReporteServicio : IReporteServicio
    {

        private readonly IConfiguracionRepositorio _configuracionRepositorio;
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly IMapper _mapper;
        private readonly DiaOperativoDemoDto _diaOperativoDemoD;
        public ReporteServicio(
            IConfiguracionRepositorio configuracionRepositorio,
            IUsuarioServicio usuarioServicio,
            IMapper mapper,
            DiaOperativoDemoDto diaOperativoDemoD
            )
        {
            _configuracionRepositorio = configuracionRepositorio;
            _usuarioServicio = usuarioServicio;
            _mapper = mapper;
            _diaOperativoDemoD = diaOperativoDemoD;
        }

        public async Task<OperacionDto<ReporteDto>> ObtenerAsync(int id, long? idUsuario)
        {
            var aa = _diaOperativoDemoD.DiaOperativo;
            var entidad = await _configuracionRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (entidad == null)
            {
                return new OperacionDto<ReporteDto>(CodigosOperacionDto.NoExiste, "No existe registro");
            }

            var dto = _mapper.Map<ReporteDto>(entidad);
            var usuarioOperacion = await _usuarioServicio.ObtenerAsync(idUsuario ?? 0);
            if (usuarioOperacion.Completado && usuarioOperacion.Resultado != null && !string.IsNullOrWhiteSpace(usuarioOperacion.Resultado.UrlFirma))
            {
                if (File.Exists(usuarioOperacion.Resultado.RutaFirma))
                {
                    dto.UrlFirma = usuarioOperacion.Resultado.UrlFirma;
                    dto.RutaFirma = usuarioOperacion.Resultado.RutaFirma;
                }               
            }
            return new OperacionDto<ReporteDto>(dto);
        }

        public async Task<OperacionDto<ReporteDto>> ObtenerAsync(string? idReporte)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idReporte);
            var entidad = await _configuracionRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (entidad == null)
            {
                return new OperacionDto<ReporteDto>(CodigosOperacionDto.NoExiste, "No existe registro");
            }
            var dto = _mapper.Map<ReporteDto>(entidad);           
            return new OperacionDto<ReporteDto>(dto);
        }

        public async Task<OperacionDto<List<ReporteDto>>> ListarAsync()
        {
            var entidad = await _configuracionRepositorio.ListarAsync();
            var dto = _mapper.Map<List<ReporteDto>>(entidad);
            return new OperacionDto<List<ReporteDto>>(dto);
        }


        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ReporteDto peticion)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(peticion.Id);
            var entidad = await _configuracionRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (entidad == null)
            {
                entidad = new Data.Reporte.Entidades.Configuracion();
            }
            entidad.NombreReporte = peticion.NombreReporte;
            entidad.Nombre = peticion.Nombre;
            entidad.Version = peticion.Version;
            entidad.PreparadoPor = peticion.PreparadoPor;
            entidad.Detalle = peticion.Detalle;
            entidad.Grupo = peticion.Grupo;
            entidad.Fecha = peticion.Fecha;
            entidad.Actualizado = DateTime.UtcNow;

            if (entidad.Id > 0)
            {
                _configuracionRepositorio.Editar(entidad);
            }
            else
            {
                _configuracionRepositorio.Insertar(entidad);
            }
            await _configuracionRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();

            return new OperacionDto<RespuestaSimpleDto<string>>(new RespuestaSimpleDto<string> { Id = RijndaelUtilitario.EncryptRijndaelToUrl(entidad.Id), Mensaje = "Se guardó correctamente" });
        }



    }
}
