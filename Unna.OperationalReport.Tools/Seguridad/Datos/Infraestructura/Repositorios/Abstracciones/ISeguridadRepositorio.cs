using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.DatosAdo.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Tools.Seguridad.Datos.Infraestructura.Repositorios.Abstracciones
{
    public interface ISeguridadRepositorio<TEntidad, TLlave> : IAdoRepositorio<TEntidad, TLlave>
      where TEntidad : class
    {
    }
}
