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
using Unna.OperationalReport.Data.Seguimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Seguimiento.Repositorios.Entidades;

namespace Unna.OperationalReport.Data.Seguimiento.Repositorios.Implementaciones
{
    public class SeguimientoRepositorio : OperacionalRepositorio<Composicion, DateTime>, ISeguimientoRepositorio
    {
        public SeguimientoRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }
        public async Task<List<SeguimientoDiario>> ListarPorFechaAsync()
        {
            var lista = new List<SeguimientoDiario>();
            var procedimientoAlmacenado = "ConsultarSeguimientoDiarioDelDia";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<SeguimientoDiario>(procedimientoAlmacenado,
                    commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }
    }
}
