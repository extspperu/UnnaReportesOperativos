using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using System.Globalization;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Servicios.Abstracciones;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuLoteIServicio : IBoletadeValorizacionPetroperuLoteIServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IFiscalizacionProductoProduccionRepositorio _fiscalizacionProductoProduccionRepositorio;
        DateTime diaOperativo = DateTime.ParseExact("30/04/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
        double vTotalGasNaturalLoteIGNAMPCSD = 0;
        //double vTotalPCBTUPC = 0;
        double vTotalGasNaturalLoteIEnergiaMMBTU = 0;
        public BoletadeValorizacionPetroperuLoteIServicio
        (
            IRegistroRepositorio registroRepositorio,
            IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio
        )
        {
            _registroRepositorio = registroRepositorio;
            _fiscalizacionProductoProduccionRepositorio = fiscalizacionProductoProduccionRepositorio;
        }
        public async Task<OperacionDto<BoletadeValorizacionPetroperuLoteIDto>> ObtenerAsync(long idUsuario)
        {
            var registrosVol = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 3, diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 3, diaOperativo);
            var registroRiq = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 3, diaOperativo);


            for (int i = 0; i < registrosVol.Count; i++)
            {
                vTotalGasNaturalLoteIGNAMPCSD = vTotalGasNaturalLoteIGNAMPCSD + (double)registrosVol[i].Valor;
                //vTotalPCBTUPC = vTotalPCBTUPC + (double)registrosPC[i].Valor;
                vTotalGasNaturalLoteIEnergiaMMBTU = vTotalGasNaturalLoteIEnergiaMMBTU + ((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000);
            }
            var dto = new BoletadeValorizacionPetroperuLoteIDto
            {
                Fecha = diaOperativo.ToString("MMM - yyyy"),//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalGasNaturalLoteIGNAMPCSD = vTotalGasNaturalLoteIGNAMPCSD,
                TotalGasNaturalLoteIEnergiaMMBTU = vTotalGasNaturalLoteIEnergiaMMBTU,
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
            var registrosVol = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 3, diaOperativo);
            var registrosVolLoteIV = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 4, diaOperativo);
            var registrosVolLoteVI = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 2, diaOperativo);
            var registrosVolLoteZ69 = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 1, diaOperativo);
            var registrosVolLoteX = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 5, diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 3, diaOperativo);
            var registroRiq = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 3, diaOperativo);
            var registroRiqLoteIV = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 4, diaOperativo);
            var registroRiqLoteVI = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 2, diaOperativo);
            var registroRiqLoteZ69 = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 1, diaOperativo);
            var registroRiqLoteX = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 5, diaOperativo);
            var fiscalizacionGlpCgn = await _fiscalizacionProductoProduccionRepositorio.FiscalizacionProductosGlpCgnMensualAsync(diaOperativo);
            //var factor =await _registroRepositorio.ObtenerFactorAsync(diaOperativo,3,)
            List<BoletadeValorizacionPetroperuLoteIDetDto> BoletadeValorizacionPetroperuLoteIDet = new List<BoletadeValorizacionPetroperuLoteIDetDto>();
            for (int i = 0; i < registrosVol.Count; i++)
            {
                BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
                {
                    Dia = registrosVol[i].Fecha.Day,
                    GasNaturalLoteIGNAMPCSD = registrosVol[i].Valor,
                    GasNaturalLoteIPCBTUPCSD = registrosPC[i].Valor,
                    GasNaturalLoteIEnergiaMMBTU = Math.Round((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000, 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                    GasNaturalLoteIRiquezaGALMPC = Math.Round((double)registroRiq[i].Valor, 4, MidpointRounding.AwayFromZero),
                    GasNaturalLoteIRiquezaBLMMPC = Math.Round((((double)registroRiq[i].Valor * 1000) / 42), 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                    GasNaturalLoteILGNRecupBBL = ((registrosVol[i].Valor * Math.Round((double)registroRiq[i].Valor, 4, MidpointRounding.AwayFromZero))/(((registrosVol[i].Valor * Math.Round((double)registroRiq[i].Valor, 4, MidpointRounding.AwayFromZero)) +(registrosVolLoteIV[i].Valor * Math.Round((double)registroRiqLoteIV[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteVI[i].Valor * Math.Round((double)registroRiqLoteVI[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteZ69[i].Valor * Math.Round((double)registroRiqLoteZ69[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteX[i].Valor * Math.Round((double)registroRiqLoteX[i].Valor, 4, MidpointRounding.AwayFromZero))) /42) / 42)* fiscalizacionGlpCgn[i].Produccion,
                    

                    GasNaturalEficienciaPGT_Porcentaje = fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * Math.Round((double)registroRiq[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteIV[i].Valor * Math.Round((double)registroRiqLoteIV[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteVI[i].Valor * Math.Round((double)registroRiqLoteVI[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteZ69[i].Valor * Math.Round((double)registroRiqLoteZ69[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteX[i].Valor * Math.Round((double)registroRiqLoteX[i].Valor, 4, MidpointRounding.AwayFromZero))) / 42) * 100,


                    GasSecoMS9215GNSLoteIMCSD = await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 3,(double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * Math.Round((double)registroRiq[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteIV[i].Valor * Math.Round((double)registroRiqLoteIV[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteVI[i].Valor * Math.Round((double)registroRiqLoteVI[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteZ69[i].Valor * Math.Round((double)registroRiqLoteZ69[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteX[i].Valor * Math.Round((double)registroRiqLoteX[i].Valor, 4, MidpointRounding.AwayFromZero))) / 42) * 100) ),
                    //_fiscalizacionProductoProduccionRepositorio.ObtenerFactorAsync(diaOperativo,3,(double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * Math.Round((double)registroRiq[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteIV[i].Valor * Math.Round((double)registroRiqLoteIV[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteVI[i].Valor * Math.Round((double)registroRiqLoteVI[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteZ69[i].Valor * Math.Round((double)registroRiqLoteZ69[i].Valor, 4, MidpointRounding.AwayFromZero)) + (registrosVolLoteX[i].Valor * Math.Round((double)registroRiqLoteX[i].Valor, 4, MidpointRounding.AwayFromZero))) / 42) * 100)),

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
            }
            return BoletadeValorizacionPetroperuLoteIDet;
        }
    }
}