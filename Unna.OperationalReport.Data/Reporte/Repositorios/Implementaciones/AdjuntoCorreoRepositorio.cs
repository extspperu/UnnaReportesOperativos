using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class AdjuntoCorreoRepositorio : OperacionalRepositorio<AdjuntoCorreo, object>, IAdjuntoCorreoRepositorio
    {
        public AdjuntoCorreoRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<AdjuntoCorreo>> ListarPorIdReporteAsync(long idReporte, DateTime? diaOperativo)
        {
            var entidad = new List<AdjuntoCorreo>();
            var sql = "SELECT i.IdConfiguracion,i.RutaArchivoPdf,i.RutaArchivoExcel, i.IdImprimir FROM Reporte.AdjuntoCorreo a INNER JOIN Reporte.Imprimir i ON a.IdConfiguracionReferencia = i.IdConfiguracion WHERE a.IdConfiguracion=@IdConfiguracion AND i.Fecha=@DiaOperativo";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                await conexion.OpenAsync();
                var resultados = await conexion.QueryAsync<AdjuntoCorreo>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdConfiguracion = idReporte,
                        DiaOperativo = diaOperativo,
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.AsList();
            }
            return entidad;
        }

    }
}
