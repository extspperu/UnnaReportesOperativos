using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones
{
    public interface IRegistroServicio
    {
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(List<RegistroDto>? peticion, long? idUsuario, bool? esEditado);
    }
}
