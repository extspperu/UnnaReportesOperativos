using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Abstracciones
{
    public interface IBoletaSuministroGNSdelLoteIVaEnelServicio
    {
        Task<OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>> ObtenerAsync(long idUsuario);
        
    }
}
