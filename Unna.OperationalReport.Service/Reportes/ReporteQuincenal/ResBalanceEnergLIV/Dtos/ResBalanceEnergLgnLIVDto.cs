using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV.Dtos
{
    public class ResBalanceEnergLgnLIVDto
    {
        // Medicion de Gas Natural Lote IV - 
        public string? Lote { get; set; }
        public string? Mes { get; set; }
        public string? Anio { get; set; }


        //Medicion de Gas Natural Lote IV - Acumulado Quincena UNNA
        public double? AcumUnnaQ1MedGasGlpVolumen { get; set; }
        public double? AcumUnnaQ1MedGasGlpPoderCal { get; set; }
        public double? AcumUnnaQ1MedGasGlpEnergia { get; set; }
        public double? AcumUnnaQ1MedGasGlpDensidad { get; set; }
        public double? AcumUnnaQ1MedGasCgnVolumen { get; set; }
        public double? AcumUnnaQ1MedGasCgnPoderCal { get; set; }
        public double? AcumUnnaQ1MedGasCgnEnergia { get; set; }
        public double? AcumUnnaQ1MedGasLgnVolumen { get; set; }
        public double? AcumUnnaQ1MedGasLgnPoderCal { get; set; }
        public double? AcumUnnaQ1MedGasLgnEnergia { get; set; }

        public double? AcumUnnaQ2MedGasGlpVolumen { get; set; }
        public double? AcumUnnaQ2MedGasGlpPoderCal { get; set; }
        public double? AcumUnnaQ2MedGasGlpEnergia { get; set; }
        public double? AcumUnnaQ2MedGasGlpDensidad { get; set; }
        public double? AcumUnnaQ2MedGasCgnVolumen { get; set; }
        public double? AcumUnnaQ2MedGasCgnPoderCal { get; set; }
        public double? AcumUnnaQ2MedGasCgnEnergia { get; set; }
        public double? AcumUnnaQ2MedGasLgnVolumen { get; set; }
        public double? AcumUnnaQ2MedGasLgnPoderCal { get; set; }
        public double? AcumUnnaQ2MedGasLgnEnergia { get; set; }

        //Medicion de Gas Natural Lote IV - Acumulado Quincena PERUPETRO
        public double? AcumPeruPetroQ1MedGasGlpVolumen { get; set; }
        public double? AcumPeruPetroQ1MedGasGlpPoderCal { get; set; }
        public double? AcumPeruPetroQ1MedGasGlpEnergia { get; set; }
        public double? AcumPeruPetroQ1MedGasGlpDensidad { get; set; }
        public double? AcumPeruPetroQ1MedGasCgnVolumen { get; set; }
        public double? AcumPeruPetroQ1MedGasCgnPoderCal { get; set; }
        public double? AcumPeruPetroQ1MedGasCgnEnergia { get; set; }
        public double? AcumPeruPetroQ1MedGasLgnVolumen { get; set; }
        public double? AcumPeruPetroQ1MedGasLgnPoderCal { get; set; }
        public double? AcumPeruPetroQ1MedGasLgnEnergia { get; set; }

        public double? AcumPeruPetroQ2MedGasGlpVolumen { get; set; }
        public double? AcumPeruPetroQ2MedGasGlpPoderCal { get; set; }
        public double? AcumPeruPetroQ2MedGasGlpEnergia { get; set; }
        public double? AcumPeruPetroQ2MedGasGlpDensidad { get; set; }
        public double? AcumPeruPetroQ2MedGasCgnVolumen { get; set; }
        public double? AcumPeruPetroQ2MedGasCgnPoderCal { get; set; }
        public double? AcumPeruPetroQ2MedGasCgnEnergia { get; set; }
        public double? AcumPeruPetroQ2MedGasLgnVolumen { get; set; }
        public double? AcumPeruPetroQ2MedGasLgnPoderCal { get; set; }
        public double? AcumPeruPetroQ2MedGasLgnEnergia { get; set; }

        //Medicion de Gas Natural Lote IV - Diff Unna-PeruPetro Quincena 1 y 2
        public double? DifUPQ1MedGasGlpVolumen { get; set; }
        public double? DifUPQ1MedGasGlpPoderCal { get; set; }
        public double? DifUPQ1MedGasGlpEnergia { get; set; }
        public double? DifUPQ1MedGasGlpDensidad { get; set; }
        public double? DifUPQ1MedGasCgnVolumen { get; set; }
        public double? DifUPQ1MedGasCgnPoderCal { get; set; }
        public double? DifUPQ1MedGasCgnEnergia { get; set; }
        public double? DifUPQ1MedGasLgnVolumen { get; set; }
        public double? DifUPQ1MedGasLgnPoderCal { get; set; }
        public double? DifUPQ1MedGasLgnEnergia { get; set; }

        public double? DifUPQ2MedGasGlpVolumen { get; set; }
        public double? DifUPQ2MedGasGlpPoderCal { get; set; }
        public double? DifUPQ2MedGasGlpEnergia { get; set; }
        public double? DifUPQ2MedGasGlpDensidad { get; set; }
        public double? DifUPQ2MedGasCgnVolumen { get; set; }
        public double? DifUPQ2MedGasCgnPoderCal { get; set; }
        public double? DifUPQ2MedGasCgnEnergia { get; set; }
        public double? DifUPQ2MedGasLgnVolumen { get; set; }
        public double? DifUPQ2MedGasLgnPoderCal { get; set; }
        public double? DifUPQ2MedGasLgnEnergia { get; set; }

        public double? PromQ1MedGasGlpDensidad { get; set; }
        public double? PromQ1MedGasGlpPoderCal { get; set; }
        public double? PromQ1MedGasCgnPoderCal { get; set; }
        public double? PromQ1MedGasLgnPoderCal { get; set; }
        public double? Q1MedGasFactorConv { get; set; }
        public double? TotalQ1MedGasGlpEnergia { get; set; }
        public double? TotalQ1MedGasCgnEnergia { get; set; }


        public double? PromQ2MedGasGlpDensidad { get; set; }
        public double? PromQ2MedGasGlpPoderCal { get; set; }
        public double? PromQ2MedGasCgnPoderCal { get; set; }
        public double? PromQ2MedGasLgnPoderCal { get; set; }
        public double? Q2MedGasFactorConv { get; set; }
        public double? TotalQ2MedGasGlpEnergia { get; set; }
        public double? TotalQ2MedGasCgnEnergia { get; set; }

        public List<ResBalanceEnergLgnLIVDetLgnDto>? ResBalanceEnergLgnLIVDetLgn { get; set; }

    }
}
