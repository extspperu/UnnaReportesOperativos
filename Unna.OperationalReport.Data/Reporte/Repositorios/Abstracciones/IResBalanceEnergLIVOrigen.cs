using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Procedimientos;
using Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones
{
    public interface IResBalanceEnergLIVOrigen : IOperacionalRepositorio<ResBalanceEnergLIVDetMedGas, long>
    {
        Task<List<ResBalanceEnergLIVDetMedGas>> ObtenerMedicionesGasAsync();
    }
}
