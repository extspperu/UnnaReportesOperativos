using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Dtos;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.Adjuntos.Servicios.Implementaciones
{
    public class AdjuntoServicio: IAdjuntoServicio
    {
        private readonly IAdjuntoRepositorio _adjuntoRepositorio;
        private readonly IMapper _mapper;
        public AdjuntoServicio(
            IAdjuntoRepositorio adjuntoRepositorio,
            IMapper mapper
            )
        {
            _adjuntoRepositorio = adjuntoRepositorio;
            _mapper = mapper;
        }

        public async Task<OperacionDto<List<AdjuntoDto>>> ListarPorGrupoAsync(string? grupo)
        {
            var entidad = await _adjuntoRepositorio.ListarPorGrupoAsync(grupo);
            var dto = _mapper.Map<List<AdjuntoDto>>(entidad);
            DateTime FechaRegistro = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow.AddDays(-1));
            string formatoFecha = FechaRegistro.ToString("dd-MM-yyyy");
            dto.ForEach(e => e.Nomenclatura = e.Nomenclatura?.Replace("FECHA", formatoFecha));

            return new OperacionDto<List<AdjuntoDto>>(dto);
        }
    }
}
