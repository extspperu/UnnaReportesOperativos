using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos
{
    public class BoletaVolumenesUNNAEnergiaCNPCDetDto
    {

        public int? Fecha { get; set; }
        public double? GNAMPCS { get; set; }
        public double? LGNGLPBLS { get; set; }
        public double? LGNCGNBLS { get; set; }
        public double? GNSMPCS { get; set; }
        public double? GCMPCS { get; set; }

    }
}
