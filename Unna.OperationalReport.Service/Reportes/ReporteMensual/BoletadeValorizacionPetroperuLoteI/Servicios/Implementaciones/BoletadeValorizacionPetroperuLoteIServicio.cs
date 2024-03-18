using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuLoteIServicio : IBoletadeValorizacionPetroperuLoteIServicio
    {
        public async Task<OperacionDto<BoletadeValorizacionPetroperuLoteIDto>> ObtenerAsync(long idUsuario)
        {
            var dto = new BoletadeValorizacionPetroperuLoteIDto
            {
                Fecha = "Diciembre-2023",//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalGasNaturalLoteIGNAMPCSD = 70841.72,
                TotalGasNaturalLoteIEnergiaMMBTU = 86248.3202,
                TotalGasNaturalLoteILGNRecupBBL = 3547.96,

                TotalGasNaturalEficienciaPGT = 88.53,
                
                TotalGasSecoMS9215GNSLoteIMCSD = 65685.3197,
                
                TotalGasSecoMS9215EnergiaMMBTU = 69356.9347,

                TotalValorLiquidosUS = 174526.77,
                TotalCostoUnitMaquilaUSMMBTU = 1.1500,
                TotalCostoMaquilaUS = 99185.57,

                TotalDensidadGLPPromMesAnt = 0.5318,
                TotalMontoFacturarporUnnaE = 99185.57,
                TotalMontoFacturarporPetroperu = 174526.77,

                Observacion1 = 0,
                Observacion2 = 0,
                Observacion3 = 0

            };
            dto.BoletadeValorizacionPetroperuLoteIDet = await BoletadeValorizacionPetroperuLoteIDet();

            return new OperacionDto<BoletadeValorizacionPetroperuLoteIDto>(dto);
        }

        private async Task<List<BoletadeValorizacionPetroperuLoteIDetDto>> BoletadeValorizacionPetroperuLoteIDet()
        {
            List<BoletadeValorizacionPetroperuLoteIDetDto> BoletadeValorizacionPetroperuLoteIDet = new List<BoletadeValorizacionPetroperuLoteIDetDto>();
            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 1,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,

             
                GasNaturalEficienciaPGT_Porcentaje = 81.46,
               

                GasSecoMS9215GNSLoteIMCSD = 2443.26,
                
                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 2,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 3,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 4,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 5,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 6,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 7,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 8,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 9,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 10,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 11,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 12,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 13,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 14,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 15,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 16,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 17,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 18,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 19,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 20,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 21,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 22,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 23,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 24,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 25,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 26,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 27,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 28,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 29,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 30,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
            {
                Dia = 31,
                GasNaturalLoteIGNAMPCSD = 2637.164,
                GasNaturalLoteIPCBTUPCSD = 1219.67,
                GasNaturalLoteIEnergiaMMBTU = 3216.4698,//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                GasNaturalLoteIRiquezaGALMPC = 2.4091,
                GasNaturalLoteIRiquezaBLMMPC = 57.36,//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                GasNaturalLoteILGNRecupBBL = 123.217057814702,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteIMCSD = 2443.26,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 2581.7,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 5922.74,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 3698.94

            });

            return BoletadeValorizacionPetroperuLoteIDet;
        }
    }
}
