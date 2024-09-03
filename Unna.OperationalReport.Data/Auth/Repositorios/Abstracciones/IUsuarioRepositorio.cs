using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Auth.Procedimientos;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones
{
    public interface IUsuarioRepositorio : IOperacionalRepositorio<Usuario, long>
    {
        Task<Usuario?> BuscarPorUsernameAsync(string username);
        Task<List<ListarUsuarios>> ListarUsuariosAsync();
        Task<(bool Existe, int? IdUsuario)> VerificarUsuarioAsync(string username);
    }
}
