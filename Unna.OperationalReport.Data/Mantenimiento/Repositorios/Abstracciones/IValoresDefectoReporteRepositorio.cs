using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Mantenimiento.Entidades;
using Unna.OperationalReport.Data.Reporte.Entidades;

namespace Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones
{
    public interface IValoresDefectoReporteRepositorio : IOperacionalRepositorio<ValoresDefectoReporte, string>
    {

        Task<ValoresDefectoReporte?> BuscarPorLlaveAsync(string? llave);
        Task<List<ValoresDefectoReporte>?> ListarAsync();
    }
}
