using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;

namespace Unna.OperationalReport.Service.Configuraciones.Archivos.Mapeo
{
    public class ArchivoProfile : Profile
    {
        public ArchivoProfile()
        {
            CreateMap<Archivo, ArchivoRespuestaDto>()
                .AfterMap<ArchivoProfileAction>();
        }
    }
}
