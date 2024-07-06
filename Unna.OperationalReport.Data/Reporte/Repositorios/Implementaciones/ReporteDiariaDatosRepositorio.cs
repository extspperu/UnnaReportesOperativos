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
    public class ReporteDiariaDatosRepositorio : OperacionalRepositorio<object, object>, IReporteDiariaDatosRepositorio
    {
        public ReporteDiariaDatosRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }



        public async Task<double?> ObtenerFactorConversionPorLotePetroperuAsync(DateTime diaOperativo, int? idLote, int? idDato, double? eficiencia)
        {
            double? entidad = new double?();
            var sql = "Reporte.ObtenerFactorConversionPorLotePetroperu_2";
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


        public async Task<List<DiarioPgtDistribucionGasNaturalSeco>> ObtenerGasNaturalSecoAsync(DateTime diaOperativo, double volumenTotalGns)
        {
            List<DiarioPgtDistribucionGasNaturalSeco> entidad = new List<DiarioPgtDistribucionGasNaturalSeco>();
            var sql = "Reporte.DiarioPgtGasNaturalSeco";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DiarioPgtDistribucionGasNaturalSeco>(sql,
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

        public async Task<double?> ObtenerProductoCgnInventarioCgnAsync(DateTime diaOperativo, string tanque)
        {
            double? entidad = new double?();
            var sql = "SELECT Reporte.ObtenerProductoCgnInventarioCgn(@DiaOperativo,@Tanque)";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<double>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        Tanque = tanque
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;

        }
        public async Task<List<DiarioVolumenLiquidosGasNatural>?> DiarioVolumenLiquidosGasNaturalAsync(DateTime diaOperativo, double lotez69, double loteVi, double loteI)
        {
            List<DiarioVolumenLiquidosGasNatural> entidad = new List<DiarioVolumenLiquidosGasNatural>();
            var sql = "Reporte.DiarioVolumenLiquidosGasNatural";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DiarioVolumenLiquidosGasNatural>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        LoteZ69 = lotez69,
                        LoteVi = loteVi,
                        LoteI = loteI
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }




        public async Task EliminarDistribucionGasNaturalSecoPorFechaAsync(DateTime diaOperativo)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "delete from Reporte.DiarioPgtDistribucionGasNaturalSeco WHERE Fecha=CAST(@DiaOperativo as DATE)";
                await conexion.QueryAsync(sql, new { DiaOperativo = diaOperativo }, commandType: CommandType.Text);
            }
        }
        public async Task GuardarDistribucionGasNaturalSecoAsync(DiarioPgtDistribucionGasNaturalSeco entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Reporte.DiarioPgtDistribucionGasNaturalSeco(Fecha,Id,Distribucion,VolumenDiaria,PoderCalorifico,PromedioMensual,EnergiaDiaria)" +
                    " VALUES(@Fecha,@Id,@Distribucion,@VolumenDiaria,@PoderCalorifico,@PromedioMensual,@EnergiaDiaria)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

    }
}
