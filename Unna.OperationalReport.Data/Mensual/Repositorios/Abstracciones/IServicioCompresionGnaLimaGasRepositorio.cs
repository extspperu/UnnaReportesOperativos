using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Mensual.Entidades;

namespace Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones
{
    public interface IServicioCompresionGnaLimaGasRepositorio : IOperacionalRepositorio<ServicioCompresionGnaLimaGas, long>
    {

        Task<ServicioCompresionGnaLimaGas?> BuscarPorFechaAsync(DateTime fecha);
        Task EliminarPorIdVentasAsync(long id);
        Task<List<ServicioCompresionGnaLimaGasVentas>?> ListarVentasPorIdAsync(long idServicioCompresionGnaLimaGas);
        Task InsertarVentasAsync(ServicioCompresionGnaLimaGasVentas entidad);
    }
}
