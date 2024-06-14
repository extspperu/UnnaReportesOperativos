using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Cartas.ResumenVentaCliente.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Cartas.ResumenVentaCliente.Servicios.Abstracciones
{
    public interface IResumenVentaClienteServicio
    {
        Task<OperacionDto<RespuestaSimpleDto<bool>>> ProcesarArchivoAsync(ProcesarArchivoCartaDto peticion);
    }
}
