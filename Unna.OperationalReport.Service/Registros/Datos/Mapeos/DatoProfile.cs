using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Registros.Datos.Dtos;

namespace Unna.OperationalReport.Service.Registros.Datos.Mapeos
{
    public class DatoProfile:Profile
    {
        public DatoProfile()
        {
            CreateMap<Dato, DatoDto>();
        }
    }
}
