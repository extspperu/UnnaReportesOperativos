using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Service.Usuarios.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Usuarios.Mapeo
{
 
    public class UsuarioProfileAction : IMappingAction<Usuario, UsuarioDto>
    {
        public void Process(Usuario source, UsuarioDto destination, ResolutionContext context)
        {
            destination.IdPersona = source.IdPersona.HasValue ? RijndaelUtilitario.EncryptRijndaelToUrl(source.IdPersona):null;
            if (source.Persona != null)
            {
                destination.Documento = source.Persona.Documento;   
                destination.Paterno = source.Persona.Paterno;   
                destination.Materno = source.Persona.Materno;   
                destination.Nombres = source.Persona.Nombres;   
            }
        }
    }
}
