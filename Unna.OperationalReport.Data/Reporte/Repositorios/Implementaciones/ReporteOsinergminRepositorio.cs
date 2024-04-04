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
    public class ReporteOsinergminRepositorio : OperacionalRepositorio<object, object>, IReporteOsinergminRepositorio
    {
        public ReporteOsinergminRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<OsinergminProcesamientoGasNatural>> ObtenerGasNaturalSecoAsync(DateTime diaOperativo, double volumenTotalGns)
        {
            List<OsinergminProcesamientoGasNatural> entidad = new List<OsinergminProcesamientoGasNatural>();
            var sql = "Reporte.OsinergminProcesamientoGasNatural";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<OsinergminProcesamientoGasNatural>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        VolumenTotalGns = volumenTotalGns
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }


    }
}
