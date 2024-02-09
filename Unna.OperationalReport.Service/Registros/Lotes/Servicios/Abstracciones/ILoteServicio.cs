using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Registros.Lotes.Servicios.Abstracciones
{
    public interface ILoteServicio
    {
        Task<OperacionDto<List<LoteDto>>> ListarTodosAsync();
    }
}
