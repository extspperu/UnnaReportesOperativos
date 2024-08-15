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
using Unna.OperationalReport.Data.Reporte.Procedimientos;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
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
        private readonly IBoletaEnelRepositorio _boletaEnelRepositorio;
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;

        public BoletaBalanceEnergiaServicio(
            IReporteServicio reporteServicio,
            IImpresionServicio impresionServicio,
            IRegistroRepositorio registroRepositorio,
            IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio,
            IFiscalizacionProductosServicio fiscalizacionProductosServicio,
            IFiscalizacionPetroPeruServicio fiscalizacionPetroPeruServicio,
            IBoletaEnelRepositorio boletaEnelRepositorio,
            IImprimirRepositorio imprimirRepositorio,
            ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio
            )
        {
            _reporteServicio = reporteServicio;
            _impresionServicio = impresionServicio;
            _registroRepositorio = registroRepositorio;
            _gnsVolumeMsYPcBrutoRepositorio = gnsVolumeMsYPcBrutoRepositorio;
            _fiscalizacionProductosServicio = fiscalizacionProductosServicio;
            _fiscalizacionPetroPeruServicio = fiscalizacionPetroPeruServicio;
            _boletaEnelRepositorio = boletaEnelRepositorio;
            _imprimirRepositorio = imprimirRepositorio;
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
        }

        public async Task<OperacionDto<BoletaBalanceEnergiaDto>> ObtenerAsync(long idUsuario)
        {
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaBalanceEnergiaDiaria, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletaBalanceEnergiaDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaBalanceEnergiaDiaria, FechasUtilitario.ObtenerDiaOperativo());
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos) && operacionImpresion.Resultado.EsEditado)
            {
                var rpta = JsonConvert.DeserializeObject<BoletaBalanceEnergiaDto>(operacionImpresion.Resultado.Datos);
                rpta.General = operacionGeneral.Resultado;
                return new OperacionDto<BoletaBalanceEnergiaDto>(rpta);
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            var dto = new BoletaBalanceEnergiaDto
            {
                Fecha = diaOperativo.ToString("dd/MM/yyyy"),
                General = operacionGeneral.Resultado,
                IdUsuario = idUsuario
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
            gnaEntregaUnna.Energia = Math.Round(gnaEntregaUnna.Volumen * gnaEntregaUnna.PoderCalorifico / 1000, 2);//1000 es un número fijo
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

            var liquidosBarrilesEntidad = await _boletaEnelRepositorio.ListarLiquidosBarrilesAsync(diaOperativo);
            var liquidosBarriles = liquidosBarrilesEntidad.Select(e => new LiquidosBarrilesDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Blsd = e.BlsdTotal,
                Enel = Math.Round(e.Enel, 2),
            }).ToList();

            dto.LiquidosBarriles = liquidosBarriles;

            #endregion

            #region EFICIENCIA DE RECUPERACION DE LGN


            if (cnpc != null)
            {
                dto.ComPesadosGna = Math.Round(cnpc.Riqueza * cnpc.VolRenominado / 42, 2);
            }
            //var horaplantafs = await _imprimirRepositorio.ObtenerHoraPlantaFsAsync(2, diaOperativo);
            //double valorhoraPlantafs = 0;
            //if (horaplantafs.Count != 0)
            //{
            //    if (horaplantafs[0].HoraPlantaFs.HasValue)
            //    {
            //        valorhoraPlantafs = (double)horaplantafs[0].HoraPlantaFs;
            //    }
            //}
            //else { valorhoraPlantafs = 0; }

            //var pgtVolumenEntidad = await _boletaEnelRepositorio.ObtenerPgtVolumen(diaOperativo);
            var pgtVolumenEntidad = await _registroRepositorio.ObtenerValorAsync((int)TiposDatos.EficienciaProduccion, (int)TiposLote.LoteX, diaOperativo, (int)TiposNumeroRegistro.SegundoRegistro);
            if (pgtVolumenEntidad != null)
            {
                dto.PorcentajeEficiencia = pgtVolumenEntidad.Valor.HasValue ? pgtVolumenEntidad.Valor.Value : null;
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
            var gnsAEnelEntidad = await _boletaEnelRepositorio.ObtenerGnsAEnelAsync(diaOperativo);

            dto.GnsAEnel = GnsAEnelAsync(gnsAEnelEntidad.Where(e => e.Nombre.Equals("GNS a ENEL")).ToList());



            dto.ConsumoPropio = GnsAEnelAsync(gnsAEnelEntidad.Where(e => e.Nombre.Equals("Consumo Propio UNNA ENERGIA")).ToList());
            double energiaHumedadGna = 0;
            var humedadEnergia = dto.ConsumoPropio.Where(e => e.Item == 4).FirstOrDefault();
            if (humedadEnergia != null)
            {
                energiaHumedadGna = humedadEnergia.Energia;
            }
            dto.ConsumoPropioGnsVendioEnel = ConsumoPropioGnsVendioEnelsync(gnsAEnelEntidad.Where(e => e.Nombre.Equals("GNS Vendido a ENEL")).ToList(), energiaHumedadGna);


            //BALANCE

            // Entregas de GNA
            dto.EntregaGna = new BalanceDto
            {
                Balance = "Entregas de GNA",
                Mpcsd = dto.GnaEntregaUnna != null ? dto.GnaEntregaUnna.Volumen : 0,
                Energia = dto.GnaEntregaUnna != null ? Math.Round(dto.GnaEntregaUnna.Energia, 0) : 0,
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
                Barriles = Math.Round((barrilesLgn != null ? barrilesLgn.Enel : 0), 0),
            };
            if (dto.Recuperacion.Barriles.HasValue)
            {
                dto.Recuperacion.Energia = Math.Round((barrilesLgn != null ? barrilesLgn.Enel : 0) * dto.ContenidoCalorificoPromLgn, 0);//Math.Round(dto.Recuperacion.Barriles.Value * dto.ContenidoCalorificoPromLgn, 0);
            }

            // BALANCE
            if (dto.GnsRestituido != null && dto.GnsConsumoPropio != null && dto.Recuperacion != null)
            {
                double diferenciaEnergetica = (dto.EntregaGna.Energia - dto.GnsRestituido.Energia - dto.GnsConsumoPropio.Energia - dto.Recuperacion.Energia);
                dto.DiferenciaEnergetica = diferenciaEnergetica < 0 ? 0 : diferenciaEnergetica;

            }

            if (dto.GnsConsumoPropio != null && dto.GnaEntregaUnna != null)
            {
                double diferenciaExesoConsumoPropio = Math.Round((dto.GnsConsumoPropio.Energia - dto.GnaEntregaUnna.Energia * 0.035), 0);
                dto.ExesoConsumoPropio = diferenciaExesoConsumoPropio < 0 ? 0 : diferenciaExesoConsumoPropio;
            }

            await GuardarAsync(dto, false);

            return new OperacionDto<BoletaBalanceEnergiaDto>(dto);
        }

        private List<DistribucionVolumenPorderCalorificoDto> GnsAEnelAsync(List<ObtenerGnsAEnel> gnsAEnelEntidad)
        {
            var gnsAEnel = gnsAEnelEntidad.Select(e => new DistribucionVolumenPorderCalorificoDto
            {
                Item = e.Id,
                Distribucion = e.Distribucion,
                Nombre = e.Nombre,
                PoderCalorifico = e.PoderCalorifico,
                Volumen = e.Volumen,
                Energia = (e.Volumen * e.PoderCalorifico / 1000)
            }).ToList();

            var totalFlare = gnsAEnel.Where(e => e.Item == 5).FirstOrDefault();
            if (totalFlare != null)
            {
                gnsAEnel.Where(e => e.Item == 4).ToList().ForEach(e => e.Energia = totalFlare.Energia);
            }
            gnsAEnel.Where(e => e.Item == 5).ToList().ForEach(e => e.Energia = Math.Floor(e.Energia));
            gnsAEnel.ForEach(e => e.Energia = Math.Round(e.Energia, MidpointRounding.AwayFromZero));
            gnsAEnel.Add(new DistribucionVolumenPorderCalorificoDto
            {
                Item = (gnsAEnel.Count + 1),
                Distribucion = "Total",
                Volumen = Math.Round(gnsAEnel.Sum(e => e.Volumen), 0),
                PoderCalorifico = Math.Round((gnsAEnel.Sum(e => e.VolumenPorderCalorifico) / gnsAEnel.Sum(e => e.Volumen)), 2),
                Energia = gnsAEnel.Sum(e => e.Energia)
            });

            return gnsAEnel;
        }

        private List<DistribucionVolumenPorderCalorificoDto> ConsumoPropioGnsVendioEnelsync(List<ObtenerGnsAEnel> gnsAEnelEntidad, double energiaHumedadGna)
        {
            var gnsAEnel = gnsAEnelEntidad.Select(e => new DistribucionVolumenPorderCalorificoDto
            {
                Item = e.Id,
                Distribucion = e.Distribucion,
                Nombre = e.Nombre,
                PoderCalorifico = e.PoderCalorifico,
                Volumen = e.Volumen,
                Energia = Math.Round(e.Volumen * e.PoderCalorifico / 1000, 0)
            }).ToList();

            gnsAEnel.Add(new DistribucionVolumenPorderCalorificoDto
            {
                Item = (gnsAEnel.Count + 1),
                Distribucion = "Total",
                Volumen = Math.Round(gnsAEnel.Sum(e => e.Volumen), 0),
                PoderCalorifico = energiaHumedadGna,
                Energia = gnsAEnel.Sum(e => e.Energia)
            });
            return gnsAEnel;
        }


        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletaBalanceEnergiaDto peticion, bool esEditado)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletaBalanceEnergiaDiaria),
                Fecha = FechasUtilitario.ObtenerDiaOperativo(),
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                EsEditado = esEditado
            };

            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(8,1);
            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(18, 1);

            return await _impresionServicio.GuardarAsync(dto);
        }

    }
}
