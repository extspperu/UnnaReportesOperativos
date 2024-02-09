using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Mapeo
{
    public class RegistroSupervisorProfileAction : IMappingAction<Data.Reporte.Entidades.RegistroSupervisor, RegistroSupervisorDto>
    {
        public void Process(Data.Reporte.Entidades.RegistroSupervisor source, RegistroSupervisorDto destination, ResolutionContext context)
        {
            destination.Id = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdRegistroSupervisor);

            if (source.IdArchivo.HasValue)
            {
                destination.IdArchivo = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdArchivo);
            }
            
            if (source.Archivo != null)
            {
                destination.Archivo = context.Mapper.Map<ArchivoRespuestaDto>(source.Archivo);
            }
        }
    }
}
