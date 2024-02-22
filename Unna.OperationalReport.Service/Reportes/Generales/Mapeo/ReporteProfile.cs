using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Mapeo;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.Generales.Mapeo
{
   

    public class ReporteProfile : Profile
    {
        public ReporteProfile()
        {
            CreateMap<Data.Reporte.Entidades.Configuracion, ReporteDto>();
        }
    }
}
