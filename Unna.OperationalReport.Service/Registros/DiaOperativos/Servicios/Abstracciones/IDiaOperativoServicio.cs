using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones
{
    public interface IDiaOperativoServicio
    {
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(DiaOperativoDto peticion);
        Task<OperacionDto<DiaOperativoDto>> ObtenerPorIdUsuarioYFechaAsync(long idUsuario, DateTime fecha, int idGrupo, int? numero);
        Task<OperacionDto<DiaOperativoDto>> ObtenerAsync(string idDiaOperativo);
        Task<bool> ExisteParaEdicionDatosAsync(long idUsuario, int idGrupo, int? numero);
        Task<List<int>> ListarNumeroEdicionFiscalizadorEnelAsync(long idUsuario, int idGrupo, int? numero);
        Task<OperacionDto<DatosFiscalizadorEnelDto>> ObtenerPermisosFiscalizadorEnelAsync(long idUsuario, string? grupo, string? edicion);
        Task<OperacionDto<SeguimientoFiscalizadorEnelDto>> SeguimientoFiscalizadorEnelAsync(long idUsuario);
        Task<OperacionDto<List<DiaOperativoDto>>> ListarRegistrosFiscalizadorRegularAsync();

        Task<OperacionDto<DatosFiscalizadorEnelDto>> ObtenerValidarDatosAsync(string idLote, DateTime fecha,int? numero);


    }
}
