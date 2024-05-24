using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Servicios.Abstracciones
{
    public interface IBoletaVentaGnsUnnaEnergiaLimagasServicio
    {

        Task<OperacionDto<BoletaVentaGnsUnnaEnergiaLimagasDto>> ObtenerAsync(long idUsuario);
        Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletaVentaGnsUnnaEnergiaLimagasDto peticion);
        Task<string> GenerarPdfAsync(string tempFilePath);
    }
}
