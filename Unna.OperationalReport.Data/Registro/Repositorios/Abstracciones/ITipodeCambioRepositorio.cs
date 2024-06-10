using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones
{
    public interface ITipodeCambioRepositorio : IOperacionalRepositorio<Registro.Entidades.TipodeCambio, long>
    {
        Task<List<Entidades.TipodeCambio?>> ObtenerTipodeCambioMensualAsync(DateTime? diaOperativo,int idTipoMoneda);
    }
}
