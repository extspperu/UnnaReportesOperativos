using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Registros.Lotes.Mapeo
{
    public class LoteProfileAction : IMappingAction<Data.Registro.Entidades.Lote, LoteDto>
    {
        public void Process(Data.Registro.Entidades.Lote source, LoteDto destination, ResolutionContext context)
        {
            destination.Id = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdLote);
        }
    }
}
