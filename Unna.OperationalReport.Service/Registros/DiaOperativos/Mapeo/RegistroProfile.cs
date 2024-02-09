using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;

namespace Unna.OperationalReport.Service.Registros.DiaOperativos.Mapeo
{
    public class RegistroProfile: Profile
    {
        public RegistroProfile()
        {
            CreateMap<Data.Registro.Entidades.Registro, RegistroDto>()
                .AfterMap<RegistroProfileAction>();
        }
    }
}
