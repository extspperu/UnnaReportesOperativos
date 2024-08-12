using Dapper;
using Microsoft.EntityFrameworkCore;
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
using Unna.OperationalReport.Data.Mensual.Entidades;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Propiedad.Entidades;
using Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Mensual.Repositorios.Implementaciones
{
    internal class ServicioCompresionGnaLimaGasRepositorio : OperacionalRepositorio<ServicioCompresionGnaLimaGas, long>, IServicioCompresionGnaLimaGasRepositorio
    {
        public ServicioCompresionGnaLimaGasRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<ServicioCompresionGnaLimaGas?> BuscarPorFechaAsync(DateTime fecha)        
        => await UnidadDeTrabajo.MensualServicioCompresionGnaLimaGas.Where(e => e.Fecha == fecha).Include(e => e.ServicioCompresionGnaLimaGasVentas).FirstOrDefaultAsync();

       
        public async Task EliminarPorIdVentasAsync(long id)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Mensual.ServicioCompresionGnaLimaGasVentas WHERE IdServicioCompresionGnaLimaGas=@IdServicioCompresionGnaLimaGas";
                await conexion.QueryAsync(sql, param: new
                {
                    IdServicioCompresionGnaLimaGas = id
                }, commandType: CommandType.Text);
            }
        }


        public async Task<List<ServicioCompresionGnaLimaGasVentas>?> ListarVentasPorIdAsync(long idServicioCompresionGnaLimaGas)
        {
            List<ServicioCompresionGnaLimaGasVentas> entidad = new List<ServicioCompresionGnaLimaGasVentas>();
            var sql = "SELECT * FROM Mensual.ServicioCompresionGnaLimaGasVentas WHERE IdServicioCompresionGnaLimaGas=@IdServicioCompresionGnaLimaGas";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ServicioCompresionGnaLimaGasVentas>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdServicioCompresionGnaLimaGas = idServicioCompresionGnaLimaGas
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;
        }
        
        public async Task InsertarVentasAsync(ServicioCompresionGnaLimaGasVentas entidad)
        {
            
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Mensual.ServicioCompresionGnaLimaGasVentas (FechaDespacho,Placa,NroConstanciaDespacho,VolumenSm3,VolumenMmpcs,PoderCalorifico,Energia,Precio,SubTotal,IdServicioCompresionGnaLimaGas,IdUsuario,InicioCarga,FinCarga)" +
                " VALUES(@FechaDespacho,@Placa,@NroConstanciaDespacho,@VolumenSm3,@VolumenMmpcs,@PoderCalorifico,@Energia,@Precio,@SubTotal,@IdServicioCompresionGnaLimaGas,@IdUsuario,@InicioCarga,@FinCarga)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

    }
}
