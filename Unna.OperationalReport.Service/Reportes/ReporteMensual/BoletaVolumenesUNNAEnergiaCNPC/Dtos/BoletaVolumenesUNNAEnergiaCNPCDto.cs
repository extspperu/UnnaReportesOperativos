using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos
{
    public class BoletaVolumenesUNNAEnergiaCNPCDto
    {
        public string? Año { get; set; }
        public string? Mes { get; set; }

        public List<BoletaVolumenesUNNAEnergiaCNPCDetDto>? BoletaVolumenesUNNAEnergiaCNPCDet { get; set; }
        public double? TotalGNAMPCS { get; set; }
        public double? TotalLGNGLPBLS { get; set; }
        public double? TotalLGNCGNBLS { get; set; }
        public double? TotalGNSMPCS { get; set; }
        public double? TotalGCMPCS { get; set; }

        public double? TotalGravedadEspacificoGLP { get; set; }

    }
}
