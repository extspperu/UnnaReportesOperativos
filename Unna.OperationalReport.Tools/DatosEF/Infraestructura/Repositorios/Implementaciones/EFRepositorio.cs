using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.DatosBase.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Tools.DatosBase.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Tools.DatosEF.Infraestructura.Contexto.Abstracciones;
using Unna.OperationalReport.Tools.DatosEF.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Tools.DatosEF.Infraestructura.Repositorios.Implementaciones
{
    public abstract class EFRepositorio<TEntidad, TLlave, TConfiguracion, TUnidadDeTrabajo> : BaseRepositorio<TConfiguracion>, IEFRepositorio<TEntidad, TLlave, TUnidadDeTrabajo>
       where TEntidad : class
       where TConfiguracion : IBaseDatosConfiguracion
       where TUnidadDeTrabajo : IEFUnidadDeTrabajo
    {

        public EFRepositorio(TUnidadDeTrabajo unidadDeTrabajo, TConfiguracion configuracion) : base(configuracion)
        {
            UnidadDeTrabajo = unidadDeTrabajo;
        }

        public TUnidadDeTrabajo UnidadDeTrabajo { get; }


        public virtual TEntidad? BuscarPorId(TLlave id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntidad?> BuscarPorIdAsync(TLlave id)
        {
            throw new NotImplementedException();
        }

        public virtual TEntidad? BuscarPorIdYNoBorrado(TLlave id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntidad?> BuscarPorIdYNoBorradoAsync(TLlave id)
        {
            throw new NotImplementedException();
        }

        public virtual void Editar(TEntidad entidad)
        {
            UnidadDeTrabajo.Entry(entidad).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public virtual Task EditarAsync(TEntidad entidad)
        {
            throw new NotImplementedException();
        }

        public virtual void Eliminar(TEntidad entidad)
        {
            UnidadDeTrabajo.Set<TEntidad>().Remove(entidad);
        }

        public virtual Task EliminarAsync(TEntidad entidad)
        {
            throw new NotImplementedException();
        }

        public virtual void Insertar(TEntidad entidad)
        {
            UnidadDeTrabajo.Set<TEntidad>().Add(entidad);
        }

        public virtual Task InsertarAsync(TEntidad entidad)
        {
            throw new NotImplementedException();
        }
    }

}
