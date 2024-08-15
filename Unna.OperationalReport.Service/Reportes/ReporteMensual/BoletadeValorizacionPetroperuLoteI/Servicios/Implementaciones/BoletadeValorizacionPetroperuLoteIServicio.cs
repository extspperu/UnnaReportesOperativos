using Newtonsoft.Json;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using System.Globalization;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Servicios.Abstracciones;

using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;


namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuLoteIServicio : IBoletadeValorizacionPetroperuLoteIServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IFiscalizacionProductoProduccionRepositorio _fiscalizacionProductoProduccionRepositorio;
        private readonly IGnsVolumeMsYPcBrutoRepositorio _gnsVolumeMsYPcBrutoRepositorio;
        private readonly IPreciosGLPRepositorio _preciosGLPRepositorio;
        private readonly ITipodeCambioRepositorio _tipodeCambioRepositorio;
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();// DateTime.ParseExact("30/04/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);//FechasUtilitario.ObtenerDiaOperativo();
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
        public async Task<OperacionDto<BoletadeValorizacionPetroperuLoteIDto>> ObtenerAsync(long idUsuario)
        {
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletadeValorizacionPetroperu, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletadeValorizacionPetroperuLoteIDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            string observacion = default(string);
            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletadeValorizacionPetroperu, diaOperativo);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                observacion = operacionImpresion.Resultado?.Comentario;
                if (new DateTime(diaOperativo.Year, diaOperativo.Month, 1) == new DateTime(operacionImpresion.Resultado.Fecha.Year, operacionImpresion.Resultado.Fecha.Month, 1))
                {
                    var rpta = JsonConvert.DeserializeObject<BoletadeValorizacionPetroperuLoteIDto>(operacionImpresion.Resultado.Datos);
                    rpta.General = operacionGeneral.Resultado;
                    return new OperacionDto<BoletadeValorizacionPetroperuLoteIDto>(rpta);
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

                vTotalGasNaturalEficienciaPGT = vTotalGasNaturalEficienciaPGT +
                    Math.Round((double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100), 2, MidpointRounding.AwayFromZero);

                vTotalGasSecoMS9215GNSLoteIMCSD = vTotalGasSecoMS9215GNSLoteIMCSD +
                    (double)registroVolGNSTransf[i].VolumenGnsTransferidoLI

                    ;

                vTotalGasSecoMS9215EnergiaMMBTU = vTotalGasSecoMS9215EnergiaMMBTU +
                    Math.Round((double)
                    (
                        (double)registroVolGNSTransf[i].VolumenGnsTransferidoLI
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
                
                TotalGasSecoMS9215GNSLoteIMCSD = Math.Round(vTotalGasSecoMS9215GNSLoteIMCSD, 4, MidpointRounding.AwayFromZero),
                
                TotalGasSecoMS9215EnergiaMMBTU = Math.Round(vTotalGasSecoMS9215EnergiaMMBTU, 4, MidpointRounding.AwayFromZero),


                TotalValorLiquidosUS = Math.Round(vTotalValorLiquidosUS, 2, MidpointRounding.AwayFromZero),
                TotalCostoUnitMaquilaUSMMBTU = Math.Round(vTotalCostoUnitMaquilaUSMMBTU / registrosVol.Count, 2, MidpointRounding.AwayFromZero),
                TotalCostoMaquilaUS = Math.Round(vTotalCostoMaquilaUS, 2, MidpointRounding.AwayFromZero),

                TotalDensidadGLPPromMesAnt = 0.5318,
                TotalMontoFacturarporUnnaE = Math.Round(vTotalCostoMaquilaUS, 2, MidpointRounding.AwayFromZero),
                TotalMontoFacturarporPetroperu = Math.Round(vTotalValorLiquidosUS, 2, MidpointRounding.AwayFromZero),

                Observacion1 = "Emergencia operativa en planta de gas Pariñas los días 5, 6, 13 y 14 debido a factor externo (falla en el suministro eléctrico por hurto de cables).\r\nPara los días 5 y 6 de Abril se realizó un ajunte en el costo de maquila por las horas de gas no procesado que fueron 4 y 6 horas respectivamente.\r\nPara los días 13 y 14 de Abril se realizó un ajunte en el costo de maquila por las horas de gas no procesado que fueron 5 y 15 horas respectivamente.\r\nEmergencia operativa para los días 10 y 11 de Abril: Lote VI reportó parada intempestiva por caida de compresor en estación pariñas. Volumen de GNA dejado de entregar 0.76 MMPCS.\r\nEmergencia operativa el 15 de Abril: Lote I reportó parada en el compresor C8  debido a falla en el termostato del sistema de refrigeración. Volumen dejado de entregar  0.41 MMPCS"
                //Observacion2 = "",
                //Observacion3 = "",
                //Observacion4 = ""

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
            var registroVolGNSTransf = await _imprimirRepositorio.ObtenerVolumenGnsTransferidoAsync(7, diaOperativo);
            List<BoletadeValorizacionPetroperuLoteIDetDto> BoletadeValorizacionPetroperuLoteIDet = new List<BoletadeValorizacionPetroperuLoteIDetDto>();
            for (int i = 0; i < registrosVol.Count; i++)
            {
                BoletadeValorizacionPetroperuLoteIDet.Add(new BoletadeValorizacionPetroperuLoteIDetDto
                {
                    Dia = (int)registrosVol[i].Fecha.Day,
                    GasNaturalLoteIGNAMPCSD = registrosVol[i].Valor,
                    GasNaturalLoteIPCBTUPCSD = registrosPC[i].Valor,
                    GasNaturalLoteIEnergiaMMBTU = Math.Round((double)registrosVol[i].Valor * (double)registrosPC[i].Valor / 1000, 4, MidpointRounding.AwayFromZero),//(GasNaturalLoteIGNAMPCSD * GasNaturalLoteIPCBTUPCSD)/1000
                    GasNaturalLoteIRiquezaGALMPC = Math.Round((double)registroRiq[i].Valor, 4, MidpointRounding.AwayFromZero),
                    GasNaturalLoteIRiquezaBLMMPC = Math.Round((((double)registroRiq[i].Valor * 1000) / 42), 2, MidpointRounding.AwayFromZero),//(GasNaturalLoteIRiquezaGALMPC*1000)/42
                    GasNaturalLoteILGNRecupBBL = Math.Round((double)((registrosVol[i].Valor * registroRiq[i].Valor) / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) /42) / 42)  * fiscalizacionGlpCgn[i].Produccion, 2, MidpointRounding.AwayFromZero),
                   
                    GasNaturalEficienciaPGT_Porcentaje =  Math.Round((double)(fiscalizacionGlpCgn[i].Produccion / (((registrosVol[i].Valor * registroRiq[i].Valor) + (registrosVolLoteIV[i].Valor * registroRiqLoteIV[i].Valor) + (registrosVolLoteVI[i].Valor * registroRiqLoteVI[i].Valor) + (registrosVolLoteZ69[i].Valor * registroRiqLoteZ69[i].Valor) + (registrosVolLoteX[i].Valor * registroRiqLoteX[i].Valor)) / 42) * 100), 2, MidpointRounding.AwayFromZero),
                    
                    GasSecoMS9215GNSLoteIMCSD = Math.Round((double)registroVolGNSTransf[i].VolumenGnsTransferidoLI, 4, MidpointRounding.AwayFromZero)
                    ,

                    GasSecoMS9215PCBTUPCSD = gnsVolumeMsYPcBruto[i].PcBrutoRepCroma,
                    
                    
                    
                    GasSecoMS9215EnergiaMMBTU =
                    Math.Round((double)
                    (
                        
                         (double)registroVolGNSTransf[i].VolumenGnsTransferidoLI
                         * gnsVolumeMsYPcBruto[i].PcBrutoRepCroma / 1000
                    ), 4, MidpointRounding.AwayFromZero)
                    ,
                   
                    PrecioGLPESinIGVSolesKG = Math.Round((double)(preciosGLP[i].PrecioGLP_E / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))), 4, MidpointRounding.AwayFromZero),
                   
                    PrecioGLPGSinIGVSolesKG = Math.Round((double)(preciosGLP[i].PrecioGLP_G / (1 + (await _registroRepositorio.ObtenerIGVGNSManualAsync() / 100))), 4, MidpointRounding.AwayFromZero),
                    
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

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletadeValorizacionPetroperuLoteIDto peticion)
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