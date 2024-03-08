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
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;

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
    }
}
