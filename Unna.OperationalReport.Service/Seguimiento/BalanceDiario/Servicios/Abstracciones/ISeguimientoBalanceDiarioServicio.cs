using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Dtos;

namespace Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones
{
    public interface ISeguimientoBalanceDiarioServicio
    {
        Task<List<ColumnaDto>> ObtenerDatosSeguimiento(int IdModuloSeguimiento);
        Task<bool> ActualizarEstadoSeguimientoDiarioAsync(int IdConfiguracionInicial, int idEstadoColor);
    }
}
