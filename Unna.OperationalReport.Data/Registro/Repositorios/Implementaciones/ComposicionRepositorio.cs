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
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones
{
    public class ComposicionRepositorio : OperacionalRepositorio<Composicion, DateTime>, IComposicionRepositorio
    {
        public ComposicionRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<Composicion>> ListarPorFechaAsync(DateTime desde, DateTime hasta)
        {
            var lista = new List<Composicion>();
            var sql = "SELECT ROW_NUMBER() OVER(ORDER BY cast(CompGnaDia as date) ASC) AS Id,CompGnaDia,CompGnaC6,CompGnaIc4,CompGnaNc4,CompGnaNeoC5,CompGnaIc5,CompGnaNc5,CompGnaNitrog,CompGnaC1,CompGnaCo2,CompGnaC2,CompGnaObservacion\r\nFROM \r\nRegistro.ComposicionGNA " +
                         " WHERE cast(c.Fecha as date) between CAST(@Desde as DATE) AND CAST(@Hasta as DATE)    ORDER BY cast(CompGnaDia as date) ASC";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Composicion>(sql,
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
                var sql = "DELETE FROM Reporte.ComposicionGNA WHERE cast(CompGnaDia as date) BETWEEN CAST(@Desde as DATE) AND CAST(@Hasta as DATE)";
                await conexion.QueryAsync(sql, new { Desde = desde, Hasta = hasta }, commandType: CommandType.Text);
            }
        }

        public override async Task InsertarAsync(Composicion entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Reporte.ComposicionGNA(CompGnaDia,CompGnaC6,CompGnaIc4,CompGnaNc4,CompGnaNeoC5,CompGnaIc5,CompGnaNc5,CompGnaNitrog,CompGnaC1,CompGnaCo2,CompGnaC2,CompGnaObservacion) VALUES(cast(@CompGnaDia as date),@CompGnaC6,@CompGnaIc4,@CompGnaNc4,@CompGnaNeoC5,@CompGnaIc5,@CompGnaNc5,@CompGnaNitrog,@CompGnaC1,@CompGnaCo2,@CompGnaC2,@CompGnaObservacion)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
    }
}
