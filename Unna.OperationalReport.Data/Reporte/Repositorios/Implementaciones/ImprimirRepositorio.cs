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

    }
}
