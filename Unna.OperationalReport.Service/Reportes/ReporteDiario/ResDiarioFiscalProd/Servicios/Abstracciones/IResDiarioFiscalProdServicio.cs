using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ResDiarioFiscalProd.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ResDiarioFiscalProd.Servicios.Abstracciones
{
    public interface IResDiarioFiscalProdServicio
    {
        Task<OperacionDto<ResDiarioFiscalProdDto>> ObtenerAsync(long idUsuario);
    }
}
