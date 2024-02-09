using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Tools.DatosBase.Infraestructura.Configuraciones.Abstracciones
{
    public interface IBaseDatosConfiguracion
    {
        string? CadenaConexion { get; set; }
    }
}
