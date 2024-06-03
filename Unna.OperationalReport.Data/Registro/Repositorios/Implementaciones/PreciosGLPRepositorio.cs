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
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones
{
    public class PreciosGLPRepositorio : OperacionalRepositorio<Entidades.PreciosGLP, long>, IPreciosGLPRepositorio
    {
        public PreciosGLPRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion)
        {
        }

        public async Task<List<Entidades.PreciosGLP?>> ObtenerPreciosGLPMensualAsync(DateTime? diaOperativo)
        {
            var lista = new List<Entidades.PreciosGLP>();
            var sql = "SELECT * FROM Mantenimiento.PreciosGLP_CUMaquila  " +
               "WHERE (cast(Fecha as date) between CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + '01')   AS DATE) and CAST(@DiaOperativo AS DATE)) ;";


            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Entidades.PreciosGLP>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                        
                        // NumeroRegistro = numeroRegistro,
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }
    }
}
