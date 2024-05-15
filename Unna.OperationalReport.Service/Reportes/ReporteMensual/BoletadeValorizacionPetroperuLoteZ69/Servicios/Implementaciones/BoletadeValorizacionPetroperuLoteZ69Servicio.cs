using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using System.Globalization;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuLoteZ69Servicio : IBoletadeValorizacionPetroperuLoteZ69Servicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        DateTime diaOperativo = DateTime.ParseExact("31/12/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
        double vTotalGasNaturalLoteZ69GNAMPCSD = 0;
        //double vTotalPCBTUPC = 0;
        double vTotalGasNaturalLoteZ69EnergiaMMBTU = 0;
        public BoletadeValorizacionPetroperuLoteZ69Servicio
        (
            IRegistroRepositorio registroRepositorio
        )
        {
            _registroRepositorio = registroRepositorio;
        }
        public async Task<OperacionDto<BoletadeValorizacionPetroperuLoteZ69Dto>> ObtenerAsync(long idUsuario)
        {
            var registrosVol = await _registroRepositorio.ObtenerValorMensualAsync(1, 1, diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorMensualAsync(2, 1, diaOperativo);
            var registroRiq = await _registroRepositorio.ObtenerValorMensualAsync(3, 1, diaOperativo);

            for (int i = 0; i < registrosVol.Count; i++)
            {
                vTotalGasNaturalLoteZ69GNAMPCSD = vTotalGasNaturalLoteZ69GNAMPCSD + (double)registrosVol[i].Valor;
                //vTotalPCBTUPC = vTotalPCBTUPC + (double)registrosPC[i].Valor;
                vTotalGasNaturalLoteZ69EnergiaMMBTU = vTotalGasNaturalLoteZ69EnergiaMMBTU + ((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000);
            }
            var dto = new BoletadeValorizacionPetroperuLoteZ69Dto
            {
                Fecha = diaOperativo.ToString("MMM - yyyy"),//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalGasNaturalLoteZ69GNAMPCSD = vTotalGasNaturalLoteZ69GNAMPCSD,
                TotalGasNaturalLoteZ69EnergiaMMBTU = vTotalGasNaturalLoteZ69EnergiaMMBTU,
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
            var registrosVol = await _registroRepositorio.ObtenerValorMensualAsync(1, 1, diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorMensualAsync(2, 1, diaOperativo);
            var registroRiq = await _registroRepositorio.ObtenerValorMensualAsync(3, 1, diaOperativo);
            for (int i = 0; i < registrosVol.Count; i++)
            {
                BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
                {
                    Dia = registrosVol[i].DiaOpetarivo.Fecha.Day,
                    GasNaturalLoteZ69GNAMPCSD = registrosVol[i].Valor,
                    GasNaturalLoteZ69PCBTUPCSD = registrosPC[i].Valor,
                    GasNaturalLoteZ69EnergiaMMBTU = Math.Round((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000, 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                    GasNaturalLoteZ69RiquezaGALMPC = Math.Round((double)registroRiq[i].Valor, 4, MidpointRounding.AwayFromZero),
                    GasNaturalLoteZ69RiquezaBLMMPC = Math.Round((((double)registroRiq[i].Valor * 1000) / 42), 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
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
            }
            return BoletadeValorizacionPetroperuLoteZ69Det;
        }
    }
}
