using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Abstracciones
{
    public interface IFiscalizacionPetroPeruServicio
    {
        Task<OperacionDto<FiscalizacionPetroPeruDto>> ObtenerAsync(long idUsuario);
    }
}
