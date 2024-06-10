using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.ResBalanceEnergLIV.Dtos
{
    public class ResBalanceEnergLIVDetGnaFiscDto
    {
        // GNA Fiscalizado
        public double? Dia { get; set; }
        public double? GnaFiscVtaRefVolumen { get; set; }
        public double? GnaFiscVtaRefPoderCal { get; set; }
        public double? GnaFiscVtaRefEnergia { get; set; }
        public double? GnaFiscVtaLimaGasVolumen { get; set; }
        public double? GnaFiscVtaLimaGasPoderCal { get; set; }
        public double? GnaFiscVtaLimaGasEnergia { get; set; }
        public double? GnaFiscGasNorpVolumen { get; set; }
        public double? GnaFiscGasNorpPoderCal { get; set; }
        public double? GnaFiscGasNorpEnergia { get; set; }
        public double? GnaFiscVtaEnelVolumen { get; set; }
        public double? GnaFiscVtaEnelPoderCal { get; set; }
        public double? GnaFiscVtaEnelEnergia { get; set; }
        public double? GnaFiscGcyLgnVolumen { get; set; }
        public double? GnaFiscGcyLgnPoderCal { get; set; }
        public double? GnaFiscGcyLgnEnergia { get; set; }
        public double? GnaFiscGnafVolumen { get; set; }
        public double? GnaFiscGnafPoderCal { get; set; }
        public double? GnaFiscGnafEnergia { get; set; }

        public double? GnaFiscTotalVolumen { get; set; }
        public double? GnaFiscTotalEnergia { get; set; }
    }
}
