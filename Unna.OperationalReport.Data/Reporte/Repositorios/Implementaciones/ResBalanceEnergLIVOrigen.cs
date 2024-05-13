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
using Unna.OperationalReport.Data.Reporte.Procedimientos;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones
{
    public class ResBalanceEnergLIVOrigen : OperacionalRepositorio<ResBalanceEnergLIVDetMedGas, long>, IResBalanceEnergLIVOrigen
    {
        public ResBalanceEnergLIVOrigen(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<ResBalanceEnergLIVDetMedGas>> ObtenerMedicionesGasAsync()
        {
            var entidad = new List<ResBalanceEnergLIVDetMedGas>();
            var sql = "Reporte.ResumenBalanceEnergiaLIV"; 
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                await conexion.OpenAsync();
                var resultados = await conexion.QueryAsync<ResBalanceEnergLIVDetMedGas>(sql,
                    commandType: CommandType.StoredProcedure
                    ).ConfigureAwait(false);
                entidad = resultados.AsList();
            }
            return entidad;
        }
    }
}
