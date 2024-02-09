using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.DatosBase.Infraestructura.Configuraciones.Abstracciones;

namespace Unna.OperationalReport.Tools.DatosBase.Infraestructura.Repositorios.Implementaciones
{
    public abstract class BaseRepositorio<TConfiguracion>
     where TConfiguracion : IBaseDatosConfiguracion
    {
        public BaseRepositorio(TConfiguracion configuracion)
        {
            Configuracion = configuracion;
        }
        protected TConfiguracion Configuracion { get; }
    }
}
