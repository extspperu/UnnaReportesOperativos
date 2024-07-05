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
    public class EnviarCorreoRepositorio : OperacionalRepositorio<EnviarCorreo, long>, IEnviarCorreoRepositorio
    {
        public EnviarCorreoRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<EnviarCorreo?> BuscarPorIdReporteYFechaAsync(int idReporte, DateTime? fecha)
        {
            EnviarCorreo? entidad = default(EnviarCorreo);
            var sql = "SELECT TOP(1) * FROM Reporte.EnviarCorreo WHERE IdReporte=@IdReporte AND Fecha= CAST(@Fecha AS DATE) ORDER BY IdEnviarCorreo DESC";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<EnviarCorreo>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdReporte = idReporte,
                        Fecha = fecha
                    }).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;
        }

        public override async Task InsertarAsync(EnviarCorreo entidad)
        {
            string sql = "INSERT INTO Reporte.EnviarCorreo (Fecha,Destinatario,Cc,Asunto,Cuerpo,Adjuntos,Creado,Actualizado,IdUsuario,IdReporte,FueEnviado) VALUES(@Fecha,@Destinatario,@Cc,@Asunto,@Cuerpo,@Adjuntos,@Creado,@Actualizado,@IdUsuario,@IdReporte,@FueEnviado)";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        public override async Task EditarAsync(EnviarCorreo entidad)
        {
            string sql = "UPDATE Reporte.EnviarCorreo SET" +
                " Fecha=@Fecha,Destinatario=@Destinatario,Cc=@Cc,Asunto=@Asunto,Cuerpo=@Cuerpo,Adjuntos=@Adjuntos,Actualizado=@Actualizado,IdUsuario=@IdUsuario,IdReporte=@IdReporte,FueEnviado=@FueEnviado" +
                " WHERE IdEnviarCorreo=@IdEnviarCorreo";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }



    }

}
