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
    public class ComposicionUnnaEnergiaPromedioRepositorio : OperacionalRepositorio<ComposicionUnnaEnergiaPromedio, long>, IComposicionUnnaEnergiaPromedioRepositorio
    {
        public ComposicionUnnaEnergiaPromedioRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<ComposicionUnnaEnergiaPromedio?>> ObtenerComposicionUnnaEnergiaPromedio(string? simbolo, DateTime? diaOperativo)
        {
            //ComposicionUnnaEnergiaPromedio? lista = default(ComposicionUnnaEnergiaPromedio?);
            var lista = new List<ComposicionUnnaEnergiaPromedio>();
            var sql = "SELECT b.IdAdjuntoSupervisor, C.Fecha,D.Id IdComponente,D.Suministrador,D.Simbolo,A.PromedioComponente,\r\nCASE WHEN D.Simbolo='C6'  THEN 1 \r\n\t WHEN D.Simbolo='C3'  THEN 2\r\n\t WHEN D.Simbolo='IC4'  THEN 3 \r\n\t WHEN D.Simbolo='NC4'  THEN 4\r\n\t WHEN D.Simbolo='NEOC5'  THEN 5\r\n\t WHEN D.Simbolo='IC5'  THEN 6\r\n\t WHEN D.Simbolo='NC5'  THEN 7\r\n\t WHEN D.Simbolo='N2'  THEN 8\r\n\t WHEN D.Simbolo='C1'  THEN 9\r\n\t WHEN D.Simbolo='CO2'  THEN 10\r\n\t WHEN D.Simbolo='C2'  THEN 11\r\n\t END Orden\r\nFROM \r\nRegistro.ComposicionUnnaEnergiaPromedio A \r\nINNER JOIN\r\n[Reporte].[AdjuntoSupervisor] B ON A.IdAdjuntoSupervisor=B.IdAdjuntoSupervisor\r\nINNER JOIN \r\n[Reporte].[RegistroSupervisor] C ON B.IdRegistroSupervisor=C.IdRegistroSupervisor\r\nINNER JOIN \r\nPropiedad.SuministradorComponente D ON A.IdComponente=D.Id" +
               " WHERE c.Fecha between '01/'+ CAST(MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(2))+'/'+ CAST(YEAR(CAST(@DiaOperativo AS DATE)) AS VARCHAR(4))   AND  CAST(@DiaOperativo AS DATE)  ORDER BY 2,7";//AND D.Simbolo LIKE @Simbolo
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ComposicionUnnaEnergiaPromedio>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        //Simbolo = simbolo,
                        //Promedio = nombre,
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }
    }
}