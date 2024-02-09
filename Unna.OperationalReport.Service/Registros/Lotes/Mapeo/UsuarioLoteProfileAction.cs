using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Registros.Lotes.Mapeo
{
    
    public class UsuarioLoteProfileAction : IMappingAction<UsuarioLote, UsuarioLoteDto>
    {
        public void Process(UsuarioLote source, UsuarioLoteDto destination, ResolutionContext context)
        {
            destination.IdLote = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdLote);
            if (source.IdGrupo.HasValue)
            {
                destination.IdGrupo = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdGrupo);
            }
            
        }
    }
}
