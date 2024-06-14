using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Data.Carta.Repositorios.Implementaciones
{
    public class VentaPorClienteDetalleRepositorio : OperacionalRepositorio<VentaPorClienteDetalle, long>, IVentaPorClienteDetalleRepositorio
    {
        public VentaPorClienteDetalleRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task InsertarAsync(VentaPorClienteDetalle entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Carta.VentaPorClienteDetalle(Periodo,Producto,Cliente,Uom,Volumen,Centaje,Brl,IdVentaPorCliente) VALUES(@Periodo,@Producto,@Cliente,@Uom,@Volumen,@Centaje,@Brl,@IdVentaPorCliente)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EliminarAsync(VentaPorClienteDetalle entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Carta.VentaPorClienteDetalle WHERE IdVentaPorCliente=@IdVentaPorCliente";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }


    }
}
