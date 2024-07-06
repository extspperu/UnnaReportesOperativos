using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones
{
    public interface IImpresionServicio
    {
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ImpresionDto peticion);
        Task<OperacionDto<ImpresionDto>> ObtenerAsync(int idReporte, DateTime fecha);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarRutaArchivosAsync(GuardarRutaArchivosDto peticion);


    }
}
