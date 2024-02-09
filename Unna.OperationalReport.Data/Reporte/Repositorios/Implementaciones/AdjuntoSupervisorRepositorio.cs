using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones
{
    public class AdjuntoSupervisorRepositorio : OperacionalRepositorio<AdjuntoSupervisor, long>, IAdjuntoSupervisorRepositorio
    {
        public AdjuntoSupervisorRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<AdjuntoSupervisor?> BuscarPorIdRegistroSupervisorYIdAdjuntoAsync(long? idRegistroSupervisor, int? idAdjunto)
        => await UnidadDeTrabajo.ReporteAdjuntoSupervisores.Where(e => e.IdRegistroSupervisor == idRegistroSupervisor && e.IdAdjunto == idAdjunto && e.EstaBorrado == false).FirstOrDefaultAsync();

        public async Task<List<AdjuntoSupervisor>?> LstarPorIdRegistroSupervisorAsync(long? idRegistroSupervisor)
        => await UnidadDeTrabajo.ReporteAdjuntoSupervisores.Include(e=>e.Adjunto).Where(e => e.IdRegistroSupervisor == idRegistroSupervisor && e.EstaBorrado == false).ToListAsync();

    }
}
