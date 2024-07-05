using AutoMapper;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
namespace Unna.OperationalReport.Service.Reportes.Generales.Mapeo
{
   
    public class ReporteProfile : Profile
    {
        public ReporteProfile()
        {
            CreateMap<Data.Reporte.Entidades.Configuracion, ReporteDto>()
                .AfterMap<ReporteProfileAction>();
        }
    }
}
