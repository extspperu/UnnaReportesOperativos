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
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Procedimientos;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones
{
    public class BoletaDiariaFiscalizacionRepositorio : OperacionalRepositorio<FactorAsignacionLiquidoGasNatural, object>, IBoletaDiariaFiscalizacionRepositorio
    {
        public BoletaDiariaFiscalizacionRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }



        public async Task<List<FactorAsignacionLiquidoGasNatural>> ListarFactorAsignacionLiquidoGasNaturalAsync(DateTime diaOperativo, int idVolumen, int idRiqueza, int idCalorifico)
        {
            var lista = new List<FactorAsignacionLiquidoGasNatural>();
            var sql = "Reporte.ListarFactorAsignacionLiquidoGasNatural";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<FactorAsignacionLiquidoGasNatural>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        IdVolumen = idVolumen,
                        IdRiqueza = idRiqueza,
                        IdCalorifico = idCalorifico
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


        public async Task<List<FactorAsignacionLiquidoGasNatural>> ListarRegistroPorDiaOperativoFactorAsignacionAsync(DateTime diaOperativo, int idVolumen, int idRiqueza, int idCalorifico)
        {
            var lista = new List<FactorAsignacionLiquidoGasNatural>();
            var sql = "Reporte.ListarRegistroPorDiaOperativoFactorAsignacion";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<FactorAsignacionLiquidoGasNatural>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        IdVolumen = idVolumen,
                        IdRiqueza = idRiqueza,
                        IdCalorifico = idCalorifico
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }






    }
}
