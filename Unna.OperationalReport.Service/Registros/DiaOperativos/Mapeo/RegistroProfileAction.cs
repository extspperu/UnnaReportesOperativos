using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Registros.DiaOperativos.Mapeo
{    
    public class RegistroProfileAction : IMappingAction<Data.Registro.Entidades.Registro, RegistroDto>
    {
        public void Process(Data.Registro.Entidades.Registro source, RegistroDto destination, ResolutionContext context)
        {
            destination.Id = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdRegistro);
            destination.IdDiaOperativo = RijndaelUtilitario.EncryptRijndaelToUrl(source.IdDiaOperativo);
        }
    }
}
