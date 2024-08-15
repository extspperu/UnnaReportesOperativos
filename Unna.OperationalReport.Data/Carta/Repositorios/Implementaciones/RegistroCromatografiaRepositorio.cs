using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Carta.Procedimientos;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Registro.Entidades;

namespace Unna.OperationalReport.Data.Carta.Repositorios.Implementaciones
{
    public class RegistroCromatografiaRepositorio : OperacionalRepositorio<RegistroCromatografia, long>, IRegistroCromatografiaRepositorio
    {
        public RegistroCromatografiaRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public override async Task InsertarAsync(RegistroCromatografia entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Carta.RegistroCromatografia (Periodo,HoraMuestreo,Tipo,IdLote,Tanque,Creado,Actualizado,EstaBorrado,IdUsuario) VALUES(@Periodo,@HoraMuestreo,@Tipo,@IdLote,@Tanque,@Creado,@Actualizado,@EstaBorrado,@IdUsuario)";
                entidad.Id = await conexion.QuerySingleAsync<long>(sql, entidad, commandType: CommandType.Text);
            }
        }
        
        public override async Task EditarAsync(RegistroCromatografia entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "UPDATE Carta.RegistroCromatografia SET Periodo=@Periodo,HoraMuestreo=@HoraMuestreo,Tipo=@Tipo,IdLote=@IdLote,Tanque=@Tanque,Actualizado=@Actualizado,EstaBorrado=@EstaBorrado,IdUsuario=@IdUsuario WHERE IdRegistroCromatografia=@Id";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }
        
      

        public async Task<RegistroCromatografia?> BuscarPorPeriodoTipoYTanqueAsync(DateTime periodo, string? tipo, string? tanque)
        {
            RegistroCromatografia? entidad = default(RegistroCromatografia?);
            var sql = " SELECT IdRegistroCromatografia as Id,Periodo,HoraMuestreo,Tipo,IdLote,Tanque,Creado,Actualizado,EstaBorrado,IdUsuario FROM Carta.RegistroCromatografia WHERE Periodo=CAST(@Periodo AS DATE) AND Tipo LIKE @Tipo AND Tanque LIKE @Tanque";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<RegistroCromatografia>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Periodo = periodo,
                        Tipo = tipo,
                        Tanque = tanque,
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;
        }

        public async Task<RegistroCromatografia?> BuscarPorPeriodoTipoYIdLoteAsync(DateTime periodo, string? tipo, int? idLote)
        {
            RegistroCromatografia? entidad = default(RegistroCromatografia?);

            string? lote = default(string?);
            if (idLote.HasValue)
            {
                lote = "AND IdLote=@IdLote";
            }
            var sql = $"SELECT IdRegistroCromatografia as Id,Periodo,HoraMuestreo,Tipo,IdLote,Tanque,Creado,Actualizado,EstaBorrado,IdUsuario FROM Carta.RegistroCromatografia WHERE Periodo=CAST(@Periodo AS DATE) AND Tipo LIKE @Tipo {lote}";

            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<RegistroCromatografia>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Periodo = periodo,
                        Tipo = tipo,
                        IdLote = idLote,
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;
        }

        public async Task<RegistroCromatografia?> BuscarPorPeriodoYTipoAsync(DateTime periodo, string? tipo)
        {
            RegistroCromatografia? entidad = default(RegistroCromatografia?);
            var sql = " SELECT IdRegistroCromatografia as Id,Periodo,HoraMuestreo,Tipo,IdLote,Tanque,Creado,Actualizado,EstaBorrado,IdUsuario FROM Carta.RegistroCromatografia WHERE Periodo=CAST(@Periodo AS DATE) AND Tipo LIKE @Tipo AND IdLote=@IdLote";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<RegistroCromatografia>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Periodo = periodo,
                        Tipo = tipo,
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;
        }

        public async Task<List<ListarCromatograficoPorLotes>?> ListarReporteCromatograficoPorLotesAsync(DateTime periodo)
        {
            List<ListarCromatograficoPorLotes> entidad = new List<ListarCromatograficoPorLotes>();
            var sql = "Carta.ListarCromatograficoPorLotes";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<ListarCromatograficoPorLotes>(sql,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Periodo = periodo,
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }


    }
}
