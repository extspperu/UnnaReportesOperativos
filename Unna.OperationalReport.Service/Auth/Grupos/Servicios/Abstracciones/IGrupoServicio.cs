using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Auth.Grupos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Auth.Grupos.Servicios.Abstracciones
{
    public interface IGrupoServicio
    {
        Task<OperacionDto<List<GrupoDto>>> ListarAsync();
    }
}
