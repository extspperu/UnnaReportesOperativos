using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Cartas.Dgh.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Abstracciones
{
    public interface ICartaDghServicio
    {
        Task<OperacionDto<CartaDto>> ObtenerAsync(long idUsuario, DateTime diaOperativo, string idCarta);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(CartaDto peticion);
        Task<CartaSolicitudDto> SolicitudAsync(DateTime diaOperativo, int idCarta, string? numero);
    }
}
