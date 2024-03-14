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
    public class VolumenDespachoRepositorio : OperacionalRepositorio<VolumenDespacho, long>, IVolumenDespachoRepositorio
    {
        public VolumenDespachoRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<List<VolumenDespacho>?> ListarPorDiaOperativoAsync(DateTime? diaOperativo)
        {
            List<VolumenDespacho>? lista = new List<VolumenDespacho>();
            var sql = "SELECT a.Id,a.Tanque,a.Cliente,a.Placa,a.Volumen, a.Tipo, a.Creado ,a.Actualizado ,a.IdRegistroSupervisor FROM Registro.VolumenDespacho a INNER JOIN Reporte.RegistroSupervisor b ON a.IdRegistroSupervisor = b.IdRegistroSupervisor WHERE b.Fecha = CAST(@DiaOperativo AS DATE)";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<VolumenDespacho>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }
    }
}
