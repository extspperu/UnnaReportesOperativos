using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Servicios.Abstracciones
{
    public interface IBoletadeValorizacionPetroperuLoteZ69Servicio
    {
        Task<OperacionDto<BoletadeValorizacionPetroperuLoteZ69Dto>> ObtenerAsync(long idUsuario);
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletadeValorizacionPetroperuLoteZ69Dto peticion);
    }
}
