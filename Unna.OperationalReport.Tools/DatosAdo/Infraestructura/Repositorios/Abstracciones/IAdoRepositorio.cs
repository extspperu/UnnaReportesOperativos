using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.DatosBase.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Tools.DatosAdo.Infraestructura.Repositorios.Abstracciones
{
    public interface IAdoRepositorio<TEntidad, TLlave> : IBaseRepositorio<TEntidad, TLlave>
        where TEntidad : class
    {
    }
}
