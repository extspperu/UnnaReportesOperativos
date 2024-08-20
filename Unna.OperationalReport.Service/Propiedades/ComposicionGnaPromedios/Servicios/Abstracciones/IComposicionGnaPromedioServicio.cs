using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Propiedades.ComposicionGnaPromedios.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Propiedades.ComposicionGnaPromedios.Servicios.Abstracciones
{
    public interface IComposicionGnaPromedioServicio
    {
        Task<OperacionDto<List<ComposicionGnaPromedioDto>>> ListarPorIdLoteYFechaAsync(DateTime? fecha, string? idLote);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(List<ComposicionGnaPromedioDto> peticion, DateTime? fecha, string? idLote, long? idUsuario);
    }
}
