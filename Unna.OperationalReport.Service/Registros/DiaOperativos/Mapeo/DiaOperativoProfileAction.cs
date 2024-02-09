using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Registros.DiaOperativos.Mapeo
{
    
    public class DiaOperativoProfileAction : IMappingAction<Data.Registro.Entidades.DiaOperativo, DiaOperativoDto>
    {
        public void Process(Data.Registro.Entidades.DiaOperativo source, DiaOperativoDto destination, ResolutionContext context)
        {
            destination.Id = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdDiaOperativo);
            destination.IdGrupo = source.IdGrupo.HasValue ? RijndaelUtilitario.EncryptRijndaelToUrl(source.IdGrupo):null;

            if (source.Registros != null)
            {
                destination.Registros = context.Mapper.Map<List<RegistroDto>>(source.Registros);
            }
            if (source.Lote != null)
            {
                destination.Lote = context.Mapper.Map<LoteDto>(source.Lote);
            }
        }
    }
}
