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
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones
{
    public class OsinergminRepositorio : OperacionalRepositorio<Osinergmin, DateTime>, IOsinergminRepositorio
    {
        public OsinergminRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public override async Task<Osinergmin?> BuscarPorIdAsync(DateTime fecha)
        {
            Osinergmin? entidad = default(Osinergmin?);
            var sql = "SELECT Fecha,Username,Password,NacionalizadaM3,NacionalizadaTn,NacionalizadaBbl,IdArchivo,IdUsuario,Creado,Actualizado FROM Registro.Osinergmin WHERE Fecha=CAST(@Fecha AS DATE)";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Osinergmin>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        Fecha = fecha,
                    }).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;
        }

        public override async Task InsertarAsync(Osinergmin entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "INSERT INTO Registro.Osinergmin(Fecha,Username,Password,NacionalizadaM3,NacionalizadaTn,NacionalizadaBbl,IdArchivo,IdUsuario) VALUES(@Fecha,@Username,@Password,@NacionalizadaM3,@NacionalizadaTn,@NacionalizadaBbl,@IdArchivo,@IdUsuario)";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

        public override async Task EditarAsync(Osinergmin entidad)
        {
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var sql = "UPDATE Registro.Osinergmin SET Username=@Username,Password=@Password,NacionalizadaM3=@NacionalizadaM3,NacionalizadaTn=@NacionalizadaTn,NacionalizadaBbl=@NacionalizadaBbl,IdArchivo=@IdArchivo,IdUsuario=@IdUsuario,Actualizado=@Actualizado WHERE Fecha=@Fecha";
                await conexion.QueryAsync(sql, entidad, commandType: CommandType.Text);
            }
        }

    }
}
