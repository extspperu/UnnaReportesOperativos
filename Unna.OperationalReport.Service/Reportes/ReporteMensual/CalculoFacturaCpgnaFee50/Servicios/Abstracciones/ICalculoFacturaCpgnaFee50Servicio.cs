using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Servicios.Abstracciones
{
    public interface ICalculoFacturaCpgnaFee50Servicio
    {
        Task<OperacionDto<CalculoFacturaCpgnaFee50Dto>> ObtenerAsync(long idUsuario, DateTime diaOperativo);
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(CalculoFacturaCpgnaFee50Dto peticion);


        Task<OperacionDto<RespuestaSimpleDto<bool>>> EliminarPrecioAsync(string id);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarPrecioAsync(PrecioGlpPeriodo peticion);
        Task<OperacionDto<List<PrecioGlpPeriodo>?>> ListarPeriodoPreciosAsync(DateTime diaOperativo);
    }
}
