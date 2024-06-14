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
using Unna.OperationalReport.Data.Mensual.Entidades;

namespace Unna.OperationalReport.Data.Carta.Repositorios.Implementaciones
{
    public class CargaInventarioRepositorio : OperacionalRepositorio<CargaInventario, long>, ICargaInventarioRepositorio
    {
        public CargaInventarioRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task InsertarAsync(CargaInventario entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Carta.CargaInventario(Periodo,Tipo,Clase,Producto,Almacen,Uom,Inventario,IdArchivo) VALUES(@Periodo,@Tipo,@Clase,@Producto,@Almacen,@Uom,@Inventario,@IdArchivo)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        
        public override async Task EliminarAsync(CargaInventario entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Carta.CargaInventario WHERE Periodo = CAST(@Periodo AS DATE) AND Tipo LIKE @Tipo";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }


    }
}
