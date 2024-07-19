using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Carta.Procedimientos;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones
{
    public interface IRegistroCromatografiaRepositorio : IOperacionalRepositorio<RegistroCromatografia, long>
    {
        Task<List<ListarCromatograficoPorLotes>?> ListarReporteCromatograficoPorLotesAsync(DateTime periodo);

        Task<RegistroCromatografia?> BuscarPorPeriodoTipoYTanqueAsync(DateTime periodo, string? tipo, string? tanque);
        Task<RegistroCromatografia?> BuscarPorPeriodoTipoYIdLoteAsync(DateTime periodo, string? tipo, int? idLote);
        Task<RegistroCromatografia?> BuscarPorPeriodoYTipoAsync(DateTime periodo, string? tipo);
    }
}
