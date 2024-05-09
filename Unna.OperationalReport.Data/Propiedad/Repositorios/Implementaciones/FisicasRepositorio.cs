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
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Propiedad.Repositorios.Implementaciones
{
    public class FisicasRepositorio : OperacionalRepositorio<Fisicas, object>, IFisicasRepositorio
    {
        public FisicasRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<ListarPropiedadesFisicas>?> ListarPropiedadesFisicasAsync(string? grupo)
        {
            List<ListarPropiedadesFisicas>? lista = new List<ListarPropiedadesFisicas>();
            var sql = "SELECT  tbl2.Id,tbl1.Grupo,tbl2.Suministrador,tbl1.PoderCalorifico,tbl1.RelacionVolumen,tbl1.PesoMolecular,tbl1.DensidadLiquido " +
                "FROM Propiedad.Fisicas tbl1 INNER JOIN Propiedad.SuministradorComponente tbl2 ON tbl1.IdSuministradorComponente = tbl2.Id " +
                "WHERE tbl1.Grupo=@Grupo";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ListarPropiedadesFisicas>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Grupo = grupo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

    }
}
