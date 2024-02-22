using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones
{
    public interface IReporteServicio
    {
        Task<OperacionDto<ReporteDto>> ObtenerAsync(int id, long? idUsuario);
    }
}
