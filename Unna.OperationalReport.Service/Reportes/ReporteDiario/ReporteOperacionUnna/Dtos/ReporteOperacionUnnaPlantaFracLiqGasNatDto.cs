using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Dtos
{
    public class ReporteOperacionUnnaPlantaFracLiqGasNatDto
    {
        public double? VolumenLgnProcesado { get; set; }
        public double? VolumenLgnProducidoCgn { get; set; }
        public double? VolumenLgnProducidoGlp { get; set; }
        public double? VolumenLgnProducidoTotal { get; set; }
        public double? VolumenProductosCondensadosLgn { get; set; }
        public double? VolumenProductosGlp { get; set; }
        public double? VolumenProductosTotal { get; set; }
        public string? EventosOperativos { get; set; }
    }
}
