using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Servicios.Abstracciones
{
    public interface IReporteExistenciaServicio
    {
        Task<OperacionDto<ReporteExistenciaDto>> ObtenerAsync(long idUsuario);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(ReporteExistenciaDto peticion);
    }
}
