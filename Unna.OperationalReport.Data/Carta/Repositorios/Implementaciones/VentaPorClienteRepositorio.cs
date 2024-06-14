using Dapper;
using System.Data;
using System.Data.SqlClient;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Data.Carta.Repositorios.Implementaciones
{
    public class VentaPorClienteRepositorio : OperacionalRepositorio<VentaPorCliente, long>, IVentaPorClienteRepositorio
    {
        public VentaPorClienteRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task InsertarAsync(VentaPorCliente entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Carta.VentaPorCliente (Fecha,Periodo,Producto,IdArchivo) VALUES(@Fecha,@Periodo,@Producto,@IdArchivo)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EliminarAsync(VentaPorCliente entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Carta.VentaPorCliente WHERE IdVentaPorCliente = @IdVentaPorCliente";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EditarAsync(VentaPorCliente entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "UPDATE Carta.VentaPorCliente SET Periodo=@Periodo, Producto = @Producto,IdArchivo=@IdArchivo WHERE Fecha=CAST(@Fecha AS DATE) AND Periodo=@Periodo";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public async Task<VentaPorCliente?> BuscarPorFechaYProductoAsync(DateTime fecha, string producto)
        {
            VentaPorCliente? entidad = default(VentaPorCliente?);
            var sql = "SELECT * FROM Carta.VentaPorCliente WHERE Fecha=CAST(@Fecha AS DATE) AND Periodo=@Periodo";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<VentaPorCliente>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Fecha = fecha,
                        Producto = producto
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;

        }
    }
}
