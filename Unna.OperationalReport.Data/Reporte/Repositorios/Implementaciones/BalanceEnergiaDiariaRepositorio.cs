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
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones
{
    public class BalanceEnergiaDiariaRepositorio : OperacionalRepositorio<BalanceEnergiaDiaria, DateTime>, IBalanceEnergiaDiariaRepositorio
    {
        public BalanceEnergiaDiariaRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }



        public async Task<BalanceEnergiaDiaria?> SumaProduccionPorFechaAsync(DateTime desde, DateTime hasta)
        {
            BalanceEnergiaDiaria? entidad =default(BalanceEnergiaDiaria?);
            var sql = "SELECT SUM(CAST(JSON_VALUE(Datos,'$.LiquidosBarriles[1].Enel') AS FLOAT)) AS ProduccionGlp, SUM(CAST(JSON_VALUE(Datos,'$.LiquidosBarriles[2].Enel') AS FLOAT)) AS ProduccionCgn FROM Reporte.Imprimir WHERE IdConfiguracion=2 AND Fecha BETWEEN CAST(@Desde as DATE) AND CAST(@Hasta as DATE)";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<BalanceEnergiaDiaria?>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Desde = desde,
                        Hasta = hasta
                    }).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;
        }

    }
}
