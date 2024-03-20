using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.ValorizacionVtaGnsPTop.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.ValorizacionVtaGnsPTop.Servicios.Abstracciones
{
    public interface IValorizacionVtaGnsPTopServicio
    {
        Task<OperacionDto<ValorizacionVtaGnsPTopDto>> ObtenerAsync(long idUsuario);
    }
}
