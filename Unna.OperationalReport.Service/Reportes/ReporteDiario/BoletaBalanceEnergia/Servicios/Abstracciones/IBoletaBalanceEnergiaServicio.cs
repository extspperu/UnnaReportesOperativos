using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones
{
    public interface IBoletaBalanceEnergiaServicio
    {
        Task<OperacionDto<BoletaBalanceEnergiaDto>> ObtenerAsync(long idUsuario);

        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletaBalanceEnergiaDto peticion);
    }
}
