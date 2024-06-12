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
using Unna.OperationalReport.Data.Mantenimiento.Entidades;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Mantenimiento.Repositorios.Implementaciones
{
    public class CartaRepositorio : OperacionalRepositorio<Entidades.Carta, int>, ICartaRepositorio
    {
        public CartaRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }

        public override async Task<Entidades.Carta?> BuscarPorIdAsync(int idCarta)
        {
            Entidades.Carta? entidad = default(Entidades.Carta);
            var sql = "SELECT * FROM Mantenimiento.Carta WHERE IdCarta=@IdCarta";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<Entidades.Carta>(sql,
                    commandType: CommandType.Text,
                    param: new
                    {
                        IdCarta = idCarta
                    }
                    ).ConfigureAwait(false);
                entidad = resultados.FirstOrDefault();
            }
            return entidad;

        }
    }
}
