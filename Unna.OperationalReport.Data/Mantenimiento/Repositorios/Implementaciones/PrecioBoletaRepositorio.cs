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
using Unna.OperationalReport.Data.Mantenimiento.Entidades;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Mantenimiento.Repositorios.Implementaciones
{
    public class PrecioBoletaRepositorio : OperacionalRepositorio<PrecioBoleta, object>, IPrecioBoletaRepositorio
    {
        public PrecioBoletaRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<PrecioBoleta>?> ListarPorFechasYIdTipoPrecioAsync(DateTime desde, DateTime hasta, int? idTipoPrecio)
        {
            List<PrecioBoleta> entidad = new List<PrecioBoleta>();
            var sql = "Mantenimiento.ListarPorFechasYIdTipoPrecio";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<PrecioBoleta>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Desde = desde,
                        Hasta = hasta,
                        IdTipoPrecio = idTipoPrecio
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }

    }
}
