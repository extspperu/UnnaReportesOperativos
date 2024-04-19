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
    public class ReporteServicio: IReporteServicio
    {

        private readonly IConfiguracionRepositorio _configuracionRepositorio;
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly IMapper _mapper;
        public ReporteServicio(
            IConfiguracionRepositorio configuracionRepositorio,
            IUsuarioServicio usuarioServicio,
            IMapper mapper
            )
        {
            _configuracionRepositorio = configuracionRepositorio;
            _usuarioServicio = usuarioServicio;
            _mapper = mapper;
        }

        public async Task<OperacionDto<ReporteDto>> ObtenerAsync(int id, long? idUsuario)
        {
            var entidad = await _configuracionRepositorio.BuscarPorIdYNoBorradoAsync(id);
            if (entidad == null)
            {
                return new OperacionDto<ReporteDto>(CodigosOperacionDto.NoExiste, "No existe registro");
            }

            var dto = _mapper.Map<ReporteDto>(entidad);
            var usuarioOperacion = await _usuarioServicio.ObtenerAsync(idUsuario??0);
            if (usuarioOperacion.Completado && usuarioOperacion.Resultado != null && !string.IsNullOrWhiteSpace(usuarioOperacion.Resultado.UrlFirma))
            {
                dto.UrlFirma = usuarioOperacion.Resultado.UrlFirma;
            }
            dto.Fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yy");
            return new OperacionDto<ReporteDto>(dto);
        }


    }
}
