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
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Newtonsoft.Json;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuLoteZ69Servicio : IBoletadeValorizacionPetroperuLoteZ69Servicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IFiscalizacionProductoProduccionRepositorio _fiscalizacionProductoProduccionRepositorio;
        private readonly IGnsVolumeMsYPcBrutoRepositorio _gnsVolumeMsYPcBrutoRepositorio;
        private readonly IPreciosGLPRepositorio _preciosGLPRepositorio;
        private readonly ITipodeCambioRepositorio _tipodeCambioRepositorio;
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        DateTime diaOperativo = DateTime.ParseExact("30/04/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
        double vTotalGasNaturalLoteZ69GNAMPCSD = 0;
        double vTotalGasNaturalLoteZ69EnergiaMMBTU = 0;
        double vTotalGasNaturalLoteZ69LGNRecupBBL = 0;
        double vTotalGasNaturalEficienciaPGT = 0;
        double vTotalGasSecoMS9215GNSLoteZ69MCSD = 0;
        double vTotalGasSecoMS9215EnergiaMMBTU = 0;
        double vTotalValorLiquidosUS = 0;
        double vTotalCostoUnitMaquilaUSMMBTU = 0;
        double vTotalCostoMaquilaUS = 0;
        public BoletadeValorizacionPetroperuLoteZ69Servicio
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
        public async Task<OperacionDto<BoletadeValorizacionPetroperuLoteZ69Dto>> ObtenerAsync(long idUsuario)
        {
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletadeValorizacionPetroperuLoteZ69, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletadeValorizacionPetroperuLoteZ69Dto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            string observacion = default(string);
            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletadeValorizacionPetroperuLoteZ69, diaOperativo);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                observacion = operacionImpresion.Resultado?.Comentario;
                if (new DateTime(diaOperativo.Year, diaOperativo.Month, 1) == new DateTime(operacionImpresion.Resultado.Fecha.Year, operacionImpresion.Resultado.Fecha.Month, 1))
                {
                    var rpta = JsonConvert.DeserializeObject<BoletadeValorizacionPetroperuLoteZ69Dto>(operacionImpresion.Resultado.Datos);
                    rpta.General = operacionGeneral.Resultado;
                    return new OperacionDto<BoletadeValorizacionPetroperuLoteZ69Dto>(rpta);
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

            for (int i = 0; i < registrosVolLoteZ69.Count; i++)
            {
                vTotalGasNaturalLoteZ69GNAMPCSD = vTotalGasNaturalLoteZ69GNAMPCSD + (double)registrosVolLoteZ69[i].Valor;
                vTotalGasNaturalLoteZ69EnergiaMMBTU = vTotalGasNaturalLoteZ69EnergiaMMBTU + ((double)registrosVolLoteZ69[i].Valor * (double)registrosPCLoteZ69[i].Valor / 1000);
                vTotalGasNaturalLoteZ69LGNRecupBBL = vTotalGasNaturalLoteZ69LGNRecupBBL + Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero);
                vTotalGasNaturalEficienciaPGT = vTotalGasNaturalEficienciaPGT +
                    Math.Round((double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100), 2, MidpointRounding.AwayFromZero);

                vTotalGasSecoMS9215GNSLoteZ69MCSD = vTotalGasSecoMS9215GNSLoteZ69MCSD +
                    (double)registroVolGNSTransf[i].VolumenGnsTransferidoZ69;

                vTotalGasSecoMS9215EnergiaMMBTU = vTotalGasSecoMS9215EnergiaMMBTU +
                    Math.Round((double)
                    (
                        (double)registroVolGNSTransf[i].VolumenGnsTransferidoZ69
                        * gnsVolumeMsYPcBruto[i].PcBrutoRepCroma / 1000
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

                            ((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion

                        )
                        , 2, MidpointRounding.AwayFromZero)
                     ), 2, MidpointRounding.AwayFromZero);

                vTotalCostoUnitMaquilaUSMMBTU = vTotalCostoUnitMaquilaUSMMBTU + preciosGLP[i].CostoUnitarioMaquila;

                vTotalCostoMaquilaUS = vTotalCostoMaquilaUS +
                    Math.Round(
                    (
                    preciosGLP[i].CostoUnitarioMaquila *
                    Math.Round((double)registrosVolLoteZ69[i].Valor * (double)registrosPCLoteZ69[i].Valor / 1000, 4, MidpointRounding.AwayFromZero)
                    )
                    , 2, MidpointRounding.AwayFromZero)
                    ;

            }
            var dto = new BoletadeValorizacionPetroperuLoteZ69Dto
            {
                Fecha = diaOperativo.ToString("MMM - yyyy"),//FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MMMM-yyyy").Substring(3),
                TotalGasNaturalLoteZ69GNAMPCSD = vTotalGasNaturalLoteZ69GNAMPCSD,
                TotalGasNaturalLoteZ69EnergiaMMBTU = Math.Round(vTotalGasNaturalLoteZ69EnergiaMMBTU, 4, MidpointRounding.AwayFromZero),
                TotalGasNaturalLoteZ69LGNRecupBBL = Math.Round(vTotalGasNaturalLoteZ69LGNRecupBBL, 2, MidpointRounding.AwayFromZero),

                TotalGasNaturalEficienciaPGT = Math.Round(vTotalGasNaturalEficienciaPGT / registrosVolLoteZ69.Count, 2, MidpointRounding.AwayFromZero),

                TotalGasSecoMS9215GNSLoteZ69MCSD = Math.Round(vTotalGasSecoMS9215GNSLoteZ69MCSD, 4, MidpointRounding.AwayFromZero),

                TotalGasSecoMS9215EnergiaMMBTU = Math.Round(vTotalGasSecoMS9215EnergiaMMBTU, 4, MidpointRounding.AwayFromZero),

                TotalValorLiquidosUS = Math.Round(vTotalValorLiquidosUS, 2, MidpointRounding.AwayFromZero),
                TotalCostoUnitMaquilaUSMMBTU = Math.Round(vTotalCostoUnitMaquilaUSMMBTU / registrosVolLoteZ69.Count, 2, MidpointRounding.AwayFromZero),
                TotalCostoMaquilaUS = Math.Round(vTotalCostoMaquilaUS, 2, MidpointRounding.AwayFromZero),

                TotalDensidadGLPPromMesAnt = 0.5318,
                TotalMontoFacturarporUnnaE = Math.Round(vTotalCostoMaquilaUS, 2, MidpointRounding.AwayFromZero),
                TotalMontoFacturarporPetroperu = Math.Round(vTotalValorLiquidosUS, 2, MidpointRounding.AwayFromZero),

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

            for (int i = 0; i < registrosVolLoteZ69.Count; i++)
            {
                BoletadeValorizacionPetroperuLoteZ69Det.Add(new BoletadeValorizacionPetroperuLoteZ69DetDto
                {
                    Dia = registrosVolLoteZ69[i].Fecha.Day,
                    GasNaturalLoteZ69GNAMPCSD = registrosVolLoteZ69[i].Valor,
                    GasNaturalLoteZ69PCBTUPCSD = registrosPCLoteZ69[i].Valor,
                    GasNaturalLoteZ69EnergiaMMBTU = Math.Round((double)registrosVolLoteZ69[i].Valor * (double)registrosPCLoteZ69[i].Valor / 1000, 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteVIGNAMPCSD * GasNaturalLoteVIPCBTUPCSD)/1000
                    GasNaturalLoteZ69RiquezaGALMPC = Math.Round((double)registroRiqLoteZ69[i].Valor, 4, MidpointRounding.AwayFromZero),
                    GasNaturalLoteZ69RiquezaBLMMPC = Math.Round((((double)registroRiqLoteZ69[i].Valor * 1000) / 42), 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteVIRiquezaGALMPC*1000)/42
                    GasNaturalLoteZ69LGNRecupBBL = Math.Round((double)((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero),


                    GasNaturalEficienciaPGT_Porcentaje = Math.Round((double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100), 2, MidpointRounding.AwayFromZero),


                    GasSecoMS9215GNSLoteZ69MCSD =
                    Math.Round((double)registroVolGNSTransf[i].VolumenGnsTransferidoZ69, 4, MidpointRounding.AwayFromZero)
                    ,

                    GasSecoMS9215PCBTUPCSD = gnsVolumeMsYPcBruto[i].PcBrutoRepCroma,

                    GasSecoMS9215EnergiaMMBTU =
                    Math.Round((double)
                    (
                        (double)registroVolGNSTransf[i].VolumenGnsTransferidoZ69
                        * gnsVolumeMsYPcBruto[i].PcBrutoRepCroma / 1000
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

                            ((registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) / 42) * fiscalizacionGlpCgn[i].Produccion

                        )
                        , 2, MidpointRounding.AwayFromZero)
                     ), 2, MidpointRounding.AwayFromZero)
                    ,//((0.75*PrecioGLPSinIGVUSBL) + (0.25*PrecioCGNUSBL)) * GasNaturalLiquidosRecupTotalesBBL
                    
                    CostoUnitMaquilaUSMMBTU = preciosGLP[i].CostoUnitarioMaquila,
                    
                    CostoMaquilaUS =
                    Math.Round(
                    (
                    preciosGLP[i].CostoUnitarioMaquila *
                    Math.Round((double)registrosVolLoteZ69[i].Valor * (double)registrosPCLoteZ69[i].Valor / 1000, 4, MidpointRounding.AwayFromZero)
                    )
                    , 2, MidpointRounding.AwayFromZero)
                    //CostoUnitMaquilaUSMMBTU * (GasNaturalLoteIEnergiaMMBTU + GasNaturalLoteVIEnergiaMMBTU + GasNaturalLoteZ69EnergiaMMBTU)


                });
            }
            return BoletadeValorizacionPetroperuLoteZ69Det;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletadeValorizacionPetroperuLoteZ69Dto peticion)
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
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletadeValorizacionPetroperuLoteZ69),
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
