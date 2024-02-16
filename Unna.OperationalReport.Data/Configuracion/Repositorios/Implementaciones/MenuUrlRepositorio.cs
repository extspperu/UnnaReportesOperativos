using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Data.Configuracion.Repositorios.Implementaciones
{
    public class MenuUrlRepositorio : OperacionalRepositorio<MenuUrl, long>, IMenuUrlRepositorio
    {
        public MenuUrlRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

      

        public async Task<List<MenuUrl>> ListarPorGrupoAsync(int? idGrupo, bool? EsParaAdmin)
        {
            List<MenuUrl> dato = new List<MenuUrl>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                using var comando = new SqlCommand("Configuracion.ObtenerMenuUrl", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                comando.Parameters.Add("@IdGrupo", SqlDbType.BigInt).Value = idGrupo;
                comando.Parameters.Add("@EsParaAdmin", SqlDbType.Bit).Value = EsParaAdmin;
                conexion.Open();
                using var reader = await comando.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    dato.Add(new MenuUrl()
                    {
                        IdMenuUrl = !reader.IsDBNull(reader.GetOrdinal("IdMenuUrl")) ? reader.GetInt64(reader.GetOrdinal("IdMenuUrl")) : 0,
                        Nombre = !reader.IsDBNull(reader.GetOrdinal("Nombre")) ? reader.GetString(reader.GetOrdinal("Nombre")) : null,
                        Icono = !reader.IsDBNull(reader.GetOrdinal("Icono")) ? reader.GetString(reader.GetOrdinal("Icono")) : null,
                        IdMenuUrlPadre = !reader.IsDBNull(reader.GetOrdinal("IdMenuUrlPadre")) ? reader.GetInt64(reader.GetOrdinal("IdMenuUrlPadre")) : new int?(),
                        Url = !reader.IsDBNull(reader.GetOrdinal("Url")) ? reader.GetString(reader.GetOrdinal("Url")) : null,
                        Orden = !reader.IsDBNull(reader.GetOrdinal("Orden")) ? reader.GetInt32(reader.GetOrdinal("Orden")) : 0,
                    });
                }
            }
            return dato;
        }

        public async Task<MenuUrl?> ObtenerPorIdAsync(int? idMenuUrl)
        {
            MenuUrl? dato = default(MenuUrl);
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                using var comando = new SqlCommand("SELECT mu.IdMenuUrl,mu.Nombre, mu.Icono, mu.Url, mu.IdMenuUrlPadre, mu.Orden FROM Configuracion.MenuUrl mu WHERE mu.IdMenuUrl = @idMenuUrl", conexion)
                {
                    CommandType = CommandType.Text
                };
                comando.Parameters.Add("@idMenuUrl", SqlDbType.BigInt).Value = idMenuUrl;
                conexion.Open();
                using var reader = await comando.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    dato = new MenuUrl()
                    {
                        IdMenuUrl = !reader.IsDBNull(reader.GetOrdinal("IdMenuUrl")) ? reader.GetInt64(reader.GetOrdinal("IdMenuUrl")) : 0,
                        Nombre = !reader.IsDBNull(reader.GetOrdinal("Nombre")) ? reader.GetString(reader.GetOrdinal("Nombre")) : null,
                        Icono = !reader.IsDBNull(reader.GetOrdinal("Icono")) ? reader.GetString(reader.GetOrdinal("Icono")) : null,
                        IdMenuUrlPadre = !reader.IsDBNull(reader.GetOrdinal("IdMenuUrlPadre")) ? reader.GetInt64(reader.GetOrdinal("IdMenuUrlPadre")) : new int?(),
                        Url = !reader.IsDBNull(reader.GetOrdinal("Url")) ? reader.GetString(reader.GetOrdinal("Url")) : null,
                        Orden = !reader.IsDBNull(reader.GetOrdinal("Orden")) ? reader.GetInt32(reader.GetOrdinal("Orden")) : 0,
                    };
                }
            }
            return dato;
        }

    }
}
