using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Registros.Lotes.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Registros.Lotes.Servicios.Abstracciones
{
    public interface IUsuarioLoteServicio
    {
        Task<OperacionDto<RespuestaSimpleDto<int>?>> ObtenerIdLotePorIdUsuarioAsync(long idUsuario);
        Task<OperacionDto<LoteDto>> ObtenerLotePorIdUsuarioAsync(long idUsuario);
        Task<OperacionDto<List<UsuarioLoteDto>>> ListarPorIdGrupoAsync(int? idGrupo);
    }
}
