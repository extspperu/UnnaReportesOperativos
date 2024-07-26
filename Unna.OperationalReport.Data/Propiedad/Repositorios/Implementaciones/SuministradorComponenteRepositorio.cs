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
using Unna.OperationalReport.Data.Propiedad.Entidades;
using Unna.OperationalReport.Data.Propiedad.Procedimientos;
using Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Propiedad.Repositorios.Implementaciones
{
    public class SuministradorComponenteRepositorio: OperacionalRepositorio<SuministradorComponente, int>, ISuministradorComponenteRepositorio
    {
        public SuministradorComponenteRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<SuministradorComponente>?> ListarComponenteAsync()
        {
            List<SuministradorComponente>? lista = new List<SuministradorComponente>();
            var sql = " SELECT Id,Suministrador,Simbolo FROM Propiedad.SuministradorComponente WHERE Id BETWEEN 1 AND 19";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<SuministradorComponente>(sql,
                    commandType: CommandType.Text).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

    }
}
