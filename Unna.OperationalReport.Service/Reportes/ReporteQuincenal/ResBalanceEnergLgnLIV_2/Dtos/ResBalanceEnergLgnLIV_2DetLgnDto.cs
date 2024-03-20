using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Dtos
{
    public class ResBalanceEnergLgnLIV_2DetLgnDto
    {
        public double? Dia { get; set; }
        public double? MedGasGlpVolumen { get; set; }
        public double? MedGasGlpPoderCal { get; set; }
        public double? MedGasGlpEnergia { get; set; }
        public double? MedGasGlpDensidad { get; set; }
        public double? MedGasCgnVolumen { get; set; }
        public double? MedGasCgnPoderCal { get; set; }
        public double? MedGasCgnEnergia { get; set; }
        public double? MedGasLgnVolumen { get; set; }
        public double? MedGasLgnPoderCal { get; set; }
        public double? MedGasLgnEnergia { get; set; }
    }
}
