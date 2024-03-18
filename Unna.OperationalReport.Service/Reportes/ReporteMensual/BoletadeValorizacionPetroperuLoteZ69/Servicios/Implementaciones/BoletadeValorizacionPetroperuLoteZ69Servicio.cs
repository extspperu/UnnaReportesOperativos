using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuLoteZ69Servicio : IBoletadeValorizacionPetroperuLoteZ69Servicio
    {

        public async Task<OperacionDto<BoletadeValorizacionPetroperuLoteZ69Dto>> ObtenerAsync(long idUsuario)
        {
            var dto = new BoletadeValorizacionPetroperuLoteZ69Dto
            {
                Fecha = "Diciembre-2023",//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalGasNaturalLoteZ69GNAMPCSD = 160602.70,
                TotalGasNaturalLoteZ69EnergiaMMBTU = 183554.66,
                TotalGasNaturalLoteZ69LGNRecupBBL = 4431.05,

                TotalGasNaturalEficienciaPGT = 88.53,

                TotalGasSecoMS9215GNSLoteZ69MCSD = 154012.7304,

                TotalGasSecoMS9215EnergiaMMBTU = 162596.7020,

                TotalValorLiquidosUS = 218614.21,
                TotalCostoUnitMaquilaUSMMBTU = 1.1500,
                TotalCostoMaquilaUS = 211087.86,

                TotalDensidadGLPPromMesAnt = 0.5318,
                TotalMontoFacturarporUnnaE = 211087.86,
                TotalMontoFacturarporPetroperu = 218614.21,

                Observacion1 = 0,
                Observacion2 = 0,
                Observacion3 = 0

            };
            dto.BoletadeValorizacionPetroperuLoteZ69Det = await BoletadeValorizacionPetroperuLoteZ69Det();

            return new OperacionDto<BoletadeValorizacionPetroperuLoteZ69Dto>(dto);
        }

        private async Task<List<BoletadeValorizacionPetroperuLoteZ69DetDto>> BoletadeValorizacionPetroperuLoteZ69Det()
        {
            List<BoletadeValorizacionPetroperuLoteZ69DetDto> BoletadeValorizacionPetroperuLoteZ69Det = new List<BoletadeValorizacionPetroperuLoteZ69DetDto>();
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 1,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 2,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 3,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 4,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 5,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 6,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 7,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 8,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 9,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 10,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 11,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 12,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });
            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 13,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 14,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 15,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 16,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 17,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 18,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 19,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 20,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 21,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 22,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 23,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 24,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 25,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 26,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 27,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 28,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 29,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 30,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
            {
                Dia = 31,
                GasNaturalLoteZ69GNAMPCSD = 6763.1490,
                GasNaturalLoteZ69PCBTUPCSD = 1137.70,
                GasNaturalLoteZ69EnergiaMMBTU = 7694.43,//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                GasNaturalLoteZ69RiquezaGALMPC = 1.2375,
                GasNaturalLoteZ69RiquezaBLMMPC = 29.46,//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                GasNaturalLoteZ69LGNRecupBBL = 162.32,


                GasNaturalEficienciaPGT_Porcentaje = 81.46,


                GasSecoMS9215GNSLoteZ69MCSD = 6486.6,

                GasSecoMS9215PCBTUPCSD = 1056.66,
                GasSecoMS9215EnergiaMMBTU = 6854.2,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                PrecioGLPESinIGVSolesKG = 1.6900, // 1.9942 / 1.18
                PrecioGLPGSinIGVSolesKG = 2.3100,// 2.7258/1.18
                PrecioRefGLPSinIGVSolesKG = 2.0000, // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
                PrecioGLPSinIGVUSBL = 45.24,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                TipodeCambioSoles_US = 3.737,
                PrecioCGNUSBL = 56.55,//PrecioGLPSinIGVUSBL*1.25
                ValorLiquidosUS = 7802.33,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                CostoUnitMaquilaUSMMBTU = 1.1500,
                CostoMaquilaUS = 8848.60
                //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


            });

            return BoletadeValorizacionPetroperuLoteZ69Det;
        }
    }
}
