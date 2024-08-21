using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Procedimientos;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones
{
    public interface IEnviarCorreoRepositorio : IOperacionalRepositorio<EnviarCorreo, long>
    {
        Task<EnviarCorreo?> BuscarPorIdReporteYFechaAsync(int idReporte, DateTime? fecha);
        Task<List<BuscarCorreosEnviados>> BuscarCorreosEnviadoAsync(DateTime? diaOperativo, string? grupo, int? idReporte);
    }
}
