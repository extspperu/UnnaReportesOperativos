using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Procedimientos;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Mensual.Entidades;

namespace Unna.OperationalReport.Data.Carta.Repositorios.Implementaciones
{
    public class InformeMensualRepositorio : OperacionalRepositorio<object, object>, IInformeMensualRepositorio
    {
        public InformeMensualRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public async Task<List<Osinergmin1>?> RecepcionGasNaturalAsync(DateTime desde, DateTime hasta)
        {
            List<Osinergmin1> entidad = new List<Osinergmin1>();
            var sql = "Carta.InformeMensualOsinergmin";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Osinergmin1>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Desde = desde,
                        Hasta = hasta
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }


        public async Task<List<Osinergmin1>?> ReporteMensualUsoDeGasAsync(DateTime desde, DateTime hasta)
        {
            List<Osinergmin1> entidad = new List<Osinergmin1>();
            var sql = "Carta.ReporteMensualUsoDeGas";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Osinergmin1>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Desde = desde,
                        Hasta = hasta
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }
        
        public async Task<List<Osinergmin1>?> ProduccionLiquidosGasNaturalAsync(DateTime desde, DateTime hasta)
        {
            List<Osinergmin1> entidad = new List<Osinergmin1>();
            var sql = "Carta.ReporteMensualProduccionLiquidosGasNatural";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Osinergmin1>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Desde = desde,
                        Hasta = hasta
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }

    }
}
