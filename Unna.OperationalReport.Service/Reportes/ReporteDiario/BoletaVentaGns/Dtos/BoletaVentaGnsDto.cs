using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos
{
    public class BoletaVentaGnsDto
    {
        public string? Fecha { get; set; }
        public decimal Mpcs { get; set; }
        public decimal BtuPcs { get; set; }
        public decimal Mmbtu { get; set; }
    }
}
