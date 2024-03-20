using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV_2.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV_2.Servicios.Abstracciones
{
    public interface IResBalanceEnergLIV_2Servicio
    {
        Task<OperacionDto<ResBalanceEnergLIV_2Dto>> ObtenerAsync(long idUsuario);
    }
}
