using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.DatosAdo.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Tools.Seguridad.Datos.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Tools.Seguridad.Datos.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Tools.Seguridad.Datos.Infraestructura.Repositorios.Implementaciones
{
    public abstract class SeguridadRepositorio<TEntidad, TLlave> : AdoRepositorio<TEntidad, TLlave, ISeguridadConfiguracion>, ISeguridadRepositorio<TEntidad, TLlave>
        where TEntidad : class
    {

        public SeguridadRepositorio(ISeguridadConfiguracion configuracion) : base(configuracion)
        {

        }

    }
}
