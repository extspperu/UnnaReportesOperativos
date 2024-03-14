using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos
{
    public class FacturacionGnsLIVDetDto
    {
        public string? Concepto { get; set; }
        public double? Mpc { get; set; }
        public double? Mmbtu { get; set; }
        public double? PrecioUs { get; set; }
        public double? ImporteUs { get; set; }
        public double? TotalUs { get; set; }

    }
}
