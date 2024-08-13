using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos
{
    public class ResBalanceEnergLIVDetMedGasDto
    {
        public int Dia { get; set; }
        public double? MedGasGasNatAsocMedVolumen { get; set; }
        public double? MedGasGasNatAsocMedPoderCal { get; set; }
        public double? MedGasGasNatAsocMedEnergia { get; set; }
        public double? MedGasGasCombSecoMedVolumen { get; set; }
        public double? MedGasGasCombSecoMedPoderCal { get; set; }
        public double? MedGasGasCombSecoMedEnergia { get; set; }
        public double? MedGasVolGasEquivLgnVolumen { get; set; }
        public double? MedGasVolGasEquivLgnPoderCal { get; set; }
        public double? MedGasVolGasEquivLgnEnergia { get; set; }
        public double? MedGasVolGasClienteVolumen { get; set; }
        public double? MedGasVolGasClientePoderCal { get; set; }
        public double? MedGasVolGasClienteEnergia { get; set; }
        public double? MedGasVolGasSaviaVolumen { get; set; }
        public double? MedGasVolGasSaviaPoderCal { get; set; }
        public double? MedGasVolGasSaviaEnergia { get; set; }
        public double? MedGasVolGasLimaGasVolumen { get; set; }
        public double? MedGasVolGasLimaGasPoderCal { get; set; }
        public double? MedGasVolGasLimaGasEnergia { get; set; }
        public double? MedGasVolGasGasNorpVolumen { get; set; }
        public double? MedGasVolGasGasNorpPoderCal { get; set; }
        public double? MedGasVolGasGasNorpEnergia { get; set; }
        public double? MedGasVolGasQuemadoVolumen { get; set; }
        public double? MedGasVolGasQuemadoPoderCal { get; set; }
        public double? MedGasVolGasQuemadoEnergia { get; set; }
    }
}
