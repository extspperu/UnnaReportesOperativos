using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ResDiarioFiscalProd.Dtos
{
    public class ResDiarioFiscalProdDto
    {
        public string? Fecha { get; set; }
        public string? Observacion { get; set; }
        public List<ResDiarioFiscalProdDetDto>? ResDiarioFiscalProdDet { get; set; }

    }
}
