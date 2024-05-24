using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using System.Globalization;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuLoteVIServicio : IBoletadeValorizacionPetroperuLoteVIServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        DateTime diaOperativo = DateTime.ParseExact("31/12/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
        double vTotalGasNaturalLoteVIGNAMPCSD = 0;
        //double vTotalPCBTUPC = 0;
        double vTotalGasNaturalLoteVIEnergiaMMBTU = 0;
        public BoletadeValorizacionPetroperuLoteVIServicio
        (
            IRegistroRepositorio registroRepositorio
        )
        {
            _registroRepositorio = registroRepositorio;
        }
        public async Task<OperacionDto<BoletadeValorizacionPetroperuLoteVIDto>> ObtenerAsync(long idUsuario)
        {
            var registrosVol = await _registroRepositorio.ObtenerValorMensualAsync(1, 2, diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorMensualAsync(2, 2, diaOperativo);
            var registroRiq = await _registroRepositorio.ObtenerValorMensualAsync(3, 2, diaOperativo);

            for (int i = 0; i < registrosVol.Count; i++)
            {
                vTotalGasNaturalLoteVIGNAMPCSD = vTotalGasNaturalLoteVIGNAMPCSD + (double)registrosVol[i].Valor;
                //vTotalPCBTUPC = vTotalPCBTUPC + (double)registrosPC[i].Valor;
                vTotalGasNaturalLoteVIEnergiaMMBTU = vTotalGasNaturalLoteVIEnergiaMMBTU + ((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000);
            }
            var dto = new BoletadeValorizacionPetroperuLoteVIDto
            {
                Fecha = diaOperativo.ToString("MMM - yyyy"),//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalGasNaturalLoteVIGNAMPCSD = vTotalGasNaturalLoteVIGNAMPCSD,
                TotalGasNaturalLoteVIEnergiaMMBTU = vTotalGasNaturalLoteVIEnergiaMMBTU,
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
            var registrosVol = await _registroRepositorio.ObtenerValorMensualAsync(1, 2, diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorMensualAsync(2, 2, diaOperativo);
            var registroRiq = await _registroRepositorio.ObtenerValorMensualAsync(3, 2, diaOperativo);

            List<BoletadeValorizacionPetroperuLoteVIDetDto> BoletadeValorizacionPetroperuLoteVIDet = new List<BoletadeValorizacionPetroperuLoteVIDetDto>();
            for (int i = 0; i < registrosVol.Count; i++)
            {
                BoletadeValorizacionPetroperuLoteVIDet.Add(new BoletadeValorizacionPetroperuLoteVIDetDto
                {
                    //Dia = registrosVol[i].Fecha.Value.Day,
                    GasNaturalLoteVIGNAMPCSD = registrosVol[i].Valor,
                    GasNaturalLoteVIPCBTUPCSD = registrosPC[i].Valor,
                    GasNaturalLoteVIEnergiaMMBTU = Math.Round((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000, 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                    GasNaturalLoteVIRiquezaGALMPC = Math.Round((double)registroRiq[i].Valor, 4, MidpointRounding.AwayFromZero),
                    GasNaturalLoteVIRiquezaBLMMPC = Math.Round((((double)registroRiq[i].Valor * 1000) / 42), 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
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

            }
            return BoletadeValorizacionPetroperuLoteVIDet;
        }
    }
}
