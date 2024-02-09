using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones
{
    public interface IUsuarioLoteRepositorio:IOperacionalRepositorio<UsuarioLote, object>
    {
        Task<UsuarioLote?> BuscarPorIdUsuarioActivoAsync(long idUsuario);
        Task<List<UsuarioLote>?> ListarPorIdGrupoAsync(int? idGrupo);
    }
}
