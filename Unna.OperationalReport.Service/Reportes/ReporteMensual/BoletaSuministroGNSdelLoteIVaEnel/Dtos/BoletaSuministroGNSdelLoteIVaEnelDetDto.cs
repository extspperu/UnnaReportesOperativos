using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos
{
    public class BoletaSuministroGNSdelLoteIVaEnelDetDto
    {
        public string? Fecha { get; set; }
        public double? VolumneMPC { get; set; }
        public double PCBTUPC { get; set; }
        public double EnergiaMMBTU { get; set; }
    }
}
