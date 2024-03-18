using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuServicio : IBoletadeValorizacionPetroperuServicio
    {

        public async Task<OperacionDto<BoletadeValorizacionPetroperuDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new BoletadeValorizacionPetroperuDto
            {
                Fecha = "Diciembre-2023",//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalGasNaturalLoteIGNAMPCSD= 70841.72,
                TotalGasNaturalLoteIEnergiaMMBTU = 86248.3202,
                TotalGasNaturalLoteILGNRecupBBL = 3547.96,

                TotalGasNaturalLoteVIGNAMPCSD = 93392.57,
                TotalGasNaturalLoteVIEnergiaMMBTU = 115369.69,
                TotalGasNaturalLoteVILGNRecupBBL = 4988.97,

                TotalGasNaturalLoteZ69GNAMPCSD = 160602.70,
                TotalGasNaturalLoteZ69EnergiaMMBTU = 183554.66,
                TotalGasNaturalLoteZ69LGNRecupBBL = 4431.05,

                TotalGasNaturalTotalGNA = 324836.99,
                TotalGasNaturalEficienciaPGT = 88.53,
                TotalGasNaturalLiquidosRecupTotales= 12967.98,

                TotalGasSecoMS9215GNSLoteIMCSD= 65685.3197,
                TotalGasSecoMS9215GNSLoteVIMCSD= 86082.0400,
                TotalGasSecoMS9215GNSLoteZ69MCSD = 154012.7304,

                TotalGasSecoMS9215GNSTotalMCSD = 305780.0900, 
                TotalGasSecoMS9215EnergiaMMBTU = 322830.9995,

                TotalValorLiquidosUS = 639494.20,
                TotalCostoUnitMaquilaUSMMBTU = 1.1500,
                TotalCostoMaquilaUS = 442948.58,

                TotalDensidadGLPPromMesAnt = 0.5318,
                TotalMontoFacturarporUnnaE = 442948.58,
                TotalMontoFacturarporPetroperu = 639494.20,

                Observacion1= 2.63,
                Observacion2 = 2.93,
                Observacion3 = 0.30

            };
            dto.BoletadeValorizacionPetroperuDet = await BoletadeValorizacionPetroperuDet();
            
            return new OperacionDto<BoletadeValorizacionPetroperuDto>(dto);
        }

        private async Task<List<BoletadeValorizacionPetroperuDetDto>> BoletadeValorizacionPetroperuDet()
        {
            List<BoletadeValorizacionPetroperuDetDto> BoletadeValorizacionPetroperuDet = new List<BoletadeValorizacionPetroperuDetDto>();
            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 1,
                GasNaturalLoteIGNAMPCSD= 2637.164,
                GasNaturalLoteIPCBTUPCSD= 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698 ,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC= 2.4091,
                GasNaturalLoteIRiquezaBLMMPC= 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU= 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL= 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 2,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 3,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 4,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 5,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 6,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 7,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 8,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 9,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 10,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 11,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 12,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 13,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 14,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 15,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 16,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 17,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 18,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 19,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 20,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 21,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 22,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 23,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 24,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 25,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 26,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 27,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 28,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 29,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 30,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)

            });

            BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
            {
                Dia = 31,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

                GasNaturalLoteVIGNAMPCSD = 2996.91,
                GasNaturalLoteVIPCBTUPCSD = 1237.52,
                GasNaturalLoteVIEnergiaMMBTU = 3708.74,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteVIRiquezaGALMPC = 2.5549,
                GasNaturalLoteVIRiquezaBLMMPC = 60.83,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteVILGNRecupBBL = 148.50,

                GasNaturalLoteZ69GNAMPCSD = 6763.15,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,

                GasNaturalTotalGNAMPCSD = 12397.22,//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
                GasNaturalLiquidosRecupTotalesBBL = 434.04, //GasNaturalLoteILGNRecupBBL+GasNaturalLoteVILGNRecupBBL+GasNaturalLoteZ69LGNRecupBBL

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                GasSecoMS9215GNSLoteVIMCSD = 2761.85,
                GasSecoMS9215GNSLoteZ69MCSD = 6486.64,
                GasSecoMS9215GNSTotalMCSD = 11691.75,//GasSecoMS9215GNSLoteIMCSD+GasSecoMS9215GNSLoteVIMCSD+GasSecoMS9215GNSLoteZ69MCSD
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 12354.2046,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 20863.09,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 16812.59 //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            return BoletadeValorizacionPetroperuDet;
        }
    }
}
