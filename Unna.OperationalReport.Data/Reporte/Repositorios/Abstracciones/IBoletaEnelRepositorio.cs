using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Infraestructura.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Procedimientos;

namespace Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones
{
    public interface IBoletaEnelRepositorio:IOperacionalRepositorio<object, object>
    {
        Task<List<ObtenerLiquidosBarriles>> ListarLiquidosBarrilesAsync(DateTime? diaOperativo);
        Task<ObtenerPgtVolumen?> ObtenerPgtVolumen(DateTime? diaOperativo);

    }
}
