using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Procedimientos;
using Unna.OperationalReport.Data.Reporte.Procedimientos;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones
{
    public interface IRegistroRepositorio : IOperacionalRepositorio<Registro.Entidades.Registro, long>
    {
        Task<Entidades.Registro?> ObtenerPorIdDatoYDiaOperativoAsync(int idDato, long idDiaOperativo);
        Task<List<Entidades.Registro>?> BuscarPorIdDiaOperativoAsync(long idDiaOperativo);
        Task<Entidades.Registro?> ObtenerValorAsync(int? idDato, int? idLote, DateTime? diaOperativo, int? numeroRegistro);

        Task<List<Entidades.Registro?>> ObtenerValorMensualAsync(int? idDato, int? idLote, DateTime? diaOperativo);
        Task<List<Entidades.Registro?>> ObtenerValorPoderCalorificoAsync(int? idDato, int? idLote, DateTime? diaOperativo);
        Task<List<Entidades.Registro?>> ObtenerValorPoderCalorificoAsync2(int? idDato, int? idLote, DateTime? diaOperativo);

        Task<List<Entidades.Registro_GNS?>> ObtenerValorMensualGNSAsync(int? idDato, int? idLote, DateTime? diaOperativo);

        Task<List<ListarValoresRegistrosPorFecha>> ListarDatosPorFechaAsync(DateTime? diaOperativo);
        Task<List<ListarGasNaturalAsociado>> ListarReporteDiarioGasNaturalAsociadoAsync(DateTime? diaOperativo);
        Task<List<BoletaCnpcFactoresDistribucionDeGasCombustible>> BoletaCnpcFactoresDistribucionDeGasCombustibleAsync(DateTime? diaOperativo);
        Task<List<ResBalanceEnergLIVDetMedGas>> ObtenerMedicionesGasAsync();
        Task<FechaActual> ObtenerFechaActualAsync();
        Task<double> ObtenerFactorAsync(DateTime diaOperativo, int idLote, double eficiencia);

        Task<double> ObtenerVolumenGNSManualAsync();
        Task<double> ObtenerIGVGNSManualAsync();


    }
}
