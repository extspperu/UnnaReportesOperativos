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
        private readonly ITipodeCambioRepositorio _tipodeCambioRepositorio;
        DateTime diaOperativo = DateTime.ParseExact("30/04/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
        double vTotalGasNaturalLoteIGNAMPCSD = 0;
        double vTotalGasNaturalLoteIEnergiaMMBTU = 0;
        double vTotalGasNaturalLoteILGNRecupBBL = 0;
        double vTotalGasNaturalEficienciaPGT = 0;
        double vTotalGasSecoMS9215GNSLoteIMCSD = 0;
        double vTotalGasSecoMS9215EnergiaMMBTU = 0;
        double vTotalValorLiquidosUS = 0;
        double vTotalCostoUnitMaquilaUSMMBTU = 0;
        double vTotalCostoMaquilaUS = 0;
        

        public BoletadeValorizacionPetroperuLoteIServicio
        (
            IRegistroRepositorio registroRepositorio,
            IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio,
            IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio,
            IPreciosGLPRepositorio preciosGLPRepositorio,
            ITipodeCambioRepositorio tipodeCambioRepositorio
        )
        {
            _registroRepositorio = registroRepositorio;
            _fiscalizacionProductoProduccionRepositorio = fiscalizacionProductoProduccionRepositorio;
            _gnsVolumeMsYPcBrutoRepositorio = gnsVolumeMsYPcBrutoRepositorio;
            _preciosGLPRepositorio = preciosGLPRepositorio;
            _tipodeCambioRepositorio = tipodeCambioRepositorio;
        }
        public async Task<OperacionDto<BoletadeValorizacionPetroperuLoteIDto>> ObtenerAsync(long idUsuario)
        {
            var registrosVol = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 3, diaOperativo);
            var registrosPC = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 3, diaOperativo);
            var registroRiq = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 3, diaOperativo);
            var registrosPCLoteVI = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 2, diaOperativo);
            var registrosPCLoteZ69 = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 1, diaOperativo);
            var registrosVolLoteIV = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 4, diaOperativo);
            var registrosVolLoteVI = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 2, diaOperativo);
            var registrosVolLoteZ69 = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 1, diaOperativo);
            var registrosVolLoteX = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 5, diaOperativo);
            var registroRiqLoteIV = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 4, diaOperativo);
            var registroRiqLoteVI = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 2, diaOperativo);
            var registroRiqLoteZ69 = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 1, diaOperativo);
            var registroRiqLoteX = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 5, diaOperativo);
            var fiscalizacionGlpCgn = await _fiscalizacionProductoProduccionRepositorio.FiscalizacionProductosGlpCgnMensualAsync(diaOperativo);
            var gnsVolumeMsYPcBruto = await _gnsVolumeMsYPcBrutoRepositorio.ObtenerPorTipoYNombreDiaOperativoMensualAsync("VolumenMsGnsAgpsa", "GNS A REFINERÍA", diaOperativo);
            var preciosGLP = await _preciosGLPRepositorio.ObtenerPreciosGLPMensualAsync(diaOperativo);
            var tipoCambio = await _tipodeCambioRepositorio.ObtenerTipodeCambioMensualAsync(diaOperativo, 1);

            for (int i = 0; i < registrosVol.Count; i++)
            {
                
                vTotalGasNaturalLoteIGNAMPCSD = vTotalGasNaturalLoteIGNAMPCSD + (double)registrosVol[i].Valor;
                //vTotalPCBTUPC = vTotalPCBTUPC + (double)registrosPC[i].Valor;
                vTotalGasNaturalLoteIEnergiaMMBTU = vTotalGasNaturalLoteIEnergiaMMBTU + ((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000);
                vTotalGasNaturalLoteILGNRecupBBL = vTotalGasNaturalLoteILGNRecupBBL + Math.Round((double)((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero);

                vTotalGasNaturalEficienciaPGT = vTotalGasNaturalEficienciaPGT +
                    Math.Round((double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100), 2, MidpointRounding.AwayFromZero);

                vTotalGasSecoMS9215GNSLoteIMCSD = vTotalGasSecoMS9215GNSLoteIMCSD +
                    //Math.Round((double)
                    (double)(
                        (registrosVol[i].Valor - ((((((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion)) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 3, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100))) / 1000))
                        -
                        (
                          (
                             (
                                (registrosVol[i].Valor - ((((((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion)) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 3, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100))) / 1000)) +
                                (registrosVolLoteVI[i].Valor - (((registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 2, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100)) / 1000)) +
                                (registrosVolLoteZ69[i].Valor - (((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 1, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100)) / 1000))
                             ) - (await _registroRepositorio.ObtenerVolumenGNSManualAsync())
                          ) *
                          (
                              (registrosVol[i].Valor * registrosPC[i].Valor) /
                              (
                               (registrosVol[i].Valor * registrosPC[i].Valor) + (registrosVolLoteVI[i].Valor * registrosPCLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registrosPCLoteZ69[i].Valor)
                              )
                          )
                        )
                    )
                    //, 11, MidpointRounding.AwayFromZero)
                    ;

                vTotalGasSecoMS9215EnergiaMMBTU = vTotalGasSecoMS9215EnergiaMMBTU +
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
                    ), 4, MidpointRounding.AwayFromZero);

                vTotalValorLiquidosUS = vTotalValorLiquidosUS +
                    Math.Round((double)
                        (
                           (
                            (
                             0.75 *
                             Math.Round((double)
                                    (
                                        (
                                            ((preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5) +
                                            ((preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5)
                                        )
                                         /
                                        (tipoCambio[i].Cambio) * 0.5318 * 42 * 3.785
                                    ), 2, MidpointRounding.AwayFromZero)
                            )
                            +
                            (
                                0.25 *
                                (double)Math.Round((decimal)
                                (
                                   Math.Round((double)
                                    (
                                        (
                                            ((preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5) +
                                            ((preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5)
                                        )
                                        /
                                        (tipoCambio[i].Cambio) * 0.5318 * 42 * 3.785
                                    ), 2, MidpointRounding.AwayFromZero) * 1.25
                                ), 4, MidpointRounding.AwayFromZero)
                            )
                        )

                        *
                        (double)Math.Round((decimal)
                        (

                            ((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion

                        )
                        , 2, MidpointRounding.AwayFromZero)
                     ), 2, MidpointRounding.AwayFromZero);

                vTotalCostoUnitMaquilaUSMMBTU = vTotalCostoUnitMaquilaUSMMBTU + preciosGLP[i].CostoUnitarioMaquila;

                    

                vTotalCostoMaquilaUS = vTotalCostoMaquilaUS +
                    Math.Round(
                    (
                    preciosGLP[i].CostoUnitarioMaquila *
                    Math.Round((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000, 4, MidpointRounding.AwayFromZero)
                    )
                    , 2, MidpointRounding.AwayFromZero)
                    ;
            }
            var dto = new BoletadeValorizacionPetroperuLoteIDto
            {
                Fecha = diaOperativo.ToString("MMM - yyyy"),//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalGasNaturalLoteIGNAMPCSD = Math.Round(vTotalGasNaturalLoteIGNAMPCSD, 2, MidpointRounding.AwayFromZero),
                TotalGasNaturalLoteIEnergiaMMBTU =Math.Round( vTotalGasNaturalLoteIEnergiaMMBTU, 4, MidpointRounding.AwayFromZero),
                TotalGasNaturalLoteILGNRecupBBL = Math.Round(vTotalGasNaturalLoteILGNRecupBBL, 2, MidpointRounding.AwayFromZero),

                TotalGasNaturalEficienciaPGT = Math.Round(vTotalGasNaturalEficienciaPGT / registrosVol.Count, 2, MidpointRounding.AwayFromZero),
                
                TotalGasSecoMS9215GNSLoteIMCSD = vTotalGasSecoMS9215GNSLoteIMCSD,
                
                TotalGasSecoMS9215EnergiaMMBTU = vTotalGasSecoMS9215EnergiaMMBTU,

                TotalValorLiquidosUS = Math.Round(vTotalValorLiquidosUS, 2, MidpointRounding.AwayFromZero),
                TotalCostoUnitMaquilaUSMMBTU = Math.Round(vTotalCostoUnitMaquilaUSMMBTU / registrosVol.Count, 2, MidpointRounding.AwayFromZero),
                TotalCostoMaquilaUS = vTotalCostoMaquilaUS,

                TotalDensidadGLPPromMesAnt = 0.5318,
                TotalMontoFacturarporUnnaE = vTotalCostoMaquilaUS,
                TotalMontoFacturarporPetroperu = Math.Round(vTotalValorLiquidosUS, 2, MidpointRounding.AwayFromZero),

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
            var tipoCambio = await _tipodeCambioRepositorio.ObtenerTipodeCambioMensualAsync(diaOperativo, 1);
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
                    

                    PrecioGLPESinIGVSolesKG = Math.Round((double)(preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))), 4, MidpointRounding.AwayFromZero),
                   
                    PrecioGLPGSinIGVSolesKG = Math.Round((double)(preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))), 4, MidpointRounding.AwayFromZero),
                    //Lotz69
                    //registrosVolLoteZ69[i].Valor - Math.Round((Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero) * 42 * await _registroRepositorio.ObtenerFactorAsync(diaOperativo, 1, (double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100)) / 1000), 4, MidpointRounding.AwayFromZero),// 2.7258/1.18
                    PrecioRefGLPSinIGVSolesKG =
                    Math.Round((double)
                    (
                        (( preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100)) ) * 0.5)  +
                        (( preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100)) ) * 0.5)
                    ), 4, MidpointRounding.AwayFromZero)
                    , 
                    PrecioGLPSinIGVUSBL =
                    Math.Round((double)
                    (
                        (
                            ((preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5) +
                            ((preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5)
                        ) 
                         /
                        (tipoCambio[i].Cambio) * 0.5318 * 42 * 3.785
                    ), 2, MidpointRounding.AwayFromZero)
                    ,

                    TipodeCambioSoles_US = Math.Round((double) tipoCambio[i].Cambio, 4, MidpointRounding.AwayFromZero),

                    PrecioCGNUSBL =
                    (double)Math.Round((decimal)
                    (
                       Math.Round((double)
                        (
                            (
                                ((preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5) +
                                ((preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5)
                            )
                            /
                            (tipoCambio[i].Cambio) * 0.5318 * 42 * 3.785
                        ), 2, MidpointRounding.AwayFromZero)
                       * 1.25
                    ), 2, MidpointRounding.AwayFromZero)
                    ,

                    ValorLiquidosUS =
                        Math.Round((double)
                        (
                           (
                            (
                             0.75 *
                             Math.Round((double)
                                    (
                                        (
                                            ((preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5) +
                                            ((preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5)
                                        )
                                         /
                                        (tipoCambio[i].Cambio) * 0.5318 * 42 * 3.785
                                    ), 2, MidpointRounding.AwayFromZero)
                            )
                            +
                            (
                                0.25 *
                                (double)Math.Round((decimal)
                                (
                                   Math.Round((double)
                                    (
                                        (
                                            ((preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5) +
                                            ((preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5)
                                        )
                                        /
                                        (tipoCambio[i].Cambio) * 0.5318 * 42 * 3.785
                                    ), 2, MidpointRounding.AwayFromZero) * 1.25
                                ), 4, MidpointRounding.AwayFromZero)
                            )
                        )
                        
                        *
                        (double)Math.Round((decimal)
                        (

                            ((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion

                        )
                        , 2, MidpointRounding.AwayFromZero)
                     ), 2, MidpointRounding.AwayFromZero)

                    ,
                    
                    CostoUnitMaquilaUSMMBTU = preciosGLP[i].CostoUnitarioMaquila,
                    
                    CostoMaquilaUS = 
                    Math.Round(
                    (
                    preciosGLP[i].CostoUnitarioMaquila * 
                    Math.Round((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000, 4, MidpointRounding.AwayFromZero)
                    )
                    ,2, MidpointRounding.AwayFromZero)


                });
            }
            return BoletadeValorizacionPetroperuLoteIDet;
        }
    }
}
