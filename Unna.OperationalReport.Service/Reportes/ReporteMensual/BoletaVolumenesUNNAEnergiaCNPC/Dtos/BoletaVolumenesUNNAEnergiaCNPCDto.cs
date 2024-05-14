using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos
{
    public class BoletaVolumenesUNNAEnergiaCNPCDto
    {
        public DateTime DiaOperativo { get; set; }
        public string? Anio { get; set; }
        public string? Mes { get; set; }
        public List<BoletaVolumenesUNNAEnergiaCNPCDetDto>? VolumenGna { get; set; }
        public double? TotalGasMpcd { get; set; }
        public double? TotalGlpBls { get; set; }
        public double? TotalCgnBls { get; set; }
        public double? TotalGnsMpc { get; set; }
        public double? TotalGcMpc { get; set; }
        public double? GravedadEspacificoGlp { get; set; }
        public string? Nota { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }

        public ReporteDto? General { get; set; }

    }
}
