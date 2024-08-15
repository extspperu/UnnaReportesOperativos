using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Seguimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Seguimiento.Repositorios.Entidades;

namespace Unna.OperationalReport.Data.Seguimiento.Repositorios.Implementaciones
{
    public class SeguimientoRepositorio : OperacionalRepositorio<Composicion, DateTime>, ISeguimientoRepositorio
    {
        public SeguimientoRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }
        public async Task<List<SeguimientoDiario>> ListarPorFechaAsync(int IdModuloSeguimiento)
        {
            var procedimientoAlmacenado = "ConsultarSeguimientoDiarioDelDia";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var parametros = new DynamicParameters();
                parametros.Add("IdModuloSeguimiento", IdModuloSeguimiento, DbType.Int32, ParameterDirection.Input);

                var resultados = await conexion.QueryAsync<SeguimientoDiario>(procedimientoAlmacenado, parametros,
                    commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return resultados.ToList();
            }
        }

        public async Task<bool> ActualizarEstadoSeguimientoDiarioAsync(int IdConfiguracionInicial, int idEstadoColor)
        {
            var procedimientoAlmacenado = "ActualizarEstadoSeguimientoDiario";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var filasAfectadas = await conexion.ExecuteAsync(procedimientoAlmacenado,
                    new { IdConfiguracionInicial = IdConfiguracionInicial, IdEstadoColor = idEstadoColor },
                    commandType: CommandType.StoredProcedure).ConfigureAwait(false);

                return filasAfectadas > 0;
            }
        }



    }
}
