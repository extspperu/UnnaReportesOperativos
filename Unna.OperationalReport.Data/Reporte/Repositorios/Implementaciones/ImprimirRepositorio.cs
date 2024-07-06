using Dapper;
using Microsoft.EntityFrameworkCore;
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
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones
{
    public class ImprimirRepositorio : OperacionalRepositorio<Imprimir, long>, IImprimirRepositorio
    {
        public ImprimirRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<Imprimir?> BuscarPorIdConfiguracionYFechaAsync(int idConfiguracion, DateTime? fecha)
        {
            Imprimir? lista = default(Imprimir);
            var sql = "SELECT * FROM [Reporte].[Imprimir] WHERE IdConfiguracion=@IdConfiguracion AND Fecha=CAST(@Fecha AS Date) AND EstaBorrado=0";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Imprimir>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdConfiguracion = idConfiguracion,
                        Fecha = fecha
                    }).ConfigureAwait(false);
                lista = resultados.FirstOrDefault();
            }
            return lista;
        }

        public async Task<List<ImprimirVolumenGNSTransf>> ObtenerVolumenGnsTransferidoAsync(int idConfiguracion, DateTime? fecha)
        {
            var lista = new List<ImprimirVolumenGNSTransf>();
            var sql = "SELECT Fecha, JSON_VALUE(Datos,'$.VolumenTransferidoRefineriaPorLote[0].VolumenGnsTransferido') as VolumenGnsTransferidoZ69, JSON_VALUE(Datos,'$.VolumenTransferidoRefineriaPorLote[1].VolumenGnsTransferido') as VolumenGnsTransferidoLVI, JSON_VALUE(Datos,'$.VolumenTransferidoRefineriaPorLote[2].VolumenGnsTransferido') as VolumenGnsTransferidoLI FROM [Reporte].[Imprimir] " +
                      "WHERE IdConfiguracion=@IdConfiguracion AND (cast(Fecha as date) between CAST((CAST((YEAR(CAST(@Fecha AS DATE))*100)+MONTH(CAST(@Fecha AS DATE)) AS VARCHAR(6)) +'01')   AS DATE) and CAST(@Fecha AS DATE)) AND EstaBorrado=0 order by Fecha ";

            
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ImprimirVolumenGNSTransf>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdConfiguracion = idConfiguracion,
                        Fecha = fecha
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<ImprimirVolumenGNSTransf>> ObtenerHoraPlantaFsAsync(int idConfiguracion, DateTime? fecha)
        {
            var lista = new List<ImprimirVolumenGNSTransf>();
            var sql = "SELECT Top 1 Fecha, CAST(JSON_VALUE(Datos,'$.HoraPlantaFs') AS DECIMAL(12,2)) as HoraPlantaFs FROM [Reporte].[Imprimir] WHERE IdConfiguracion=@IdConfiguracion AND cast(Fecha as date) = CAST(@Fecha AS DATE) AND EstaBorrado=0 order by Actualizado desc ";


            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ImprimirVolumenGNSTransf>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdConfiguracion = idConfiguracion,
                        Fecha = fecha
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<ImprimirVolumenGNSTransf>> ObtenerVolumentotalGNSAsync(int idConfiguracion, DateTime? fecha)
        {
            var lista = new List<ImprimirVolumenGNSTransf>();
            var sql = "SELECT Top 1 Fecha, CAST(JSON_VALUE(Datos,'$.VolumenTotalGns') AS DECIMAL(12,3)) as VolumenTotalGNS FROM [Reporte].[Imprimir] WHERE IdConfiguracion=@IdConfiguracion AND cast(Fecha as date) = CAST(@Fecha AS DATE) AND EstaBorrado=0 order by Actualizado desc ";


            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ImprimirVolumenGNSTransf>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdConfiguracion = idConfiguracion,
                        Fecha = fecha
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task ActualizarRutaArchivosAsync(Imprimir entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                await conexion.QueryAsync("UPDATE Reporte.Imprimir SET RutaArchivoPdf=@RutaArchivoPdf,RutaArchivoExcel=@RutaArchivoExcel,Actualizado=@Actualizado WHERE IdImprimir=@IdImprimir", entidad, commandType: CommandType.Text);
            }
        }

    }
}
