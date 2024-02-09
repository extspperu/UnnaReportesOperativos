using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones
{
    public class DiaOperativoRepositorio : OperacionalRepositorio<DiaOperativo, long>, IDiaOperativoRepositorio
    {
        public DiaOperativoRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task<DiaOperativo?> BuscarPorIdYNoBorradoAsync(long id)
        => await UnidadDeTrabajo.RegistroDiaOperativos.Where(e => e.IdDiaOperativo == id && e.EstaBorrado == false).FirstOrDefaultAsync();


        public async Task<List<DiaOperativo>?> ListarPorFechaYIdGrupoAsync(DateTime? fecha, int? idGrupo)
       => await UnidadDeTrabajo.RegistroDiaOperativos.Include(e=>e.Lote).Where(e => e.Fecha == fecha && e.IdGrupo == idGrupo && e.EstaBorrado == false).ToListAsync();



        public async Task<DiaOperativo?> ObtenerPorIdLoteYFechaAsync(int idLote, DateTime? fecha,int? idGrupo, int? numeroRegistro)
        {
            DiaOperativo? dato = default(DiaOperativo);
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sqlWhere = new StringBuilder();
                sqlWhere.Append(" WHERE Fecha=@fecha AND IdLote = @idLote ");
                if (idGrupo.HasValue) sqlWhere.Append(" AND IdGrupo=@idGrupo ");
                if (numeroRegistro.HasValue) sqlWhere.Append(" AND NumeroRegistro = @numeroRegistro ");
                var sql = $"SELECT * FROM [Registro].[DiaOperativo]  " + $" {sqlWhere} ";
                using var comando = new SqlCommand(sql, conexion)
                {
                    CommandType = CommandType.Text
                };
                comando.Parameters.Add("@fecha", SqlDbType.Date).Value = fecha;
                comando.Parameters.Add("@idLote", SqlDbType.Int).Value = idLote;
                if (idGrupo.HasValue)
                {
                    comando.Parameters.Add("@idGrupo", SqlDbType.Int).Value = idGrupo;
                }
                if (numeroRegistro.HasValue)
                {
                    comando.Parameters.Add("@numeroRegistro", SqlDbType.Int).Value = numeroRegistro;
                }
                conexion.Open();
                using var reader = await comando.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    dato = DeReader(reader);
                }
            }
            return dato;
        }
        public async Task<List<DiaOperativo>?> ListarPorIdLoteYFechaAsync(int idLote, DateTime? fecha, int? idGrupo, int? numeroRegistro)
        {
            List<DiaOperativo>? dato = new List<DiaOperativo>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sqlWhere = new StringBuilder();
                sqlWhere.Append(" WHERE Fecha=@fecha AND IdLote = @idLote ");
                if (idGrupo.HasValue) sqlWhere.Append(" AND IdGrupo=@idGrupo ");
                if (numeroRegistro.HasValue) sqlWhere.Append(" AND NumeroRegistro = @numeroRegistro ");
                var sql = $"SELECT * FROM [Registro].[DiaOperativo]  " + $" {sqlWhere} ";
                using var comando = new SqlCommand(sql, conexion)
                {
                    CommandType = CommandType.Text
                };
                comando.Parameters.Add("@fecha", SqlDbType.Date).Value = fecha;
                comando.Parameters.Add("@idLote", SqlDbType.Int).Value = idLote;
                if (idGrupo.HasValue)
                {
                    comando.Parameters.Add("@idGrupo", SqlDbType.Int).Value = idGrupo;
                }
                if (numeroRegistro.HasValue)
                {
                    comando.Parameters.Add("@numeroRegistro", SqlDbType.Int).Value = numeroRegistro;
                }
                conexion.Open();
                using var reader = await comando.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    dato.Add(DeReader(reader));
                }
            }
            return dato;
        }


        private DiaOperativo DeReader(SqlDataReader reader)
        {
            var diaOperativo = new DiaOperativo()
            {
                IdDiaOperativo = !reader.IsDBNull(reader.GetOrdinal("IdDiaOperativo")) ? reader.GetInt64(reader.GetOrdinal("IdDiaOperativo")) : 0,
                NumeroRegistro = !reader.IsDBNull(reader.GetOrdinal("NumeroRegistro")) ? reader.GetInt32(reader.GetOrdinal("NumeroRegistro")) : new int?(),
                Fecha = !reader.IsDBNull(reader.GetOrdinal("Fecha")) ? reader.GetDateTime(reader.GetOrdinal("Fecha")) : new DateTime(),
                Adjuntos = !reader.IsDBNull(reader.GetOrdinal("Adjuntos")) ? reader.GetString(reader.GetOrdinal("Adjuntos")) : null,
                Comentario = !reader.IsDBNull(reader.GetOrdinal("Comentario")) ? reader.GetString(reader.GetOrdinal("Comentario")) : null,
                IdLote = !reader.IsDBNull(reader.GetOrdinal("IdLote")) ? reader.GetInt32(reader.GetOrdinal("IdLote")) : null,
                IdUsuario = !reader.IsDBNull(reader.GetOrdinal("IdUsuario")) ? reader.GetInt64(reader.GetOrdinal("IdUsuario")) : new int?(),
                DatoCulminado = !reader.IsDBNull(reader.GetOrdinal("DatoCulminado")) ? reader.GetBoolean(reader.GetOrdinal("DatoCulminado")) : new bool?(),
                DatoValidado = !reader.IsDBNull(reader.GetOrdinal("DatoValidado")) ? reader.GetBoolean(reader.GetOrdinal("DatoValidado")) : new bool?(),
                Creado = !reader.IsDBNull(reader.GetOrdinal("Creado")) ? reader.GetDateTime(reader.GetOrdinal("Creado")) : new DateTime(),
                Actualizado = !reader.IsDBNull(reader.GetOrdinal("Actualizado")) ? reader.GetDateTime(reader.GetOrdinal("Actualizado")) : new DateTime(),
                EsObservado = !reader.IsDBNull(reader.GetOrdinal("EsObservado")) ? reader.GetBoolean(reader.GetOrdinal("EsObservado")) : new bool?(),
                FechaObservado = !reader.IsDBNull(reader.GetOrdinal("FechaObservado")) ? reader.GetDateTime(reader.GetOrdinal("FechaObservado")) : new DateTime?(),
                IdUsuarioObservado = !reader.IsDBNull(reader.GetOrdinal("IdUsuarioObservado")) ? reader.GetInt64(reader.GetOrdinal("IdUsuarioObservado")) : new long?(),
                IdGrupo = !reader.IsDBNull(reader.GetOrdinal("IdGrupo")) ? reader.GetInt32(reader.GetOrdinal("IdGrupo")) : new int?(),
            };
            return diaOperativo;
        }


    }

}

