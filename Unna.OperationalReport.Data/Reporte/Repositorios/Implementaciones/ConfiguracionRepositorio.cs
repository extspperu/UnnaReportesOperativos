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
    public class ConfiguracionRepositorio : OperacionalRepositorio<Entidades.Configuracion, int>, IConfiguracionRepositorio
    {
        public ConfiguracionRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public override async Task<Entidades.Configuracion?> BuscarPorIdYNoBorradoAsync(int id)
       => await UnidadDeTrabajo.ReporteConfiguraciones.Where(e => e.Id == id && e.EstaBorrado == false).FirstOrDefaultAsync();

        public async Task<List<Entidades.Configuracion>?> ListarAsync()
        => await UnidadDeTrabajo.ReporteConfiguraciones.Where(e => e.EstaBorrado == false).ToListAsync();


    }
}
