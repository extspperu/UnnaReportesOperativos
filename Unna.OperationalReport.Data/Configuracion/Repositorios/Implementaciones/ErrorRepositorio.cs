using System;
using System.Collections.Generic;
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
    public  class ErrorRepositorio : OperacionalRepositorio<Error, long>, IErrorRepositorio
    {
        public ErrorRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task GuardarErrorAsync(string mensaje, string? traza, string? jsonAdicionales)
        {

            //var sql = "INSERT INTO [Configuracion].[Error] (Mensaje, Traza, Adicionales, Creado) VALUES (@mensaje, @traza, @adicionales, @creado)";

            //using var conexion = new SqlConnection(Configuracion.CadenaConexion);
            //using var comando = new SqlCommand(sql, conexion);
            //conexion.Open();
            //comando.Parameters.AddWithValue("@mensaje", !string.IsNullOrWhiteSpace(mensaje) ? mensaje : (object)DBNull.Value);
            //comando.Parameters.AddWithValue("@traza", !string.IsNullOrWhiteSpace(traza) ? traza : (object)DBNull.Value);
            //comando.Parameters.AddWithValue("@adicionales", !string.IsNullOrWhiteSpace(jsonAdicionales) ? jsonAdicionales : (object)DBNull.Value);
            //comando.Parameters.AddWithValue("@creado", DateTime.UtcNow);

            //await comando.ExecuteNonQueryAsync();
        }
    }
}
