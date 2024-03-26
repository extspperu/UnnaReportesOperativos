using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Servicios.Abstracciones
{
    public interface IFiscalizacionProductosServicio
    {
        Task<OperacionDto<FiscalizacionProductosDto>> ObtenerAsync(long? idUsuario);
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(FiscalizacionProductosDto peticion);
    }
}
