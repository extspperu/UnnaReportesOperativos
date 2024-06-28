using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.Osinergmin.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Registros.Osinergmin.Servicios.Abstracciones
{
    public interface IOsinergminServicio
    {
        Task<OperacionDto<OsinergminDto?>> ObtenerAsync(DateTime fecha);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(OsinergminDto peticion);
    }
}
