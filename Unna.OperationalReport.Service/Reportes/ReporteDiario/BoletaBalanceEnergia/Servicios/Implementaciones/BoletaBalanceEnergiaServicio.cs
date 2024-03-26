using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Win32;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Procedimientos;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Implementaciones
{
    public class BoletaBalanceEnergiaServicio : IBoletaBalanceEnergiaServicio
    {

        private readonly IReporteServicio _reporteServicio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IGnsVolumeMsYPcBrutoRepositorio _gnsVolumeMsYPcBrutoRepositorio;
        private readonly IFiscalizacionProductosServicio _fiscalizacionProductosServicio;
        private readonly IFiscalizacionPetroPeruServicio _fiscalizacionPetroPeruServicio;
        public BoletaBalanceEnergiaServicio(
            IReporteServicio reporteServicio,
            IImpresionServicio impresionServicio,
            IRegistroRepositorio registroRepositorio,
            IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio,
            IFiscalizacionProductosServicio fiscalizacionProductosServicio,
            IFiscalizacionPetroPeruServicio fiscalizacionPetroPeruServicio
            )
        {
            _reporteServicio = reporteServicio;
            _impresionServicio = impresionServicio;
            _registroRepositorio = registroRepositorio;
            _gnsVolumeMsYPcBrutoRepositorio = gnsVolumeMsYPcBrutoRepositorio;
            _fiscalizacionProductosServicio = fiscalizacionProductosServicio;
            _fiscalizacionPetroPeruServicio = fiscalizacionPetroPeruServicio;
        }

        public async Task<OperacionDto<BoletaBalanceEnergiaDto>> ObtenerAsync(long idUsuario)
        {
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaBalanceEnergiaDiaria, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletaBalanceEnergiaDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaBalanceEnergiaDiaria, FechasUtilitario.ObtenerDiaOperativo());
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<BoletaBalanceEnergiaDto>(operacionImpresion.Resultado.Datos);
                rpta.General = operacionGeneral.Resultado;
                return new OperacionDto<BoletaBalanceEnergiaDto>(rpta);
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            var dto = new BoletaBalanceEnergiaDto
            {
                Fecha = diaOperativo.ToString("dd/MM/yyyy"),
                General = operacionGeneral.Resultado
            };

            //GNA Entregado a UNNA ENERGIA Gas Natural
            var registrosDatos = await _registroRepositorio.ListarDatosPorFechaAsync(diaOperativo);

            var cnpc = registrosDatos.Where(e => e.IdLote == (int)TiposLote.LoteX).FirstOrDefault();

            var gnaEntregaUnna = new GnaEntregaAUnnaDto
            {
                Entrega = "CNPC PERU",
                Volumen = cnpc != null ? cnpc.VolRenominado : 0,
                PoderCalorifico = cnpc != null ? cnpc.Calorifico : 0,
                Riqueza = cnpc != null ? cnpc.Riqueza : 0,
            };
            gnaEntregaUnna.Energia = Math.Round(gnaEntregaUnna.Energia ?? 0 * gnaEntregaUnna.PoderCalorifico ?? 0, 2);
            dto.GnaEntregaUnna = gnaEntregaUnna;
            #region LIQUIDOS Y BARRILES
            double liquidoGlp = 0;
            double liquidoCgn = 0;
            var operacionProductos = await _fiscalizacionProductosServicio.ObtenerAsync(idUsuario);
            if (operacionProductos.Completado && operacionProductos.Resultado != null && operacionProductos.Resultado.ProductoGlpCgn != null)
            {
                var glp = operacionProductos.Resultado.ProductoGlpCgn.Where(e => e.Equals("GLP")).FirstOrDefault();
                liquidoGlp = glp != null ? glp.Produccion ?? 0 : 0;
                var cgn = operacionProductos.Resultado.ProductoGlpCgn.Where(e => e.Equals("CGN")).FirstOrDefault();
                liquidoCgn = cgn != null ? cgn.Produccion ?? 0 : 0;

            }
            var liquidosBarriles = new List<LiquidosBarrilesDto>();
            liquidosBarriles.Add(new LiquidosBarrilesDto
            {
                Nombre = "Com. Pesados ó LGN  Recuperados",
                Blsd = liquidoCgn + liquidoGlp,
                Enel = 0,// Falta el valor
            });
            liquidosBarriles.Add(new LiquidosBarrilesDto
            {
                Nombre = "Producción de GLP",
                Blsd = liquidoGlp,
                Enel = 0,// Falta el valor
            });
            liquidosBarriles.Add(new LiquidosBarrilesDto
            {
                Nombre = "Producción de CGN",
                Blsd = liquidoCgn,
                Enel = 0,// Falta el valor
            });

            dto.LiquidosBarriles = liquidosBarriles;

            #endregion

            #region EFICIENCIA DE RECUPERACION DE LGN


            if (cnpc != null)
            {
                dto.ComPesadosGna = Math.Round(cnpc.Riqueza * cnpc.VolRenominado / 42,2);
            }
            var operacionPetroperu = await _fiscalizacionPetroPeruServicio.ObtenerAsync(idUsuario);
            if (operacionPetroperu.Completado && operacionPetroperu.Resultado != null)
            {
                dto.PorcentajeEficiencia = operacionPetroperu.Resultado.Eficiencia;
            }
            dto.ContenidoCalorificoPromLgn = 4.311; //  Valor es fijo
            #endregion

            //GNS  a ENEL - Distribución, GNS a ENEL
            double gnsEnelPcBruto = 0;
            var entidadTotalGns = await _gnsVolumeMsYPcBrutoRepositorio.ObtenerPorTipoYNombreDiaOperativoAsync(TiposTablasSupervisorPgt.VolumenMsGnsAgpsa, TiposGnsVolumeMsYPcBruto.GnsAEgpsa, diaOperativo);
            if (entidadTotalGns != null)
            {
                gnsEnelPcBruto = entidadTotalGns.PcBrutoRepCroma ?? 0;
            }
            dto.GnsAEnel = await GnsAEnelAsync(diaOperativo, gnsEnelPcBruto, dto.GnaEntregaUnna?.PoderCalorifico);

            // Consumo Propio UNNA ENERGIA
            var consumoPropio = new List<DistribucionVolumenPorderCalorificoDto>();
            consumoPropio.Add( new DistribucionVolumenPorderCalorificoDto{
                Item = 1,
                Distribucion = "Gas a Horno HOT OIL",
                Volumen = 0,// Falta calcular
                PoderCalorifico = gnsEnelPcBruto,
                Energia = Math.Round(0 * gnsEnelPcBruto / 1000, 2)//1000 es un valor fijo
            });
            consumoPropio.Add(new DistribucionVolumenPorderCalorificoDto
            {
                Item = 2,
                Distribucion = "Total",
                Volumen = consumoPropio.Sum(e => e.Volumen),
                PoderCalorifico = consumoPropio.Sum(e => e.Volumen) <= 0 ? 0 : (consumoPropio.Sum(e => e.VolumenPorderCalorifico) / consumoPropio.Sum(e => e.Volumen)),
                Energia = consumoPropio.Sum(e => e.Energia),
            });

            dto.ConsumoPropio = consumoPropio;

            double volumeMs = 0;
            if (entidadTotalGns != null)
            {
                volumeMs = entidadTotalGns.VolumeMs ?? 0;
            }
            // GNS Vendido a ENEL
            var consumoPropioGnsVendioEnel = new List<DistribucionVolumenPorderCalorificoDto>();
            consumoPropioGnsVendioEnel.Add(new DistribucionVolumenPorderCalorificoDto
            {
                Item = 1,
                Distribucion = "GNS Vendido Lote IV",
                Volumen = volumeMs,
                PoderCalorifico = gnsEnelPcBruto,
                Energia = Math.Round(volumeMs * gnsEnelPcBruto / 1000, 2)//1000 es un valor fijo
            });
            consumoPropioGnsVendioEnel.Add(new DistribucionVolumenPorderCalorificoDto
            {
                Item = 2,
                Distribucion = "Total",
                Volumen = consumoPropioGnsVendioEnel.Sum(e => e.Volumen),
                PoderCalorifico = consumoPropioGnsVendioEnel.Sum(e => e.Volumen) <= 0 ? 0 : (consumoPropioGnsVendioEnel.Sum(e => e.VolumenPorderCalorifico) / consumoPropioGnsVendioEnel.Sum(e => e.Volumen)),
                Energia = consumoPropioGnsVendioEnel.Sum(e => e.Energia),
            });

            dto.ConsumoPropioGnsVendioEnel = consumoPropioGnsVendioEnel;


            //BALANCE

            // Entregas de GNA
            dto.EntregaGna = new BalanceDto
            {
                Balance = "Entregas de GNA",
                Mpcsd = dto.GnaEntregaUnna != null ? dto.GnaEntregaUnna.Volumen : 0,
                Energia = dto.GnaEntregaUnna != null ? dto.GnaEntregaUnna.Energia : 0,
            };
            // GNS Restituido a ENEL
            var gnsRestituido = dto.GnsAEnel?.Where(e => e.Distribucion.Equals("Total")).FirstOrDefault();
            dto.GnsRestituido = new BalanceDto
            {
                Balance = "GNS Restituido a ENEL",
                Mpcsd = gnsRestituido != null ? gnsRestituido.Volumen : 0,
                Energia = gnsRestituido != null ? gnsRestituido.Energia : 0,
            };
            // GNS Consumo Propio UNNA
            var gnsConsumoPropio = dto.ConsumoPropio?.Where(e => e.Distribucion.Equals("Total")).FirstOrDefault();
            dto.GnsConsumoPropio = new BalanceDto
            {
                Balance = "GNS Consumo Propio UNNA",
                Mpcsd = gnsConsumoPropio != null ? gnsConsumoPropio.Volumen : 0,
                Energia = gnsConsumoPropio != null ? gnsConsumoPropio.Energia : 0,
            };

            // Recuperación de  Com. Pes.
            var barrilesLgn = dto.LiquidosBarriles?.Where(e => e.Nombre.Equals("Com. Pesados ó LGN  Recuperados")).FirstOrDefault();
            dto.Recuperacion = new BalanceDto
            {
                Balance = "Recuperación de  Com. Pes.",
                Barriles = barrilesLgn != null ? barrilesLgn.Enel : 0,
                Energia = (barrilesLgn != null ? barrilesLgn.Enel : 0) * dto.ContenidoCalorificoPromLgn
            };

            // BALANCE
            double diferenciaEnergetica = (dto.EntregaGna?.Energia ?? 0 - dto.GnsRestituido?.Energia ?? 0 - dto.GnsConsumoPropio?.Energia ?? 0 - dto.Recuperacion?.Energia ?? 0);
            dto.DiferenciaEnergetica = diferenciaEnergetica < 0 ? 0 : diferenciaEnergetica;

            return new OperacionDto<BoletaBalanceEnergiaDto>(dto);
        }

        private async Task<List<DistribucionVolumenPorderCalorificoDto>> GnsAEnelAsync(DateTime diaOperativo, double gnsEnelPcBruto, double? poderCalorifico)
        {

            double valorVolumenGna = 1000;
            var registro = await _registroRepositorio.ObtenerValorAsync((int)TiposDatos.GnsTotalConsumoEnel, (int)TiposLote.LoteX, diaOperativo, (int)TiposNumeroRegistro.SegundoRegistro);


            var gnsAEnel = new List<DistribucionVolumenPorderCalorificoDto>();
            gnsAEnel.Add(new DistribucionVolumenPorderCalorificoDto
            {
                Item = 1,
                Distribucion = "Pñs. a Malacas (Ducto N° 3 )",
                Volumen = registro != null ? registro.Valor ?? 0 : 0,
                PoderCalorifico = gnsEnelPcBruto,
                Energia = Math.Round((registro != null ? registro.Valor ?? 0 : 0) * gnsEnelPcBruto / 1000, 2)//1000 es un valor fijo
            });

            gnsAEnel.Add(new DistribucionVolumenPorderCalorificoDto
            {
                Item = 2,
                Distribucion = "Pñs. a Refinería",
                Volumen = 0,//  Es un valor fijo
                PoderCalorifico = gnsEnelPcBruto,
                Energia = Math.Round(0 * gnsEnelPcBruto / valorVolumenGna, 2)
            });
            gnsAEnel.Add(new DistribucionVolumenPorderCalorificoDto
            {
                Item = 3,
                Distribucion = "Ajuste Balance GNS",
                Volumen = 0,//  Es un valor fijo
                PoderCalorifico = gnsEnelPcBruto,
                Energia = Math.Round(0 * gnsEnelPcBruto / valorVolumenGna, 2)
            });
            gnsAEnel.Add(new DistribucionVolumenPorderCalorificoDto
            {
                Item = 4,
                Distribucion = "Humedad en GNA",
                Volumen = 0,//  falta el valor
                PoderCalorifico = poderCalorifico,
                Energia = Math.Round(0 * poderCalorifico ?? 0 / valorVolumenGna, 2)
            });
            gnsAEnel.Add(new DistribucionVolumenPorderCalorificoDto
            {
                Item = 5,
                Distribucion = "Gas a Flare 1 Pñs. 1",
                Volumen = 0,//  falta el valor
                PoderCalorifico = gnsEnelPcBruto,
                Energia = Math.Round(0 * gnsEnelPcBruto / valorVolumenGna, 2)
            });
            gnsAEnel.Add(new DistribucionVolumenPorderCalorificoDto
            {
                Item = 6,
                Distribucion = "Total",
                Volumen = gnsAEnel.Sum(e => e.Volumen),
                PoderCalorifico = gnsAEnel.Sum(e => e.VolumenPorderCalorifico) / gnsAEnel.Sum(e => e.Volumen),
                Energia = gnsAEnel.Sum(e => e.Energia)
            });

            return gnsAEnel;
        }

        //public async Task<List<ListarValoresRegistrosPorFecha>> ListarDatosPorFechaAsync(DateTime diaOperativo)
        //{



        //}


    }
}
