using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Servicios.Abstracciones
{
    public interface IBoletaProcesamientoGnaLoteIvServicio
    {
        Task<OperacionDto<BoletaProcesamientoGnaLoteIvDto>> ObtenerAsync(long idUsuario);
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletaProcesamientoGnaLoteIvDto peticion);
    }
}
