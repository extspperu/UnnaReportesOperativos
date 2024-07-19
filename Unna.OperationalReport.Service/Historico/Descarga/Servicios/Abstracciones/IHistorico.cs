using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Historico.Descarga.Servicios.Abstracciones
{
    public interface IHistorico
    {
        Task<MemoryStream> ExistenciasExcel();
    }
}
