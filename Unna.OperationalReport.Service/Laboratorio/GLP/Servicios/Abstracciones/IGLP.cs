 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Laboratorio.GLP.Dto;

namespace Unna.OperationalReport.Service.Laboratorio.GLP.Servicios.Abstracciones
{
    public interface IGLP
    {
        List<GLPDataDto> ObtenerAsync();
    }
}
