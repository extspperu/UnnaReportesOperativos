using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Laboratorio.GNSGNA.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Laboratorio.GNSGNA.Servicios.Abstracciones
{
    public interface IGNSGNA
    {
        List<GNSGNAData> ObtenerAsync();
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(GNSGNADto peticion);
    }
}
