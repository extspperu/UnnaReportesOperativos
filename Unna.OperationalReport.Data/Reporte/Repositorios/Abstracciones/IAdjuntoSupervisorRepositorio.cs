using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones
{
    public interface IAdjuntoSupervisorRepositorio : IOperacionalRepositorio<AdjuntoSupervisor, long>
    {
        Task<AdjuntoSupervisor?> BuscarPorIdRegistroSupervisorYIdAdjuntoAsync(long? idRegistroSupervisor, int? idAdjunto);
        Task<List<AdjuntoSupervisor>?> LstarPorIdRegistroSupervisorAsync(long? idRegistroSupervisor);
    }
}
