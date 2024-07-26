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
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones
{
    public class DatoCgnRepositorio : OperacionalRepositorio<DatoCgn, long>, IDatoCgnRepositorio
    {
        public DatoCgnRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<DatoCgn>> BuscarDatoCgnPorDiaOperativoAsync(DateTime diaOperativo)
        {
            var lista = new List<DatoCgn>();
            var sql = "Registro.BuscarDatoCgnPorDiaOperativo";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DatoCgn>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Fecha = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


    }
}
