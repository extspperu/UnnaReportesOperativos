using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Servicios.Abstracciones
{
    public interface IFacturacionGnsLIVServicio
    {
        Task<OperacionDto<FacturacionGnsLIVDto>> ObtenerAsync(long idUsuario);
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(FacturacionGnsLIVDto peticion);
    }
}
