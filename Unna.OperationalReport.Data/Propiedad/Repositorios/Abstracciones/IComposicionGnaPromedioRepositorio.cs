using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Propiedad.Entidades;
using Unna.OperationalReport.Data.Propiedad.Procedimientos;

namespace Unna.OperationalReport.Data.Propiedad.Repositorios.Abstracciones
{
    public interface IComposicionGnaPromedioRepositorio : IOperacionalRepositorio<ComposicionGnaPromedio, object>
    {
        Task<List<BuscarComposicionGnaPromedio>?> ListarPorIdLoteYFechaAsync(DateTime? fecha, int idLote);
    }
}
