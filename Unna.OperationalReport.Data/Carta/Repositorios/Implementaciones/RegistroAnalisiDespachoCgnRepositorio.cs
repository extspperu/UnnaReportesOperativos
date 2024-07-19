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

namespace Unna.OperationalReport.Data.Carta.Repositorios.Implementaciones
{
    public class RegistroAnalisiDespachoCgnRepositorio : OperacionalRepositorio<RegistroAnalisiDespachoCgn, object>, IRegistroAnalisiDespachoCgnRepositorio
    {

        public RegistroAnalisiDespachoCgnRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public override async Task InsertarAsync(RegistroAnalisiDespachoCgn entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Carta.RegistroAnalisiDespachoCgn (Fecha,IdRegistroCromatografia,Pinic,P5,P10,P30,P50,P70,P90,P95,Pfin,Api,Gesp,Rvp,NroDespacho,Creado,Actualizado,IdUsuario) VALUES(@Fecha,@IdRegistroCromatografia,@Pinic,@P5,@P10,@P30,@P50,@P70,@P90,@P95,@Pfin,@Api,@Gesp,@Rvp,@NroDespacho,@Creado,@Actualizado,@IdUsuario)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EliminarAsync(RegistroAnalisiDespachoCgn entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Carta.RegistroAnalisiDespachoCgn WHERE IdRegistroCromatografia=@IdRegistroCromatografia";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public async Task<List<RegistroAnalisiDespachoCgn>?> ListarPorIdRegistroCromatografiaAsync(long idRegistroCromatografia)
        {
            List<RegistroAnalisiDespachoCgn> entidad = new List<RegistroAnalisiDespachoCgn>();
            var sql = "SELECT * FROM Carta.RegistroAnalisiDespachoCgn WHERE IdRegistroCromatografia=@IdRegistroCromatografia";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<RegistroAnalisiDespachoCgn>(sql,
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
