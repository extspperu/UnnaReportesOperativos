using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Servicios.Abstracciones
{
    public interface IComposicionGnaLIVServicio
    {
        Task<OperacionDto<ComposicionGnaLIVDto>> ObtenerAsync(long idUsuario, string? grupo);
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ComposicionGnaLIVDto peticion);
    }
}
