using Autofac.Features.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos
{
    public class ReporteDiarioPgtDto
    {
        public string? Fecha { get; set; }

        //1. RECEPCION DE GAS NATURAL ASOCIADO(GNA) :
        public double? GnaTotalVolDia { get; set; }
        public double? GnaTotalPodCal { get; set; }
        public double? GnaTotalRiqGM { get; set; }
        public double? GnaTotalRiqBM { get; set; }
        public double? GnaTotalEnerDia { get; set; }
        public double? GnaTotalVolPromMes { get; set; }
        public double? GnaTotalGasProc { get; set; }
        public double? GnaTotalGasNoProc { get; set; }
        public double? GnaTotalUtiPlanPar { get; set; }
        public double? GnaTotalHorasPlanFs { get; set; }

        //2. DISTRIBUCIÓN DE GAS NATURAL SECO TOTAL(GNS):
        public double? GnsTotalVolDia { get; set; }
        public double? GnsTotalPodCal { get; set; }
        public double? GnsTotalVolPromMes { get; set; }
        public double? GnsTotalEnerDia { get; set; }

        //3. PRODUCCIÓN Y VENTA DE LÍQUIDOS DE GAS NATURAL(LGN)
        public double? LgnEfiRecLgn { get; set; }
        public double? LgnGlpProdDiaBls { get; set; }
        public double? LgnGlpProdMesBls { get; set; }
        public double? LgnGlpVtaDiaBls { get; set; }
        public double? LgnGlpVtaMesBls { get; set; }
        public double? LgnCgnProdDiaBls { get; set; }
        public double? LgnCgnProdMesBls { get; set; }
        public double? LgnCgnVtaDiaBls { get; set; }
        public double? LgnCgnVtaMesBls { get; set; }

        public string? LgnGlpTanqueNro1Desp { get; set; }
        public string? LgnGlpTanqueNro2Desp { get; set; }
        public string? LgnGlpTanqueNro3Desp { get; set; }
        public string? LgnGlpTanqueNro4Desp { get; set; }
        public string? LgnGlpTanqueNro5Desp { get; set; }
        public string? LgnGlpTanqueNro6Desp { get; set; }
        public string? LgnGlpTanqueNro7Desp { get; set; }
        public string? LgnGlpTanqueNro8Desp { get; set; }
        public double? LgnGlpTanqueNroTotalGalGlp { get; set; }
        public string? LgnGlpCliente1Desp { get; set; }
        public string? LgnGlpCliente2Desp { get; set; }
        public string? LgnGlpCliente3Desp { get; set; }
        public string? LgnGlpCliente4Desp { get; set; }
        public string? LgnGlpCliente5Desp { get; set; }
        public string? LgnGlpCliente6Desp { get; set; }
        public string? LgnGlpCliente7Desp { get; set; }
        public string? LgnGlpCliente8Desp { get; set; }
        public double? LgnGlpClienteTotalGalGlp { get; set; }
        public string? LgnGlpPlacaCist1Desp { get; set; }
        public string? LgnGlpPlacaCist2Desp { get; set; }
        public string? LgnGlpPlacaCist3Desp { get; set; }
        public string? LgnGlpPlacaCist4Desp { get; set; }
        public string? LgnGlpPlacaCist5Desp { get; set; }
        public string? LgnGlpPlacaCist6Desp { get; set; }
        public string? LgnGlpPlacaCist7Desp { get; set; }
        public string? LgnGlpPlacaCist8Desp { get; set; }
        public double? LgnGlpPlacaCistTotalGalGlp { get; set; }
        public string? LgnGlpVolumen1Desp { get; set; }
        public string? LgnGlpVolumen2Desp { get; set; }
        public string? LgnGlpVolumen3Desp { get; set; }
        public string? LgnGlpVolumen4Desp { get; set; }
        public string? LgnGlpVolumen5Desp { get; set; }
        public string? LgnGlpVolumen6Desp { get; set; }
        public string? LgnGlpVolumen7Desp { get; set; }
        public string? LgnGlpVolumen8Desp { get; set; }
        public double? LgnGlpVolumenTotalGalGlp { get; set; }

        public string? LgnCgnTanqueNro1Desp { get; set; }
        public string? LgnCgnTanqueNro2Desp { get; set; }
        public string? LgnCgnTanqueNro3Desp { get; set; }
        public string? LgnCgnTanqueNro4Desp { get; set; }
        public string? LgnCgnTanqueNro5Desp { get; set; }
        public string? LgnCgnTanqueNro6Desp { get; set; }
        public string? LgnCgnTanqueNro7Desp { get; set; }
        public string? LgnCgnTanqueNro8Desp { get; set; }
        public double? LgnCgnTanqueNroTotalGalGlp { get; set; }
        public string? LgnCgnCliente1Desp { get; set; }
        public string? LgnCgnCliente2Desp { get; set; }
        public string? LgnCgnCliente3Desp { get; set; }
        public string? LgnCgnCliente4Desp { get; set; }
        public string? LgnCgnCliente5Desp { get; set; }
        public string? LgnCgnCliente6Desp { get; set; }
        public string? LgnCgnCliente7Desp { get; set; }
        public string? LgnCgnCliente8Desp { get; set; }
        public double? LgnCgnClienteTotalGalGlp { get; set; }
        public string? LgnCgnPlacaCist1Desp { get; set; }
        public string? LgnCgnPlacaCist2Desp { get; set; }
        public string? LgnCgnPlacaCist3Desp { get; set; }
        public string? LgnCgnPlacaCist4Desp { get; set; }
        public string? LgnCgnPlacaCist5Desp { get; set; }
        public string? LgnCgnPlacaCist6Desp { get; set; }
        public string? LgnCgnPlacaCist7Desp { get; set; }
        public string? LgnCgnPlacaCist8Desp { get; set; }
        public double? LgnCgnPlacaCistTotalGalGlp { get; set; }
        public string? LgnCgnVolumen1Desp { get; set; }
        public string? LgnCgnVolumen2Desp { get; set; }
        public string? LgnCgnVolumen3Desp { get; set; }
        public string? LgnCgnVolumen4Desp { get; set; }
        public string? LgnCgnVolumen5Desp { get; set; }
        public string? LgnCgnVolumen6Desp { get; set; }
        public string? LgnCgnVolumen7Desp { get; set; }
        public string? LgnCgnVolumen8Desp { get; set; }
        public double? LgnCgnVolumenTotalGalGlp { get; set; }

        //4.VOLUMEN DE GAS Y PRODUCCIÓN DE GNA ADICIONAL DEL LOTE X(CNPC PERÚ) :
        public double? VgpgaGnaTotalCnpcVolNomVolDia { get; set; }
        public double? VgpgaGnaTotalCnpcVolAdicVolDia { get; set; }
                       
        public double? VgpgaLgnGlpVolumen { get; set; }
        public double? VgpgaLgnCgnVolumen { get; set; }
        public double? VgpgaLgnTotalVolumen { get; set; }


        //5.  VOLUMEN DE GAS Y PRODUCCIÓN DE ENEL:
        public double? VgpeEnelRecGnaVolumen { get; set; }
        public double? VgpeEnelGnaEnelVolumen { get; set; }
        public double? VgpeEnelHumGnaVolumen { get; set; }
        public double? VgpeEnelGasFlareVolumen { get; set; }
        public double? VgpeEnelGasCombVolumen { get; set; }
        public double? VgpeEnelTotalVolumen { get; set; }

        public double? VgpeEnelLgnGlpVolumen { get; set; }
        public double? VgpeEnelLgnCgnVolumen { get; set; }
        public double? VgpeEnelLgnTotalVolumen { get; set; }


        //6.  VOLUMEN DE GAS Y PRODUCCIÓN DE PETROPERU(LOTE I, VI y Z-69) :
        public double? VgppPetroLz69GnaRec { get; set; }
        public double? VgppPetroLz69GnsTrans { get; set; }
        public double? VgppPetroLviGnaRec { get; set; }
        public double? VgppPetroLviGnsTrans { get; set; }
        public double? VgppPetroLiGnaRec { get; set; }
        public double? VgppPetroLiGnsTrans { get; set; }
        public double? VgppPetroTotalGnaRec { get; set; }
        public double? VgppPetroTotalGnsTrans { get; set; }

        public double? VgppLgnGlpLz69 { get; set; }
        public double? VgppLgnGlpLvi { get; set; }
        public double? VgppLgnGlpLi { get; set; }
        public double? VgppLgnCgnLz69 { get; set; }
        public double? VgppLgnCgnLvi { get; set; }
        public double? VgppLgnCgnLi { get; set; }
        public double? VgppLgnTotalLz69 { get; set; }
        public double? VgppLgnTotalLvi { get; set; }
        public double? VgppLgnTotalLi { get; set; }

        public double? VgppVolGasFlare { get; set; }


        //7.  VOLUMEN DE GAS Y PRODUCCIÓN UNNA ENERGIA LOTE IV:
        public double? VgpueUeVolGnaVol { get; set; }
        public double? VgpueUeVtaGnsLimaGasVol { get; set; }
        public double? VgpueUeVtaGnsGasNorpVol { get; set; }
        public double? VgpueUeVtaGnsEnelVol { get; set; }
        public double? VgpueUeGasCombVol { get; set; }
        public double? VgpueUeVolGnsEqvLgnVol { get; set; }
        public double? VgpueUeFlareVol { get; set; }

        public double? VgpueLgnGlpVol { get; set; }
        public double? VgpueLgnCgnVol { get; set; }
        public double? VgpueLgnTotalVol { get; set; }

        //8. OCURRENCIAS Y COMENTARIOS
        public string? OcurrenciaComentarios { get; set; }


        public List<ReporteDiarioPgtDetGnaDto>? ReporteDiarioPgtDetGna { get; set; }

        public List<ReporteDiarioPgtDetGnsDto>? ReporteDiarioPgtDetGns { get; set; }

    }
}
