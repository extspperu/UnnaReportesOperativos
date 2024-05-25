using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Reporte.Entidades;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones
{
    public interface IComposicionRepositorio : IOperacionalRepositorio<Composicion,DateTime>
    {
        Task EliminarPorFechaAsync(DateTime desde, DateTime hasta);
        Task<List<Composicion>> ListarPorFechaAsync(DateTime desde, DateTime hasta);
    }
}
