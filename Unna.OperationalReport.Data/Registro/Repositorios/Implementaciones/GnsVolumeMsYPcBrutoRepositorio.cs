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
    public class GnsVolumeMsYPcBrutoRepositorio : OperacionalRepositorio<GnsVolumeMsYPcBruto, long>, IGnsVolumeMsYPcBrutoRepositorio
    {
        public GnsVolumeMsYPcBrutoRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<GnsVolumeMsYPcBruto?> ObtenerPorTipoYNombreDiaOperativoAsync(string? tipo, string? nombre, DateTime? diaOperativo)
        {
            GnsVolumeMsYPcBruto? lista = default(GnsVolumeMsYPcBruto?);
            var sql = "SELECT TOP(1) a.Id,a.Nombre,a.VolumeMs,a.PcBrutoRepCroma,a.Tipo FROM Registro.GnsVolumeMsYPcBruto a INNER JOIN Reporte.RegistroSupervisor b ON a.IdRegistroSupervisor = b.IdRegistroSupervisor" +
                " WHERE b.Fecha = CAST(@DiaOperativo AS DATE) AND a.Tipo LIKE @Tipo AND a.Nombre LIKE @Nombre";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<GnsVolumeMsYPcBruto>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        Tipo = tipo,
                        Nombre = nombre,
                    }).ConfigureAwait(false);
                lista = resultados.FirstOrDefault();
            }
            return lista;
        }

        public async Task<List<GnsVolumeMsYPcBruto?>> ObtenerPorTipoYNombreDiaOperativoMensualAsync(string? tipo, string? nombre, DateTime? diaOperativo)
        {
            var lista =  new List<GnsVolumeMsYPcBruto?>();
            var sql = "SELECT a.Id,a.Nombre,a.VolumeMs,a.PcBrutoRepCroma,a.Tipo FROM Registro.GnsVolumeMsYPcBruto a INNER JOIN Reporte.RegistroSupervisor b ON a.IdRegistroSupervisor = b.IdRegistroSupervisor" +
                " WHERE b.Fecha between CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + '01')   AS DATE) and CAST(@DiaOperativo AS DATE) AND a.Tipo LIKE @Tipo AND a.Nombre LIKE @Nombre";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<GnsVolumeMsYPcBruto>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        Tipo = tipo,
                        Nombre = nombre,
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }
    }
}
