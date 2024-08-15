using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Dtos;
using Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
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
        private readonly IMensualRepositorio _mensualRepositorio;
<<<<<<< HEAD
        private readonly IPrecioBoletaRepositorio _precioBoletaRepositorio;
        private readonly IValoresDefectoReporteServicio _valoresDefectoReporteServicio;
        private readonly ITipoCambioServicio _tipoCambioServicio;
=======
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;
>>>>>>> fafbb8744fc13ff0060b52ee25dd0cf31af1fa9f


        public BoletadeValorizacionPetroperuServicio
       (
           IRegistroRepositorio registroRepositorio,
           IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio,
           IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio,
           IPreciosGLPRepositorio preciosGLPRepositorio,
           ITipodeCambioRepositorio tipodeCambioRepositorio,
           IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio,
            IImprimirRepositorio imprimirRepositorio,
            IMensualRepositorio mensualRepositorio,
<<<<<<< HEAD
            IPrecioBoletaRepositorio precioBoletaRepositorio,
            IValoresDefectoReporteServicio valoresDefectoReporteServicio,
            ITipoCambioServicio tipoCambioServicio
=======
            ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio
>>>>>>> fafbb8744fc13ff0060b52ee25dd0cf31af1fa9f
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
            _mensualRepositorio = mensualRepositorio;
<<<<<<< HEAD
            _precioBoletaRepositorio = precioBoletaRepositorio;
            _valoresDefectoReporteServicio = valoresDefectoReporteServicio;
            _tipoCambioServicio = tipoCambioServicio;
=======
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
>>>>>>> fafbb8744fc13ff0060b52ee25dd0cf31af1fa9f
        }
        public async Task<OperacionDto<BoletadeValorizacionPetroperuDto>> ObtenerAsync(long idUsuario)
        {

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletadeValorizacionPetroperu, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletadeValorizacionPetroperuDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddMonths(-1);

            DateTime desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
            DateTime hasta = new DateTime(diaOperativo.Year, diaOperativo.Month, 1).AddMonths(1).AddDays(-1);

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletadeValorizacionPetroperu, desde);
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos) && operacionImpresion.Resultado.EsEditado)
            {
                var rpta = JsonConvert.DeserializeObject<BoletadeValorizacionPetroperuDto>(operacionImpresion.Resultado.Datos);
                if (rpta != null)
                {
                    rpta.RutaFirma = operacionGeneral?.Resultado?.RutaFirma;
                    return new OperacionDto<BoletadeValorizacionPetroperuDto>(rpta);
                }

            }

            var datos = await _mensualRepositorio.BoletaValorizacionPetroPeruAsync(desde, hasta);
            if (datos == null)
            {
                return new OperacionDto<BoletadeValorizacionPetroperuDto>(CodigosOperacionDto.NoExiste, "No se puede obtener el reporte, contactarse con soporte");
            }

            var precioGlpG = await _precioBoletaRepositorio.ListarPorFechasYIdTipoPrecioAsync(desde, hasta, (int)TiposPrecioBoleta.PrecioGlpG);
            var precioGlpE = await _precioBoletaRepositorio.ListarPorFechasYIdTipoPrecioAsync(desde, hasta, (int)TiposPrecioBoleta.PrecioGlpE);
            var precioUnitarioMaquilas = await _precioBoletaRepositorio.ListarPorFechasYIdTipoPrecioAsync(desde, hasta, (int)TiposPrecioBoleta.PrecioUnitarioMaquila);

            var boletadeValorizacionPetroperu = datos.Select(e => new BoletadeValorizacionPetroperuDetDto
            {
                Fecha = e.Fecha,
                Dia = e.Fecha.Day,
                GnaLoteI = e.GnaLoteI,
                PoderCalorificoLoteI = e.PoderCalorificoLoteI,
                EnergiaLoteI = e.EnergiaLoteI,
                RiquezaLoteI = e.RiquezaLoteI,
                RiquezaBlLoteI = e.RiquezaBlLoteI,
                LgnRecuperadosLoteI = e.LgnRecuperadosLoteI,
                GnaLoteVi = e.GnaLoteVi,
                PoderCalorificoLoteVi = e.PoderCalorificoLoteVi,
                EnergiaLoteVi = e.EnergiaLoteVi,
                RiquezaLoteVi = e.RiquezaLoteVi,
                RiquezaBlLoteVi = e.RiquezaBlLoteVi,
                LgnRecuperadosLoteVi = e.LgnRecuperadosLoteVi,
                GnaLoteZ69 = e.GnaLoteZ69,
                PoderCalorificoLoteZ69 = e.PoderCalorificoLoteZ69,
                EnergiaLoteZ69 = e.EnergiaLoteZ69,
                RiquezaLoteZ69 = e.RiquezaLoteZ69,
                RiquezaBlLoteZ69 = e.RiquezaBlLoteZ69,
                LgnRecuperadosLoteZ69 = e.LgnRecuperadosLoteZ69,
                TotalGna = e.TotalGna,
                Eficiencia = e.Eficiencia,
                TotalLiquidosRecuperados = e.TotalLiquidosRecuperados,
                GnsLoteI = e.GnsLoteI,
                GnsLoteVi = e.GnsLoteVi,
                GnsLoteZ69 = e.GnsLoteZ69,
                GnsTotal = e.GnsTotal,
                PoderCalorificoBtuPcsd = e.PoderCalorificoBtuPcsd,
                EnergiaMmbtu = e.EnergiaMmbtu
            }).ToList();

           
            List<TipoCambioDto> tipoCambios = new List<TipoCambioDto>();
            var operacionTipoCambio = await _tipoCambioServicio.ListarParaMesCompletoPorFechasAsync(desde, hasta, (int)TiposMonedas.Soles);
            if (operacionTipoCambio.Completado && operacionTipoCambio.Resultado != null)
            {
                tipoCambios = operacionTipoCambio.Resultado;
            }

            double igv = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.Igv) ?? 0;
            double densidadGlp = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.DensidadGlpPromedio) ?? 0;

            double sinIgv = (igv + 100) / 100;
            if (sinIgv > 0)
            {
                for (int i = 0; i < boletadeValorizacionPetroperu.Count; i++)
                {
                    if (precioGlpE != null)
                    {
                        var glpE = precioGlpE.Where(e => e.Fecha <= boletadeValorizacionPetroperu[i].Fecha).OrderByDescending(e => e.Fecha).Take(1).FirstOrDefault();
                        if (glpE != null) boletadeValorizacionPetroperu[i].PrecioGlpESinIgvSolesKg = glpE.Precio.HasValue ? (glpE.Precio.Value / sinIgv) : 0;
                    }

                    if (precioGlpG != null)
                    {
                        var glpG = precioGlpG.Where(e => e.Fecha <= boletadeValorizacionPetroperu[i].Fecha).OrderByDescending(e => e.Fecha).Take(1).FirstOrDefault();
                        if (glpG != null) boletadeValorizacionPetroperu[i].PrecioGlpGSinIGVSolesKg = glpG.Precio.HasValue ? (glpG.Precio.Value / sinIgv) : 0;
                    }

                    boletadeValorizacionPetroperu[i].PrecioRefGlpSinIgvSolesKg = (boletadeValorizacionPetroperu[i].PrecioGlpESinIgvSolesKg * 0.5) + (boletadeValorizacionPetroperu[i].PrecioGlpGSinIGVSolesKg * 0.5);

                    if (tipoCambios.Count > 0)
                    {
                        var tipoCambio = tipoCambios.Where(e => e.Fecha <= boletadeValorizacionPetroperu[i].Fecha).OrderByDescending(e => e.Fecha).Take(1).FirstOrDefault();
                        if (tipoCambio != null) boletadeValorizacionPetroperu[i].TipodeCambioSolesUs = tipoCambio.Cambio;
                    }

                    double precio = boletadeValorizacionPetroperu[i].TipodeCambioSolesUs * densidadGlp * 42 * 3.785;
                    if (precio > 0)
                    {
                        boletadeValorizacionPetroperu[i].PrecioGLPSinIgvUsBl = Math.Round(boletadeValorizacionPetroperu[i].PrecioRefGlpSinIgvSolesKg / precio, 2);
                    }

                    boletadeValorizacionPetroperu[i].PrecioCgnUsBl = boletadeValorizacionPetroperu[i].PrecioGLPSinIgvUsBl * 1.25;
                    boletadeValorizacionPetroperu[i].ValorLiquidosUs = ((0.75 * boletadeValorizacionPetroperu[i].PrecioGLPSinIgvUsBl) + (0.25 * boletadeValorizacionPetroperu[i].PrecioCgnUsBl)) * boletadeValorizacionPetroperu[i].TotalLiquidosRecuperados;

                    if (precioUnitarioMaquilas != null && precioUnitarioMaquilas.Count > 0)
                    {
                        var precioMaquila = precioUnitarioMaquilas.Where(e => e.Fecha <= boletadeValorizacionPetroperu[i].Fecha).OrderByDescending(e => e.Fecha).Take(1).FirstOrDefault();
                        if (precioMaquila != null) boletadeValorizacionPetroperu[i].CostoUnitMaquilaUsMmbtu = precioMaquila.Precio ?? 0;
                    }
                    boletadeValorizacionPetroperu[i].CostoMaquilaUs = boletadeValorizacionPetroperu[i].CostoUnitMaquilaUsMmbtu * (boletadeValorizacionPetroperu[i].EnergiaLoteI + boletadeValorizacionPetroperu[i].EnergiaLoteVi + boletadeValorizacionPetroperu[i].EnergiaLoteZ69);

                }
            }

            double menores10M = boletadeValorizacionPetroperu.Where(e => e.TotalGna < 10000).Sum(e => e.TotalGna);

            double emergenciaUnna = (40000 - menores10M) / 2000;
            double emergenciaPetroperu = (40000 - menores10M) / 2000 + (10000 - emergenciaUnna) / 1000;
            double emergencia = (10000 - emergenciaUnna) / 1000;

            string? mes = FechasUtilitario.ObtenerNombreMes(desde);
            string? observacion = $"- Emergencia operativa de {mes} de UNNA Energía debido de intervención del Ducto N°12. Volumen de maquila suspendido de maquila: {Math.Round(emergenciaUnna, 2)} MMPCSD \n";
            observacion += $"- Emergencia operativa de {mes} de PETROPERU debido de intervención del Ducto N°12. Volumen de GNA dejado de entregar: {Math.Round(emergenciaPetroperu, 2)} MMPCSD \n";
            observacion += $"- Emergencia operativa de PETROPERU debido de intervención a compresor de campo del Lote I. Volumen de GNA dejado de entregar: {Math.Round(emergencia, 2)} MMPCSD";

            var dto = new BoletadeValorizacionPetroperuDto
            {
                NombreReporte = operacionGeneral?.Resultado?.NombreReporte,
                VersionReporte = operacionGeneral?.Resultado?.Version,
                CompaniaReporte = operacionGeneral?.Resultado?.Nombre,
                Periodo = $"{mes}-{desde.Year}",
                RutaFirma = operacionGeneral?.Resultado?.RutaFirma,
                UrlFirma = operacionGeneral?.Resultado?.UrlFirma,

                Eficiencia = boletadeValorizacionPetroperu.Average(e => e.Eficiencia),
                CostoUnitMaquilaUsMmbtu = boletadeValorizacionPetroperu.Average(e => e.CostoUnitMaquilaUsMmbtu),
                GnaLoteI = boletadeValorizacionPetroperu.Sum(e => e.GnaLoteI),
                EnergiaLoteI = boletadeValorizacionPetroperu.Sum(e => e.EnergiaLoteI),
                LgnRecuperadosLoteI = boletadeValorizacionPetroperu.Sum(e => e.LgnRecuperadosLoteI),
                GnaLoteVi = boletadeValorizacionPetroperu.Sum(e => e.GnaLoteVi),
                EnergiaLoteVi = boletadeValorizacionPetroperu.Sum(e => e.EnergiaLoteVi),
                LgnRecuperadosLoteVi = boletadeValorizacionPetroperu.Sum(e => e.LgnRecuperadosLoteVi),
                GnaLoteZ69 = boletadeValorizacionPetroperu.Sum(e => e.GnaLoteZ69),
                EnergiaLoteZ69 = boletadeValorizacionPetroperu.Sum(e => e.EnergiaLoteZ69),
                LgnRecuperadosLoteZ69 = boletadeValorizacionPetroperu.Sum(e => e.LgnRecuperadosLoteZ69),
                TotalGna = boletadeValorizacionPetroperu.Sum(e => e.TotalGna),
                LiquidosRecuperados = boletadeValorizacionPetroperu.Sum(e => e.TotalLiquidosRecuperados),
                GnsLoteI = boletadeValorizacionPetroperu.Sum(e => e.GnsLoteI),
                GnsLoteVi = boletadeValorizacionPetroperu.Sum(e => e.GnsLoteVi),
                GnsLoteZ69 = boletadeValorizacionPetroperu.Sum(e => e.GnsLoteZ69),
                GnsTotal = boletadeValorizacionPetroperu.Sum(e => e.GnsTotal),
                EnergiaMmbtu = boletadeValorizacionPetroperu.Sum(e => e.EnergiaMmbtu),
                ValorLiquidosUs = boletadeValorizacionPetroperu.Sum(e => e.ValorLiquidosUs),
                CostoMaquilaUs = boletadeValorizacionPetroperu.Sum(e => e.CostoMaquilaUs),
                MontoFacturarPetroperu = boletadeValorizacionPetroperu.Sum(e => e.ValorLiquidosUs),
                MontoFacturarUnna = boletadeValorizacionPetroperu.Sum(e => e.CostoMaquilaUs),
                DensidadGlp = densidadGlp,
                EnergiaMmbtuLoteI = boletadeValorizacionPetroperu.Sum(e => e.EnergiaMmbtuLoteI),
                EnergiaMmbtuLoteVi = boletadeValorizacionPetroperu.Sum(e => e.EnergiaMmbtuLoteVi),
                EnergiaMmbtuLoteZ69 = boletadeValorizacionPetroperu.Sum(e => e.EnergiaMmbtuLoteZ69),
                ValorLiquidosLoteI = Math.Round(boletadeValorizacionPetroperu.Sum(e => e.ValorLiquidosLotI), 2),
                ValorLiquidosLoteVi = Math.Round(boletadeValorizacionPetroperu.Sum(e => e.ValorLiquidosLotVi), 2),
                ValorLiquidosLoteZ69 = Math.Round(boletadeValorizacionPetroperu.Sum(e => e.ValorLiquidosLotZ69), 2),
                CostoMaquillaLoteI = Math.Round(boletadeValorizacionPetroperu.Sum(e => e.CostoMaquilaUsLotI), 2),
                CostoMaquillaLoteVi = Math.Round(boletadeValorizacionPetroperu.Sum(e => e.CostoMaquilaUsLotVi), 2),
                CostoMaquillaLoteZ69 = Math.Round(boletadeValorizacionPetroperu.Sum(e => e.CostoMaquilaUsLotZ69), 2),
                MontoFacturarLoteI = Math.Round(boletadeValorizacionPetroperu.Sum(e => e.CostoMaquilaUsLotI), 2),
                MontoFacturarLoteVi = Math.Round(boletadeValorizacionPetroperu.Sum(e => e.CostoMaquilaUsLotI), 2),
                MontoFacturarLoteZ69 = Math.Round(boletadeValorizacionPetroperu.Sum(e => e.CostoMaquilaUsLotI), 2),
                Observacion = observacion
            };

            for (int i = 0; i < boletadeValorizacionPetroperu.Count;i++)
            {
                boletadeValorizacionPetroperu[i].GnaLoteI = Math.Round(boletadeValorizacionPetroperu[i].GnaLoteI, 2);
                boletadeValorizacionPetroperu[i].PoderCalorificoLoteI = Math.Round(boletadeValorizacionPetroperu[i].PoderCalorificoLoteI, 2);
                boletadeValorizacionPetroperu[i].EnergiaLoteI = Math.Round(boletadeValorizacionPetroperu[i].EnergiaLoteI, 4);
                boletadeValorizacionPetroperu[i].RiquezaLoteI = Math.Round(boletadeValorizacionPetroperu[i].RiquezaLoteI, 4);
                boletadeValorizacionPetroperu[i].RiquezaBlLoteI = Math.Round(boletadeValorizacionPetroperu[i].RiquezaBlLoteI, 2);
                boletadeValorizacionPetroperu[i].LgnRecuperadosLoteI = Math.Round(boletadeValorizacionPetroperu[i].LgnRecuperadosLoteI, 2);
                boletadeValorizacionPetroperu[i].GnaLoteVi = Math.Round(boletadeValorizacionPetroperu[i].GnaLoteVi, 2);
                boletadeValorizacionPetroperu[i].PoderCalorificoLoteVi = Math.Round(boletadeValorizacionPetroperu[i].PoderCalorificoLoteVi, 2);
                boletadeValorizacionPetroperu[i].EnergiaLoteVi = Math.Round(boletadeValorizacionPetroperu[i].EnergiaLoteVi, 2);
                boletadeValorizacionPetroperu[i].RiquezaLoteVi = Math.Round(boletadeValorizacionPetroperu[i].RiquezaLoteVi, 4);

                boletadeValorizacionPetroperu[i].RiquezaBlLoteVi = Math.Round(boletadeValorizacionPetroperu[i].RiquezaBlLoteVi, 2);
                boletadeValorizacionPetroperu[i].LgnRecuperadosLoteVi = Math.Round(boletadeValorizacionPetroperu[i].LgnRecuperadosLoteVi, 2);
                boletadeValorizacionPetroperu[i].GnaLoteZ69 = Math.Round(boletadeValorizacionPetroperu[i].GnaLoteZ69, 2);
                boletadeValorizacionPetroperu[i].PoderCalorificoLoteZ69 = Math.Round(boletadeValorizacionPetroperu[i].PoderCalorificoLoteZ69, 2);
                boletadeValorizacionPetroperu[i].EnergiaLoteZ69 = Math.Round(boletadeValorizacionPetroperu[i].EnergiaLoteZ69, 2);
                boletadeValorizacionPetroperu[i].RiquezaLoteZ69 = Math.Round(boletadeValorizacionPetroperu[i].RiquezaLoteZ69, 4);

                boletadeValorizacionPetroperu[i].RiquezaBlLoteZ69 = Math.Round(boletadeValorizacionPetroperu[i].RiquezaBlLoteZ69, 2);
                boletadeValorizacionPetroperu[i].LgnRecuperadosLoteZ69 = Math.Round(boletadeValorizacionPetroperu[i].LgnRecuperadosLoteZ69, 2);
                boletadeValorizacionPetroperu[i].TotalGna = Math.Round(boletadeValorizacionPetroperu[i].TotalGna, 2);
                boletadeValorizacionPetroperu[i].Eficiencia = Math.Round(boletadeValorizacionPetroperu[i].Eficiencia, 2);

                boletadeValorizacionPetroperu[i].TotalLiquidosRecuperados = Math.Round(boletadeValorizacionPetroperu[i].TotalLiquidosRecuperados, 2);
                boletadeValorizacionPetroperu[i].GnsLoteI = Math.Round(boletadeValorizacionPetroperu[i].GnsLoteI, 2);
                boletadeValorizacionPetroperu[i].GnsLoteVi = Math.Round(boletadeValorizacionPetroperu[i].GnsLoteVi, 2);
                boletadeValorizacionPetroperu[i].GnsLoteZ69 = Math.Round(boletadeValorizacionPetroperu[i].GnsLoteZ69, 2);
                boletadeValorizacionPetroperu[i].GnsTotal = Math.Round(boletadeValorizacionPetroperu[i].GnsTotal, 2);
                boletadeValorizacionPetroperu[i].PoderCalorificoBtuPcsd = Math.Round(boletadeValorizacionPetroperu[i].PoderCalorificoBtuPcsd, 2);
                boletadeValorizacionPetroperu[i].EnergiaMmbtu = Math.Round(boletadeValorizacionPetroperu[i].EnergiaMmbtu, 2);
                boletadeValorizacionPetroperu[i].RiquezaLoteI = Math.Round(boletadeValorizacionPetroperu[i].RiquezaLoteI, 2);
                boletadeValorizacionPetroperu[i].PrecioGlpESinIgvSolesKg = Math.Round(boletadeValorizacionPetroperu[i].PrecioGlpESinIgvSolesKg, 4);
                boletadeValorizacionPetroperu[i].PrecioGlpGSinIGVSolesKg = Math.Round(boletadeValorizacionPetroperu[i].PrecioGlpGSinIGVSolesKg, 4);
                boletadeValorizacionPetroperu[i].PrecioRefGlpSinIgvSolesKg = Math.Round(boletadeValorizacionPetroperu[i].PrecioRefGlpSinIgvSolesKg, 4);
                boletadeValorizacionPetroperu[i].TipodeCambioSolesUs = Math.Round(boletadeValorizacionPetroperu[i].TipodeCambioSolesUs, 2);
                boletadeValorizacionPetroperu[i].PrecioCgnUsBl = Math.Round(boletadeValorizacionPetroperu[i].PrecioCgnUsBl, 2);
                boletadeValorizacionPetroperu[i].ValorLiquidosUs = Math.Round(boletadeValorizacionPetroperu[i].ValorLiquidosUs, 4);
                boletadeValorizacionPetroperu[i].CostoUnitMaquilaUsMmbtu = Math.Round(boletadeValorizacionPetroperu[i].CostoUnitMaquilaUsMmbtu, 2);
            }
            dto.BoletadeValorizacionPetroperu = boletadeValorizacionPetroperu;
            return new OperacionDto<BoletadeValorizacionPetroperuDto>(dto);
        }



        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletadeValorizacionPetroperuDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddMonths(-1);

            DateTime fecha = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);

            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletadeValorizacionPetroperu),
                Fecha = fecha,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                Comentario = peticion.Observacion,
                EsEditado = true,
            };

            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(36,3);
            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(46,3);
            return await _impresionServicio.GuardarAsync(dto);

        }
    }
}
