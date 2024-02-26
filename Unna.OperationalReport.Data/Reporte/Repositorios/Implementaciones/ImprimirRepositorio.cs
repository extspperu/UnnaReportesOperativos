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
    public class ImprimirRepositorio : OperacionalRepositorio<Imprimir, long>, IImprimirRepositorio
    {
        public ImprimirRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<Imprimir?> BuscarPorIdConfiguracionYFechaAsync(int idConfiguracion, DateTime? fecha)
        => await UnidadDeTrabajo.ReporteImpresiones.Where(e => e.IdConfiguracion == idConfiguracion && e.Fecha == fecha && e.EstaBorrado == false).FirstOrDefaultAsync();


    }
}
