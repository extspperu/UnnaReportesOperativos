using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Data.Carta.Repositorios.Implementaciones
{
    public class RegistroCromatografiaGlpRepositorio : OperacionalRepositorio<RegistroCromatografiaGlp, long>, IRegistroCromatografiaGlpRepositorio
    {
        public RegistroCromatografiaGlpRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }



        public override async Task InsertarAsync(RegistroCromatografiaGlp entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Carta.RegistroCromatografiaGlp (Fecha,C1,C2,C3,Ic4,Nc4,NeoC5,Ic5,Nc5,C6,Drel,PresionVapor,T95,MolarTotal,Tk,NroDespacho,Creado,Actualizado,IdRegistroCromatografia,IdUsuario) VALUES(@Fecha,@C1,@C2,@C3,@Ic4,@Nc4,@NeoC5,@Ic5,@Nc5,@C6,@Drel,@PresionVapor,@T95,@MolarTotal,@Tk,@NroDespacho,@Creado,@Actualizado,@IdRegistroCromatografia,@IdUsuario)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EliminarAsync(RegistroCromatografiaGlp entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Carta.RegistroCromatografiaGlp WHERE IdRegistroCromatografia=@IdRegistroCromatografia";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public async Task<List<RegistroCromatografiaGlp>?> ListarPorIdRegistroCromatografiaAsync(long idRegistroCromatografia)
        {
            List<RegistroCromatografiaGlp> entidad = new List<RegistroCromatografiaGlp>();
            var sql = "SELECT * FROM Carta.RegistroCromatografiaGlp WHERE IdRegistroCromatografia=@IdRegistroCromatografia";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<RegistroCromatografiaGlp>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdRegistroCromatografia = idRegistroCromatografia,
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.ToList();
            }
            return entidad;

        }


    }
}
