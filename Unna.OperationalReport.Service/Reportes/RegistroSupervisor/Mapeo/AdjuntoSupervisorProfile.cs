using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Dtos;

namespace Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Mapeo
{
    public class AdjuntoSupervisorProfile : Profile
    {
        public AdjuntoSupervisorProfile()
        {
            CreateMap<Data.Reporte.Entidades.AdjuntoSupervisor, AdjuntoSupervisorDto>()
                .AfterMap<AdjuntoSupervisorProfileAction>();
        }
    }
}
