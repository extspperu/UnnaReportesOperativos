using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.DatosAdo.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Tools.DatosBase.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Tools.DatosBase.Infraestructura.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Tools.DatosAdo.Infraestructura.Repositorios.Implementaciones
{
    public abstract class AdoRepositorio<TEntidad, TLlave, TConfiguracion> : BaseRepositorio<TConfiguracion>, IAdoRepositorio<TEntidad, TLlave>
           where TEntidad : class
           where TConfiguracion : IBaseDatosConfiguracion
    {

        public AdoRepositorio(TConfiguracion configuracion) : base(configuracion)
        {
        }

        public virtual TEntidad? BuscarPorId(TLlave id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntidad?> BuscarPorIdAsync(TLlave id)
        {
            throw new NotImplementedException();
        }

        public virtual void Editar(TEntidad entidad)
        {
            throw new NotImplementedException();
        }

        public virtual Task EditarAsync(TEntidad entidad)
        {
            throw new NotImplementedException();
        }

        public virtual void Eliminar(TEntidad entidad)
        {
            throw new NotImplementedException();
        }

        public virtual Task EliminarAsync(TEntidad entidad)
        {
            throw new NotImplementedException();
        }

        public virtual void Insertar(TEntidad entidad)
        {
            throw new NotImplementedException();
        }

        public virtual Task InsertarAsync(TEntidad entidad)
        {
            throw new NotImplementedException();
        }
    }
}
