using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Laboratorio.CGN.Dtos;

namespace Unna.OperationalReport.Service.Laboratorio.CGN.Servicios.Abstracciones
{
    public interface ICGN
    {
        List<CGNDataDto> ObtenerAsync();
    }
}
