using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones
{
    public class LoteRepositorio : OperacionalRepositorio<Lote, int>, ILoteRepositorio
    {
        public LoteRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task<Lote?> BuscarPorIdYNoBorradoAsync(int id)
       => await UnidadDeTrabajo.RegistroLotes.Where(e => e.IdLote == id).FirstOrDefaultAsync();

        public async Task<List<Lote>> ListarTodosAsync()
       => await UnidadDeTrabajo.RegistroLotes.ToListAsync();


    }
}
