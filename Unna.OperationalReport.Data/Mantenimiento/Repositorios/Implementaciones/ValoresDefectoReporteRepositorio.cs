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
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Mantenimiento.Repositorios.Implementaciones
{
    public class ValoresDefectoReporteRepositorio : OperacionalRepositorio<ValoresDefectoReporte, string>, IValoresDefectoReporteRepositorio
    {
        public ValoresDefectoReporteRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<ValoresDefectoReporte?> BuscarPorLlaveAsync(string? llave)
        {
            ValoresDefectoReporte? entidad = default(ValoresDefectoReporte?);
            var sql = "SELECT Llave,Valor,Comentario,EstaHabilitado,Creado,Actualizado,IdUsuario FROM Mantenimiento.ValoresDefectoReporte WHERE Llave LIKE @Llave";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ValoresDefectoReporte?>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Llave = llave
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;

        }

        public async Task<List<ValoresDefectoReporte>?> ListarAsync()
        {
            List<ValoresDefectoReporte>? entidad = new List<ValoresDefectoReporte>();
            var sql = "SELECT Llave,Valor,Comentario,EstaHabilitado,Creado,Actualizado,IdUsuario FROM Mantenimiento.ValoresDefectoReporte ORDER BY Actualizado DESC";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ValoresDefectoReporte>(sql,
                    commandType: CommandType.Text
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;
        }

    }
}
