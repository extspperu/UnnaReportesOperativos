using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;

namespace Unna.OperationalReport.Data.Infraestructura.Configuraciones.Implementaciones
{
    public class OperacionalConfiguracion : IOperacionalConfiguracion
    {
        public string? CadenaConexion { get; set; }
    }
}
