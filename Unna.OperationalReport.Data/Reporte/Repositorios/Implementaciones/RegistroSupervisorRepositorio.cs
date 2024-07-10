using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class RegistroSupervisorRepositorio : OperacionalRepositorio<RegistroSupervisor, long>, IRegistroSupervisorRepositorio
    {
        public RegistroSupervisorRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public override async Task<RegistroSupervisor?> BuscarPorIdYNoBorradoAsync(long id)
        => await UnidadDeTrabajo.ReporteRegistroSupervisores.Where(e => e.IdRegistroSupervisor == id && e.EstaBorrado == false).FirstOrDefaultAsync();


        public async Task<RegistroSupervisor?> BuscarPorFechaAsync(DateTime fecha)
        {
            RegistroSupervisor? entidad = default(RegistroSupervisor);
            var sql = "SELECT * FROM Reporte.RegistroSupervisor WHERE Fecha=CAST(@Fecha as Date) AND EstaBorrado=0";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<RegistroSupervisor>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Fecha = fecha
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;        

        }

    }
}
