using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones
{
    public interface IReporteDiariaDatosRepositorio : IOperacionalRepositorio<object, object>
    {

        Task<double?> ObtenerFactorConversionPorLotePetroperuAsync(DateTime diaOperativo, int? idLote, int? idDato, double? eficiencia);
    }
}
