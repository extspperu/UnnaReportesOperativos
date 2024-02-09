using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Seguridad.Datos.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Tools.Seguridad.Datos.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Tools.Seguridad.Datos.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Tools.Seguridad.Datos.Repositorios.Implementaciones
{
    public class ErrorRepositorio : SeguridadRepositorio<object, object>, IErrorRepositorio
    {
        public ErrorRepositorio(ISeguridadConfiguracion configuracion) : base(configuracion) { }

        public async Task GuardarErrorAsync(string mensaje, string? traza, string? jsonAdicionales, string? appName)
        {

            //var sql = "INSERT INTO error(mensaje, traza, adicionales, creado, app_name) VALUES (@mensaje, @traza, @adicionales, @creado, @appName)";

            //using var conexion = new MySqlConnection(Configuracion.CadenaConexion);
            //using var comando = new MySqlCommand(sql, conexion);
            //conexion.Open();
            //comando.Parameters.AddWithValue("@mensaje", !string.IsNullOrWhiteSpace(mensaje) ? mensaje : (object)DBNull.Value);
            //comando.Parameters.AddWithValue("@traza", !string.IsNullOrWhiteSpace(traza) ? traza : (object)DBNull.Value);
            //comando.Parameters.AddWithValue("@adicionales", !string.IsNullOrWhiteSpace(jsonAdicionales) ? jsonAdicionales : (object)DBNull.Value);
            //comando.Parameters.AddWithValue("@creado", DateTime.UtcNow);
            //comando.Parameters.AddWithValue("@appName", appName);

            //await comando.ExecuteNonQueryAsync();
        }
    }
}
