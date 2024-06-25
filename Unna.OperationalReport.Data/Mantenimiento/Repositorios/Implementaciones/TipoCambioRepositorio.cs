using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Mantenimiento.Entidades;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Mantenimiento.Repositorios.Implementaciones
{
    public class TipoCambioRepositorio : OperacionalRepositorio<TipoCambio, object>, ITipoCambioRepositorio
    {
        public TipoCambioRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task InsertarAsync(TipoCambio entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Mantenimiento.TipoCambio (Fecha,IdTipoMoneda,Cambio,EstaBorrado)" +
                    " VALUES(@Fecha,@IdTipoMoneda,@Cambio,@EstaBorrado)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EliminarAsync(TipoCambio entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Mantenimiento.TipoCambio WHERE Fecha=@Fecha AND IdTipoMoneda=@IdTipoMoneda";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EditarAsync(TipoCambio entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "UPDATE Mantenimiento.TipoCambio SET Fecha = @Fecha,IdTipoMoneda=@IdTipoMoneda,Cambio=@Cambio,Actualizado=@Actualizado,EstaBorrado=@EstaBorrado WHERE Fecha = @Fecha AND IdTipoMoneda=@IdTipoMoneda";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public async Task<TipoCambio?> BuscarPorFechasAsync(DateTime fecha, int idTipoMoneda)
        {
            TipoCambio? entidad = default(TipoCambio);
            var sql = "SELECT Fecha,IdTipoMoneda,Cambio,Creado,Actualizado,EstaBorrado FROM Mantenimiento.TipoCambio WHERE Fecha = CAST(@Fecha AS Date) AND IdTipoMoneda=@IdTipoMoneda";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<TipoCambio>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Fecha = fecha,
                        IdTipoMoneda = idTipoMoneda
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;

        }

        public async Task<List<TipoCambio>?> ListarPorFechasAsync(DateTime desde, DateTime hasta, int idTipoMoneda)
        {
            List<TipoCambio> entidad = new List<TipoCambio>();
            var sql = "SELECT Fecha,IdTipoMoneda,Cambio,Creado,Actualizado,EstaBorrado FROM Mantenimiento.TipoCambio WHERE Fecha BETWEEN @Desde AND @Hasta AND EstaBorrado=0 AND IdTipoMoneda=@IdTipoMoneda AND datepart(dw,Fecha) NOT IN (1,7) ORDER BY Fecha ASC";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<TipoCambio>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Desde = desde,
                        Hasta = hasta,
                        IdTipoMoneda = idTipoMoneda
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }


    }
}
