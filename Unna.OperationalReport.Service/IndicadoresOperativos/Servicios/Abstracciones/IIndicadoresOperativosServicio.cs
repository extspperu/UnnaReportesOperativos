using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.IndicadoresOperativos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.IndicadoresOperativos.Servicios.Abstracciones
{
    public interface IIndicadoresOperativosServicio
    {
        OperacionDto<List<PeriodoIndicadoresDto>> ListarPeriodosAsync();
        Task<OperacionDto<List<IndicadoresOperativosDto>>> BusquedaIndicadoresAsync(string periodo);
    }
}
