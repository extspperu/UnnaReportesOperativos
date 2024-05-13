using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos
{
    public class ResBalanceEnergLIVDto
    {
        // Medicion de Gas Natural Lote IV - 
        public string? Lote { get; set; }
        public string? Mes { get; set; }
        public string? Anio { get; set; }


        //Medicion de Gas Natural Lote IV - Acumulado Quincena UNNA
        public double? AcumUnnaQ1MedGasGasNatAsocMedVolumen { get; set; }
        public double? AcumUnnaQ1MedGasGasNatAsocMedPoderCal { get; set; }
        public double? AcumUnnaQ1MedGasGasNatAsocMedEnergia { get; set; }
        public double? AcumUnnaQ1MedGasGasCombSecoMedVolumen { get; set; }
        public double? AcumUnnaQ1MedGasGasCombSecoMedPoderCal { get; set; }
        public double? AcumUnnaQ1MedGasGasCombSecoMedEnergia { get; set; }
        public double? AcumUnnaQ1MedGasVolGasEquivLgnVolumen { get; set; }
        public double? AcumUnnaQ1MedGasVolGasEquivLgnPoderCal { get; set; }
        public double? AcumUnnaQ1MedGasVolGasEquivLgnEnergia { get; set; }
        public double? AcumUnnaQ1MedGasVolGasClienteVolumen { get; set; }
        public double? AcumUnnaQ1MedGasVolGasClientePoderCal { get; set; }
        public double? AcumUnnaQ1MedGasVolGasClienteEnergia { get; set; }
        public double? AcumUnnaQ1MedGasVolGasSaviaVolumen { get; set; }
        public double? AcumUnnaQ1MedGasVolGasSaviaPoderCal { get; set; }
        public double? AcumUnnaQ1MedGasVolGasSaviaEnergia { get; set; }
        public double? AcumUnnaQ1MedGasVolGasLimaGasVolumen { get; set; }
        public double? AcumUnnaQ1MedGasVolGasLimaGasPoderCal { get; set; }
        public double? AcumUnnaQ1MedGasVolGasLimaGasEnergia { get; set; }
        public double? AcumUnnaQ1MedGasVolGasGasNorpVolumen { get; set; }
        public double? AcumUnnaQ1MedGasVolGasGasNorpPoderCal { get; set; }
        public double? AcumUnnaQ1MedGasVolGasGasNorpEnergia { get; set; }
        public double? AcumUnnaQ1MedGasVolGasQuemadoVolumen { get; set; }
        public double? AcumUnnaQ1MedGasVolGasQuemadoPoderCal { get; set; }
        public double? AcumUnnaQ1MedGasVolGasQuemadoEnergia { get; set; }

        public double? AcumUnnaQ2MedGasGasNatAsocMedVolumen { get; set; }
        public double? AcumUnnaQ2MedGasGasNatAsocMedPoderCal { get; set; }
        public double? AcumUnnaQ2MedGasGasNatAsocMedEnergia { get; set; }
        public double? AcumUnnaQ2MedGasGasCombSecoMedVolumen { get; set; }
        public double? AcumUnnaQ2MedGasGasCombSecoMedPoderCal { get; set; }
        public double? AcumUnnaQ2MedGasGasCombSecoMedEnergia { get; set; }
        public double? AcumUnnaQ2MedGasVolGasEquivLgnVolumen { get; set; }
        public double? AcumUnnaQ2MedGasVolGasEquivLgnPoderCal { get; set; }
        public double? AcumUnnaQ2MedGasVolGasEquivLgnEnergia { get; set; }
        public double? AcumUnnaQ2MedGasVolGasClienteVolumen { get; set; }
        public double? AcumUnnaQ2MedGasVolGasClientePoderCal { get; set; }
        public double? AcumUnnaQ2MedGasVolGasClienteEnergia { get; set; }
        public double? AcumUnnaQ2MedGasVolGasSaviaVolumen { get; set; }
        public double? AcumUnnaQ2MedGasVolGasSaviaPoderCal { get; set; }
        public double? AcumUnnaQ2MedGasVolGasSaviaEnergia { get; set; }
        public double? AcumUnnaQ2MedGasVolGasLimaGasVolumen { get; set; }
        public double? AcumUnnaQ2MedGasVolGasLimaGasPoderCal { get; set; }
        public double? AcumUnnaQ2MedGasVolGasLimaGasEnergia { get; set; }
        public double? AcumUnnaQ2MedGasVolGasGasNorpVolumen { get; set; }
        public double? AcumUnnaQ2MedGasVolGasGasNorpPoderCal { get; set; }
        public double? AcumUnnaQ2MedGasVolGasGasNorpEnergia { get; set; }
        public double? AcumUnnaQ2MedGasVolGasQuemadoVolumen { get; set; }
        public double? AcumUnnaQ2MedGasVolGasQuemadoPoderCal { get; set; }
        public double? AcumUnnaQ2MedGasVolGasQuemadoEnergia { get; set; }

        //Medicion de Gas Natural Lote IV - Acumulado Quincena PERUPETRO
        public double? AcumPeruPQ1MedGasGasNatAsocMedVolumen { get; set; }
        public double? AcumPeruPQ1MedGasGasNatAsocMedPoderCal { get; set; }
        public double? AcumPeruPQ1MedGasGasNatAsocMedEnergia { get; set; }
        public double? AcumPeruPQ1MedGasGasCombSecoMedVolumen { get; set; }
        public double? AcumPeruPQ1MedGasGasCombSecoMedPoderCal { get; set; }
        public double? AcumPeruPQ1MedGasGasCombSecoMedEnergia { get; set; }
        public double? AcumPeruPQ1MedGasVolGasEquivLgnVolumen { get; set; }
        public double? AcumPeruPQ1MedGasVolGasEquivLgnPoderCal { get; set; }
        public double? AcumPeruPQ1MedGasVolGasEquivLgnEnergia { get; set; }
        public double? AcumPeruPQ1MedGasVolGasClienteVolumen { get; set; }
        public double? AcumPeruPQ1MedGasVolGasClientePoderCal { get; set; }
        public double? AcumPeruPQ1MedGasVolGasClienteEnergia { get; set; }
        public double? AcumPeruPQ1MedGasVolGasSaviaVolumen { get; set; }
        public double? AcumPeruPQ1MedGasVolGasSaviaPoderCal { get; set; }
        public double? AcumPeruPQ1MedGasVolGasSaviaEnergia { get; set; }
        public double? AcumPeruPQ1MedGasVolGasLimaGasVolumen { get; set; }
        public double? AcumPeruPQ1MedGasVolGasLimaGasPoderCal { get; set; }
        public double? AcumPeruPQ1MedGasVolGasLimaGasEnergia { get; set; }
        public double? AcumPeruPQ1MedGasVolGasGasNorpVolumen { get; set; }
        public double? AcumPeruPQ1MedGasVolGasGasNorpPoderCal { get; set; }
        public double? AcumPeruPQ1MedGasVolGasGasNorpEnergia { get; set; }
        public double? AcumPeruPQ1MedGasVolGasQuemadoVolumen { get; set; }
        public double? AcumPeruPQ1MedGasVolGasQuemadoPoderCal { get; set; }
        public double? AcumPeruPQ1MedGasVolGasQuemadoEnergia { get; set; }

        public double? AcumPeruPQ2MedGasGasNatAsocMedVolumen { get; set; }
        public double? AcumPeruPQ2MedGasGasNatAsocMedPoderCal { get; set; }
        public double? AcumPeruPQ2MedGasGasNatAsocMedEnergia { get; set; }
        public double? AcumPeruPQ2MedGasGasCombSecoMedVolumen { get; set; }
        public double? AcumPeruPQ2MedGasGasCombSecoMedPoderCal { get; set; }
        public double? AcumPeruPQ2MedGasGasCombSecoMedEnergia { get; set; }
        public double? AcumPeruPQ2MedGasVolGasEquivLgnVolumen { get; set; }
        public double? AcumPeruPQ2MedGasVolGasEquivLgnPoderCal { get; set; }
        public double? AcumPeruPQ2MedGasVolGasEquivLgnEnergia { get; set; }
        public double? AcumPeruPQ2MedGasVolGasClienteVolumen { get; set; }
        public double? AcumPeruPQ2MedGasVolGasClientePoderCal { get; set; }
        public double? AcumPeruPQ2MedGasVolGasClienteEnergia { get; set; }
        public double? AcumPeruPQ2MedGasVolGasSaviaVolumen { get; set; }
        public double? AcumPeruPQ2MedGasVolGasSaviaPoderCal { get; set; }
        public double? AcumPeruPQ2MedGasVolGasSaviaEnergia { get; set; }
        public double? AcumPeruPQ2MedGasVolGasLimaGasVolumen { get; set; }
        public double? AcumPeruPQ2MedGasVolGasLimaGasPoderCal { get; set; }
        public double? AcumPeruPQ2MedGasVolGasLimaGasEnergia { get; set; }
        public double? AcumPeruPQ2MedGasVolGasGasNorpVolumen { get; set; }
        public double? AcumPeruPQ2MedGasVolGasGasNorpPoderCal { get; set; }
        public double? AcumPeruPQ2MedGasVolGasGasNorpEnergia { get; set; }
        public double? AcumPeruPQ2MedGasVolGasQuemadoVolumen { get; set; }
        public double? AcumPeruPQ2MedGasVolGasQuemadoPoderCal { get; set; }
        public double? AcumPeruPQ2MedGasVolGasQuemadoEnergia { get; set; }

        //Medicion de Gas Natural Lote IV - Diff Unna-PeruPetro Quincena 1 y 2
        public double? DifUPQ1MedGasGasNatAsocMedVolumen { get; set; }
        public double? DifUPQ1MedGasGasNatAsocMedPoderCal { get; set; }
        public double? DifUPQ1MedGasGasNatAsocMedEnergia { get; set; }
        public double? DifUPQ1MedGasGasCombSecoMedVolumen { get; set; }
        public double? DifUPQ1MedGasGasCombSecoMedPoderCal { get; set; }
        public double? DifUPQ1MedGasGasCombSecoMedEnergia { get; set; }
        public double? DifUPQ1MedGasVolGasEquivLgnVolumen { get; set; }
        public double? DifUPQ1MedGasVolGasEquivLgnPoderCal { get; set; }
        public double? DifUPQ1MedGasVolGasEquivLgnEnergia { get; set; }
        public double? DifUPQ1MedGasVolGasClienteVolumen { get; set; }
        public double? DifUPQ1MedGasVolGasClientePoderCal { get; set; }
        public double? DifUPQ1MedGasVolGasClienteEnergia { get; set; }
        public double? DifUPQ1MedGasVolGasSaviaVolumen { get; set; }
        public double? DifUPQ1MedGasVolGasSaviaPoderCal { get; set; }
        public double? DifUPQ1MedGasVolGasSaviaEnergia { get; set; }
        public double? DifUPQ1MedGasVolGasLimaGasVolumen { get; set; }
        public double? DifUPQ1MedGasVolGasLimaGasPoderCal { get; set; }
        public double? DifUPQ1MedGasVolGasLimaGasEnergia { get; set; }
        public double? DifUPQ1MedGasVolGasGasNorpVolumen { get; set; }
        public double? DifUPQ1MedGasVolGasGasNorpPoderCal { get; set; }
        public double? DifUPQ1MedGasVolGasGasNorpEnergia { get; set; }
        public double? DifUPQ1MedGasVolGasQuemadoVolumen { get; set; }
        public double? DifUPQ1MedGasVolGasQuemadoPoderCal { get; set; }
        public double? DifUPQ1MedGasVolGasQuemadoEnergia { get; set; }

        public double? DifUPQ2MedGasGasNatAsocMedVolumen { get; set; }
        public double? DifUPQ2MedGasGasNatAsocMedPoderCal { get; set; }
        public double? DifUPQ2MedGasGasNatAsocMedEnergia { get; set; }
        public double? DifUPQ2MedGasGasCombSecoMedVolumen { get; set; }
        public double? DifUPQ2MedGasGasCombSecoMedPoderCal { get; set; }
        public double? DifUPQ2MedGasGasCombSecoMedEnergia { get; set; }
        public double? DifUPQ2MedGasVolGasEquivLgnVolumen { get; set; }
        public double? DifUPQ2MedGasVolGasEquivLgnPoderCal { get; set; }
        public double? DifUPQ2MedGasVolGasEquivLgnEnergia { get; set; }
        public double? DifUPQ2MedGasVolGasClienteVolumen { get; set; }
        public double? DifUPQ2MedGasVolGasClientePoderCal { get; set; }
        public double? DifUPQ2MedGasVolGasClienteEnergia { get; set; }
        public double? DifUPQ2MedGasVolGasSaviaVolumen { get; set; }
        public double? DifUPQ2MedGasVolGasSaviaPoderCal { get; set; }
        public double? DifUPQ2MedGasVolGasSaviaEnergia { get; set; }
        public double? DifUPQ2MedGasVolGasLimaGasVolumen { get; set; }
        public double? DifUPQ2MedGasVolGasLimaGasPoderCal { get; set; }
        public double? DifUPQ2MedGasVolGasLimaGasEnergia { get; set; }
        public double? DifUPQ2MedGasVolGasGasNorpVolumen { get; set; }
        public double? DifUPQ2MedGasVolGasGasNorpPoderCal { get; set; }
        public double? DifUPQ2MedGasVolGasGasNorpEnergia { get; set; }
        public double? DifUPQ2MedGasVolGasQuemadoVolumen { get; set; }
        public double? DifUPQ2MedGasVolGasQuemadoPoderCal { get; set; }
        public double? DifUPQ2MedGasVolGasQuemadoEnergia { get; set; }


        // GNA Fiscalizado - Acumulado Quincenal UNNA
        public double? AcumUnnaQ1GnaFiscVtaRefVolumen { get; set; }
        public double? AcumUnnaQ1GnaFiscVtaRefPoderCal { get; set; }
        public double? AcumUnnaQ1GnaFiscVtaRefEnergia { get; set; }
        public double? AcumUnnaQ1GnaFiscVtaLimaGasVolumen { get; set; }
        public double? AcumUnnaQ1GnaFiscVtaLimaGasPoderCal { get; set; }
        public double? AcumUnnaQ1GnaFiscVtaLimaGasEnergia { get; set; }
        public double? AcumUnnaQ1GnaFiscGasNorpVolumen { get; set; }
        public double? AcumUnnaQ1GnaFiscGasNorpPoderCal { get; set; }
        public double? AcumUnnaQ1GnaFiscGasNorpEnergia { get; set; }
        public double? AcumUnnaQ1GnaFiscVtaEnelVolumen { get; set; }
        public double? AcumUnnaQ1GnaFiscVtaEnelPoderCal { get; set; }
        public double? AcumUnnaQ1GnaFiscVtaEnelEnergia { get; set; }
        public double? AcumUnnaQ1GnaFiscGcyLgnVolumen { get; set; }
        public double? AcumUnnaQ1GnaFiscGcyLgnPoderCal { get; set; }
        public double? AcumUnnaQ1GnaFiscGcyLgnEnergia { get; set; }
        public double? AcumUnnaQ1GnaFiscGnafVolumen { get; set; }
        public double? AcumUnnaQ1GnaFiscGnafPoderCal { get; set; }
        public double? AcumUnnaQ1GnaFiscGnafEnergia { get; set; }

        public double? AcumUnnaQ2GnaFiscVtaRefVolumen { get; set; }
        public double? AcumUnnaQ2GnaFiscVtaRefPoderCal { get; set; }
        public double? AcumUnnaQ2GnaFiscVtaRefEnergia { get; set; }
        public double? AcumUnnaQ2GnaFiscVtaLimaGasVolumen { get; set; }
        public double? AcumUnnaQ2GnaFiscVtaLimaGasPoderCal { get; set; }
        public double? AcumUnnaQ2GnaFiscVtaLimaGasEnergia { get; set; }
        public double? AcumUnnaQ2GnaFiscGasNorpVolumen { get; set; }
        public double? AcumUnnaQ2GnaFiscGasNorpPoderCal { get; set; }
        public double? AcumUnnaQ2GnaFiscGasNorpEnergia { get; set; }
        public double? AcumUnnaQ2GnaFiscVtaEnelVolumen { get; set; }
        public double? AcumUnnaQ2GnaFiscVtaEnelPoderCal { get; set; }
        public double? AcumUnnaQ2GnaFiscVtaEnelEnergia { get; set; }
        public double? AcumUnnaQ2GnaFiscGcyLgnVolumen { get; set; }
        public double? AcumUnnaQ2GnaFiscGcyLgnPoderCal { get; set; }
        public double? AcumUnnaQ2GnaFiscGcyLgnEnergia { get; set; }
        public double? AcumUnnaQ2GnaFiscGnafVolumen { get; set; }
        public double? AcumUnnaQ2GnaFiscGnafPoderCal { get; set; }
        public double? AcumUnnaQ2GnaFiscGnafEnergia { get; set; }


        public double? AcumPeruPQ1GnaFiscVtaRefVolumen { get; set; }
        public double? AcumPeruPQ1GnaFiscVtaRefPoderCal { get; set; }
        public double? AcumPeruPQ1GnaFiscVtaRefEnergia { get; set; }
        public double? AcumPeruPQ1GnaFiscVtaLimaGasVolumen { get; set; }
        public double? AcumPeruPQ1GnaFiscVtaLimaGasPoderCal { get; set; }
        public double? AcumPeruPQ1GnaFiscVtaLimaGasEnergia { get; set; }
        public double? AcumPeruPQ1GnaFiscGasNorpVolumen { get; set; }
        public double? AcumPeruPQ1GnaFiscGasNorpPoderCal { get; set; }
        public double? AcumPeruPQ1GnaFiscGasNorpEnergia { get; set; }
        public double? AcumPeruPQ1GnaFiscVtaEnelVolumen { get; set; }
        public double? AcumPeruPQ1GnaFiscVtaEnelPoderCal { get; set; }
        public double? AcumPeruPQ1GnaFiscVtaEnelEnergia { get; set; }
        public double? AcumPeruPQ1GnaFiscGcyLgnVolumen { get; set; }
        public double? AcumPeruPQ1GnaFiscGcyLgnPoderCal { get; set; }
        public double? AcumPeruPQ1GnaFiscGcyLgnEnergia { get; set; }
        public double? AcumPeruPQ1GnaFiscGnafVolumen { get; set; }
        public double? AcumPeruPQ1GnaFiscGnafPoderCal { get; set; }
        public double? AcumPeruPQ1GnaFiscGnafEnergia { get; set; }

        public double? AcumPeruPQ2GnaFiscVtaRefVolumen { get; set; }
        public double? AcumPeruPQ2GnaFiscVtaRefPoderCal { get; set; }
        public double? AcumPeruPQ2GnaFiscVtaRefEnergia { get; set; }
        public double? AcumPeruPQ2GnaFiscVtaLimaGasVolumen { get; set; }
        public double? AcumPeruPQ2GnaFiscVtaLimaGasPoderCal { get; set; }
        public double? AcumPeruPQ2GnaFiscVtaLimaGasEnergia { get; set; }
        public double? AcumPeruPQ2GnaFiscGasNorpVolumen { get; set; }
        public double? AcumPeruPQ2GnaFiscGasNorpPoderCal { get; set; }
        public double? AcumPeruPQ2GnaFiscGasNorpEnergia { get; set; }
        public double? AcumPeruPQ2GnaFiscVtaEnelVolumen { get; set; }
        public double? AcumPeruPQ2GnaFiscVtaEnelPoderCal { get; set; }
        public double? AcumPeruPQ2GnaFiscVtaEnelEnergia { get; set; }
        public double? AcumPeruPQ2GnaFiscGcyLgnVolumen { get; set; }
        public double? AcumPeruPQ2GnaFiscGcyLgnPoderCal { get; set; }
        public double? AcumPeruPQ2GnaFiscGcyLgnEnergia { get; set; }
        public double? AcumPeruPQ2GnaFiscGnafVolumen { get; set; }
        public double? AcumPeruPQ2GnaFiscGnafPoderCal { get; set; }
        public double? AcumPeruPQ2GnaFiscGnafEnergia { get; set; }

        public double? AcumPeruPTotalGnaFiscVtaRefVolumen { get; set; }
        public double? AcumPeruPTotalGnaFiscVtaRefPoderCal { get; set; }
        public double? AcumPeruPTotalGnaFiscVtaRefEnergia { get; set; }
        public double? AcumPeruPTotalGnaFiscVtaLimaGasVolumen { get; set; }
        public double? AcumPeruPTotalGnaFiscVtaLimaGasPoderCal { get; set; }
        public double? AcumPeruPTotalGnaFiscVtaLimaGasEnergia { get; set; }
        public double? AcumPeruPTotalGnaFiscGasNorpVolumen { get; set; }
        public double? AcumPeruPTotalGnaFiscGasNorpPoderCal { get; set; }
        public double? AcumPeruPTotalGnaFiscGasNorpEnergia { get; set; }
        public double? AcumPeruPTotalGnaFiscVtaEnelVolumen { get; set; }
        public double? AcumPeruPTotalGnaFiscVtaEnelPoderCal { get; set; }
        public double? AcumPeruPTotalGnaFiscVtaEnelEnergia { get; set; }
        public double? AcumPeruPTotalGnaFiscGcyLgnVolumen { get; set; }
        public double? AcumPeruPTotalGnaFiscGcyLgnPoderCal { get; set; }
        public double? AcumPeruPTotalGnaFiscGcyLgnEnergia { get; set; }
        public double? AcumPeruPTotalGnaFiscGnafVolumen { get; set; }
        public double? AcumPeruPTotalGnaFiscGnafPoderCal { get; set; }
        public double? AcumPeruPTotalGnaFiscGnafEnergia { get; set; }

        public double? AcumUnnaTotalGnaFiscVtaRefVolumen { get; set; }
        public double? AcumUnnaTotalGnaFiscVtaRefPoderCal { get; set; }
        public double? AcumUnnaTotalGnaFiscVtaRefEnergia { get; set; }
        public double? AcumUnnaTotalGnaFiscVtaLimaGasVolumen { get; set; }
        public double? AcumUnnaTotalGnaFiscVtaLimaGasPoderCal { get; set; }
        public double? AcumUnnaTotalGnaFiscVtaLimaGasEnergia { get; set; }
        public double? AcumUnnaTotalGnaFiscGasNorpVolumen { get; set; }
        public double? AcumUnnaTotalGnaFiscGasNorpPoderCal { get; set; }
        public double? AcumUnnaTotalGnaFiscGasNorpEnergia { get; set; }
        public double? AcumUnnaTotalGnaFiscVtaEnelVolumen { get; set; }
        public double? AcumUnnaTotalGnaFiscVtaEnelPoderCal { get; set; }
        public double? AcumUnnaTotalGnaFiscVtaEnelEnergia { get; set; }
        public double? AcumUnnaTotalGnaFiscGcyLgnVolumen { get; set; }
        public double? AcumUnnaTotalGnaFiscGcyLgnPoderCal { get; set; }
        public double? AcumUnnaTotalGnaFiscGcyLgnEnergia { get; set; }
        public double? AcumUnnaTotalGnaFiscGnafVolumen { get; set; }
        public double? AcumUnnaTotalGnaFiscGnafPoderCal { get; set; }
        public double? AcumUnnaTotalGnaFiscGnafEnergia { get; set; }


        // GNA Fiscalizado - Diff Unna-PeruPetro Quincena 1 y 2
        public double? DifUPQ1GnaFiscVtaRefVolumen { get; set; }
        public double? DifUPQ1GnaFiscVtaRefPoderCal { get; set; }
        public double? DifUPQ1GnaFiscVtaRefEnergia { get; set; }
        public double? DifUPQ1GnaFiscVtaLimaGasVolumen { get; set; }
        public double? DifUPQ1GnaFiscVtaLimaGasPoderCal { get; set; }
        public double? DifUPQ1GnaFiscVtaLimaGasEnergia { get; set; }
        public double? DifUPQ1GnaFiscGasNorpVolumen { get; set; }
        public double? DifUPQ1GnaFiscGasNorpPoderCal { get; set; }
        public double? DifUPQ1GnaFiscGasNorpEnergia { get; set; }
        public double? DifUPQ1GnaFiscVtaEnelVolumen { get; set; }
        public double? DifUPQ1GnaFiscVtaEnelPoderCal { get; set; }
        public double? DifUPQ1GnaFiscVtaEnelEnergia { get; set; }
        public double? DifUPQ1GnaFiscGcyLgnVolumen { get; set; }
        public double? DifUPQ1GnaFiscGcyLgnPoderCal { get; set; }
        public double? DifUPQ1GnaFiscGcyLgnEnergia { get; set; }
        public double? DifUPQ1GnaFiscGnafVolumen { get; set; }
        public double? DifUPQ1GnaFiscGnafPoderCal { get; set; }
        public double? DifUPQ1GnaFiscGnafEnergia { get; set; }

        public double? DifUPQ2GnaFiscVtaRefVolumen { get; set; }
        public double? DifUPQ2GnaFiscVtaRefPoderCal { get; set; }
        public double? DifUPQ2GnaFiscVtaRefEnergia { get; set; }
        public double? DifUPQ2GnaFiscVtaLimaGasVolumen { get; set; }
        public double? DifUPQ2GnaFiscVtaLimaGasPoderCal { get; set; }
        public double? DifUPQ2GnaFiscVtaLimaGasEnergia { get; set; }
        public double? DifUPQ2GnaFiscGasNorpVolumen { get; set; }
        public double? DifUPQ2GnaFiscGasNorpPoderCal { get; set; }
        public double? DifUPQ2GnaFiscGasNorpEnergia { get; set; }
        public double? DifUPQ2GnaFiscVtaEnelVolumen { get; set; }
        public double? DifUPQ2GnaFiscVtaEnelPoderCal { get; set; }
        public double? DifUPQ2GnaFiscVtaEnelEnergia { get; set; }
        public double? DifUPQ2GnaFiscGcyLgnVolumen { get; set; }
        public double? DifUPQ2GnaFiscGcyLgnPoderCal { get; set; }
        public double? DifUPQ2GnaFiscGcyLgnEnergia { get; set; }
        public double? DifUPQ2GnaFiscGnafVolumen { get; set; }
        public double? DifUPQ2GnaFiscGnafPoderCal { get; set; }
        public double? DifUPQ2GnaFiscGnafEnergia { get; set; }


        public double? TotalQ1MedGasGasCombSecoMedEnergia { get; set; }
        public double? TotalQ2MedGasGasCombSecoMedEnergia { get; set; }

        public List<ResBalanceEnergLIVDetMedGasDto>? ResBalanceEnergLIVDetMedGas { get; set; }
        public List<ResBalanceEnergLIVDetGnaFiscDto>? ResBalanceEnergLIVDetGnaFisc { get; set; }
        public List<ResBalanceEnergLgnLIV_2DetLgnDto> ResBalanceEnergLgnLIV_2DetLgnDto { get; set; }

    }
}
