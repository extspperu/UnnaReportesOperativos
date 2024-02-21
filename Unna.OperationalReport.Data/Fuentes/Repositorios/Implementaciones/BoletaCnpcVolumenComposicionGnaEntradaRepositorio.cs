using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Fuentes.Entidades;
using Unna.OperationalReport.Data.Fuentes.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Configuraciones.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Contextos.Abstracciones;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Fuentes.Repositorios.Implementaciones
{
    public class BoletaCnpcVolumenComposicionGnaEntradaRepositorio : OperacionalRepositorio<BoletaCnpcVolumenComposicionGnaEntrada, int>, IBoletaCnpcVolumenComposicionGnaEntradaRepositorio
    {
        public BoletaCnpcVolumenComposicionGnaEntradaRepositorio(IOperacionalUnidadDeTrabajo unidadDeTrabajo, IOperacionalConfiguracion configuracion) : base(unidadDeTrabajo, configuracion) { }


        public async Task<List<BoletaCnpcVolumenComposicionGnaEntrada>> ListarAsync()
        {
            var lista = new List<BoletaCnpcVolumenComposicionGnaEntrada>();
            var sql = "SELECT * FROM OperacionalReporte.Fuentes.BoletaCnpcVolumenComposicionGnaEntrada";
            using (var conexion = new SqlConnection(Configuracion.CadenaConexion))
            {
                var resultados = await conexion.QueryAsync<BoletaCnpcVolumenComposicionGnaEntrada>(sql,
                    commandType: CommandType.Text
                   ).ConfigureAwait(false);
                lista = resultados.ToList();
            }
            return lista;
        }


    }
}
