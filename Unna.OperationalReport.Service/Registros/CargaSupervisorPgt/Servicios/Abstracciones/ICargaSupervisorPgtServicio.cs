using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Servicios.Abstracciones
{
    public interface ICargaSupervisorPgtServicio
    {

        Task<OperacionDto<RespuestaSimpleDto<bool>>> ProcesarDocuemtoAsync(long idArchivo, long idAdjuntoSupervisor);
    }
}
