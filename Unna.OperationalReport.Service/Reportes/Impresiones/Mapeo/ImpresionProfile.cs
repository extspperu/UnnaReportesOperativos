using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;

namespace Unna.OperationalReport.Service.Reportes.Impresiones.Mapeo
{
    public class ImpresionProfile : Profile
    {
        public ImpresionProfile()
        {
            CreateMap<Imprimir, ImpresionDto>();
        }
    }
}
