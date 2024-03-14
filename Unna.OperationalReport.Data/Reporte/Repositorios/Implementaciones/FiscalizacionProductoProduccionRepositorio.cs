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
using Unna.OperationalReport.Data.Reporte.Procedimientos;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones
{
    public class FiscalizacionProductoProduccionRepositorio : OperacionalRepositorio<FiscalizacionProductoProduccion, object>, IFiscalizacionProductoProduccionRepositorio
    {
        public FiscalizacionProductoProduccionRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<List<ReporteDiarioLiquidoGasNatural>> ListarReporteDiarioGasNaturalAsociadoAsync(DateTime? diaOperativo)
        {
            List<ReporteDiarioLiquidoGasNatural> lista = new List<ReporteDiarioLiquidoGasNatural>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ReporteDiarioLiquidoGasNatural>("Reporte.ReporteDiarioLiquidoGasNatural",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }



    }
}
