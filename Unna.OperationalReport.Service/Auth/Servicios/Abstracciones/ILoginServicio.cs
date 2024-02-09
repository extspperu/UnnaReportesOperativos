using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Auth.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Auth.Servicios.Abstracciones
{
    public interface ILoginServicio
    {
        Task<OperacionDto<LoginRespuestaDto>> LoginAsync(LoginPeticionDto peticion);
    }
}
