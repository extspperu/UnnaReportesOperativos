using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Dtos
{
    public class BarrilesProductoDto
    {
        public int Item { get; set; }
        public string? Fecha { get; set; }
        public double? Glp { get; set; }
        public double? Cgn { get; set; }
        public double? Total { get; set; }
    }
}
