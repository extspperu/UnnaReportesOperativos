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
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Procedimientos;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones
{
    public class BoletaEnelRepositorio : OperacionalRepositorio<object, object>, IBoletaEnelRepositorio
    {
        public BoletaEnelRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<List<ObtenerLiquidosBarriles>> ListarLiquidosBarrilesAsync(DateTime? diaOperativo)
        {
            List<ObtenerLiquidosBarriles> lista = new List<ObtenerLiquidosBarriles>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ObtenerLiquidosBarriles>("Reporte.LiquidosBarriles",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<ObtenerPgtVolumen?> ObtenerPgtVolumen(DateTime? diaOperativo)
        {
            ObtenerPgtVolumen? lista = default(ObtenerPgtVolumen?);
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ObtenerPgtVolumen>("Reporte.PgtVolumen",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.FirstOrDefault();
            }
            return lista;
        }

        public async Task<List<ObtenerGnsAEnel>> ObtenerGnsAEnelAsync(DateTime? diaOperativo)
        {
            List<ObtenerGnsAEnel> lista = new List<ObtenerGnsAEnel>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ObtenerGnsAEnel>("Reporte.BalanceEnergiaGnsAEnel",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<double?> ObtenerEficienciaPlantaBalanceDeEnergiaAsync(DateTime? diaOperativo)
        {
            double? valor = default(double?);
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<double>("SELECT ISNULL(JSON_VALUE(Datos,'$.PorcentajeEficiencia'),0) as Total FROM Reporte.Imprimir WHERE IdConfiguracion=@IdReporte AND Fecha=CAST(@DiaOperativo AS DATE)",
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        IdReporte = (int)TiposReportes.BoletaBalanceEnergiaDiaria
                    }).ConfigureAwait(false);
                valor = resultados.FirstOrDefault();
            }
            return valor;
        }



    }
}
