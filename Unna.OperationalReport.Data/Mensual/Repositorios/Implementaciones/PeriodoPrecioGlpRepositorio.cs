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
using Unna.OperationalReport.Data.Mensual.Entidades;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Mensual.Repositorios.Implementaciones
{
    public class PeriodoPrecioGlpRepositorio : OperacionalRepositorio<PeriodoPrecioGlp, long>, IPeriodoPrecioGlpRepositorio
    {
        public PeriodoPrecioGlpRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public override async Task<PeriodoPrecioGlp?> BuscarPorIdAsync(long idPeriodoPrecioGlp)
        {
            PeriodoPrecioGlp? entidad = default(PeriodoPrecioGlp);
            var sql = "SELECT IdPeriodoPrecioGlp,Periodo,Desde,Hasta,PrecioKg,NroDia,Creado,Actualizado FROM Mensual.PeriodoPrecioGlp WHERE IdPeriodoPrecioGlp=@IdPeriodoPrecioGlp";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<PeriodoPrecioGlp?>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdPeriodoPrecioGlp = idPeriodoPrecioGlp
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;
        }

        public override async Task InsertarAsync(PeriodoPrecioGlp entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Mensual.PeriodoPrecioGlp (Periodo,Desde,Hasta,PrecioKg,NroDia,Creado,Actualizado) VALUES(@Periodo,@Desde,@Hasta,@PrecioKg,@NroDia,@Creado,@Actualizado)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EliminarAsync(PeriodoPrecioGlp entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Mensual.PeriodoPrecioGlp WHERE IdPeriodoPrecioGlp=@IdPeriodoPrecioGlp";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EditarAsync(PeriodoPrecioGlp entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "UPDATE Mensual.PeriodoPrecioGlp SET Periodo = @Periodo,Desde=@Desde,Hasta=@Hasta,PrecioKg=@PrecioKg,NroDia=@NroDia,Actualizado=@Actualizado WHERE IdPeriodoPrecioGlp=@IdPeriodoPrecioGlp";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public async Task<List<PeriodoPrecioGlp>?> ListarPorPeriodoAsync(DateTime periodo)
        {
            List<PeriodoPrecioGlp> entidad = new List<PeriodoPrecioGlp>();
            var sql = "SELECT IdPeriodoPrecioGlp,Periodo,Desde,Hasta,PrecioKg,NroDia,Creado,Actualizado FROM Mensual.PeriodoPrecioGlp WHERE Periodo=@Periodo";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<PeriodoPrecioGlp>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Periodo = periodo
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }

    }
}
