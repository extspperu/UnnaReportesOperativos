using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones
{
    public interface IValoresDefectoReporteServicio
    {
        Task<double?> ObtenerValorAsync(string? llave);
        Task<OperacionDto<List<ValoresDefectoReporteDto>>> ListarAsync();
        Task<OperacionDto<ValoresDefectoReporteDto>> ObtenerAsync(string id);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(ValoresDefectoReporteDto peticion);
    }
}
