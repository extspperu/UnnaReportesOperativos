using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Tools.DatosEF.Infraestructura.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones
{
    public class OperacionalRepositorio<TEntidad, TLlave> : EFRepositorio<TEntidad, TLlave, IOperacionalConfiguracion, IOperacionalUnidadDeTrabajo>, IOperacionalRepositorio<TEntidad, TLlave>
         where TEntidad : class
    {
        public OperacionalRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion)
        {
        }
    }
}
