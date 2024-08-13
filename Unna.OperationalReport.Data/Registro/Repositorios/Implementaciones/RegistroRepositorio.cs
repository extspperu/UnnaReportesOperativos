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
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Procedimientos;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones
{
    public class RegistroRepositorio : OperacionalRepositorio<Entidades.Registro, long>, IRegistroRepositorio
    {
        public RegistroRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task<Entidades.Registro?> BuscarPorIdYNoBorradoAsync(long id)
       => await UnidadDeTrabajo.RegistroRegistros.Where(e => e.IdRegistro == id && e.EstaBorrado == false).FirstOrDefaultAsync();

        public async Task<List<Entidades.Registro>?> BuscarPorIdDiaOperativoAsync(long idDiaOperativo)
        => await UnidadDeTrabajo.RegistroRegistros.Where(e => e.IdDiaOperativo == idDiaOperativo && e.EstaBorrado == false).Include(e => e.Dato).ToListAsync();


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

            var sql = "SELECT b.Fecha,a.* FROM [Registro].[Registro] a inner join [Registro].[DiaOperativo] b on a.IdDiaOperativo=b.IdDiaOperativo \r\ninner join [Registro].[Dato] c on a.IdDato=c.IdDato " +
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
        public async Task<List<ResBalanceEnergLIVDetMedGas>> ObtenerMedicionesGasAsync(DateTime diaOperativo, int tipoReporte)
        {
            var entidad = new List<ResBalanceEnergLIVDetMedGas>();

            try
            {
                var sql = "Reporte.ResumenBalanceEnergiaLIV";
                using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
                {
                    await conexion.OpenAsync();

                    var parametros = new DynamicParameters();
                    parametros.Add("@diaOperativo", diaOperativo, DbType.Date);
                    parametros.Add("@tipoReporte", tipoReporte, DbType.Int32);

                    var resultados = await conexion.QueryAsync<ResBalanceEnergLIVDetMedGas>(
                        sql,
                        parametros,
                        commandType: CommandType.StoredProcedure
                    ).ConfigureAwait(false);

                    entidad = resultados.AsList();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }         
            
            return entidad;
        }

        public async Task<List<ParametrosQuincenalLGN>> ObtenerResumenBalanceEnergiaLGNParametrosAsync()
        {
            var entidad = new List<ParametrosQuincenalLGN>();
            var sql = "Reporte.ListarQuincenalResumenBalanceEnergiaLGNIV";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                await conexion.OpenAsync();
                var resultados = await conexion.QueryAsync<ParametrosQuincenalLGN>(sql,
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
        public async Task<List<ResumenBalanceEnergiaLGNResult>> EjecutarResumenBalanceEnergiaLGNAsync(DateTime diaOperativo, int tipoReporte)
        {
            List<ResumenBalanceEnergiaLGNResult> resultados = new List<ResumenBalanceEnergiaLGNResult>();

            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("Reporte.ResumenBalanceEnergiaLGN", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                //cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                //cmd.Parameters.AddWithValue("@tipoReporte", tipoReporte);
                cmd.Parameters.AddWithValue("@diaOperativo", diaOperativo);
                cmd.Parameters.AddWithValue("@tipoReporte", tipoReporte);
                await conexion.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ResumenBalanceEnergiaLGNResult resultado = new ResumenBalanceEnergiaLGNResult();

                        resultado.Dia = reader.GetInt32(reader.GetOrdinal("Dia"));
                        resultado.GASNaturalAsociadoMedido = reader.GetDecimal(reader.GetOrdinal("GASNaturalAsociadoMedido"));                      
                        resultado.GasCombustibleMedidoSeco = reader.GetDecimal(reader.GetOrdinal("GasCombustibleMedidoSeco"));
                        resultado.VolumenGasEquivalenteLGN = reader.GetDecimal(reader.GetOrdinal("VolumenGasEquivalenteLGN"));

                        resultados.Add(resultado);
                    }
                }
            }
            return resultados;
        }

        public async Task<List<ResumenBalanceEnergiaLGNCalculos>> EjecutarResumenBalanceEnergiaLGNCalculosAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            List<ResumenBalanceEnergiaLGNCalculos> resultados = new List<ResumenBalanceEnergiaLGNCalculos>();

            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("Reporte.ResumenBalanceEnergiaLGNCalculos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                await conexion.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ResumenBalanceEnergiaLGNCalculos resultado = new ResumenBalanceEnergiaLGNCalculos();

                        resultado.NumeroQuincena = reader.GetInt32(reader.GetOrdinal("NumeroQuincena"));
                        resultado.DensidadGlpKgBl = reader.GetDouble(reader.GetOrdinal("DensidadGlpKgBl"));
                        resultado.PcGlp = reader.GetDouble(reader.GetOrdinal("PcGlp"));
                        resultado.PcCgn = reader.GetDouble(reader.GetOrdinal("PcCgn"));
                        resultado.PcLgn = reader.GetDouble(reader.GetOrdinal("PcLgn"));
                        resultado.AvgFactorCoversion = reader.GetDouble(reader.GetOrdinal("AvgFactorCoversion"));

                        resultados.Add(resultado);
                    }
                }
            }
            return resultados;
        }

        public async Task<double> ObtenerVolumenGNSManualAsync()
        {
            double resultados = 0;
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                double resultado = 0;
                SqlCommand cmd = new SqlCommand("SELECT Valor FROM [Mantenimiento].[ValoresDefectoReporte] WHERE Llave = 'VOLUMEN_TOTAL_GNS'", conexion);
                conexion.Open();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.Text;
                resultado = (double)cmd.ExecuteScalar();
                resultados = resultado;

            }
            return resultados;

        }

        public async Task<double> ObtenerIGVGNSManualAsync()
        {
            double resultados = 0;
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                double resultado = 0;
                SqlCommand cmd = new SqlCommand("SELECT Valor FROM [Mantenimiento].[ValoresDefectoReporte] WHERE Llave = 'IGV'", conexion);
                conexion.Open();
                cmd.Connection = conexion;
                cmd.CommandType = CommandType.Text;
                resultado = (double)cmd.ExecuteScalar();
                resultados = resultado;

            }
            return resultados;

        }



    }
}