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
    public class BoletaCnpcRepositorio : OperacionalRepositorio<BoletaCnpc, DateTime>, IBoletaCnpcRepositorio
    {
        public BoletaCnpcRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<List<BoletaCnpc>> ListarPorFechaAsync(DateTime desde, DateTime hasta)
        {
            var lista = new List<BoletaCnpc>();
            var sql = "SELECT ROW_NUMBER() OVER(ORDER BY Fecha ASC) AS Id," +
                " Fecha,GasMpcd,GlpBls,CgnBls,GnsMpc,GcMpc,Creado,Actualizado" +
                " FROM Reporte.BoletaCnpc WHERE Fecha BETWEEN CAST(@Desde as DATE) AND CAST(@Hasta as DATE) ORDER BY Fecha ASC";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<BoletaCnpc>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Desde = desde,
                        Hasta = hasta
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }



        public async Task EliminarPorFechaAsync(DateTime desde, DateTime hasta)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Reporte.BoletaCnpc WHERE Fecha BETWEEN CAST(@Desde as DATE) AND CAST(@Hasta as DATE)";
                await conexion.QueryAsync(sql, new { Desde = desde, Hasta = hasta }, commandType: CommandType.Text);
            }
        }
        public override async Task InsertarAsync(BoletaCnpc entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Reporte.BoletaCnpc(Fecha,GasMpcd,GlpBls,CgnBls,GnsMpc,GcMpc) VALUES(@Fecha,@GasMpcd,@GlpBls,@CgnBls,@GnsMpc,@GcMpc)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }


    }
}
