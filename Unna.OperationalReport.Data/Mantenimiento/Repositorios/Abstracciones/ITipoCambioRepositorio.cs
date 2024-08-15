using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Mantenimiento.Entidades;

namespace Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones
{
    public interface ITipoCambioRepositorio : IOperacionalRepositorio<TipoCambio, object>
    {
        Task<TipoCambio?> BuscarPorFechasAsync(DateTime fecha, int idTipoMoneda);
        Task<List<TipoCambio>?> ListarPorFechasAsync(DateTime desde, DateTime hasta, int idTipoMoneda);
        Task<List<TipoCambio>?> ListarParaMesCompletoPorFechasAsync(DateTime desde, DateTime hasta, int idTipoMoneda);
    }
}
