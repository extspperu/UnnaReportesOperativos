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

        public async Task<List<SeguimientoDiario>> ListarPorFechaAsync(int IdModuloSeguimiento, DateTime diaOperativo)
        {
            await ActualizarSeguimientoDiarioAsync(diaOperativo); 

            var procedimientoAlmacenado = "ConsultarSeguimientoDiarioDelDia";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var parametros = new DynamicParameters();
                parametros.Add("IdModuloSeguimiento", IdModuloSeguimiento, DbType.Int32, ParameterDirection.Input);
                parametros.Add("DiaOperativo", diaOperativo, DbType.Date, ParameterDirection.Input);

                var resultados = await conexion.QueryAsync<SeguimientoDiario>(procedimientoAlmacenado, parametros,
                    commandType: CommandType.StoredProcedure).ConfigureAwait(false);

                return resultados.ToList();
            }
        }

        public async Task<bool> ActualizarEstadoSeguimientoDiarioAsync(int IdConfiguracionInicial, int idEstadoColor, DateTime diaOperativo)
        {
            await ActualizarSeguimientoDiarioAsync(diaOperativo); 

            var procedimientoAlmacenado = "ActualizarEstadoSeguimientoDiario";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var parametros = new DynamicParameters();
                parametros.Add("IdConfiguracionInicial", IdConfiguracionInicial, DbType.Int32, ParameterDirection.Input);
                parametros.Add("IdEstadoColor", idEstadoColor, DbType.Int32, ParameterDirection.Input);
                parametros.Add("DiaOperativo", diaOperativo, DbType.Date, ParameterDirection.Input);

                var filasAfectadas = await conexion.ExecuteAsync(procedimientoAlmacenado, parametros,
                    commandType: CommandType.StoredProcedure).ConfigureAwait(false);

                return filasAfectadas > 0;
            }
        }

        private async Task ActualizarSeguimientoDiarioAsync(DateTime diaOperativo)
        {
            var procedimientoAlmacenado = "ActualizarSeguimientoDiario";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var parametros = new DynamicParameters();
                parametros.Add("diaOperativo", diaOperativo, DbType.Date, ParameterDirection.Input);

                await conexion.ExecuteAsync(procedimientoAlmacenado, parametros,
                    commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            }
        }
    }

}
