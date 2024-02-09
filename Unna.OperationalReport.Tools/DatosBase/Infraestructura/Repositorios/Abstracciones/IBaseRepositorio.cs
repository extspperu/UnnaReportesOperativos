using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Tools.DatosBase.Infraestructura.Repositorios.Abstracciones
{
    public interface IBaseRepositorio<TEntidad, TLlave>
    where TEntidad : class
    {

        Task<TEntidad?> BuscarPorIdAsync(TLlave id);
        Task InsertarAsync(TEntidad entidad);
        Task EliminarAsync(TEntidad entidad);
        Task EditarAsync(TEntidad entidad);


        TEntidad? BuscarPorId(TLlave id);
        void Insertar(TEntidad entidad);
        void Eliminar(TEntidad entidad);
        void Editar(TEntidad entidad);

    }
}
