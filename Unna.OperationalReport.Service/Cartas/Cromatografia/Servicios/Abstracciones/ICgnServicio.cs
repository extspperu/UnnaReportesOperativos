using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Cartas.Cromatografia.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Cartas.Cromatografia.Servicios.Abstracciones
{
    public interface ICgnServicio
    {
        Task<OperacionDto<RegistroCromatografiaDto>> ObtenerAsync();
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(RegistroCromatografiaDto peticion);
    }
}
