using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuLoteVIServicio : IBoletadeValorizacionPetroperuLoteVIServicio
    {
        public async Task<OperacionDto<BoletadeValorizacionPetroperuLoteVIDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new BoletadeValorizacionPetroperuLoteVIDto
            {
                Fecha = "Diciembre-2023",//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalGasNaturalLoteVIGNAMPCSD = 93392.57,
                TotalGasNaturalLoteVIEnergiaMMBTU = 115369.69,
                TotalGasNaturalLoteVILGNRecupBBL = 4988.97,

                TotalGasNaturalEficienciaPGT = 88.53,

                TotalGasSecoMS9215GNSLoteVIMCSD = 86082.0400,

                TotalGasSecoMS9215EnergiaMMBTU = 90877.3629,

                TotalValorLiquidosUS = 246353.23,
                TotalCostoUnitMaquilaUSMMBTU = 1.1500,
                TotalCostoMaquilaUS = 132675.15,

                TotalDensidadGLPPromMesAnt = 0.5318,
                TotalMontoFacturarporUnnaE = 132675.15,
                TotalMontoFacturarporPetroperu = 246353.22,

                Observacion1 = 0,
                Observacion2 = 0,
                Observacion3 = 0

            };
            dto.BoletadeValorizacionPetroperuLoteVIDet = await BoletadeValorizacionPetroperuLoteVIDet();

            return new OperacionDto<BoletadeValorizacionPetroperuLoteVIDto>(dto);
        }

        private async Task<List<BoletadeValorizacionPetroperuLoteVIDetDto>> BoletadeValorizacionPetroperuLoteVIDet()
        {
            List<BoletadeValorizacionPetroperuLoteVIDetDto> BoletadeValorizacionPetroperuLoteVIDet = new List<BoletadeValorizacionPetroperuLoteVIDetDto>();
            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 1,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 2,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 3,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 4,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 5,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 6,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 7,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 8,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 9,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 10,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 11,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 12,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 13,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 14,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 15,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 16,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 17,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 18,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 19,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 20,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 21,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 22,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 23,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 24,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 25,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 26,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 27,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 28,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 29,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 30,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
            {
                Dia = 31,
                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteVIMCSD = 2761.85,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2918.3,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7138.02,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 4265.05//CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            return BoletadeValorizacionPetroperuLoteVIDet;
        }
    }
}
