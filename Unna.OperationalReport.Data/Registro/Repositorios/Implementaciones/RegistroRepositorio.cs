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
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Procedimientos;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Procedimientos;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones
{
    public class RegistroRepositorio : OperacionalRepositorio<Entidades.Registro, long>, IRegistroRepositorio
    {
        public RegistroRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task<Entidades.Registro?> BuscarPorIdYNoBorradoAsync(long id)
       => await UnidadDeTrabajo.RegistroRegistros.Where(e => e.IdRegistro == id && e.EstaBorrado == false).FirstOrDefaultAsync();

        public async Task<List<Entidades.Registro>?> BuscarPorIdDiaOperativoAsync(long idDiaOperativo)
        => await UnidadDeTrabajo.RegistroRegistros.Where(e => e.IdDiaOperativo == idDiaOperativo && e.EstaBorrado == false).Include(e=>e.Dato).ToListAsync();


        public async Task<Entidades.Registro?> ObtenerPorIdDatoYDiaOperativoAsync(int idDato, long idDiaOperativo)
        => await UnidadDeTrabajo.RegistroRegistros.Where(e => e.IdDato == idDato && e.IdDiaOperativo == idDiaOperativo).FirstOrDefaultAsync();

        public async Task<Entidades.Registro?> ObtenerValorAsync(int? idDato, int? idLote, DateTime? diaOperativo, int? numeroRegistro)
        {
            Entidades.Registro? lista = default(Entidades.Registro?);
            var sql = "SELECT Valor FROM Registro.Registro WHERE IdDiaOperativo IN (SELECT IdDiaOperativo FROM Registro.DiaOperativo" +
                " WHERE NumeroRegistro=@NumeroRegistro AND IdLote=@IdLote AND Fecha=CAST(@DiaOperativo AS DATE)) AND IdDato = @IdDato;";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Entidades.Registro>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        IdLote = idLote,
                        IdDato = idDato,
                        NumeroRegistro = numeroRegistro,
                    }).ConfigureAwait(false);
                lista = resultados.FirstOrDefault();
            }
            return lista;
        }

        public async Task<List<Entidades.Registro?>> ObtenerValorPoderCalorificoAsync(int? idDato, int? idLote, DateTime? diaOperativo)
        {
            var lista = new List<Entidades.Registro>();
            
               var  sql = "SELECT b.Fecha,a.* FROM [Registro].[Registro] a inner join [Registro].[DiaOperativo] b on a.IdDiaOperativo=b.IdDiaOperativo \r\ninner join [Registro].[Dato] c on a.IdDato=c.IdDato " +
                "WHERE cast(b.Fecha as date) between CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + '01')   AS DATE) and CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + '15')   AS DATE) \r\nand \r\nc.IdDato=@IdDato  AND b.IdLote=@IdLote\r\norder by b.Fecha;";
           
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Entidades.Registro>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        IdLote = idLote,
                        IdDato = idDato,
                       // NumeroRegistro = numeroRegistro,
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<Entidades.Registro?>> ObtenerValorPoderCalorificoAsync2(int? idDato, int? idLote, DateTime? diaOperativo)
        {
            var lista = new List<Entidades.Registro>();

            var sql = "SELECT b.Fecha,a.* FROM [Registro].[Registro] a inner join [Registro].[DiaOperativo] b on a.IdDiaOperativo=b.IdDiaOperativo \r\ninner join [Registro].[Dato] c on a.IdDato=c.IdDato " +
             "WHERE cast(b.Fecha as date) between CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + '16')   AS DATE) and CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + CAST(DAY(EOMONTH(CAST(@DiaOperativo AS DATE))) AS VARCHAR(2)) )   AS DATE) \r\nand \r\nc.IdDato=@IdDato  AND b.IdLote=@IdLote\r\norder by b.Fecha;";

            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Entidades.Registro>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        IdLote = idLote,
                        IdDato = idDato,
                        // NumeroRegistro = numeroRegistro,
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<Entidades.Registro?>> ObtenerValorMensualAsync(int? idDato, int? idLote, DateTime? diaOperativo)
        {
            var lista = new List<Entidades.Registro>();
            var sql = "SELECT b.Fecha,a.* FROM [Registro].[Registro] a inner join [Registro].[DiaOperativo] b on a.IdDiaOperativo=b.IdDiaOperativo \r\ninner join [Registro].[Dato] c on a.IdDato=c.IdDato " +
               "WHERE (cast(b.Fecha as date) between CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + '01')   AS DATE) and CAST(@DiaOperativo AS DATE)) \r\nand \r\nc.IdDato=@IdDato  AND b.IdLote=@IdLote\r\norder by b.Fecha;";
           
           
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Entidades.Registro>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        IdLote = idLote,
                        IdDato = idDato,
                        // NumeroRegistro = numeroRegistro,
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<Entidades.Registro_GNS?>> ObtenerValorMensualGNSAsync(int? idDato, int? idLote, DateTime? diaOperativo)
        {
            var lista = new List<Entidades.Registro_GNS>();
            var sql = "SELECT b.Fecha,a.* FROM [Registro].[Registro] a inner join [Registro].[DiaOperativo] b on a.IdDiaOperativo=b.IdDiaOperativo \r\ninner join [Registro].[Dato] c on a.IdDato=c.IdDato " +
               "WHERE (cast(b.Fecha as date) between CAST( (CAST((YEAR(CAST(@DiaOperativo AS DATE))*100)+MONTH(CAST(@DiaOperativo AS DATE)) AS VARCHAR(6)) + '01')   AS DATE) and CAST(@DiaOperativo AS DATE)) \r\nand \r\nc.IdDato=@IdDato  AND b.IdLote=@IdLote\r\norder by b.Fecha;";


            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Entidades.Registro_GNS>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        DiaOperativo = diaOperativo,
                        IdLote = idLote,
                        IdDato = idDato,
                        // NumeroRegistro = numeroRegistro,
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }

        public async Task<List<ListarValoresRegistrosPorFecha>> ListarDatosPorFechaAsync(DateTime? diaOperativo)
        {
            List<ListarValoresRegistrosPorFecha> lista = new List<ListarValoresRegistrosPorFecha>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ListarValoresRegistrosPorFecha>("Registro.ListarValoresRegistrosPorFecha",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }



        public async Task<List<ListarGasNaturalAsociado>> ListarReporteDiarioGasNaturalAsociadoAsync(DateTime? diaOperativo)
        {
            List<ListarGasNaturalAsociado> lista = new List<ListarGasNaturalAsociado>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ListarGasNaturalAsociado>("Registro.ListarReporteDiarioGasNaturalAsociado",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }
        public async Task<List<BoletaCnpcFactoresDistribucionDeGasCombustible>> BoletaCnpcFactoresDistribucionDeGasCombustibleAsync(DateTime? diaOperativo)
        {
            List<BoletaCnpcFactoresDistribucionDeGasCombustible> lista = new List<BoletaCnpcFactoresDistribucionDeGasCombustible>();
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<BoletaCnpcFactoresDistribucionDeGasCombustible>("Registro.BoletaCnpcFactoresDistribucionDeGasCombustible",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DiaOperativo = diaOperativo
                    }).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }
        public async Task<List<ResBalanceEnergLIVDetMedGas>> ObtenerMedicionesGasAsync()
        {
            var entidad = new List<ResBalanceEnergLIVDetMedGas>();
            var sql = "Reporte.ResumenBalanceEnergiaLIV";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                await conexion.OpenAsync();
                var resultados = await conexion.QueryAsync<ResBalanceEnergLIVDetMedGas>(sql,
                    commandType: CommandType.StoredProcedure
                    ).ConfigureAwait(false);
                entidad = resultados.AsList();
            }
            return entidad;
        }

        public async Task<FechaActual> ObtenerFechaActualAsync()
        {
            var sql = "Reporte.FechaActual";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                await conexion.OpenAsync();
                var resultado = await conexion.QueryFirstOrDefaultAsync<FechaActual>(sql,
                    commandType: CommandType.StoredProcedure
                ).ConfigureAwait(false);
                return resultado;
            }
        }

        public async Task<double> ObtenerFactorAsync(DateTime diaOperativo, int idLote, double eficiencia)
        {
            double resultados = 0;
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                double resultado = 0;
                SqlCommand cmd = new SqlCommand("Reporte.ObtenerFactorConversionPorLotePetroperu_2", conexion);
                conexion.Open();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DiaOperativo", diaOperativo);
                cmd.Parameters.AddWithValue("@IdLote", idLote);
                cmd.Parameters.AddWithValue("@IdDato", 1);
                cmd.Parameters.AddWithValue("@Eficiencia", eficiencia);
                resultado = (double)cmd.ExecuteScalar();
                resultados = resultado;

            }
            return resultados;
        }

    }
}
