using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;

namespace Unna.OperationalReport.Service.Registros.Lotes.Mapeo
{
    public class LoteProfile : Profile
    {
        public LoteProfile()
        {
            CreateMap<Lote, LoteDto>()
                .AfterMap<LoteProfileAction>();
        }
    }
}
