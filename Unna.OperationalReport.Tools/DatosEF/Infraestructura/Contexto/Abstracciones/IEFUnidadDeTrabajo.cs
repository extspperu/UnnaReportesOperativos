using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Tools.DatosEF.Infraestructura.Contexto.Abstracciones
{
    public interface IEFUnidadDeTrabajo : IDisposable
    {

        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(
            TEntity entity) where TEntity : class;

        int GuardarCambios();



        Task<int> GuardarCambiosAsync();

    }
}
