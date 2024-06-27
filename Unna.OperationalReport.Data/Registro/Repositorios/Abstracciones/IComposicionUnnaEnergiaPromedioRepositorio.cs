using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Tools.DatosBase.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Tools.DatosEF.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones
{
    public interface IComposicionUnnaEnergiaPromedioRepositorio :IOperacionalRepositorio<ComposicionUnnaEnergiaPromedio,DateTime>//,IOperacionalRepositorio<Composicion,DateTime>
    {
       //Task EliminarPorFechaAsync(DateTime desde, DateTime hasta);
        
        
        Task<List<ComposicionUnnaEnergiaPromedio?>> ObtenerComposicionUnnaEnergiaPromedio(DateTime? diaOperativo);
        Task<List<ComposicionUnnaEnergiaPromedio?>> ObtenerComposicionUnnaEnergiaPromedio2(DateTime? diaOperativo);
        Task<List<ComposicionUnnaEnergiaPromedio?>> ObtenerComposicionUnnaEnergiaPromedioDiario(DateTime? diaOperativo);
    }
}
