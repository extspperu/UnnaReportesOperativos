using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones
{
    public interface IDatoCgnRepositorio : IOperacionalRepositorio<DatoCgn, long>
    {
        Task<List<DatoCgn>> BuscarDatoCgnPorDiaOperativoAsync(DateTime diaOperativo);
    }
}
