using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Dtos;

namespace Unna.OperationalReport.Service.Reportes.Adjuntos.Mapeo
{
    public class AdjuntoProfile:Profile
    {
        public AdjuntoProfile()
        {
            CreateMap<Adjunto, AdjuntoDto>();
        }
    }
}
