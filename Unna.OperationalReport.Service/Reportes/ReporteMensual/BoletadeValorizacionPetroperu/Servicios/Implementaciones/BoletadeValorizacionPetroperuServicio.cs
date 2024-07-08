using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuServicio : IBoletadeValorizacionPetroperuServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IFiscalizacionProductoProduccionRepositorio _fiscalizacionProductoProduccionRepositorio;
        private readonly IGnsVolumeMsYPcBrutoRepositorio _gnsVolumeMsYPcBrutoRepositorio;
        private readonly IPreciosGLPRepositorio _preciosGLPRepositorio;
        private readonly ITipodeCambioRepositorio _tipodeCambioRepositorio;
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo(); //DateTime.ParseExact("30/04/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
        double vTotalGasNaturalLoteIGNAMPCSD = 0;
        double vTotalGasNaturalLoteVIGNAMPCSD = 0;
        double vTotalGasNaturalLoteZ69GNAMPCSD = 0;
        double vTotalGasNaturalLoteIEnergiaMMBTU = 0;
        double vTotalGasNaturalLoteVIEnergiaMMBTU = 0;
        double vTotalGasNaturalLoteZ69EnergiaMMBTU = 0;
        double vTotalGasNaturalLoteILGNRecupBBL = 0;
        double vTotalGasNaturalLoteVILGNRecupBBL = 0;
        double vTotalGasNaturalLoteZ69LGNRecupBBL = 0;
        double vTotalGasNaturalTotalGNA = 0;
        double vTotalGasNaturalEficienciaPGT = 0;
        double vTotalGasNaturalLiquidosRecupTotales = 0;
        double vTotalGasSecoMS9215GNSLoteIMCSD = 0;
        double vTotalGasSecoMS9215GNSLoteVIMCSD = 0;
        double vTotalGasSecoMS9215GNSLoteZ69MCSD = 0;
        double vTotalGasSecoMS9215GNSTotalMCSD = 0;
        double vTotalGasSecoMS9215EnergiaMMBTU = 0;
        double vTotalValorLiquidosUS = 0;
        double vTotalCostoUnitMaquilaUSMMBTU = 0;
        double vTotalCostoMaquilaUS = 0;

        public BoletadeValorizacionPetroperuServicio
       (
           IRegistroRepositorio registroRepositorio,
           IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio,
           IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio,
           IPreciosGLPRepositorio preciosGLPRepositorio,
           ITipodeCambioRepositorio tipodeCambioRepositorio,
           IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio,
            IImprimirRepositorio imprimirRepositorio
       )
        {
            _registroRepositorio = registroRepositorio;
            _fiscalizacionProductoProduccionRepositorio = fiscalizacionProductoProduccionRepositorio;
            _gnsVolumeMsYPcBrutoRepositorio = gnsVolumeMsYPcBrutoRepositorio;
            _preciosGLPRepositorio = preciosGLPRepositorio;
            _tipodeCambioRepositorio = tipodeCambioRepositorio;
            _impresionServicio = impresionServicio;
            _reporteServicio = reporteServicio;
            _imprimirRepositorio = imprimirRepositorio;
        }
        public async Task<OperacionDto<BoletadeValorizacionPetroperuDto>> ObtenerAsync(long idUsuario)
        {

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletadeValorizacionPetroperu, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletadeValorizacionPetroperuDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            string observacion = default(string);
            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletadeValorizacionPetroperu, diaOperativo);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                observacion = operacionImpresion.Resultado?.Comentario;
                if (new DateTime(diaOperativo.Year, diaOperativo.Month, 1) == new DateTime(operacionImpresion.Resultado.Fecha.Year, operacionImpresion.Resultado.Fecha.Month, 1))
                {
                    var rpta = JsonConvert.DeserializeObject<BoletadeValorizacionPetroperuDto>(operacionImpresion.Resultado.Datos);
                    rpta.General = operacionGeneral.Resultado;
                    return new OperacionDto<BoletadeValorizacionPetroperuDto>(rpta);
                }
            }

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
            var registroVolGNSTransf = await _imprimirRepositorio.ObtenerVolumenGnsTransferidoAsync(7, diaOperativo);

            for (int i = 0; i < registrosVol.Count; i++)
            {
                vTotalGasNaturalLoteIGNAMPCSD = vTotalGasNaturalLoteIGNAMPCSD + (double)registrosVol[i].Valor;
                vTotalGasNaturalLoteIEnergiaMMBTU = vTotalGasNaturalLoteIEnergiaMMBTU + ((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000);
                vTotalGasNaturalLoteILGNRecupBBL = vTotalGasNaturalLoteILGNRecupBBL + Math.Round((double)((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero);

                vTotalGasNaturalLoteVIGNAMPCSD = vTotalGasNaturalLoteVIGNAMPCSD + (double)registrosVolLoteVI[i].Valor;
                vTotalGasNaturalLoteVIEnergiaMMBTU = vTotalGasNaturalLoteVIEnergiaMMBTU + ((double)registrosVolLoteVI[i].Valor * (double)registrosPCLoteVI[i].Valor / 1000);
                vTotalGasNaturalLoteVILGNRecupBBL = vTotalGasNaturalLoteVILGNRecupBBL + Math.Round((double)((registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero);

                vTotalGasNaturalLoteZ69GNAMPCSD = vTotalGasNaturalLoteZ69GNAMPCSD + (double)registrosVolLoteZ69[i].Valor;
                vTotalGasNaturalLoteZ69EnergiaMMBTU = vTotalGasNaturalLoteZ69EnergiaMMBTU + ((double)registrosVolLoteZ69[i].Valor * (double)registrosPCLoteZ69[i].Valor / 1000);
                vTotalGasNaturalLoteZ69LGNRecupBBL = vTotalGasNaturalLoteZ69LGNRecupBBL + Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero);

                vTotalGasNaturalTotalGNA = (double)(vTotalGasNaturalTotalGNA + (registrosVol[i].Valor + registrosVolLoteVI[i].Valor + registrosVolLoteZ69[i].Valor));

                vTotalGasNaturalEficienciaPGT = vTotalGasNaturalEficienciaPGT +
                    Math.Round((double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100), 2, MidpointRounding.AwayFromZero);

                vTotalGasNaturalLiquidosRecupTotales = vTotalGasNaturalLiquidosRecupTotales +
                    (
                    Math.Round((double)((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                    +
                    Math.Round((double)((registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                    +
                    Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                    )
                    ;

                vTotalGasSecoMS9215GNSLoteIMCSD = vTotalGasSecoMS9215GNSLoteIMCSD +
                    (double)registroVolGNSTransf[i].VolumenGnsTransferidoLI;

                vTotalGasSecoMS9215GNSLoteVIMCSD = vTotalGasSecoMS9215GNSLoteVIMCSD +
                     (double)registroVolGNSTransf[i].VolumenGnsTransferidoLVI;

                vTotalGasSecoMS9215GNSLoteZ69MCSD = vTotalGasSecoMS9215GNSLoteZ69MCSD +
                    (double)registroVolGNSTransf[i].VolumenGnsTransferidoZ69;

                vTotalGasSecoMS9215GNSTotalMCSD = vTotalGasSecoMS9215GNSTotalMCSD +
                   ((double)registroVolGNSTransf[i].VolumenGnsTransferidoLI + (double)registroVolGNSTransf[i].VolumenGnsTransferidoLVI + (double)registroVolGNSTransf[i].VolumenGnsTransferidoZ69) ;

                vTotalGasSecoMS9215EnergiaMMBTU = vTotalGasSecoMS9215EnergiaMMBTU +
                   (double)
                   (
                    ((double)registroVolGNSTransf[i].VolumenGnsTransferidoLI + (double)registroVolGNSTransf[i].VolumenGnsTransferidoLVI + (double)registroVolGNSTransf[i].VolumenGnsTransferidoZ69)
                    * registrosPC[i].Valor / 1000
                   );

                vTotalValorLiquidosUS = vTotalValorLiquidosUS +
                    Math.Round((double)
                           Math.Round((double)
                               (
                                   Math.Round((double)
                                       (
                                        0.75 *
                                        Math.Round(
                                               (
                                                   (
                                                       ((preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5) +
                                                       ((preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5)
                                                   )
                                                    /
                                                   (tipoCambio[i].Cambio) * 0.5318 * 42 * 3.785
                                               )
                                        , 2, MidpointRounding.AwayFromZero)
                                       )
                                   , 5, MidpointRounding.AwayFromZero)
                                  +
                                   Math.Round((double)
                                       0.25 *

                                           (
                                               Math.Round((double)
                                               (
                                                   (
                                                       ((preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5) +
                                                       ((preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5)
                                                   )
                                                   /
                                                   (tipoCambio[i].Cambio) * 0.5318 * 42 * 3.785
                                               )
                                              , 2, MidpointRounding.AwayFromZero)
                                              * 1.25
                                           )
                                    , 5, MidpointRounding.AwayFromZero)

                              )
                           , 5, MidpointRounding.AwayFromZero)

                           *

                           Math.Round(
                                Math.Round((double)((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                                +
                                Math.Round((double)((registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                                +
                                Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                          , 2, MidpointRounding.AwayFromZero)
                        , 2, MidpointRounding.AwayFromZero);

                vTotalCostoUnitMaquilaUSMMBTU = vTotalCostoUnitMaquilaUSMMBTU + preciosGLP[i].CostoUnitarioMaquila;

                vTotalCostoMaquilaUS = vTotalCostoMaquilaUS +
                     Math.Round(
                    (
                    preciosGLP[i].CostoUnitarioMaquila *
                        (
                        Math.Round((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000, 4, MidpointRounding.AwayFromZero) +
                        Math.Round((double)registrosVolLoteVI[i].Valor * (double)registrosPCLoteVI[i].Valor / 1000, 4, MidpointRounding.AwayFromZero) +
                        Math.Round((double)registrosVolLoteZ69[i].Valor * (double)registrosPCLoteZ69[i].Valor / 1000, 4, MidpointRounding.AwayFromZero)
                        )
                    )
                    , 2, MidpointRounding.AwayFromZero);


            }
            var dto = new BoletadeValorizacionPetroperuDto
            {
                Fecha = diaOperativo.ToString("MMM - yyyy"),//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalGasNaturalLoteIGNAMPCSD= Math.Round(vTotalGasNaturalLoteIGNAMPCSD, 2, MidpointRounding.AwayFromZero),
                TotalGasNaturalLoteIEnergiaMMBTU = Math.Round(vTotalGasNaturalLoteIEnergiaMMBTU, 4, MidpointRounding.AwayFromZero),
                TotalGasNaturalLoteILGNRecupBBL = Math.Round(vTotalGasNaturalLoteILGNRecupBBL, 2, MidpointRounding.AwayFromZero),

                TotalGasNaturalLoteVIGNAMPCSD = Math.Round(vTotalGasNaturalLoteVIGNAMPCSD, 2, MidpointRounding.AwayFromZero),
                TotalGasNaturalLoteVIEnergiaMMBTU = Math.Round(vTotalGasNaturalLoteVIEnergiaMMBTU, 4, MidpointRounding.AwayFromZero),
                TotalGasNaturalLoteVILGNRecupBBL = Math.Round(vTotalGasNaturalLoteVILGNRecupBBL, 2, MidpointRounding.AwayFromZero),

                TotalGasNaturalLoteZ69GNAMPCSD = Math.Round(vTotalGasNaturalLoteZ69GNAMPCSD, 2, MidpointRounding.AwayFromZero),
                TotalGasNaturalLoteZ69EnergiaMMBTU = Math.Round(vTotalGasNaturalLoteZ69EnergiaMMBTU, 4, MidpointRounding.AwayFromZero),
                TotalGasNaturalLoteZ69LGNRecupBBL = Math.Round(vTotalGasNaturalLoteZ69LGNRecupBBL, 2, MidpointRounding.AwayFromZero),

                TotalGasNaturalTotalGNA = Math.Round(vTotalGasNaturalTotalGNA, 4, MidpointRounding.AwayFromZero),
                TotalGasNaturalEficienciaPGT = Math.Round(vTotalGasNaturalEficienciaPGT / registrosVol.Count, 2, MidpointRounding.AwayFromZero),
                TotalGasNaturalLiquidosRecupTotales= Math.Round(vTotalGasNaturalLiquidosRecupTotales, 2, MidpointRounding.AwayFromZero),

                TotalGasSecoMS9215GNSLoteIMCSD= Math.Round(vTotalGasSecoMS9215GNSLoteIMCSD, 4, MidpointRounding.AwayFromZero),
                TotalGasSecoMS9215GNSLoteVIMCSD= Math.Round(vTotalGasSecoMS9215GNSLoteVIMCSD, 4, MidpointRounding.AwayFromZero),
                TotalGasSecoMS9215GNSLoteZ69MCSD = Math.Round(vTotalGasSecoMS9215GNSLoteZ69MCSD, 4, MidpointRounding.AwayFromZero),

                TotalGasSecoMS9215GNSTotalMCSD = Math.Round(vTotalGasSecoMS9215GNSTotalMCSD, 4, MidpointRounding.AwayFromZero), 
                TotalGasSecoMS9215EnergiaMMBTU = Math.Round(vTotalGasSecoMS9215EnergiaMMBTU, 4, MidpointRounding.AwayFromZero),

                TotalValorLiquidosUS = Math.Round(vTotalValorLiquidosUS, 2, MidpointRounding.AwayFromZero),
                TotalCostoUnitMaquilaUSMMBTU = Math.Round(vTotalCostoUnitMaquilaUSMMBTU / registrosVol.Count, 2, MidpointRounding.AwayFromZero),
                TotalCostoMaquilaUS = Math.Round(vTotalCostoMaquilaUS,2, MidpointRounding.AwayFromZero),

                TotalDensidadGLPPromMesAnt = 0.5318,
                TotalMontoFacturarporUnnaE = Math.Round(vTotalCostoMaquilaUS, 2, MidpointRounding.AwayFromZero),
                TotalMontoFacturarporPetroperu = Math.Round(vTotalValorLiquidosUS, 2, MidpointRounding.AwayFromZero),

                Observacion1= "Emergencia operativa en planta de gas Pariñas los días 5, 6, 13 y 14 debido a factor externo (falla en el suministro eléctrico por hurto de cables).\r\nPara los días 5 y 6 de Abril se realizó un ajunte en el costo de maquila por las horas de gas no procesado que fueron 4 y 6 horas respectivamente.\r\nPara los días 13 y 14 de Abril se realizó un ajunte en el costo de maquila por las horas de gas no procesado que fueron 5 y 15 horas respectivamente.\r\nEmergencia operativa para los días 10 y 11 de Abril: Lote VI reportó parada intempestiva por caida de compresor en estación pariñas. Volumen de GNA dejado de entregar 0.76 MMPCS.\r\nEmergencia operativa el 15 de Abril: Lote I reportó parada en el compresor C8  debido a falla en el termostato del sistema de refrigeración. Volumen dejado de entregar  0.41 MMPCS",
                Observacion2 = "Volumen de GNA dejado de entregar por emergencia operativa de PETROPERU en el 2023: 9.96 MMPCS Nov-2023: 9.66 MMPCSD y Dic-2023: 0.3 MMPCSD",
                Observacion3 = "Volumen de GNA dejado de entregar por emergencia operativa de PETROPERU en el 2024: 2.31 MMPCS Mar-2024: 1.14 MMPCSD y Abr-2024: 1.17 MMPCSD",
                Observacion4 = "Volumen total de GNA dejado de entregar por emergencia operativa de PETROPERU: 12.27 MMPCSD"

            };
            dto.BoletadeValorizacionPetroperuDet = await BoletadeValorizacionPetroperuDet();
            
            return new OperacionDto<BoletadeValorizacionPetroperuDto>(dto);
        }

        private async Task<List<BoletadeValorizacionPetroperuDetDto>> BoletadeValorizacionPetroperuDet()
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
            var gnsVolumeMsYPcBruto = await _gnsVolumeMsYPcBrutoRepositorio.ObtenerPorTipoYNombreDiaOperativoMensualAsync("VolumenMsGnsAgpsa", "GNS A REFINERÍA", diaOperativo);
            var preciosGLP = await _preciosGLPRepositorio.ObtenerPreciosGLPMensualAsync(diaOperativo);
            var tipoCambio = await _tipodeCambioRepositorio.ObtenerTipodeCambioMensualAsync(diaOperativo, 1);
            var registroVolGNSTransf = await _imprimirRepositorio.ObtenerVolumenGnsTransferidoAsync(7, diaOperativo);

            List<BoletadeValorizacionPetroperuDetDto> BoletadeValorizacionPetroperuDet = new List<BoletadeValorizacionPetroperuDetDto>();
            for (int i = 0; i < registrosVol.Count; i++)
            {
                BoletadeValorizacionPetroperuDet.Add(new BoletadeValorizacionPetroperuDetDto
                {
                    Dia = registrosVol[i].Fecha.Day,
                    GasNaturalLoteIGNAMPCSD = registrosVol[i].Valor,
                    GasNaturalLoteIPCBTUPCSD = registrosPC[i].Valor,
                    GasNaturalLoteIEnergiaMMBTU = Math.Round((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000, 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                    GasNaturalLoteIRiquezaGALMPC = Math.Round((double)registroRiq[i].Valor, 4, MidpointRounding.AwayFromZero),
                    GasNaturalLoteIRiquezaBLMMPC = Math.Round((((double)registroRiq[i].Valor * 1000) / 42), 2, MidpointRounding.AwayFromZero),//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                    GasNaturalLoteILGNRecupBBL = Math.Round((double)((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero),

                    GasNaturalLoteVIGNAMPCSD = registrosVolLoteVI[i].Valor,
                    GasNaturalLoteVIPCBTUPCSD = registrosPCLoteVI[i].Valor,
                    GasNaturalLoteVIEnergiaMMBTU = Math.Round((double)registrosVolLoteVI[i].Valor * (double)registrosPCLoteVI[i].Valor / 1000, 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                    GasNaturalLoteVIRiquezaGALMPC = Math.Round((double)registroRiqLoteVI[i].Valor, 4, MidpointRounding.AwayFromZero),
                    GasNaturalLoteVIRiquezaBLMMPC = Math.Round((((double)registroRiqLoteVI[i].Valor * 1000) / 42), 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                    GasNaturalLoteVILGNRecupBBL = Math.Round((double)((registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero),

                    GasNaturalLoteZ69GNAMPCSD = registrosVolLoteZ69[i].Valor,
                    GasNaturalLoteZ69PCBTUPCSD = registrosPCLoteZ69[i].Valor,
                    GasNaturalLoteZ69EnergiaMMBTU = Math.Round((double)registrosVolLoteZ69[i].Valor * (double)registrosPCLoteZ69[i].Valor / 1000, 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteZ69GNAMPCSD * GasNaturalLoteZ69PCBTUPCSD)/1000
                    GasNaturalLoteZ69RiquezaGALMPC = Math.Round((double)registroRiqLoteZ69[i].Valor, 4, MidpointRounding.AwayFromZero),
                    GasNaturalLoteZ69RiquezaBLMMPC = Math.Round((((double)registroRiqLoteZ69[i].Valor * 1000) / 42), 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteZ69RiquezaGALMPC*1000)/42
                    GasNaturalLoteZ69LGNRecupBBL = Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero),

                    GasNaturalTotalGNAMPCSD =Math.Round((double) (registrosVol[i].Valor + registrosVolLoteVI[i].Valor + registrosVolLoteZ69[i].Valor), 4, MidpointRounding.AwayFromZero),//GasNaturalLoteIGNAMPCSD+GasNaturalLoteVIGNAMPCSD+GasNaturalLoteZ69GNAMPCSD
                    GasNaturalEficienciaPGT_Porcentaje = Math.Round((double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100), 2, MidpointRounding.AwayFromZero),
                    GasNaturalLiquidosRecupTotalesBBL =
                        Math.Round(
                            Math.Round((double)((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                            +
                            Math.Round((double)((registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                            +
                            Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                        , 2, MidpointRounding.AwayFromZero)

                    , 

                    GasSecoMS9215GNSLoteIMCSD =
                    Math.Round(
                           (double)registroVolGNSTransf[i].VolumenGnsTransferidoLI , 4, MidpointRounding.AwayFromZero)
                    ,
                    GasSecoMS9215GNSLoteVIMCSD =
                    Math.Round(
                       (double)registroVolGNSTransf[i].VolumenGnsTransferidoLVI, 4, MidpointRounding.AwayFromZero)
                    ,
                    GasSecoMS9215GNSLoteZ69MCSD =
                    Math.Round(
                            (double)registroVolGNSTransf[i].VolumenGnsTransferidoZ69, 4, MidpointRounding.AwayFromZero)
                    ,
                    GasSecoMS9215GNSTotalMCSD =
                    Math.Round(
                        ((double)registroVolGNSTransf[i].VolumenGnsTransferidoLI + (double)registroVolGNSTransf[i].VolumenGnsTransferidoLVI + (double)registroVolGNSTransf[i].VolumenGnsTransferidoZ69), 4, MidpointRounding.AwayFromZero)
                    ,
                    GasSecoMS9215PCBTUPCSD = gnsVolumeMsYPcBruto[i].PcBrutoRepCroma,
                    GasSecoMS9215EnergiaMMBTU =
                        Math.Round((double)
                      (
                            ((double)registroVolGNSTransf[i].VolumenGnsTransferidoLI + (double)registroVolGNSTransf[i].VolumenGnsTransferidoLVI + (double)registroVolGNSTransf[i].VolumenGnsTransferidoZ69)
                       *
                       gnsVolumeMsYPcBruto[i].PcBrutoRepCroma
                       / 1000
                       ), 4, MidpointRounding.AwayFromZero)

                    ,//(GasSecoMS9215GNSTotalMCSD*GasSecoMS9215PCBTUPCSD)/1000

                    PrecioGLPESinIGVSolesKG = Math.Round((double)(preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))), 4, MidpointRounding.AwayFromZero), // 1.9942 / 1.18
                    PrecioGLPGSinIGVSolesKG = Math.Round((double)(preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))), 4, MidpointRounding.AwayFromZero),// 2.7258/1.18
                    PrecioRefGLPSinIGVSolesKG =
                    Math.Round((double)
                    (
                        ((preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5) +
                        ((preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5)
                    ), 4, MidpointRounding.AwayFromZero)
                    , // PrecioGLPESinIGVSolesKG*0.5 + PrecioGLPGSinIGVSolesKG * 0.5
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
                    ,//PrecioRefGLPSinIGVSolesKG/TipodeCambioSoles_US * TotalDensidadGLPPromMesAnt * 42 * 3.785
                    TipodeCambioSoles_US = Math.Round((double)tipoCambio[i].Cambio, 4, MidpointRounding.AwayFromZero),
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
                    ,//PrecioGLPSinIGVUSBL*1.25
                    ValorLiquidosUS =
                        Math.Round((double)
                           Math.Round((double)
                               (
                                   Math.Round((double)
                                       (
                                        0.75 *
                                        Math.Round(
                                               (
                                                   (
                                                       ((preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5) +
                                                       ((preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5)
                                                   )
                                                    /
                                                   (tipoCambio[i].Cambio) * 0.5318 * 42 * 3.785
                                               )
                                        , 2, MidpointRounding.AwayFromZero)
                                       )
                                   , 5, MidpointRounding.AwayFromZero)
                                  +
                                   Math.Round((double)
                                       0.25 *

                                           (
                                               Math.Round((double)
                                               (
                                                   (
                                                       ((preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5) +
                                                       ((preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))) * 0.5)
                                                   )
                                                   /
                                                   (tipoCambio[i].Cambio) * 0.5318 * 42 * 3.785
                                               )
                                              , 2, MidpointRounding.AwayFromZero)
                                              * 1.25
                                           )
                                    , 5, MidpointRounding.AwayFromZero)

                              )
                           , 5, MidpointRounding.AwayFromZero)

                           *

                           Math.Round(
                                Math.Round((double)((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                                +
                                Math.Round((double)((registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                                +
                                Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero)
                          , 5, MidpointRounding.AwayFromZero)
                        , 2, MidpointRounding.AwayFromZero)

                    ,
                    CostoUnitMaquilaUSMMBTU = preciosGLP[i].CostoUnitarioMaquila,
                    CostoMaquilaUS =
                     Math.Round(
                    (
                    preciosGLP[i].CostoUnitarioMaquila *
                        (
                        Math.Round((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000, 4, MidpointRounding.AwayFromZero) +
                        Math.Round((double)registrosVolLoteVI[i].Valor * (double)registrosPCLoteVI[i].Valor / 1000, 4, MidpointRounding.AwayFromZero) +
                        Math.Round((double)registrosVolLoteZ69[i].Valor * (double)registrosPCLoteZ69[i].Valor / 1000, 4, MidpointRounding.AwayFromZero)
                        )
                    )
                    , 2, MidpointRounding.AwayFromZero)
                    


                });

            }

            return BoletadeValorizacionPetroperuDet;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletadeValorizacionPetroperuDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            DateTime fecha = diaOperativo;// FechasUtilitario.ObtenerDiaOperativo();
            peticion.General = null;
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletadeValorizacionPetroperu),
                Fecha = fecha,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                Comentario = null
            };

            //DateTime Desde = new DateTime(fecha.Year, fecha.Month, 1);
            //DateTime Hasta = new DateTime(fecha.Year, fecha.Month, 15);//.AddMonths(1).AddDays(-1);
            //if (peticion.ComposicionGnaLIVDetComposicion != null && peticion.ComposicionGnaLIVDetComposicion.Count > 0)
            //{
            //    await _composicionRepositorio.EliminarPorFechaAsync(Desde, Hasta);
            //    foreach (var item in peticion.ComposicionGnaLIVDetComposicion)
            //    {
            //        //DateTime compGnaDia = DateTime.ParseExact(item.CompGnaDia, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //        if (!item.CompGnaDia.Equals(null))
            //        {
            //            continue;
            //        }

            //        //var compo = new ComposicionGnaLIVDetComposicionDto
            //        //{
            //        //    CompGnaDia = item.CompGnaDia,
            //        //    CompGnaC6 = item.CompGnaC6,
            //        //    CompGnaC3 = item.CompGnaC3,
            //        //    CompGnaIc4 = item.CompGnaIc4,
            //        //    CompGnaNc4 = item.CompGnaNc4,
            //        //    CompGnaNeoC5 = item.CompGnaNeoC5,
            //        //    CompGnaIc5 = item.CompGnaIc5,
            //        //    CompGnaNc5 = item.CompGnaNc5,
            //        //    CompGnaNitrog = item.CompGnaNitrog,
            //        //    CompGnaC1 = item.CompGnaC1,
            //        //    CompGnaCo2 = item.CompGnaCo2,
            //        //    CompGnaC2 = item.CompGnaC2

            //        //};

            //        //var composicion = new Data.Registro.Entidades.Composicion
            //        //{
            //        //    //Fecha = item.Fecha.Value,
            //        //    CompGnaDia = item.CompGnaDia,
            //        //    CompGnaC6 = item.CompGnaC6,
            //        //    CompGnaC3 = item.CompGnaC3,
            //        //    CompGnaIc4 = item.CompGnaIc4,
            //        //    CompGnaNc4 = item.CompGnaNc4,
            //        //    CompGnaNeoC5 = item.CompGnaNeoC5,
            //        //    CompGnaIc5 = item.CompGnaIc5,
            //        //    CompGnaNc5 = item.CompGnaNc5,
            //        //    CompGnaNitrog = item.CompGnaNitrog,
            //        //    CompGnaC1 = item.CompGnaC1,
            //        //    CompGnaCo2 = item.CompGnaCo2,
            //        //    CompGnaC2 = item.CompGnaC2
            //        //    //Orden = item.GlpBls,
            //        //    //Simbolo = item.GnsMpc,
            //        //    //Actualizado = DateTime.UtcNow
            //        //};
            //        //await _composicionRepositorio.InsertarAsync(compo);
            //        //await _composicionRepositorio.InsertarAsync(composicion);
            //    }
            //}

            return await _impresionServicio.GuardarAsync(dto);

        }
    }
}
