using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos
{
    public class VolumenGasProduccionPetroperuDto
    {
        public int Item { get; set; }
        public string? Suministrador { get; set; }
        public double GnaRecibido { get; set; }
        public double GnsTrasferido { get; set; }
    }
}
