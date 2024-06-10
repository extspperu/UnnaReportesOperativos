using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Servicios.Abstracciones
{
    public interface IBoletadeValorizacionPetroperuLoteIServicio
    {
        Task<OperacionDto<BoletadeValorizacionPetroperuLoteIDto>> ObtenerAsync(long idUsuario);
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletadeValorizacionPetroperuLoteIDto peticion);
    }
}
