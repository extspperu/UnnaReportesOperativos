using AutoMapper;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.Generales.Mapeo
{
    class ReporteProfileAction : IMappingAction<Data.Reporte.Entidades.Configuracion, ReporteDto>
    {
        public void Process(Data.Reporte.Entidades.Configuracion source, ReporteDto destination, ResolutionContext context)
        {
            destination.Id = RijndaelUtilitario.EncryptRijndaelToUrl(source.Id);

        }
    }
}
