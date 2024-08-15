using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Servicios.Abstracciones
{
    public interface ITipoCambioServicio
    {
        Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarMasivaAsync(List<TipoCambioDto> parametros);
        Task<OperacionDto<List<TipoCambioDto>>> ListarPorFechasAsync(DateTime desde, DateTime hasta, int idTipoMoneda);
        Task<OperacionDto<List<TipoCambioDto>>> ListarDelMesAync(DateTime fecha, int idTipoMoneda);
        Task<OperacionDto<RespuestaSimpleDto<bool>>> ProcesarArchivoAsync(IFormFile file, int idTipoMoneda);
        Task<OperacionDto<List<TipoCambioDto>>> ListarParaMesCompletoPorFechasAsync(DateTime desde, DateTime hasta, int idTipoMoneda);

    }
}
