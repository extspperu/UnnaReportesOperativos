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
    public class ReporteDiariaDatosRepositorio : OperacionalRepositorio<object, object>, IReporteDiariaDatosRepositorio
    {
        public ReporteDiariaDatosRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }



        public async Task<double?> ObtenerFactorConversionPorLotePetroperuAsync(DateTime diaOperativo, int? idLote, int? idDato, double? eficiencia)
        {
            double? entidad = new double?();
            var sql = "Reporte.ObtenerFactorConversionPorLotePetroperu";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<double?>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        IdLote = idLote,
                        IdDato = idDato,
                        Eficiencia = eficiencia,
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;

        }


    }
}
