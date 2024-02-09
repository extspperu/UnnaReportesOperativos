using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;

namespace Unna.OperationalReport.Service.Registros.DiaOperativos.Mapeo
{
    public class DiaOperativoProfile:Profile
    {
        public DiaOperativoProfile()
        {
            CreateMap<DiaOperativo, DiaOperativoDto>()
                .AfterMap<DiaOperativoProfileAction>();
        }
    }
}
