using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Procedimientos;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones
{
    public interface IBoletaDiariaFiscalizacionRepositorio:IOperacionalRepositorio<FactorAsignacionLiquidoGasNatural, object>
    {
        Task<List<FactorAsignacionLiquidoGasNatural>> ListarFactorAsignacionLiquidoGasNaturalAsync(DateTime diaOperativo, int idVolumen, int idRiqueza, int idCalorifico);
        Task<List<FactorAsignacionLiquidoGasNatural>> ListarRegistroPorDiaOperativoFactorAsignacionAsync(DateTime diaOperativo, int idVolumen, int idRiqueza, int idCalorifico, bool? verificacionGnaVenta);

    }
}
