using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones
{
    public interface IValoresDefectoReporteServicio
    {
        Task<double?> ObtenerValorAsync(string? llave);
    }
}
