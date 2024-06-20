using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Carta.Procedimientos;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Data.Carta.Repositorios.Implementaciones
{
    public class RegistroCromatografiaRepositorio : OperacionalRepositorio<RegistroCromatografia, long>, IRegistroCromatografiaRepositorio
    {
        public RegistroCromatografiaRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<ListarCromatograficoPorLotes>?> ListarReporteCromatograficoPorLotesAsync(DateTime periodo)
        {
            List<ListarCromatograficoPorLotes> entidad = new List<ListarCromatograficoPorLotes>();
            var sql = "Carta.ListarCromatograficoPorLotes";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ListarCromatograficoPorLotes>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Periodo = periodo,
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }


    }
}
