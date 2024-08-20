using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Seguimiento.Repositorios.Entidades;

namespace Unna.OperationalReport.Data.Seguimiento.Repositorios.Abstracciones
{
    public interface ISeguimientoRepositorio
    {
        Task<List<SeguimientoDiario>> ListarPorFechaAsync(int IdModuloSeguimiento, DateTime diaOperativo);
        Task<bool> ActualizarEstadoSeguimientoDiarioAsync(int IdConfiguracionInicial, int idEstadoColor, DateTime diaOperativo);
    }
}
