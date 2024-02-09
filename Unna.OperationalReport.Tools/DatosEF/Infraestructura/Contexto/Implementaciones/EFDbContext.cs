using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.DatosEF.Infraestructura.Contexto.Abstracciones;

namespace Unna.OperationalReport.Tools.DatosEF.Infraestructura.Contexto.Implementaciones
{
    public abstract class EFDbContext : DbContext, IEFUnidadDeTrabajo
    {

        public EFDbContext(DbContextOptions opciones) : base(opciones)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public int GuardarCambios()
        {
            return SaveChanges();
        }

        public Task<int> GuardarCambiosAsync()
        {
            return SaveChangesAsync();
        }
    }
}
