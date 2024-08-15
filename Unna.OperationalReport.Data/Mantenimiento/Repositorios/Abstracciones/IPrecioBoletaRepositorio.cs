using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Mantenimiento.Entidades;

namespace Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones
{
    public interface IPrecioBoletaRepositorio : IOperacionalRepositorio<PrecioBoleta, object>
    {
        Task<List<PrecioBoleta>?> ListarPorFechasYIdTipoPrecioAsync(DateTime desde, DateTime hasta, int? idTipoPrecio);
    }
}
