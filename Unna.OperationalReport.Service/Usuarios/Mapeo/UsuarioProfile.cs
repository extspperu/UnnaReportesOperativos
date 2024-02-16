using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Service.Usuarios.Dtos;

namespace Unna.OperationalReport.Service.Usuarios.Mapeo
{
    public class UsuarioProfile:Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDto>()
                .AfterMap<UsuarioProfileAction>();
        }
    }
}
