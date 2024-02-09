using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Servicios.Abstracciones
{
    public interface IRegistroSupervisorServicio
    {
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(string accion,RegistroSupervisorDto peticion);
        Task<OperacionDto<RegistroSupervisorDto>> ObtenerPorFechaAsync(DateTime fecha);
        Task<OperacionDto<List<AdjuntoSupervisorDto>?>> ValidarArhivosAsync(List<IFormFile> files);

    }
}
