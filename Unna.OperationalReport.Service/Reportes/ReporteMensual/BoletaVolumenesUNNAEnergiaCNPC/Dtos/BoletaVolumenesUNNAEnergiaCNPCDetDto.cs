using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos
{
    public class BoletaVolumenesUNNAEnergiaCNPCDetDto
    {

        public DateTime? Fecha { get; set; }
        public int? Id { get; set; }
        public double? GasMpcd { get; set; }
        public double? GlpBls { get; set; }
        public double? CgnBls { get; set; }
        public double? GnsMpc { get; set; }
        public double? GcMpc { get; set; }

    }
}
