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
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones
{
    public class DiarioPgtProduccionRepositorio : OperacionalRepositorio<DiarioPgtProduccion, object>, IDiarioPgtProduccionRepositorio
    {
        public DiarioPgtProduccionRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }



        public async Task GuardarProduccionAsync(DiarioPgtProduccion entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                await conexion.QueryAsync("Reporte.GuardarProduccionPorLote", entidad, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
