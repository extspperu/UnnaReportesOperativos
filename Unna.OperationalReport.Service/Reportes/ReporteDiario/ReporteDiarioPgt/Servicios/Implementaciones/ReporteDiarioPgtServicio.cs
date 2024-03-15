using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Servicios.Implementaciones
{
    public class ReporteDiarioPgtServicio : IReporteDiarioPgtServicio
    {
        public async Task<OperacionDto<ReporteDiarioPgtDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new ReporteDiarioPgtDto
            {
                Fecha = "NOVIEMBRE 2023",
                //1. RECEPCION DE GAS NATURAL ASOCIADO(GNA) :
                GnaTotalVolDia = 27738,
                GnaTotalPodCal = 1158.88,
                GnaTotalRiqGM = 1.68606393755858,
                GnaTotalRiqBM = 40.1443794656806,
                GnaTotalEnerDia = 32145.02,
                GnaTotalVolPromMes = 27738,
                GnaTotalGasProc = 27738,
                GnaTotalGasNoProc = 0,
                GnaTotalUtiPlanPar = 0.630409090909091,
                GnaTotalHorasPlanFs = 0,

                //2. DISTRIBUCIÓN DE GAS NATURAL SECO TOTAL(GNS):
                GnsTotalVolDia = 27738,
                GnsTotalPodCal = 0,
                GnsTotalVolPromMes = 0,
                GnsTotalEnerDia = 33001.26,

                //3. PRODUCCIÓN Y VENTA DE LÍQUIDOS DE GAS NATURAL(LGN)
                LgnEfiRecLgn = 0.898,
                LgnGlpProdDiaBls = 734.054285714285,
                LgnGlpProdMesBls = 0,
                LgnGlpVtaDiaBls = 883.214285714286,
                LgnGlpVtaMesBls = 23185.81,
                LgnCgnProdDiaBls = 265.853333333333,
                LgnCgnProdMesBls = 7671.35,
                LgnCgnVtaDiaBls = 327.333333333333,
                LgnCgnVtaMesBls = 9469.92,

                LgnGlpTanqueNro1Desp = "TK-4607",
                LgnGlpTanqueNro2Desp = "TK-4607",
                LgnGlpTanqueNro3Desp = "TK-4607",
                LgnGlpTanqueNro4Desp = "0",
                LgnGlpTanqueNro5Desp = "0",
                LgnGlpTanqueNro6Desp = "0",
                LgnGlpTanqueNro7Desp = "0",
                LgnGlpTanqueNro8Desp = "0",
                LgnGlpTanqueNroTotalGalGlp = 3,
                LgnGlpCliente1Desp = "PIURA GAS S.A.C.",
                LgnGlpCliente2Desp = "PIURA GAS S.A.C.",
                LgnGlpCliente3Desp = "LIMA GAS S.A.",
                LgnGlpCliente4Desp = "0",
                LgnGlpCliente5Desp = "0",
                LgnGlpCliente6Desp = "0",
                LgnGlpCliente7Desp = "0",
                LgnGlpCliente8Desp = "0",
                LgnGlpClienteTotalGalGlp = 0,
                LgnGlpPlacaCist1Desp = "BAA-982",
                LgnGlpPlacaCist2Desp = "C9K-983",
                LgnGlpPlacaCist3Desp = "F0G-980",
                LgnGlpPlacaCist4Desp = "0",
                LgnGlpPlacaCist5Desp = "0",
                LgnGlpPlacaCist6Desp = "0",
                LgnGlpPlacaCist7Desp = "0",
                LgnGlpPlacaCist8Desp = "0",
                LgnGlpPlacaCistTotalGalGlp = 0,
                LgnGlpVolumen1Desp = "13552",
                LgnGlpVolumen2Desp = "12074",
                LgnGlpVolumen3Desp = "11469",
                LgnGlpVolumen4Desp = "0",
                LgnGlpVolumen5Desp = "0",
                LgnGlpVolumen6Desp = "0",
                LgnGlpVolumen7Desp = "0",
                LgnGlpVolumen8Desp = "0",
                LgnGlpVolumenTotalGalGlp = 37095,

                LgnCgnTanqueNro1Desp = "TK-4601",
                LgnCgnTanqueNro2Desp = "0",
                LgnCgnTanqueNro3Desp = "0",
                LgnCgnTanqueNro4Desp = "0",
                LgnCgnTanqueNro5Desp = "0",
                LgnCgnTanqueNro6Desp = "0",
                LgnCgnTanqueNro7Desp = "0",
                LgnCgnTanqueNro8Desp = "0",
                LgnCgnTanqueNroTotalGalGlp = 1,
                LgnCgnCliente1Desp = "AERO GAS DEL NORTE S.A.C.",
                LgnCgnCliente2Desp = "0",
                LgnCgnCliente3Desp = "0",
                LgnCgnCliente4Desp = "0",
                LgnCgnCliente5Desp = "0",
                LgnCgnCliente6Desp = "0",
                LgnCgnCliente7Desp = "0",
                LgnCgnCliente8Desp = "0",
                LgnCgnClienteTotalGalGlp = 0,
                LgnCgnPlacaCist1Desp = "F8R-976",
                LgnCgnPlacaCist2Desp = "0",
                LgnCgnPlacaCist3Desp = "0",
                LgnCgnPlacaCist4Desp = "0",
                LgnCgnPlacaCist5Desp = "0",
                LgnCgnPlacaCist6Desp = "0",
                LgnCgnPlacaCist7Desp = "0",
                LgnCgnPlacaCist8Desp = "0",
                LgnCgnPlacaCistTotalGalGlp = 0,
                LgnCgnVolumen1Desp = "13748",
                LgnCgnVolumen2Desp = "0",
                LgnCgnVolumen3Desp = "0",
                LgnCgnVolumen4Desp = "0",
                LgnCgnVolumen5Desp = "0",
                LgnCgnVolumen6Desp = "0",
                LgnCgnVolumen7Desp = "0",
                LgnCgnVolumen8Desp = "0",
                LgnCgnVolumenTotalGalGlp = 13748,

                //4.VOLUMEN DE GAS Y PRODUCCIÓN DE GNA ADICIONAL DEL LOTE X(CNPC PERÚ) :
                VgpgaGnaTotalCnpcVolNomVolDia = 13000,
                VgpgaGnaTotalCnpcVolAdicVolDia = 353,

                VgpgaLgnGlpVolumen = 11.07,
                VgpgaLgnCgnVolumen = 8.13,
                VgpgaLgnTotalVolumen = 19.2,


                //5.  VOLUMEN DE GAS Y PRODUCCIÓN DE ENEL:
                VgpeEnelRecGnaVolumen = 13000,
                VgpeEnelGnaEnelVolumen = 7545,
                VgpeEnelHumGnaVolumen = 31,
                VgpeEnelGasFlareVolumen = 4335.76,
                VgpeEnelGasCombVolumen = 488,
                VgpeEnelTotalVolumen = 12399.76,

                VgpeEnelLgnGlpVolumen = 299.31,
                VgpeEnelLgnCgnVolumen = 108.4,
                VgpeEnelLgnTotalVolumen = 407.71,


                //6.  VOLUMEN DE GAS Y PRODUCCIÓN DE PETROPERU(LOTE I, VI y Z-69) :
                VgppPetroLz69GnaRec = 4999.97,
                VgppPetroLz69GnsTrans = 4799.46901353959,
                VgppPetroLviGnaRec = 3265.0482,
                VgppPetroLviGnsTrans = 3011.47687001467,
                VgppPetroLiGnaRec = 2704.515,
                VgppPetroLiGnsTrans = 2513.76411644573,
                VgppPetroTotalGnaRec = 10969.5332,
                VgppPetroTotalGnsTrans = 10324.71,

                VgppLgnGlpLz69 = 100.24,
                VgppLgnGlpLvi = 128.02,
                VgppLgnGlpLi = 97.2,
                VgppLgnCgnLz69 = 36.31,
                VgppLgnCgnLvi = 46.37,
                VgppLgnCgnLi = 35.21,
                VgppLgnTotalLz69 = 136.55,
                VgppLgnTotalLvi = 174.39,
                VgppLgnTotalLi = 132.41,

                VgppVolGasFlare = 0.00430000000051223,


                //7.  VOLUMEN DE GAS Y PRODUCCIÓN UNNA ENERGIA LOTE IV:
                VgpueUeVolGnaVol = 3415.231,
                VgpueUeVtaGnsLimaGasVol = 810.22,
                VgpueUeVtaGnsGasNorpVol = 0,
                VgpueUeVtaGnsEnelVol = 0,
                VgpueUeGasCombVol = 132.3206,
                VgpueUeVolGnsEqvLgnVol = 198.1894,
                VgpueUeFlareVol = 2274.501,

                VgpueLgnGlpVol = 101.11,
                VgpueLgnCgnVol = 36.62,
                VgpueLgnTotalVol = 137.73,

                //8. OCURRENCIAS Y COMENTARIOS
                OcurrenciaComentarios = "Comentarios"

            };

            dto.ReporteDiarioPgtDetGna = await ReporteDiarioPgtDetGna();
            dto.ReporteDiarioPgtDetGns = await ReporteDiarioPgtDetGns();

            return new OperacionDto<ReporteDiarioPgtDto>(dto);
        }


        private async Task<List<ReporteDiarioPgtDetGnaDto>> ReporteDiarioPgtDetGna()
        {

            List<ReporteDiarioPgtDetGnaDto> ReporteDiarioPgtDetGna = new List<ReporteDiarioPgtDetGnaDto>();

            ReporteDiarioPgtDetGna.Add(new ReporteDiarioPgtDetGnaDto
            {
                Suministrador = "CNPC (LOTE X)",
                VolumenDiario = 13353,
                PoderCalorifico = 1133.72,
                RiquezaGM = 1.4670,
                RiquezaBM = 34.93,
                EnergiaDiaria = 15138.56,
                VolumenPromMes = 13353
            }
            );

            ReporteDiarioPgtDetGna.Add(new ReporteDiarioPgtDetGnaDto
            {
                Suministrador = "LOTE Z-69",
                VolumenDiario = 13353,
                PoderCalorifico = 1133.72,
                RiquezaGM = 1.4670,
                RiquezaBM = 34.93,
                EnergiaDiaria = 15138.56,
                VolumenPromMes = 13353
            }
            );

            ReporteDiarioPgtDetGna.Add(new ReporteDiarioPgtDetGnaDto
            {
                Suministrador = "LOTE VI",
                VolumenDiario = 13353,
                PoderCalorifico = 1133.72,
                RiquezaGM = 1.4670,
                RiquezaBM = 34.93,
                EnergiaDiaria = 15138.56,
                VolumenPromMes = 13353
            }
            );

            ReporteDiarioPgtDetGna.Add(new ReporteDiarioPgtDetGnaDto
            {
                Suministrador = "LOTE I",
                VolumenDiario = 13353,
                PoderCalorifico = 1133.72,
                RiquezaGM = 1.4670,
                RiquezaBM = 34.93,
                EnergiaDiaria = 15138.56,
                VolumenPromMes = 13353
            }
            );

            ReporteDiarioPgtDetGna.Add(new ReporteDiarioPgtDetGnaDto
            {
                Suministrador = "UNNA ENERGÍA (LOTE IV)",
                VolumenDiario = 13353,
                PoderCalorifico = 1133.72,
                RiquezaGM = 1.4670,
                RiquezaBM = 34.93,
                EnergiaDiaria = 15138.56,
                VolumenPromMes = 13353
            }
            );

            return ReporteDiarioPgtDetGna;
        }

        private async Task<List<ReporteDiarioPgtDetGnsDto>> ReporteDiarioPgtDetGns()
        {

            List<ReporteDiarioPgtDetGnsDto> ReporteDiarioPgtDetGns = new List<ReporteDiarioPgtDetGnsDto>();

            ReporteDiarioPgtDetGns.Add(new ReporteDiarioPgtDetGnsDto
            {
                Distribucion = "GNS A REFINERIA DE PETROPERU",
                VolumenDiario = 0.3491040,
                PoderCalorifico = 1056.31,
                VolumenPromMes = 2.140200,
                EnergiaDiaria = 0.811257
            }
            );

            ReporteDiarioPgtDetGns.Add(new ReporteDiarioPgtDetGnsDto
            {
                Distribucion = "GNS A ENEL",
                VolumenDiario = 0.3491040,
                PoderCalorifico = 1056.31,
                VolumenPromMes = 2.140200,
                EnergiaDiaria = 0.811257
            }
            );

            ReporteDiarioPgtDetGns.Add(new ReporteDiarioPgtDetGnsDto
            {
                Distribucion = "GNS DE VENTA A LIMAGAS",
                VolumenDiario = 0.3491040,
                PoderCalorifico = 1056.31,
                VolumenPromMes = 2.140200,
                EnergiaDiaria = 0.811257
            }
            );

            ReporteDiarioPgtDetGns.Add(new ReporteDiarioPgtDetGnsDto
            {
                Distribucion = "GNS DE VENTA A GASNORP",
                VolumenDiario = 0.3491040,
                PoderCalorifico = 1056.31,
                VolumenPromMes = 2.140200,
                EnergiaDiaria = 0.811257
            }
            );

            ReporteDiarioPgtDetGns.Add(new ReporteDiarioPgtDetGnsDto
            {
                Distribucion = "GNS DE VENTA A ENEL DEL LOTE IV",
                VolumenDiario = 0.3491040,
                PoderCalorifico = 1056.31,
                VolumenPromMes = 2.140200,
                EnergiaDiaria = 0.811257
            }
            );

            ReporteDiarioPgtDetGns.Add(new ReporteDiarioPgtDetGnsDto
            {
                Distribucion = "GNS CONSUMO PROPIO",
                VolumenDiario = 0.3491040,
                PoderCalorifico = 1056.31,
                VolumenPromMes = 2.140200,
                EnergiaDiaria = 0.811257
            }
            );

            ReporteDiarioPgtDetGns.Add(new ReporteDiarioPgtDetGnsDto
            {
                Distribucion = "CONV LIQUIDOS",
                VolumenDiario = 0.3491040,
                PoderCalorifico = 1056.31,
                VolumenPromMes = 2.140200,
                EnergiaDiaria = 0.811257
            }
            );

            ReporteDiarioPgtDetGns.Add(new ReporteDiarioPgtDetGnsDto
            {
                Distribucion = "CONV. AGUA",
                VolumenDiario = 0.3491040,
                PoderCalorifico = 1056.31,
                VolumenPromMes = 2.140200,
                EnergiaDiaria = 0.811257
            }
            );

            ReporteDiarioPgtDetGns.Add(new ReporteDiarioPgtDetGnsDto
            {
                Distribucion = "GAS FLARE",
                VolumenDiario = 0.3491040,
                PoderCalorifico = 1056.31,
                VolumenPromMes = 2.140200,
                EnergiaDiaria = 0.811257
            }
            );

            return ReporteDiarioPgtDetGns;
        }

    }
}
