using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaValorizacionProcesamientoGNALoteIV.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaValorizacionProcesamientoGNALoteIV.Servicios.Abstracciones
{
    public interface IBoletaValorizacionProcesamientoGNALoteIVServicio
    {
        

            Task<OperacionDto<BoletaValorizacionProcesamientoGNALoteIVDto>> ObtenerAsync(long idUsuario);
    }
}
