using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.Impresiones.Mapeo
{
    public class ImpresionProfileAction : IMappingAction<Imprimir, ImpresionDto>
    {
        public void Process(Imprimir source, ImpresionDto destination, ResolutionContext context)
        {
            destination.Id = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdImprimir);
            destination.IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdConfiguracion);
        }
    }
}
