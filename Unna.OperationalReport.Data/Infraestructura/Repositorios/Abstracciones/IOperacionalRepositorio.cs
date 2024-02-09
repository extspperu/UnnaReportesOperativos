using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Tools.DatosEF.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones
{
    public interface IOperacionalRepositorio<TEntidad, TLlave> : IEFRepositorio<TEntidad, TLlave, IOperacionalUnidadDeTrabajo>
        where TEntidad : class
    {
    }
}
