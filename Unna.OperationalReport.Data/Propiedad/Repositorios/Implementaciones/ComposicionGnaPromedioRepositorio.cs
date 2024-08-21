using Dapper;
using System.Data;
using System.Data.SqlClient;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Propiedad.Entidades;
using Unna.OperationalReport.Data.Propiedad.Procedimientos;
using Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Procedimientos;

namespace Unna.OperationalReport.Data.Propiedad.Repositorios.Implementaciones
{
    public class ComposicionGnaPromedioRepositorio : OperacionalRepositorio<ComposicionGnaPromedio, object>, IComposicionGnaPromedioRepositorio
    {
        public ComposicionGnaPromedioRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<List<BuscarComposicionGnaPromedio>?> ListarPorIdLoteYFechaAsync(DateTime? fecha, int idLote)
        {
            List<BuscarComposicionGnaPromedio>? lista = new List<BuscarComposicionGnaPromedio>();
            var sql = "Propiedad.BuscarComposicionGnaPromedio";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<BuscarComposicionGnaPromedio>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Fecha = fecha,
                        IdLote = idLote,
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public override async Task InsertarAsync(ComposicionGnaPromedio entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Propiedad.ComposicionGnaPromedio (IdLote,IdSuministradorComponente,Porcentaje,Fecha) VALUES(@IdLote,@IdSuministradorComponente,@Porcentaje,@Fecha)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EliminarAsync(ComposicionGnaPromedio entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Propiedad.ComposicionGnaPromedio WHERE IdLote=@IdLote AND Fecha=@Fecha";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

    }
}
