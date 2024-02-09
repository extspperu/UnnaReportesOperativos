using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Dtos;
using Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Mapeo
{
    public class AdjuntoSupervisorProfileAction : IMappingAction<Data.Reporte.Entidades.AdjuntoSupervisor, AdjuntoSupervisorDto>
    {
        public void Process(Data.Reporte.Entidades.AdjuntoSupervisor source, AdjuntoSupervisorDto destination, ResolutionContext context)
        {
            destination.Id = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdAdjuntoSupervisor);
            destination.IdRegistroSupervisor = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdRegistroSupervisor);

            if (source.IdArchivo.HasValue)
            {
                destination.IdArchivo = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdArchivo);
            }

            if (source.Archivo != null)
            {
                destination.Archivo = context.Mapper.Map<ArchivoRespuestaDto>(source.Archivo);
            }
            if (source.Adjunto != null)
            {
                destination.Adjunto = context.Mapper.Map<AdjuntoDto>(source.Adjunto);
            }
        }
    }
}
