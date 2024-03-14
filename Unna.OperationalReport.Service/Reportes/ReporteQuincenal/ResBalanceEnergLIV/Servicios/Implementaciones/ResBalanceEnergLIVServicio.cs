using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Implementaciones
{
    public class ResBalanceEnergLIVServicio: IResBalanceEnergLIVServicio
    {

        public async Task<OperacionDto<ResBalanceEnergLIVDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new ResBalanceEnergLIVDto
            {
                Lote = "LOTE IV",
                Mes = "NOVIEMBRE",
                Anio = "2023",

                //Medicion de Gas Natural Lote IV - Acumulado Quincena UNNA
                AcumUnnaQ1MedGasGasNatAsocMedVolumen = 61623.494,
                AcumUnnaQ1MedGasGasNatAsocMedPoderCal = 1153.08,
                AcumUnnaQ1MedGasGasNatAsocMedEnergia = 71057.1427,
                AcumUnnaQ1MedGasGasCombSecoMedVolumen = 2189.3963,
                AcumUnnaQ1MedGasGasCombSecoMedPoderCal = 1069.02,
                AcumUnnaQ1MedGasGasCombSecoMedEnergia = 2331.2336,
                AcumUnnaQ1MedGasVolGasEquivLgnVolumen = 2660.3854,
                AcumUnnaQ1MedGasVolGasEquivLgnPoderCal = 3105.31,
                AcumUnnaQ1MedGasVolGasEquivLgnEnergia = 8261.3213,
                AcumUnnaQ1MedGasVolGasClienteVolumen = 0,
                AcumUnnaQ1MedGasVolGasClientePoderCal = 1068.78,
                AcumUnnaQ1MedGasVolGasClienteEnergia = 0,
                AcumUnnaQ1MedGasVolGasSaviaVolumen = 9395.44,
                AcumUnnaQ1MedGasVolGasSaviaPoderCal = 1068.66,
                AcumUnnaQ1MedGasVolGasSaviaEnergia = 10132.5701,
                AcumUnnaQ1MedGasVolGasLimaGasVolumen = 6397.34,
                AcumUnnaQ1MedGasVolGasLimaGasPoderCal = 1068.96,
                AcumUnnaQ1MedGasVolGasLimaGasEnergia = 6853.334,
                AcumUnnaQ1MedGasVolGasGasNorpVolumen = 38945.68,
                AcumUnnaQ1MedGasVolGasGasNorpPoderCal = 1068.46,
                AcumUnnaQ1MedGasVolGasGasNorpEnergia = 41447.9428,
                AcumUnnaQ1MedGasVolGasQuemadoVolumen = 2035.2523,
                AcumUnnaQ1MedGasVolGasQuemadoPoderCal = 1068.78,
                AcumUnnaQ1MedGasVolGasQuemadoEnergia = 2268.6965,

                AcumUnnaQ2MedGasGasNatAsocMedVolumen = 0,
                AcumUnnaQ2MedGasGasNatAsocMedPoderCal = 0,
                AcumUnnaQ2MedGasGasNatAsocMedEnergia = 0,
                AcumUnnaQ2MedGasGasCombSecoMedVolumen = 0,
                AcumUnnaQ2MedGasGasCombSecoMedPoderCal = 0,
                AcumUnnaQ2MedGasGasCombSecoMedEnergia = 0,
                AcumUnnaQ2MedGasVolGasEquivLgnVolumen = 0,
                AcumUnnaQ2MedGasVolGasEquivLgnPoderCal = 3208.21,
                AcumUnnaQ2MedGasVolGasEquivLgnEnergia = 0,
                AcumUnnaQ2MedGasVolGasClienteVolumen = 0,
                AcumUnnaQ2MedGasVolGasClientePoderCal = 0,
                AcumUnnaQ2MedGasVolGasClienteEnergia = 0,
                AcumUnnaQ2MedGasVolGasSaviaVolumen = 0,
                AcumUnnaQ2MedGasVolGasSaviaPoderCal = 0,
                AcumUnnaQ2MedGasVolGasSaviaEnergia = 0,
                AcumUnnaQ2MedGasVolGasLimaGasVolumen = 0,
                AcumUnnaQ2MedGasVolGasLimaGasPoderCal = 0,
                AcumUnnaQ2MedGasVolGasLimaGasEnergia = 0,
                AcumUnnaQ2MedGasVolGasGasNorpVolumen = 0,
                AcumUnnaQ2MedGasVolGasGasNorpPoderCal = 0,
                AcumUnnaQ2MedGasVolGasGasNorpEnergia = 0,
                AcumUnnaQ2MedGasVolGasQuemadoVolumen = 0,
                AcumUnnaQ2MedGasVolGasQuemadoPoderCal = 0,
                AcumUnnaQ2MedGasVolGasQuemadoEnergia = 0,

                //Medicion de Gas Natural Lote IV - Acumulado Quincena PERUPETRO
                AcumPeruPQ1MedGasGasNatAsocMedVolumen = 61623.494,
                AcumPeruPQ1MedGasGasNatAsocMedPoderCal = 1153.08,
                AcumPeruPQ1MedGasGasNatAsocMedEnergia = 71057.1427,
                AcumPeruPQ1MedGasGasCombSecoMedVolumen = 2189.3963,
                AcumPeruPQ1MedGasGasCombSecoMedPoderCal = 1069.02,
                AcumPeruPQ1MedGasGasCombSecoMedEnergia = 2331.2336,
                AcumPeruPQ1MedGasVolGasEquivLgnVolumen = 2660.3854,
                AcumPeruPQ1MedGasVolGasEquivLgnPoderCal = 3105.31,
                AcumPeruPQ1MedGasVolGasEquivLgnEnergia = 8261.3213,
                AcumPeruPQ1MedGasVolGasClienteVolumen = 0,
                AcumPeruPQ1MedGasVolGasClientePoderCal = 1068.78,
                AcumPeruPQ1MedGasVolGasClienteEnergia = 0,
                AcumPeruPQ1MedGasVolGasSaviaVolumen = 9395.44,
                AcumPeruPQ1MedGasVolGasSaviaPoderCal = 1068.66,
                AcumPeruPQ1MedGasVolGasSaviaEnergia = 10132.5701,
                AcumPeruPQ1MedGasVolGasLimaGasVolumen = 6397.34,
                AcumPeruPQ1MedGasVolGasLimaGasPoderCal = 1068.96,
                AcumPeruPQ1MedGasVolGasLimaGasEnergia = 6853.334,
                AcumPeruPQ1MedGasVolGasGasNorpVolumen = 38945.68,
                AcumPeruPQ1MedGasVolGasGasNorpPoderCal = 1068.46,
                AcumPeruPQ1MedGasVolGasGasNorpEnergia = 41447.9428,
                AcumPeruPQ1MedGasVolGasQuemadoVolumen = 2035.2523,
                AcumPeruPQ1MedGasVolGasQuemadoPoderCal = 1068.78,
                AcumPeruPQ1MedGasVolGasQuemadoEnergia = 2268.6965,

                AcumPeruPQ2MedGasGasNatAsocMedVolumen = 0,
                AcumPeruPQ2MedGasGasNatAsocMedPoderCal = 0,
                AcumPeruPQ2MedGasGasNatAsocMedEnergia = 0,
                AcumPeruPQ2MedGasGasCombSecoMedVolumen = 0,
                AcumPeruPQ2MedGasGasCombSecoMedPoderCal = 0,
                AcumPeruPQ2MedGasGasCombSecoMedEnergia = 0,
                AcumPeruPQ2MedGasVolGasEquivLgnVolumen = 0,
                AcumPeruPQ2MedGasVolGasEquivLgnPoderCal = 3208.21,
                AcumPeruPQ2MedGasVolGasEquivLgnEnergia = 0,
                AcumPeruPQ2MedGasVolGasClienteVolumen = 0,
                AcumPeruPQ2MedGasVolGasClientePoderCal = 0,
                AcumPeruPQ2MedGasVolGasClienteEnergia = 0,
                AcumPeruPQ2MedGasVolGasSaviaVolumen = 0,
                AcumPeruPQ2MedGasVolGasSaviaPoderCal = 0,
                AcumPeruPQ2MedGasVolGasSaviaEnergia = 0,
                AcumPeruPQ2MedGasVolGasLimaGasVolumen = 0,
                AcumPeruPQ2MedGasVolGasLimaGasPoderCal = 0,
                AcumPeruPQ2MedGasVolGasLimaGasEnergia = 0,
                AcumPeruPQ2MedGasVolGasGasNorpVolumen = 0,
                AcumPeruPQ2MedGasVolGasGasNorpPoderCal = 0,
                AcumPeruPQ2MedGasVolGasGasNorpEnergia = 0,
                AcumPeruPQ2MedGasVolGasQuemadoVolumen = 0,
                AcumPeruPQ2MedGasVolGasQuemadoPoderCal = 0,
                AcumPeruPQ2MedGasVolGasQuemadoEnergia = 0,


                DifUPQ1MedGasGasNatAsocMedVolumen = 0,
                DifUPQ1MedGasGasNatAsocMedPoderCal = 0,
                DifUPQ1MedGasGasNatAsocMedEnergia = 0,
                DifUPQ1MedGasGasCombSecoMedVolumen = 0,
                DifUPQ1MedGasGasCombSecoMedPoderCal = 0,
                DifUPQ1MedGasGasCombSecoMedEnergia = 0,
                DifUPQ1MedGasVolGasEquivLgnVolumen = 0,
                DifUPQ1MedGasVolGasEquivLgnPoderCal = 0,
                DifUPQ1MedGasVolGasEquivLgnEnergia = 0,
                DifUPQ1MedGasVolGasClienteVolumen = 0,
                DifUPQ1MedGasVolGasClientePoderCal = 0,
                DifUPQ1MedGasVolGasClienteEnergia = 0,
                DifUPQ1MedGasVolGasSaviaVolumen = 0,
                DifUPQ1MedGasVolGasSaviaPoderCal = 0,
                DifUPQ1MedGasVolGasSaviaEnergia = 0,
                DifUPQ1MedGasVolGasLimaGasVolumen = 0,
                DifUPQ1MedGasVolGasLimaGasPoderCal = 0,
                DifUPQ1MedGasVolGasLimaGasEnergia = 0,
                DifUPQ1MedGasVolGasGasNorpVolumen = 0,
                DifUPQ1MedGasVolGasGasNorpPoderCal = 0,
                DifUPQ1MedGasVolGasGasNorpEnergia = 0,
                DifUPQ1MedGasVolGasQuemadoVolumen = 0,
                DifUPQ1MedGasVolGasQuemadoPoderCal = 0,
                DifUPQ1MedGasVolGasQuemadoEnergia = 0,

                DifUPQ2MedGasGasNatAsocMedVolumen = 0,
                DifUPQ2MedGasGasNatAsocMedPoderCal = 0,
                DifUPQ2MedGasGasNatAsocMedEnergia = 0,
                DifUPQ2MedGasGasCombSecoMedVolumen = 0,
                DifUPQ2MedGasGasCombSecoMedPoderCal = 0,
                DifUPQ2MedGasGasCombSecoMedEnergia = 0,
                DifUPQ2MedGasVolGasEquivLgnVolumen = 0,
                DifUPQ2MedGasVolGasEquivLgnPoderCal = -3208.21,
                DifUPQ2MedGasVolGasEquivLgnEnergia = 0,
                DifUPQ2MedGasVolGasClienteVolumen = 0,
                DifUPQ2MedGasVolGasClientePoderCal = 0,
                DifUPQ2MedGasVolGasClienteEnergia = 0,
                DifUPQ2MedGasVolGasSaviaVolumen = 0,
                DifUPQ2MedGasVolGasSaviaPoderCal = 0,
                DifUPQ2MedGasVolGasSaviaEnergia = 0,
                DifUPQ2MedGasVolGasLimaGasVolumen = 0,
                DifUPQ2MedGasVolGasLimaGasPoderCal = 0,
                DifUPQ2MedGasVolGasLimaGasEnergia = 0,
                DifUPQ2MedGasVolGasGasNorpVolumen = 0,
                DifUPQ2MedGasVolGasGasNorpPoderCal = 0,
                DifUPQ2MedGasVolGasGasNorpEnergia = 0,
                DifUPQ2MedGasVolGasQuemadoVolumen = 0,
                DifUPQ2MedGasVolGasQuemadoPoderCal = 0,
                DifUPQ2MedGasVolGasQuemadoEnergia = 0,


                // GNA Fiscalizado - Acumulado Quincenal UNNA
                AcumUnnaQ1GnaFiscVtaRefVolumen = 9395.44,
                AcumUnnaQ1GnaFiscVtaRefPoderCal = 1153.4,
                AcumUnnaQ1GnaFiscVtaRefEnergia = 10836.7263,
                AcumUnnaQ1GnaFiscVtaLimaGasVolumen = 6397.34,
                AcumUnnaQ1GnaFiscVtaLimaGasPoderCal = 1153.25,
                AcumUnnaQ1GnaFiscVtaLimaGasEnergia = 7377.7498,
                AcumUnnaQ1GnaFiscGasNorpVolumen = 38945.68,
                AcumUnnaQ1GnaFiscGasNorpPoderCal = 1153.03,
                AcumUnnaQ1GnaFiscGasNorpEnergia = 44905.508,
                AcumUnnaQ1GnaFiscVtaEnelVolumen = 0,
                AcumUnnaQ1GnaFiscVtaEnelPoderCal = 0,
                AcumUnnaQ1GnaFiscVtaEnelEnergia = 0,
                AcumUnnaQ1GnaFiscGcyLgnVolumen = 4849.7817,
                AcumUnnaQ1GnaFiscGcyLgnPoderCal = 1153.08,
                AcumUnnaQ1GnaFiscGcyLgnEnergia = 5592.2066,
                AcumUnnaQ1GnaFiscGnafVolumen = 59588.2417,
                AcumUnnaQ1GnaFiscGnafPoderCal = 1153.12,
                AcumUnnaQ1GnaFiscGnafEnergia = 68712.1907,

                AcumUnnaQ2GnaFiscVtaRefVolumen = 0,
                AcumUnnaQ2GnaFiscVtaRefPoderCal = 0,
                AcumUnnaQ2GnaFiscVtaRefEnergia = 0,
                AcumUnnaQ2GnaFiscVtaLimaGasVolumen = 0,
                AcumUnnaQ2GnaFiscVtaLimaGasPoderCal = 0,
                AcumUnnaQ2GnaFiscVtaLimaGasEnergia = 0,
                AcumUnnaQ2GnaFiscGasNorpVolumen = 0,
                AcumUnnaQ2GnaFiscGasNorpPoderCal = 0,
                AcumUnnaQ2GnaFiscGasNorpEnergia = 0,
                AcumUnnaQ2GnaFiscVtaEnelVolumen = 0,
                AcumUnnaQ2GnaFiscVtaEnelPoderCal = 0,
                AcumUnnaQ2GnaFiscVtaEnelEnergia = 0,
                AcumUnnaQ2GnaFiscGcyLgnVolumen = 0,
                AcumUnnaQ2GnaFiscGcyLgnPoderCal = 0,
                AcumUnnaQ2GnaFiscGcyLgnEnergia = 0,
                AcumUnnaQ2GnaFiscGnafVolumen = 0,
                AcumUnnaQ2GnaFiscGnafPoderCal = 0,
                AcumUnnaQ2GnaFiscGnafEnergia = 0,


                AcumPeruPQ1GnaFiscVtaRefVolumen = 9395.44,
                AcumPeruPQ1GnaFiscVtaRefPoderCal = 1153.4,
                AcumPeruPQ1GnaFiscVtaRefEnergia = 10836.7263,
                AcumPeruPQ1GnaFiscVtaLimaGasVolumen = 6397.34,
                AcumPeruPQ1GnaFiscVtaLimaGasPoderCal = 1153.25,
                AcumPeruPQ1GnaFiscVtaLimaGasEnergia = 7377.7498,
                AcumPeruPQ1GnaFiscGasNorpVolumen = 38945.68,
                AcumPeruPQ1GnaFiscGasNorpPoderCal = 1153.03,
                AcumPeruPQ1GnaFiscGasNorpEnergia = 44905.508,
                AcumPeruPQ1GnaFiscVtaEnelVolumen = 0,
                AcumPeruPQ1GnaFiscVtaEnelPoderCal = 0,
                AcumPeruPQ1GnaFiscVtaEnelEnergia = 0,
                AcumPeruPQ1GnaFiscGcyLgnVolumen = 4849.7817,
                AcumPeruPQ1GnaFiscGcyLgnPoderCal = 1153.08,
                AcumPeruPQ1GnaFiscGcyLgnEnergia = 5592.2066,
                AcumPeruPQ1GnaFiscGnafVolumen = 59588.2417,
                AcumPeruPQ1GnaFiscGnafPoderCal = 1153.12,
                AcumPeruPQ1GnaFiscGnafEnergia = 68712.1907,

                AcumPeruPQ2GnaFiscVtaRefVolumen = 0,
                AcumPeruPQ2GnaFiscVtaRefPoderCal = 0,
                AcumPeruPQ2GnaFiscVtaRefEnergia = 0,
                AcumPeruPQ2GnaFiscVtaLimaGasVolumen = 0,
                AcumPeruPQ2GnaFiscVtaLimaGasPoderCal = 0,
                AcumPeruPQ2GnaFiscVtaLimaGasEnergia = 0,
                AcumPeruPQ2GnaFiscGasNorpVolumen = 0,
                AcumPeruPQ2GnaFiscGasNorpPoderCal = 0,
                AcumPeruPQ2GnaFiscGasNorpEnergia = 0,
                AcumPeruPQ2GnaFiscVtaEnelVolumen = 0,
                AcumPeruPQ2GnaFiscVtaEnelPoderCal = 0,
                AcumPeruPQ2GnaFiscVtaEnelEnergia = 0,
                AcumPeruPQ2GnaFiscGcyLgnVolumen = 0,
                AcumPeruPQ2GnaFiscGcyLgnPoderCal = 0,
                AcumPeruPQ2GnaFiscGcyLgnEnergia = 0,
                AcumPeruPQ2GnaFiscGnafVolumen = 0,
                AcumPeruPQ2GnaFiscGnafPoderCal = 0,
                AcumPeruPQ2GnaFiscGnafEnergia = 0,

                AcumPeruPTotalGnaFiscVtaRefVolumen = 0,
                AcumPeruPTotalGnaFiscVtaRefPoderCal = 0,
                AcumPeruPTotalGnaFiscVtaRefEnergia = 0,
                AcumPeruPTotalGnaFiscVtaLimaGasVolumen = 0,
                AcumPeruPTotalGnaFiscVtaLimaGasPoderCal = 0,
                AcumPeruPTotalGnaFiscVtaLimaGasEnergia = 0,
                AcumPeruPTotalGnaFiscGasNorpVolumen = 0,
                AcumPeruPTotalGnaFiscGasNorpPoderCal = 0,
                AcumPeruPTotalGnaFiscGasNorpEnergia = 0,
                AcumPeruPTotalGnaFiscVtaEnelVolumen = 0,
                AcumPeruPTotalGnaFiscVtaEnelPoderCal = 0,
                AcumPeruPTotalGnaFiscVtaEnelEnergia = 0,
                AcumPeruPTotalGnaFiscGcyLgnVolumen = 0,
                AcumPeruPTotalGnaFiscGcyLgnPoderCal = 0,
                AcumPeruPTotalGnaFiscGcyLgnEnergia = 0,
                AcumPeruPTotalGnaFiscGnafVolumen = 0,
                AcumPeruPTotalGnaFiscGnafPoderCal = 0,
                AcumPeruPTotalGnaFiscGnafEnergia = 0,

                AcumUnnaTotalGnaFiscVtaRefVolumen = 9395.44,
                AcumUnnaTotalGnaFiscVtaRefPoderCal = 1222.35,
                AcumUnnaTotalGnaFiscVtaRefEnergia = 10836.7263,
                AcumUnnaTotalGnaFiscVtaLimaGasVolumen = 6397.34,
                AcumUnnaTotalGnaFiscVtaLimaGasPoderCal = 0,
                AcumUnnaTotalGnaFiscVtaLimaGasEnergia = 7377.7498,
                AcumUnnaTotalGnaFiscGasNorpVolumen = 38945.68,
                AcumUnnaTotalGnaFiscGasNorpPoderCal = 0,
                AcumUnnaTotalGnaFiscGasNorpEnergia = 0,
                AcumUnnaTotalGnaFiscVtaEnelVolumen = 0,
                AcumUnnaTotalGnaFiscVtaEnelPoderCal = 1222.35,
                AcumUnnaTotalGnaFiscVtaEnelEnergia = 0,
                AcumUnnaTotalGnaFiscGcyLgnVolumen = 4849.7817,
                AcumUnnaTotalGnaFiscGcyLgnPoderCal = 1221.59,
                AcumUnnaTotalGnaFiscGcyLgnEnergia = 5592.2066,
                AcumUnnaTotalGnaFiscGnafVolumen = 59588.2417,
                AcumUnnaTotalGnaFiscGnafPoderCal = 1222.05,
                AcumUnnaTotalGnaFiscGnafEnergia = 68712.1907,


                // GNA Fiscalizado - Diff Unna-PeruPetro Quincena 1 y 2
                DifUPQ1GnaFiscVtaRefVolumen = 0,
                DifUPQ1GnaFiscVtaRefPoderCal = 0,
                DifUPQ1GnaFiscVtaRefEnergia = 0,
                DifUPQ1GnaFiscVtaLimaGasVolumen = 0,
                DifUPQ1GnaFiscVtaLimaGasPoderCal = 0,
                DifUPQ1GnaFiscVtaLimaGasEnergia = 0,
                DifUPQ1GnaFiscGasNorpVolumen = 0,
                DifUPQ1GnaFiscGasNorpPoderCal = 0,
                DifUPQ1GnaFiscGasNorpEnergia = 0,
                DifUPQ1GnaFiscVtaEnelVolumen = 0,
                DifUPQ1GnaFiscVtaEnelPoderCal = 0,
                DifUPQ1GnaFiscVtaEnelEnergia = 0,
                DifUPQ1GnaFiscGcyLgnVolumen = 0,
                DifUPQ1GnaFiscGcyLgnPoderCal = 0,
                DifUPQ1GnaFiscGcyLgnEnergia = 0,
                DifUPQ1GnaFiscGnafVolumen = 0,
                DifUPQ1GnaFiscGnafPoderCal = 0,
                DifUPQ1GnaFiscGnafEnergia = 0,

                DifUPQ2GnaFiscVtaRefVolumen = 0,
                DifUPQ2GnaFiscVtaRefPoderCal = 0,
                DifUPQ2GnaFiscVtaRefEnergia = 0,
                DifUPQ2GnaFiscVtaLimaGasVolumen = 0,
                DifUPQ2GnaFiscVtaLimaGasPoderCal = 0,
                DifUPQ2GnaFiscVtaLimaGasEnergia = 0,
                DifUPQ2GnaFiscGasNorpVolumen = 0,
                DifUPQ2GnaFiscGasNorpPoderCal = 0,
                DifUPQ2GnaFiscGasNorpEnergia = 0,
                DifUPQ2GnaFiscVtaEnelVolumen = 0,
                DifUPQ2GnaFiscVtaEnelPoderCal = 0,
                DifUPQ2GnaFiscVtaEnelEnergia = 0,
                DifUPQ2GnaFiscGcyLgnVolumen = 0,
                DifUPQ2GnaFiscGcyLgnPoderCal = 0,
                DifUPQ2GnaFiscGcyLgnEnergia = 0,
                DifUPQ2GnaFiscGnafVolumen = 0,
                DifUPQ2GnaFiscGnafPoderCal = 0,
                DifUPQ2GnaFiscGnafEnergia = 0,

                TotalQ1MedGasGasCombSecoMedEnergia = 2331.2336,
                TotalQ2MedGasGasCombSecoMedEnergia = 0

            };

            dto.ResBalanceEnergLIVDetMedGas = await ResBalanceEnergLIVDetMedGas();
            dto.ResBalanceEnergLIVDetGnaFisc = await ResBalanceEnergLIVDetGnaFisc();

            return new OperacionDto<ResBalanceEnergLIVDto>(dto);
        }

        private async Task<List<ResBalanceEnergLIVDetMedGasDto>> ResBalanceEnergLIVDetMedGas()
        {

            List<ResBalanceEnergLIVDetMedGasDto> ResBalanceEnergLIVDetMedGas = new List<ResBalanceEnergLIVDetMedGasDto>();

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia=1,
                MedGasGasNatAsocMedVolumen = 4193.708,
                MedGasGasNatAsocMedPoderCal = 1152.93,
                MedGasGasNatAsocMedEnergia = 4835.0518,
                MedGasGasCombSecoMedVolumen = 160.95,
                MedGasGasCombSecoMedPoderCal = 1055.08,
                MedGasGasCombSecoMedEnergia = 169.8151,
                MedGasVolGasEquivLgnVolumen = 213.097,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 661.7322,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1054.94,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 507.7,
                MedGasVolGasSaviaPoderCal = 1055.03,
                MedGasVolGasSaviaEnergia = 535.6387,
                MedGasVolGasLimaGasVolumen = 0,
                MedGasVolGasLimaGasPoderCal = 1058.62,
                MedGasVolGasLimaGasEnergia = 0,
                MedGasVolGasGasNorpVolumen = 3311.15,
                MedGasVolGasGasNorpPoderCal = 1055.75,
                MedGasVolGasGasNorpEnergia = 3495.7466,
                MedGasVolGasQuemadoVolumen = 0.810999999999694,
                MedGasVolGasQuemadoPoderCal = 1054.94,
                MedGasVolGasQuemadoEnergia = 0.8556
            }
            );

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 2,
                MedGasGasNatAsocMedVolumen = 4155.188,
                MedGasGasNatAsocMedPoderCal = 1151.49,
                MedGasGasNatAsocMedEnergia = 4784.6574,
                MedGasGasCombSecoMedVolumen = 159.6462,
                MedGasGasCombSecoMedPoderCal = 1055.15,
                MedGasGasCombSecoMedEnergia = 168.4507,
                MedGasVolGasEquivLgnVolumen = 205.5726,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 638.3667,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1055.15,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 290.46,
                MedGasVolGasSaviaPoderCal = 1055.15,
                MedGasVolGasSaviaEnergia = 306.4789,
                MedGasVolGasLimaGasVolumen = 483.77,
                MedGasVolGasLimaGasPoderCal = 1056.32,
                MedGasVolGasLimaGasEnergia = 511.0159,
                MedGasVolGasGasNorpVolumen = 3002.77,
                MedGasVolGasGasNorpPoderCal = 1055.15,
                MedGasVolGasGasNorpEnergia = 3168.3728,
                MedGasVolGasQuemadoVolumen = 12.9692,
                MedGasVolGasQuemadoPoderCal = 1055.15,
                MedGasVolGasQuemadoEnergia = 13.6845
            }
            );

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 3,
                MedGasGasNatAsocMedVolumen = 4048.387,
                MedGasGasNatAsocMedPoderCal = 1154.19,
                MedGasGasNatAsocMedEnergia = 4672.6078,
                MedGasGasCombSecoMedVolumen = 156.6039,
                MedGasGasCombSecoMedPoderCal = 1055.97,
                MedGasGasCombSecoMedEnergia = 165.369,
                MedGasVolGasEquivLgnVolumen = 204.9228,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 636.3488,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1055.97,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 212.15,
                MedGasVolGasSaviaPoderCal = 1055.97,
                MedGasVolGasSaviaEnergia = 224.024,
                MedGasVolGasLimaGasVolumen = 459.49,
                MedGasVolGasLimaGasPoderCal = 1055.4,
                MedGasVolGasLimaGasEnergia = 484.9457,
                MedGasVolGasGasNorpVolumen = 2994.11,
                MedGasVolGasGasNorpPoderCal = 1055.97,
                MedGasVolGasGasNorpEnergia = 3161.6903,
                MedGasVolGasQuemadoVolumen = 21.1102999999998,
                MedGasVolGasQuemadoPoderCal = 1055.97,
                MedGasVolGasQuemadoEnergia = 22.2918
            }
            );

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 4,
                MedGasGasNatAsocMedVolumen = 4084.943,
                MedGasGasNatAsocMedPoderCal = 1152.83,
                MedGasGasNatAsocMedEnergia = 4709.2448,
                MedGasGasCombSecoMedVolumen = 154.318,
                MedGasGasCombSecoMedPoderCal = 1056.62,
                MedGasGasCombSecoMedEnergia = 163.0555,
                MedGasVolGasEquivLgnVolumen = 204.0029,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 633.4922,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1056.58,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 268.12,
                MedGasVolGasSaviaPoderCal = 1056.58,
                MedGasVolGasSaviaEnergia = 283.2902,
                MedGasVolGasLimaGasVolumen = 500.74,
                MedGasVolGasLimaGasPoderCal = 1057.85,
                MedGasVolGasLimaGasEnergia = 529.7078,
                MedGasVolGasGasNorpVolumen = 2920.57,
                MedGasVolGasGasNorpPoderCal = 1055.87,
                MedGasVolGasGasNorpEnergia = 3083.7422,
                MedGasVolGasQuemadoVolumen = 37.1920999999998,
                MedGasVolGasQuemadoPoderCal = 1056.58,
                MedGasVolGasQuemadoEnergia = 39.2964
            }
            );

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 5,
                MedGasGasNatAsocMedVolumen = 4005.724,
                MedGasGasNatAsocMedPoderCal = 1153.72,
                MedGasGasNatAsocMedEnergia = 4621.4839,
                MedGasGasCombSecoMedVolumen = 151.372,
                MedGasGasCombSecoMedPoderCal = 1056.86,
                MedGasGasCombSecoMedEnergia = 159.979,
                MedGasVolGasEquivLgnVolumen = 200.3239,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 622.0678,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1056.83,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 192.04,
                MedGasVolGasSaviaPoderCal = 1056.82,
                MedGasVolGasSaviaEnergia = 202.9517,
                MedGasVolGasLimaGasVolumen = 432.51,
                MedGasVolGasLimaGasPoderCal = 1056.72,
                MedGasVolGasLimaGasEnergia = 457.042,
                MedGasVolGasGasNorpVolumen = 3000.7,
                MedGasVolGasGasNorpPoderCal = 1056.26,
                MedGasVolGasGasNorpEnergia = 3169.5194,
                MedGasVolGasQuemadoVolumen = 28.7781,
                MedGasVolGasQuemadoPoderCal = 1056.83,
                MedGasVolGasQuemadoEnergia = 30.4136
            }
            );
            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 6,
                MedGasGasNatAsocMedVolumen = 4100.4,
                MedGasGasNatAsocMedPoderCal = 1152.81,
                MedGasGasNatAsocMedEnergia = 4726.9821,
                MedGasGasCombSecoMedVolumen = 155.0617,
                MedGasGasCombSecoMedPoderCal = 1056.72,
                MedGasGasCombSecoMedEnergia = 163.8568,
                MedGasVolGasEquivLgnVolumen = 206.5541,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 641.4145,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1056.65,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 580.55,
                MedGasVolGasSaviaPoderCal = 1056.62,
                MedGasVolGasSaviaEnergia = 613.4207,
                MedGasVolGasLimaGasVolumen = 0,
                MedGasVolGasLimaGasPoderCal = 1055.71,
                MedGasVolGasLimaGasEnergia = 0,
                MedGasVolGasGasNorpVolumen = 3008.36,
                MedGasVolGasGasNorpPoderCal = 1056.09,
                MedGasVolGasGasNorpEnergia = 3177.0989,
                MedGasVolGasQuemadoVolumen = 149.8742,
                MedGasVolGasQuemadoPoderCal = 1056.65,
                MedGasVolGasQuemadoEnergia = 158.3646
            }
            );

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 7,
                MedGasGasNatAsocMedVolumen = 4082.561,
                MedGasGasNatAsocMedPoderCal = 1152.5,
                MedGasGasNatAsocMedEnergia = 4705.1516,
                MedGasGasCombSecoMedVolumen = 150.6639,
                MedGasGasCombSecoMedPoderCal = 1057.2,
                MedGasGasCombSecoMedEnergia = 159.2819,
                MedGasVolGasEquivLgnVolumen = 201.6367,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 626.1445,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1057.15,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 672.12,
                MedGasVolGasSaviaPoderCal = 1057.13,
                MedGasVolGasSaviaEnergia = 710.5182,
                MedGasVolGasLimaGasVolumen = 0,
                MedGasVolGasLimaGasPoderCal = 1056.21,
                MedGasVolGasLimaGasEnergia = 0,
                MedGasVolGasGasNorpVolumen = 2997.07,
                MedGasVolGasGasNorpPoderCal = 1056.38,
                MedGasVolGasGasNorpEnergia = 3166.0448,
                MedGasVolGasQuemadoVolumen = 61.0704000000001,
                MedGasVolGasQuemadoPoderCal = 1057.15,
                MedGasVolGasQuemadoEnergia = 64.5606
            }
            );

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 8,
                MedGasGasNatAsocMedVolumen = 4166.253,
                MedGasGasNatAsocMedPoderCal = 1152.95,
                MedGasGasNatAsocMedEnergia = 4803.4814,
                MedGasGasCombSecoMedVolumen = 154.8951,
                MedGasGasCombSecoMedPoderCal = 1056.71,
                MedGasGasCombSecoMedEnergia = 163.6792,
                MedGasVolGasEquivLgnVolumen = 208.6777,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 648.0089,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1056.69,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 610.57,
                MedGasVolGasSaviaPoderCal = 1056.71,
                MedGasVolGasSaviaEnergia = 645.1954,
                MedGasVolGasLimaGasVolumen = 166.82,
                MedGasVolGasLimaGasPoderCal = 1055.63,
                MedGasVolGasLimaGasEnergia = 176.1002,
                MedGasVolGasGasNorpVolumen = 3015.29,
                MedGasVolGasGasNorpPoderCal = 1056.07,
                MedGasVolGasGasNorpEnergia = 3184.3573,
                MedGasVolGasQuemadoVolumen = 10.000199999999,
                MedGasVolGasQuemadoPoderCal = 1056.69,
                MedGasVolGasQuemadoEnergia = 10.5671
            }
            );

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 9,
                MedGasGasNatAsocMedVolumen = 4134.603,
                MedGasGasNatAsocMedPoderCal = 1151.55,
                MedGasGasNatAsocMedEnergia = 4761.2021,
                MedGasGasCombSecoMedVolumen = 154.4393,
                MedGasGasCombSecoMedPoderCal = 1057.06,
                MedGasGasCombSecoMedEnergia = 163.2516,
                MedGasVolGasEquivLgnVolumen = 204.8629,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 636.1628,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1057,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 466.43,
                MedGasVolGasSaviaPoderCal = 1057.02,
                MedGasVolGasSaviaEnergia = 493.0258,
                MedGasVolGasLimaGasVolumen = 214.78,
                MedGasVolGasLimaGasPoderCal = 1056.4,
                MedGasVolGasLimaGasEnergia = 226.8936,
                MedGasVolGasGasNorpVolumen = 3063.71,
                MedGasVolGasGasNorpPoderCal = 1056.39,
                MedGasVolGasGasNorpEnergia = 3236.4726,
                MedGasVolGasQuemadoVolumen = 30.3807999999999,
                MedGasVolGasQuemadoPoderCal = 1057,
                MedGasVolGasQuemadoEnergia = 32.1125
            }
            );

            ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
            {
                Dia = 10,
                MedGasGasNatAsocMedVolumen = 4023.96,
                MedGasGasNatAsocMedPoderCal = 1152,
                MedGasGasNatAsocMedEnergia = 4635.6019,
                MedGasGasCombSecoMedVolumen = 156.7593,
                MedGasGasCombSecoMedPoderCal = 1056.69,
                MedGasGasCombSecoMedEnergia = 165.646,
                MedGasVolGasEquivLgnVolumen = 199.1084,
                MedGasVolGasEquivLgnPoderCal = 3105.31,
                MedGasVolGasEquivLgnEnergia = 618.2933,
                MedGasVolGasClienteVolumen = 0,
                MedGasVolGasClientePoderCal = 1056.59,
                MedGasVolGasClienteEnergia = 0,
                MedGasVolGasSaviaVolumen = 117.81,
                MedGasVolGasSaviaPoderCal = 1056.56,
                MedGasVolGasSaviaEnergia = 124.4733,
                MedGasVolGasLimaGasVolumen = 1200,
                MedGasVolGasLimaGasPoderCal = 1057.12,
                MedGasVolGasLimaGasEnergia = 1268.544,
                MedGasVolGasGasNorpVolumen = 2350.09,
                MedGasVolGasGasNorpPoderCal = 1055.92,
                MedGasVolGasGasNorpEnergia = 2481.507,
                MedGasVolGasQuemadoVolumen = 0.192299999999705,
                MedGasVolGasQuemadoPoderCal = 1056.59,
                MedGasVolGasQuemadoEnergia = 0.2032
            }
            );


            return ResBalanceEnergLIVDetMedGas;
        }

        private async Task<List<ResBalanceEnergLIVDetGnaFiscDto>> ResBalanceEnergLIVDetGnaFisc()
        {

            List<ResBalanceEnergLIVDetGnaFiscDto> ResBalanceEnergLIVDetGnaFisc = new List<ResBalanceEnergLIVDetGnaFiscDto>();

            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 1,
                GnaFiscVtaRefVolumen = 507.7,
                GnaFiscVtaRefPoderCal = 1152.93,
                GnaFiscVtaRefEnergia = 585.3426,
                GnaFiscVtaLimaGasVolumen = 0,
                GnaFiscVtaLimaGasPoderCal = 1152.93,
                GnaFiscVtaLimaGasEnergia = 0,
                GnaFiscGasNorpVolumen = 3311.15,
                GnaFiscGasNorpPoderCal = 1152.93,
                GnaFiscGasNorpEnergia = 3817.5242,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152.93,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 374.047,
                GnaFiscGcyLgnPoderCal = 1152.93,
                GnaFiscGcyLgnEnergia = 431.25,
                GnaFiscGnafVolumen = 4192.897,
                GnaFiscGnafPoderCal = 1152.93,
                GnaFiscGnafEnergia = 4834.1168,
                GnaFiscTotalVolumen = 4192.897,
                GnaFiscTotalEnergia = 4834.1168
            }
            );

            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 2,
                GnaFiscVtaRefVolumen = 290.46,
                GnaFiscVtaRefPoderCal = 1151.49,
                GnaFiscVtaRefEnergia = 334.4618,
                GnaFiscVtaLimaGasVolumen = 483.77,
                GnaFiscVtaLimaGasPoderCal = 1151.49,
                GnaFiscVtaLimaGasEnergia = 557.0563,
                GnaFiscGasNorpVolumen = 3002.77,
                GnaFiscGasNorpPoderCal = 1151.49,
                GnaFiscGasNorpEnergia = 3457.6596,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1151.49,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 365.2188,
                GnaFiscGcyLgnPoderCal = 1151.49,
                GnaFiscGcyLgnEnergia = 420.5458,
                GnaFiscGnafVolumen = 4142.2188,
                GnaFiscGnafPoderCal = 1151.49,
                GnaFiscGnafEnergia = 4769.7235,
                GnaFiscTotalVolumen = 8335.1158,
                GnaFiscTotalEnergia = 9603.8403
            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 3,
                GnaFiscVtaRefVolumen = 212.15,
                GnaFiscVtaRefPoderCal = 1154.19,
                GnaFiscVtaRefEnergia = 244.8614,
                GnaFiscVtaLimaGasVolumen = 459.49,
                GnaFiscVtaLimaGasPoderCal = 1154.19,
                GnaFiscVtaLimaGasEnergia = 530.3388,
                GnaFiscGasNorpVolumen = 2994.11,
                GnaFiscGasNorpPoderCal = 1154.19,
                GnaFiscGasNorpEnergia = 3455.7718,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1154.19,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 361.5267,
                GnaFiscGcyLgnPoderCal = 1154.19,
                GnaFiscGcyLgnEnergia = 417.2705,
                GnaFiscGnafVolumen = 4027.2767,
                GnaFiscGnafPoderCal = 1154.19,
                GnaFiscGnafEnergia = 4648.2425,
                GnaFiscTotalVolumen = 12362.3925,
                GnaFiscTotalEnergia = 14252.0828

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 4,
                GnaFiscVtaRefVolumen = 268.12,
                GnaFiscVtaRefPoderCal = 1152.83,
                GnaFiscVtaRefEnergia = 309.0968,
                GnaFiscVtaLimaGasVolumen = 500.74,
                GnaFiscVtaLimaGasPoderCal = 1152.83,
                GnaFiscVtaLimaGasEnergia = 577.2681,
                GnaFiscGasNorpVolumen = 2920.57,
                GnaFiscGasNorpPoderCal = 1152.83,
                GnaFiscGasNorpEnergia = 3366.9207,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152.83,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 358.3209,
                GnaFiscGcyLgnPoderCal = 1152.83,
                GnaFiscGcyLgnEnergia = 413.0831,
                GnaFiscGnafVolumen = 4047.7509,
                GnaFiscGnafPoderCal = 1152.83,
                GnaFiscGnafEnergia = 4666.3687,
                GnaFiscTotalVolumen = 16410.1434,
                GnaFiscTotalEnergia = 18918.4515

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 5,
                GnaFiscVtaRefVolumen = 192.04,
                GnaFiscVtaRefPoderCal = 1153.72,
                GnaFiscVtaRefEnergia = 221.5604,
                GnaFiscVtaLimaGasVolumen = 432.51,
                GnaFiscVtaLimaGasPoderCal = 1153.72,
                GnaFiscVtaLimaGasEnergia = 498.9954,
                GnaFiscGasNorpVolumen = 3000.7,
                GnaFiscGasNorpPoderCal = 1153.72,
                GnaFiscGasNorpEnergia = 3461.9676,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1153.72,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 351.6959,
                GnaFiscGcyLgnPoderCal = 1153.72,
                GnaFiscGcyLgnEnergia = 405.7586,
                GnaFiscGnafVolumen = 3976.9459,
                GnaFiscGnafPoderCal = 1153.72,
                GnaFiscGnafEnergia = 4588.282,
                GnaFiscTotalVolumen = 20387.0893,
                GnaFiscTotalEnergia = 23506.7335

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 6,
                GnaFiscVtaRefVolumen = 580.55,
                GnaFiscVtaRefPoderCal = 1152.81,
                GnaFiscVtaRefEnergia = 669.2638,
                GnaFiscVtaLimaGasVolumen = 0,
                GnaFiscVtaLimaGasPoderCal = 1152.81,
                GnaFiscVtaLimaGasEnergia = 0,
                GnaFiscGasNorpVolumen = 3008.36,
                GnaFiscGasNorpPoderCal = 1152.81,
                GnaFiscGasNorpEnergia = 3468.0675,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152.81,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 361.6158,
                GnaFiscGcyLgnPoderCal = 1152.81,
                GnaFiscGcyLgnEnergia = 416.8743,
                GnaFiscGnafVolumen = 3950.5258,
                GnaFiscGnafPoderCal = 1152.81,
                GnaFiscGnafEnergia = 4554.2056,
                GnaFiscTotalVolumen = 24337.6151,
                GnaFiscTotalEnergia = 28060.9391

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 7,
                GnaFiscVtaRefVolumen = 672.12,
                GnaFiscVtaRefPoderCal = 1152.5,
                GnaFiscVtaRefEnergia = 774.6183,
                GnaFiscVtaLimaGasVolumen = 0,
                GnaFiscVtaLimaGasPoderCal = 1152.5,
                GnaFiscVtaLimaGasEnergia = 0,
                GnaFiscGasNorpVolumen = 2997.07,
                GnaFiscGasNorpPoderCal = 1152.5,
                GnaFiscGasNorpEnergia = 3454.1232,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152.5,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 352.3006,
                GnaFiscGcyLgnPoderCal = 1152.5,
                GnaFiscGcyLgnEnergia = 406.0264,
                GnaFiscGnafVolumen = 4021.4906,
                GnaFiscGnafPoderCal = 1152.5,
                GnaFiscGnafEnergia = 4634.7679,
                GnaFiscTotalVolumen = 28359.1057,
                GnaFiscTotalEnergia = 32695.707

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 8,
                GnaFiscVtaRefVolumen = 610.57,
                GnaFiscVtaRefPoderCal = 1152.95,
                GnaFiscVtaRefEnergia = 703.9567,
                GnaFiscVtaLimaGasVolumen = 166.82,
                GnaFiscVtaLimaGasPoderCal = 1152.95,
                GnaFiscVtaLimaGasEnergia = 192.3351,
                GnaFiscGasNorpVolumen = 3015.29,
                GnaFiscGasNorpPoderCal = 1152.95,
                GnaFiscGasNorpEnergia = 3476.4786,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152.95,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 363.5728,
                GnaFiscGcyLgnPoderCal = 1152.95,
                GnaFiscGcyLgnEnergia = 419.1813,
                GnaFiscGnafVolumen = 4156.2528,
                GnaFiscGnafPoderCal = 1152.95,
                GnaFiscGnafEnergia = 4791.9517,
                GnaFiscTotalVolumen = 32515.3585,
                GnaFiscTotalEnergia = 37487.6587

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 9,
                GnaFiscVtaRefVolumen = 466.43,
                GnaFiscVtaRefPoderCal = 1151.55,
                GnaFiscVtaRefEnergia = 537.1175,
                GnaFiscVtaLimaGasVolumen = 214.78,
                GnaFiscVtaLimaGasPoderCal = 1151.55,
                GnaFiscVtaLimaGasEnergia = 247.3299,
                GnaFiscGasNorpVolumen = 3063.71,
                GnaFiscGasNorpPoderCal = 1151.55,
                GnaFiscGasNorpEnergia = 3528.0153,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1151.55,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 359.3022,
                GnaFiscGcyLgnPoderCal = 1151.55,
                GnaFiscGcyLgnEnergia = 413.7544,
                GnaFiscGnafVolumen = 4104.2222,
                GnaFiscGnafPoderCal = 1151.55,
                GnaFiscGnafEnergia = 4726.2171,
                GnaFiscTotalVolumen = 36619.5807,
                GnaFiscTotalEnergia = 42213.8758

            }
            );
            ResBalanceEnergLIVDetGnaFisc.Add(new ResBalanceEnergLIVDetGnaFiscDto
            {
                Dia = 10,
                GnaFiscVtaRefVolumen = 117.81,
                GnaFiscVtaRefPoderCal = 1152,
                GnaFiscVtaRefEnergia = 135.7171,
                GnaFiscVtaLimaGasVolumen = 1200,
                GnaFiscVtaLimaGasPoderCal = 1152,
                GnaFiscVtaLimaGasEnergia = 1382.4,
                GnaFiscGasNorpVolumen = 2350.09,
                GnaFiscGasNorpPoderCal = 1152,
                GnaFiscGasNorpEnergia = 2707.3037,
                GnaFiscVtaEnelVolumen = 0,
                GnaFiscVtaEnelPoderCal = 1152,
                GnaFiscVtaEnelEnergia = 0,
                GnaFiscGcyLgnVolumen = 355.8677,
                GnaFiscGcyLgnPoderCal = 1152,
                GnaFiscGcyLgnEnergia = 409.9596,
                GnaFiscGnafVolumen = 4023.7677,
                GnaFiscGnafPoderCal = 1152,
                GnaFiscGnafEnergia = 4635.3804,
                GnaFiscTotalVolumen = 40643.3484,
                GnaFiscTotalEnergia = 46849.2562

            }
            );

            return ResBalanceEnergLIVDetGnaFisc;
        }


    }
}
