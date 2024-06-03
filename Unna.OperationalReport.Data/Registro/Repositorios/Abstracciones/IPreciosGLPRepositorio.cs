using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones
{
    public interface IPreciosGLPRepositorio : IOperacionalRepositorio<Registro.Entidades.PreciosGLP, long>
    {
        Task<List<Entidades.PreciosGLP?>> ObtenerPreciosGLPMensualAsync(DateTime? diaOperativo);
    }
}
