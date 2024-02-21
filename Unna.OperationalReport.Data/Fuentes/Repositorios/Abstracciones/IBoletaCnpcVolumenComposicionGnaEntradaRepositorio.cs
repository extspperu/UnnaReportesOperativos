using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Fuentes.Entidades;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Fuentes.Repositorios.Abstracciones
{
    public interface IBoletaCnpcVolumenComposicionGnaEntradaRepositorio:IOperacionalRepositorio<BoletaCnpcVolumenComposicionGnaEntrada, int>
    {

        Task<List<BoletaCnpcVolumenComposicionGnaEntrada>> ListarAsync();
    }
}
