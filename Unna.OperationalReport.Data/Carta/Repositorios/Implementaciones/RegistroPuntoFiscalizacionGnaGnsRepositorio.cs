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
    public class RegistroPuntoFiscalizacionGnaGnsRepositorio : OperacionalRepositorio<RegistroPuntoFiscalizacionGnaGns, long>, IRegistroPuntoFiscalizacionGnaGnsRepositorio
    {
        public RegistroPuntoFiscalizacionGnaGnsRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public override async Task InsertarAsync(RegistroPuntoFiscalizacionGnaGns entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Carta.RegistroPuntoFiscalizacionGnaGns (Fecha,C6,C3,Ic4,Nc4,NeoC5,Ic5,Nc5,Nitrog,C1,Co2,C2,O2,Grav,Btu,Lgn,LgnRpte,Conciliado,Comentario,Creado,Actualizado,IdRegistroCromatografia,IdUsuario) VALUES(@Fecha,@C6,@C3,@Ic4,@Nc4,@NeoC5,@Ic5,@Nc5,@Nitrog,@C1,@Co2,@C2,@O2,@Grav,@Btu,@Lgn,@LgnRpte,@Conciliado,@Comentario,@Creado,@Actualizado,@IdRegistroCromatografia,@IdUsuario)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EliminarAsync(RegistroPuntoFiscalizacionGnaGns entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "DELETE FROM Carta.RegistroPuntoFiscalizacionGnaGns WHERE IdRegistroCromatografia=@IdRegistroCromatografia";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public async Task<List<RegistroPuntoFiscalizacionGnaGns>?> ListarPorIdRegistroCromatografiaAsync(long idRegistroCromatografia)
        {
            List<RegistroPuntoFiscalizacionGnaGns> entidad = new List<RegistroPuntoFiscalizacionGnaGns>();
            var sql = "SELECT * FROM Carta.RegistroPuntoFiscalizacionGnaGns WHERE IdRegistroCromatografia=@IdRegistroCromatografia";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<RegistroPuntoFiscalizacionGnaGns>(sql,
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
