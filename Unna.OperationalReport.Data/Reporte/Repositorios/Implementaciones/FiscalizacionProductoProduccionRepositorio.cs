using Dapper;
using System;
using System.Collections;
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
    public class FiscalizacionProductoProduccionRepositorio : OperacionalRepositorio<FiscalizacionProductoProduccion, object>, IFiscalizacionProductoProduccionRepositorio
    {
        public FiscalizacionProductoProduccionRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<List<ReporteDiarioLiquidoGasNatural>> ListarReporteDiarioGasNaturalAsociadoAsync(DateTime? diaOperativo)
        {
            List<ReporteDiarioLiquidoGasNatural> lista = new List<ReporteDiarioLiquidoGasNatural>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ReporteDiarioLiquidoGasNatural>("Reporte.ReporteDiarioLiquidoGasNatural",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }
        
        public async Task<List<FiscalizacionProductosGlpCgn>> FiscalizacionProductosGlpCgnAsycn(DateTime? diaOperativo)
        {
            List<FiscalizacionProductosGlpCgn> lista = new List<FiscalizacionProductosGlpCgn>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<FiscalizacionProductosGlpCgn>("Reporte.FiscalizacionProductosGlpCgn",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<FiscalizacionProductosGlpCgn>> FiscalizacionProductosGlpCgnMensualAsync(DateTime? diaOperativo)
        {
            List<FiscalizacionProductosGlpCgn> lista = new List<FiscalizacionProductosGlpCgn>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<FiscalizacionProductosGlpCgn>("Reporte.FiscalizacionProductosGlpCgnMensual",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task EliminarPorFechaAsync(DateTime diaOperativo)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "delete from Reporte.FiscalizacionProductoProduccion WHERE Fecha=CAST(@DiaOperativo as DATE)";
                await conexion.QueryAsync(sql, new { DiaOperativo = diaOperativo }, commandType: CommandType.Text);
            }
        }
        public async Task GuardarAsync(FiscalizacionProductoProduccion entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Reporte.FiscalizacionProductoProduccion(Fecha,Producto,Produccion,Despacho,Inventario,IdUsuario,IdImprimir)" +
                    " VALUES(@Fecha,@Producto,@Produccion,@Despacho,@Inventario,@IdUsuario,@IdImprimir)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }


        public async Task EliminarFiscalizacionProductoAsync(DateTime diaOperativo, string? producto)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Reporte.FiscalizacionProducto WHERE Fecha=CAST(@DiaOperativo as DATE) AND Producto=@Producto";
                await conexion.QueryAsync(sql, new { DiaOperativo = diaOperativo, Producto = producto }, commandType: CommandType.Text);
            }
        }

        public void InsertarFiscalizacionProducto(List<FiscalizacionProducto> productos)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var table = new DataTable();
                conexion.Open();
                using (var adapter = new SqlDataAdapter($"SELECT TOP(0) * FROM Reporte.FiscalizacionProducto", conexion))
                {
                    adapter.Fill(table);
                };
                foreach (var item in productos)
                {
                    var row = table.NewRow();
                    row["Fecha"] = item.Fecha;
                    row["Producto"] = item.Producto;
                    row["Tanque"] = item.Tanque;
                    row["Nivel"] = item.Inventario;
                    row["Inventario"] = item.Inventario;
                    row["Creado"] = DateTime.UtcNow;
                    row["Actualizado"] = DateTime.UtcNow;
                    row["IdUsuario"] = item.IdUsuario;
                    table.Rows.Add(row);                    
                }
                using (var bulk = new SqlBulkCopy(conexion))
                {
                    bulk.DestinationTableName = "Reporte.FiscalizacionProducto";
                    bulk.WriteToServer(table);
                }
            }
        }



    }
}
