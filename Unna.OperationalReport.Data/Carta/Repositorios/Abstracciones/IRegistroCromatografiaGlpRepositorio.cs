using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones
{
    public interface IRegistroCromatografiaGlpRepositorio : IOperacionalRepositorio<RegistroCromatografiaGlp, long>
    {
        Task<List<RegistroCromatografiaGlp>?> ListarPorIdRegistroCromatografiaAsync(long idRegistroCromatografia);
    }
}
