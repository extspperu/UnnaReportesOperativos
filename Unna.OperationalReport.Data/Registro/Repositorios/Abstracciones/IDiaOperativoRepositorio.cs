using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones
{
    public interface IDiaOperativoRepositorio :IOperacionalRepositorio<DiaOperativo, long>
    {
        Task<DiaOperativo?> ObtenerPorIdLoteYFechaAsync(int idLote, DateTime? fecha, int? idGrupo, int? numeroRegistro);
        Task<List<DiaOperativo>?> ListarPorIdLoteYFechaAsync(int idLote, DateTime? fecha, int? idGrupo, int? numeroRegistro);
        Task<List<DiaOperativo>?> ListarPorFechaYIdGrupoAsync(DateTime? fecha, int? idGrupo);
    }
}
