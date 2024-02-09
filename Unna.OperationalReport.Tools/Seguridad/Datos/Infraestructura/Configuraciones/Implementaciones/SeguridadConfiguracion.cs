using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Seguridad.Datos.Infraestructura.Configuraciones.Abstracciones;

namespace Unna.OperationalReport.Tools.Seguridad.Datos.Infraestructura.Configuraciones.Implementaciones
{
    public class SeguridadConfiguracion : ISeguridadConfiguracion
    {
        public SeguridadConfiguracion(string cadenaConexion)
        {
            CadenaConexion = cadenaConexion;
        }
        public string? CadenaConexion { get; set; }
    }
}
