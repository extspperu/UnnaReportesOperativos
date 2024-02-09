using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Registros.Datos.Dtos;
using Unna.OperationalReport.Service.Registros.Datos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Registros.Datos.Servicios.Implementaciones
{
    public class DatoServicio: IDatoServicio
    {
        private readonly IDatoRepositorio _datoRepositorio;
        private readonly IMapper _mapper;
        public DatoServicio(
            IDatoRepositorio datoRepositorio,
            IMapper mapper
            )
        {
            _datoRepositorio = datoRepositorio;
            _mapper = mapper;
        }

        public async Task<OperacionDto<List<DatoDto>>> ListarPorTipoAsync(string? tipo)
        {            
            var entidad = await _datoRepositorio.ListarPorTipoAsync(tipo);
            var dto = _mapper.Map<List<DatoDto>>(entidad);
            return new OperacionDto<List<DatoDto>>(dto);
        }

    }
}
