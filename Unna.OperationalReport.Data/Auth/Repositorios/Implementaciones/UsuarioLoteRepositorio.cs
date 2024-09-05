using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Auth.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Data.Auth.Repositorios.Implementaciones
{
    public class UsuarioLoteRepositorio : OperacionalRepositorio<UsuarioLote, object>, IUsuarioLoteRepositorio
    {
        public UsuarioLoteRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<UsuarioLote?> BuscarPorIdUsuarioActivoAsync(long idUsuario)
       => await UnidadDeTrabajo.AuthUsuarioLotes.Where(e => e.IdUsuario == idUsuario && e.EstaActivo == true).FirstOrDefaultAsync();

        public async Task<List<UsuarioLote>?> ListarPorIdGrupoAsync(int? idGrupo)
        => await UnidadDeTrabajo.AuthUsuarioLotes.Where(e => e.IdGrupo == idGrupo && e.EstaActivo == true).ToListAsync();

        public override async Task InsertarAsync(UsuarioLote entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Auth.UsuarioLote(IdUsuario,IdLote,Creado,EstaActivo,IdGrupo) VALUES(@IdUsuario,@IdLote,GETUTCDATE(),@EstaActivo,@IdGrupo)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EditarAsync(UsuarioLote entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "UPDATE  Auth.UsuarioLote SET  IdLote=@IdLote,EstaActivo=@EstaActivo,IdGrupo=@IdGrupo WHERE IdUsuario=@IdUsuario";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
    }
}
