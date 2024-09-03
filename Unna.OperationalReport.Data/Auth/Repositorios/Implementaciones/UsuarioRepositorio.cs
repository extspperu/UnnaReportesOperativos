using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Auth.Procedimientos;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Data.Auth.Repositorios.Implementaciones
{
    public class UsuarioRepositorio : OperacionalRepositorio<Usuario, long>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task<Usuario?> BuscarPorIdAsync(long id)
        => await UnidadDeTrabajo.AuthUsuarios.Where(e => e.IdUsuario == id).FirstOrDefaultAsync();

        public override async Task<Usuario?> BuscarPorIdYNoBorradoAsync(long id)
        => await UnidadDeTrabajo.AuthUsuarios.Where(e => e.IdUsuario == id && e.EstaBorrado == false).FirstOrDefaultAsync();

        public async Task<Usuario?> BuscarPorUsernameAsync(string username)
        => await UnidadDeTrabajo.AuthUsuarios.Where(x => x.Username == username).FirstOrDefaultAsync();


        public async Task<List<ListarUsuarios>> ListarUsuariosAsync()
        {
            List<ListarUsuarios> entidad = new List<ListarUsuarios>();
            var sql = "Auth.ListarUsuarios";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ListarUsuarios>(sql,
                    commandType: CommandType.StoredProcedure
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }

        public async Task<(bool Existe, int? IdUsuario)> VerificarUsuarioAsync(string username)
        {
            var sql = "Auth.VerificarUsuario";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var parametros = new { Username = username };
                var resultado = await conexion.QueryFirstOrDefaultAsync<(int IdUsuario, bool Existe)>(
                    sql,
                    parametros,
                    commandType: CommandType.StoredProcedure
                ).ConfigureAwait(false);

                if (resultado.Equals(default((int, bool))))
                {
                    return (false, null);
                }

                return (resultado.Existe, resultado.IdUsuario);
            }
        }
    }
}
