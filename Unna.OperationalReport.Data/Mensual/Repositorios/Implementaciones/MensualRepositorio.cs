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
using Unna.OperationalReport.Data.Mensual.Entidades;
using Unna.OperationalReport.Data.Mensual.Procedimientos;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Mensual.Repositorios.Implementaciones
{
    public class MensualRepositorio : OperacionalRepositorio<object, object>, IMensualRepositorio
    {
        public MensualRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<DatoCpgna50?> BuscarDatoCpgna50Async(DateTime desde, DateTime hasta,int? idLote)
        {
            DatoCpgna50? entidad = default(DatoCpgna50);
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<DatoCpgna50?>("Mensual.DatoCpgna50",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Desde = desde,
                        Hasta = hasta,
                        IdLote = idLote,
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;
        }



    }
}
