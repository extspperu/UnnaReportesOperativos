using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;
using Unna.OperationalReport.Service.Registros.Lotes.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Registros.Lotes.Servicios.Implementaciones
{
    public class LoteServicio: ILoteServicio
    {

        private readonly ILoteRepositorio _loteRepositorio;
        private readonly IMapper _mapper;
        public LoteServicio(
            ILoteRepositorio loteRepositorio,
            IMapper mapper
            )
        {
            _loteRepositorio = loteRepositorio;
            _mapper = mapper;
        }


        public async Task<OperacionDto<List<LoteDto>>> ListarTodosAsync()
        {
            var lotes = await _loteRepositorio.ListarTodosAsync();
            var dto = _mapper.Map<List<LoteDto>>(lotes);
            return new OperacionDto<List<LoteDto>>(dto);
        }
    }
}
