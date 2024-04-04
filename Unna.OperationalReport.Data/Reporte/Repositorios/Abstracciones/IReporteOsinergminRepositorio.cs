using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Procedimientos;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones
{
    public interface IReporteOsinergminRepositorio:IOperacionalRepositorio<object, object>
    {
        Task<List<OsinergminProcesamientoGasNatural>> ObtenerGasNaturalSecoAsync(DateTime diaOperativo, double volumenTotalGns);

    }
}
