using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV_2.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV_2.Servicios.Abstracciones
{
    public interface IComposicionGnaLIV_2Servicio
    {
        Task<OperacionDto<ComposicionGnaLIV_2Dto>> ObtenerAsync(long idUsuario);
    }
}
