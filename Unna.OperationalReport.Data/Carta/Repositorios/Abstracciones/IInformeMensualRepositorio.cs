using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Procedimientos;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones
{
    public interface IInformeMensualRepositorio:IOperacionalRepositorio<object, object>
    {
        Task<List<Osinergmin1>?> RecepcionGasNaturalAsync(DateTime desde, DateTime hasta);
        Task<List<Osinergmin1>?> ReporteMensualUsoDeGasAsync(DateTime desde, DateTime hasta);
        Task<List<Osinergmin1>?> ProduccionLiquidosGasNaturalAsync(DateTime desde, DateTime hasta);
        Task<VentaLiquidosGasNatural?> VentaLiquidosGasNaturalAsync(DateTime desde, DateTime hasta);

        Task<List<VolumenVendieronProductos>?> VolumenVendieronProductosAsync(DateTime desde, DateTime hasta);
        Task<List<InventarioLiquidoGasNatural>?> InventarioLiquidoGasNaturalAsync(DateTime desde, DateTime hasta);
    }
}
