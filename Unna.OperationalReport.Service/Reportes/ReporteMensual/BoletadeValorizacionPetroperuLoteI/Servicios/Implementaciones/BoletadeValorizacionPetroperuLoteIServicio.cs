using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuLoteIServicio : IBoletadeValorizacionPetroperuLoteIServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IFiscalizacionProductoProduccionRepositorio _fiscalizacionProductoProduccionRepositorio;
        private readonly IGnsVolumeMsYPcBrutoRepositorio _gnsVolumeMsYPcBrutoRepositorio;
        private readonly IPreciosGLPRepositorio _preciosGLPRepositorio;
        DateTime diaOperativo = DateTime.ParseExact("30/04/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
        double vTotalGasNaturalLoteIGNAMPCSD = 0;
        //double vTotalPCBTUPC = 0;
        double vTotalGasNaturalLoteIEnergiaMMBTU = 0;
        public BoletadeValorizacionPetroperuLoteIServicio
        (
            IRegistroRepositorio registroRepositorio,
            IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio,
            IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio,
            IPreciosGLPRepositorio preciosGLPRepositorio
        )
        {
            _registroRepositorio = registroRepositorio;
            _fiscalizacionProductoProduccionRepositorio = fiscalizacionProductoProduccionRepositorio;
            _gnsVolumeMsYPcBrutoRepositorio = gnsVolumeMsYPcBrutoRepositorio;
            _preciosGLPRepositorio = preciosGLPRepositorio;
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
            var registrosPCLoteVI = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 2, diaOperativo);
            var registrosPCLoteZ69 = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 1, diaOperativo);
            var registroRiq = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 3, diaOperativo);
            var registroRiqLoteIV = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 4, diaOperativo);
            var registroRiqLoteVI = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 2, diaOperativo);
            var registroRiqLoteZ69 = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 1, diaOperativo);
            var registroRiqLoteX = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 5, diaOperativo);
            var fiscalizacionGlpCgn = await _fiscalizacionProductoProduccionRepositorio.FiscalizacionProductosGlpCgnMensualAsync(diaOperativo);
            var gnsVolumeMsYPcBruto = await _gnsVolumeMsYPcBrutoRepositorio.ObtenerPorTipoYNombreDiaOperativoMensualAsync("VolumenMsGnsAgpsa", "GNS A REFINERÍA",diaOperativo);
            var preciosGLP = await _preciosGLPRepositorio.ObtenerPreciosGLPMensualAsync(diaOperativo);
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
                    GasNaturalLoteIRiquezaBLMMPC = Math.Round((((double)registroRiq[i].Valor * 1000) / 42), 2, MidpointRounding.AwayFromZero),//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                    GasNaturalLoteILGNRecupBBL = Math.Round((double)((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) /42) / 42)  * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero),
                   
                    GasNaturalEficienciaPGT_Porcentaje =  Math.Round((double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100), 2, MidpointRounding.AwayFromZero),
                    
                    GasSecoMS9215GNSLoteIMCSD =
                    Math.Round((double)
                    (
                        (registrosVol[i].Valor - Math.Round( ((Math.Round((double)((((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion)), 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 3, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100))) / 1000),4, MidpointRounding.AwayFromZero)) 
                        - 
                        (
                          (
                             (
                                (registrosVol[i].Valor - Math.Round(((Math.Round((double)((((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion)), 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 3, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100))) / 1000), 4, MidpointRounding.AwayFromZero)) +
                                (registrosVolLoteVI[i].Valor - Math.Round((Math.Round((double)((registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 2, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100)) / 1000), 4, MidpointRounding.AwayFromZero)) +
                                (registrosVolLoteZ69[i].Valor - Math.Round((Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 1, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100)) / 1000), 4, MidpointRounding.AwayFromZero))
                             ) - (await _registroRepositorio.ObtenerVolumenGNSManualAsync())
                          ) *
                          (
                              (registrosVol[i].Valor * registrosPC[i].Valor) /
                              (
                               (registrosVol[i].Valor * registrosPC[i].Valor) + (registrosVolLoteVI[i].Valor * registrosPCLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registrosPCLoteZ69[i].Valor)
                              )
                          )
                        )
                    ),4, MidpointRounding.AwayFromZero),
                    
                    GasSecoMS9215PCBTUPCSD = gnsVolumeMsYPcBruto[i].PcBrutoRepCroma,//await _registroRepositorio.ObtenerPCGNSManualAsync(diaOperativo),
                    //Volumen Flare
                    //(
                    // (
                    //    (registrosVol[i].Valor - Math.Round(((Math.Round((double)((((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion)), 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 3, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100))) / 1000), 4, MidpointRounding.AwayFromZero)) +
                    //    (registrosVolLoteVI[i].Valor - Math.Round((Math.Round((double)((registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 2, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100)) / 1000), 4, MidpointRounding.AwayFromZero)) +
                    //    (registrosVolLoteZ69[i].Valor - Math.Round((Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 1, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100)) / 1000), 4, MidpointRounding.AwayFromZero))
                    // ) - (await _registroRepositorio.ObtenerVolumenGNSManualAsync())
                    //) *
                    //(
                    //  (registrosVol[i].Valor * registrosPC[i].Valor) / 
                    //  (
                    //   (registrosVol[i].Valor * registrosPC[i].Valor) + (registrosVolLoteVI[i].Valor * registrosPCLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registrosPCLoteZ69[i].Valor)
                    //  )
                    //)
                    
                    
                    GasSecoMS9215EnergiaMMBTU =
                    Math.Round((double)
                    (
                        (
                            (registrosVol[i].Valor - Math.Round(((Math.Round((double)((((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion)), 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 3, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100))) / 1000), 4, MidpointRounding.AwayFromZero))
                            -
                            (
                              (
                                 (
                                    (registrosVol[i].Valor - Math.Round(((Math.Round((double)((((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion)), 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 3, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100))) / 1000), 4, MidpointRounding.AwayFromZero)) +
                                    (registrosVolLoteVI[i].Valor - Math.Round((Math.Round((double)((registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 2, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100)) / 1000), 4, MidpointRounding.AwayFromZero)) +
                                    (registrosVolLoteZ69[i].Valor - Math.Round((Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 1, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100)) / 1000), 4, MidpointRounding.AwayFromZero))
                                 ) - (await _registroRepositorio.ObtenerVolumenGNSManualAsync())
                              ) *
                              (
                                  (registrosVol[i].Valor * registrosPC[i].Valor) /
                                  (
                                   (registrosVol[i].Valor * registrosPC[i].Valor) + (registrosVolLoteVI[i].Valor * registrosPCLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registrosPCLoteZ69[i].Valor)
                                  )
                              )
                            )
                        ) * gnsVolumeMsYPcBruto[i].PcBrutoRepCroma / 1000
                    ), 4, MidpointRounding.AwayFromZero)
                    ,
                    //Lote VI
                    //registrosVolLoteVI[i].Valor - Math.Round((Math.Round((double)((registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 2, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100)) / 1000), 4, MidpointRounding.AwayFromZero),
                    

                    PrecioGLPESinIGVSolesKG = preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100)),
                   
                    PrecioGLPGSinIGVSolesKG = preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100)),
                    //Lotz69
                    //registrosVolLoteZ69[i].Valor - Math.Round((Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 1, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100)) / 1000), 4, MidpointRounding.AwayFromZero),// 2.7258/1.18
                    PrecioRefGLPSinIGVSolesKG =
                    (( preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100)) ) * 0.5)  +
                    (( preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100)) ) * 0.5)
                    , // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
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
