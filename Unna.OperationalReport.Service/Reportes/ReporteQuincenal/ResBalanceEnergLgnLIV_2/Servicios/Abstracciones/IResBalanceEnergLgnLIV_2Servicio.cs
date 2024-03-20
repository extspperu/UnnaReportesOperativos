using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Servicios.Abstracciones
{
    public interface IResBalanceEnergLgnLIV_2Servicio
    {
        Task<OperacionDto<ResBalanceEnergLgnLIV_2Dto>> ObtenerAsync(long idUsuario);
    }
}
