using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLgnLIV_2.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Implementaciones
{
    public class ResBalanceEnergLIVServicio : IResBalanceEnergLIVServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        public ResBalanceEnergLIVServicio(IRegistroRepositorio registroRepositorio)
        {
            _registroRepositorio = registroRepositorio;
        }
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
            dto.ResBalanceEnergLgnLIV_2DetLgnDto = await ResBalanceEnergLIVDetMedGasLGN();
            return new OperacionDto<ResBalanceEnergLIVDto>(dto);
        }

        private async Task<List<ResBalanceEnergLIVDetMedGasDto>> ResBalanceEnergLIVDetMedGas()
        {

            List<ResBalanceEnergLIVDetMedGasDto> ResBalanceEnergLIVDetMedGas = new List<ResBalanceEnergLIVDetMedGasDto>();

            var test = await _registroRepositorio.ObtenerMedicionesGasAsync();
            foreach (var item in test)
            {
                ResBalanceEnergLIVDetMedGas.Add(new ResBalanceEnergLIVDetMedGasDto
                {
                    Dia = item.Dia,
                    MedGasGasNatAsocMedVolumen = 4193.708,
                    MedGasGasNatAsocMedPoderCal = 1152.93,
                    MedGasGasNatAsocMedEnergia = 4835.0518,

                    MedGasGasCombSecoMedVolumen = item.MedGasGasCombSecoMedVolumen, // Valor
                    MedGasGasCombSecoMedPoderCal = item.MedGasGasCombSecoMedPoderCal, // Valor
                    MedGasGasCombSecoMedEnergia = item.MedGasGasCombSecoMedEnergia, // Calculo

                    MedGasVolGasEquivLgnVolumen = 213.097,
                    MedGasVolGasEquivLgnPoderCal = 3105.31,
                    MedGasVolGasEquivLgnEnergia = 661.7322,
                    MedGasVolGasClienteVolumen = 0,
                    MedGasVolGasClientePoderCal = 1054.94,
                    MedGasVolGasClienteEnergia = 0,
                    MedGasVolGasSaviaVolumen = 507.7,
                    MedGasVolGasSaviaPoderCal = 1055.03,
                    MedGasVolGasSaviaEnergia = 535.6387,

                    MedGasVolGasLimaGasVolumen = item.MedGasVolGasLimaGasVolumen, // Valor
                    MedGasVolGasLimaGasPoderCal = item.MedGasVolGasLimaGasPoderCal, // Valor
                    MedGasVolGasLimaGasEnergia = item.MedGasVolGasLimaGasEnergia, // Calculo

                    MedGasVolGasGasNorpVolumen = item.MedGasVolGasGasNorpVolumen, // Valor
                    MedGasVolGasGasNorpPoderCal = item.MedGasVolGasGasNorpPoderCal, // Valor
                    MedGasVolGasGasNorpEnergia = item.MedGasVolGasGasNorpEnergia, // Calculo

                    MedGasVolGasQuemadoVolumen = item.MedGasVolGasQuemadoVolumen, // Valor
                    MedGasVolGasQuemadoPoderCal = item.MedGasVolGasQuemadoPoderCal, // Valor
                    MedGasVolGasQuemadoEnergia = item.MedGasVolGasQuemadoEnergia // Calculo
                }
                );
            }
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

        private async Task<List<ResBalanceEnergLgnLIV_2DetLgnDto>> ResBalanceEnergLIVDetMedGasLGN()
        {

            List<ResBalanceEnergLgnLIV_2DetLgnDto> ResBalanceEnergLgnLIV_2DetLgn = new List<ResBalanceEnergLgnLIV_2DetLgnDto>();

            var test = await _registroRepositorio.ObtenerMedicionesGasAsync();
            foreach (var item in test)
            {
                ResBalanceEnergLgnLIV_2DetLgn.Add(new ResBalanceEnergLgnLIV_2DetLgnDto
                {
                    Dia = item.Dia,
                    MedGasGlpVolumen = 4193.708,
                    MedGasGlpPoderCal = 1152.93,
                    MedGasGlpEnergia = 4835.0518,
                    MedGasGlpDensidad = item.MedGasGasCombSecoMedVolumen, 

                    MedGasCgnVolumen = item.MedGasGasCombSecoMedPoderCal, 
                    MedGasCgnPoderCal = item.MedGasGasCombSecoMedEnergia, 
                    MedGasCgnEnergia = item.MedGasGasCombSecoMedEnergia, 

                    MedGasLgnVolumen = item.MedGasGasCombSecoMedEnergia, 
                    MedGasLgnPoderCal = item.MedGasGasCombSecoMedEnergia, 
                    MedGasLgnEnergia = item.MedGasGasCombSecoMedEnergia, 
                }
                );
            }
            return ResBalanceEnergLgnLIV_2DetLgn;
        }
    }
}
