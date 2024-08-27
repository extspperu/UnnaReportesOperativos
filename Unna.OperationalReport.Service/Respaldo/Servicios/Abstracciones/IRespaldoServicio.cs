using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Respaldo.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Respaldo.Servicios.Abstracciones
{
    public interface IRespaldoServicio
    {
        Task<OperacionDto<RespuestaSimpleDto<bool>>> EnviarAsync(RespaldoDto peticion);
    }
}
