using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Procedimientos;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones
{
    public interface IFiscalizacionProductoProduccionRepositorio : IOperacionalRepositorio<FiscalizacionProductoProduccion, object>
    {
        Task<List<ReporteDiarioLiquidoGasNatural>> ListarReporteDiarioGasNaturalAsociadoAsync(DateTime? diaOperativo);
        Task<List<FiscalizacionProductosGlpCgn>> FiscalizacionProductosGlpCgnAsycn(DateTime? diaOperativo);
        Task GuardarAsync(FiscalizacionProductoProduccion entidad);
        Task EliminarPorFechaAsync(DateTime diaOperativo);
    }
}
