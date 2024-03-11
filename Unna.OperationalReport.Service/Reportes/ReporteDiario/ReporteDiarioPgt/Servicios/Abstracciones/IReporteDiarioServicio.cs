using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Servicios.Abstracciones
{
    public interface IReporteDiarioServicio
    {
        Task<OperacionDto<ReporteDiarioDto>> ObtenerAsync(long? idUsuario);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(ReporteExistenciaDto peticion);
    }
}
