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


        public async Task<DatoCpgna50?> BuscarDatoCpgna50Async(DateTime desde, DateTime hasta, int? idLote)
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


        public async Task<List<ResumenEntrega>?> ListarFactura50VolumenEntregadaAsync(DateTime desde, DateTime hasta)
        {
            List<ResumenEntrega>? entidad = new List<ResumenEntrega>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ResumenEntrega>("Mensual.Factura50VolumenEntregada",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Desde = desde,
                        Hasta = hasta,
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;
        }


        public async Task<List<Factura50Barriles>?> ListarFactura50BarrilesAsync(DateTime desde, DateTime hasta)
        {
            List<Factura50Barriles>? entidad = new List<Factura50Barriles>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Factura50Barriles>("Mensual.Factura50Barriles",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Desde = desde,
                        Hasta = hasta,
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;
        }



        public async Task<List<BuscarIndicadoresOperativos>?> BuscarIndicadoresOperativosAsync(DateTime periodo)
        {
            List<BuscarIndicadoresOperativos>? entidad = new List<BuscarIndicadoresOperativos>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<BuscarIndicadoresOperativos>("Mensual.BuscarIndicadoresOperativos",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Periodo = periodo
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;
        }

        public async Task<List<ValorizacionQuincenalVentaGnsLoteIv>?> BuscarValorizacionQuincenalVentaGnsLoteIvAsync(DateTime? desde, DateTime? hasta)
        {
            List<ValorizacionQuincenalVentaGnsLoteIv>? entidad = new List<ValorizacionQuincenalVentaGnsLoteIv>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ValorizacionQuincenalVentaGnsLoteIv?>("Mensual.ValorizacionQuincenalVentaGnsLoteIv",
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

        public async Task<List<BoletaSuministroGnsDeLoteIvAEnel>?> BuscarBoletaSuministroGnsDeLoteIvAEnelAsync(DateTime? desde, DateTime? hasta)
        {
            List<BoletaSuministroGnsDeLoteIvAEnel>? entidad = new List<BoletaSuministroGnsDeLoteIvAEnel>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<BoletaSuministroGnsDeLoteIvAEnel>("Mensual.BoletaSuministroGnsDeLoteIvAEnel",
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

        public async Task<List<BoletaValorizacionPetroPeru>?> BoletaValorizacionPetroPeruAsync(DateTime desde, DateTime hasta)
        {
            List<BoletaValorizacionPetroPeru>? entidad = new List<BoletaValorizacionPetroPeru>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<BoletaValorizacionPetroPeru>("Mensual.BoletaValorizacionPetroPeru",
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
