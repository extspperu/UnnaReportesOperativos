using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos
{
    public class BoletaCnpcTabla1Dto
    {
        public string? Fecha { get; set; }
        public double? GasMpcd { get; set; }
        public double? GlpBls { get; set; }
        public double? CgnBls { get; set; }
        public double? CnsMpc { get; set; }
        public double? CgMpc { get; set; }
    }
}
