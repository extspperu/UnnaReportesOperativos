using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Mensual.Entidades;

namespace Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones
{
    public interface IPeriodoPrecioGlpRepositorio : IOperacionalRepositorio<PeriodoPrecioGlp, long>
    {
        Task<List<PeriodoPrecioGlp>?> ListarPorPeriodoAsync(DateTime periodo);
    }
}
