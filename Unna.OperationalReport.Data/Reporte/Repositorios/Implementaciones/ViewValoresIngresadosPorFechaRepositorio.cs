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
    public class ViewValoresIngresadosPorFechaRepositorio : OperacionalRepositorio<ViewValoresIngresadosPorFecha, object>, IViewValoresIngresadosPorFechaRepositorio
    {
        public ViewValoresIngresadosPorFechaRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<List<ViewValoresIngresadosPorFecha>> ListarPorLoteYFechasAsync(int idLote, DateTime desde, DateTime hasta)
        {
            var entidad = new List<ViewValoresIngresadosPorFecha>();
            var sql = "SELECT Fecha,IdLote,Lote,Volumen,Calorifico,Riqueza FROM [Reporte].[ViewValoresIngresadosPorFecha] WHERE IdLote=@IdLote AND Fecha BETWEEN @Desde AND @Hasta ORDER BY Fecha ASC";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                await conexion.OpenAsync();
                var resultados = await conexion.QueryAsync<ViewValoresIngresadosPorFecha>(sql,
                commandType: CommandType.Text,
                param: new
                {
                    IdLote = idLote,
                    Desde = desde,
                    Hasta = hasta,
                }
                    ).ConfigureAwait(false);
                entidad = resultados.AsList();
            }
            return entidad;
        }
    }


}
}
