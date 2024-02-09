using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.DatosBase.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Tools.DatosEF.Infraestructura.Contexto.Abstracciones;

namespace Unna.OperationalReport.Tools.DatosEF.Infraestructura.Repositorios.Abstracciones
{
    public interface IEFRepositorio<TEntidad, TLlave, TUnidadDeTrabajo> : IBaseRepositorio<TEntidad, TLlave>
       where TEntidad : class
       where TUnidadDeTrabajo : IEFUnidadDeTrabajo
    {

        TUnidadDeTrabajo UnidadDeTrabajo { get; }

        TEntidad? BuscarPorIdYNoBorrado(TLlave id);

        Task<TEntidad?> BuscarPorIdYNoBorradoAsync(TLlave id);
    }
}
