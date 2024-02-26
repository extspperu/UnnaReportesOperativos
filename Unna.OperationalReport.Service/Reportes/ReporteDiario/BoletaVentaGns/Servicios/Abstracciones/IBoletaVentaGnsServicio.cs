using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Abstracciones
{
    public interface IBoletaVentaGnsServicio
    {
        Task<OperacionDto<BoletaVentaGnsDto>> ObtenerAsync(long idUsuario);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(BoletaVentaGnsDto peticion);
    }
}
