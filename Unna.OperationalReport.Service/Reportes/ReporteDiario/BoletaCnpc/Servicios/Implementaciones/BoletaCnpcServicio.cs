using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Drawing.Printing;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Fuentes.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Fuentes.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Procedimientos;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Implementaciones
{
    public class BoletaCnpcServicio : IBoletaCnpcServicio
    {



        private readonly IDiaOperativoRepositorio _diaOperativoRepositorio;
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IBoletaCnpcVolumenComposicionGnaEntradaRepositorio _boletaCnpcVolumenComposicionGnaEntradaRepositorio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IGnsVolumeMsYPcBrutoRepositorio _gnsVolumeMsYPcBrutoRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IBoletaBalanceEnergiaServicio _boletaBalanceEnergiaServicio;
        private readonly IFiscalizacionProductosServicio _fiscalizacionProductosServicio;
        private readonly IBoletaCnpcRepositorio _boletaCnpcRepositorio;
        public BoletaCnpcServicio(
            IDiaOperativoRepositorio diaOperativoRepositorio,
            IRegistroRepositorio registroRepositorio,
            IBoletaCnpcVolumenComposicionGnaEntradaRepositorio boletaCnpcVolumenComposicionGnaEntradaRepositorio,
            IReporteServicio reporteServicio,
            IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio,
            IImpresionServicio impresionServicio,
            IBoletaBalanceEnergiaServicio boletaBalanceEnergiaServicio,
            IFiscalizacionProductosServicio fiscalizacionProductosServicio,
            IBoletaCnpcRepositorio boletaCnpcRepositorio
            )
        {
            _diaOperativoRepositorio = diaOperativoRepositorio;
            _registroRepositorio = registroRepositorio;
            _boletaCnpcVolumenComposicionGnaEntradaRepositorio = boletaCnpcVolumenComposicionGnaEntradaRepositorio;
            _reporteServicio = reporteServicio;
            _gnsVolumeMsYPcBrutoRepositorio = gnsVolumeMsYPcBrutoRepositorio;
            _impresionServicio = impresionServicio;
            _boletaBalanceEnergiaServicio = boletaBalanceEnergiaServicio;
            _fiscalizacionProductosServicio = fiscalizacionProductosServicio;
            _boletaCnpcRepositorio = boletaCnpcRepositorio;
        }

        public async Task<OperacionDto<BoletaCnpcDto>> ObtenerAsync(long idUsuario)
        {

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaCnpc, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletaCnpcDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaCnpc, FechasUtilitario.ObtenerDiaOperativo());
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos) && operacionImpresion.Resultado.EsEditado)
            {
                var rpta = JsonConvert.DeserializeObject<BoletaCnpcDto>(operacionImpresion.Resultado.Datos);
                rpta.General = operacionGeneral.Resultado;
                return new OperacionDto<BoletaCnpcDto>(rpta);
            }


            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            BoletaCnpcTabla1Dto tabla1 = new BoletaCnpcTabla1Dto();


            var dto = new BoletaCnpcDto
            {
                Fecha = diaOperativo.ToString("dd/MM/yyyy")
            };

            dto.General = operacionGeneral.Resultado;

            // tabla N° 01
            tabla1.Fecha = dto.Fecha;
            var entidadLotes = await _registroRepositorio.BoletaCnpcFactoresDistribucionDeGasCombustibleAsync(diaOperativo);


            var entidadGasMpcd = entidadLotes.Where(e => e.IdLote == 6).FirstOrDefault();
            if (entidadGasMpcd != null)
            {
                tabla1.GasMpcd = entidadGasMpcd.Volumen;
            }

            tabla1.GlpBls = 0;
            tabla1.CgnBls = 0;
            dto.Tabla1 = tabla1;


            //Cuadro N° 1. Fiscalización de GNS del GAS Adicional del Lote X
            var volumenTotalGns = await _gnsVolumeMsYPcBrutoRepositorio.ObtenerPorTipoYNombreDiaOperativoAsync(TiposTablasSupervisorPgt.VolumenMsGnsAgpsa, TiposGnsVolumeMsYPcBruto.GnsAEgpsa, diaOperativo);
            if (volumenTotalGns != null)
            {
                dto.VolumenTotalGnsEnMs = volumenTotalGns.VolumeMs ?? 0;
            }

            var boletaBalance = new BoletaBalanceEnergiaDto();
            var operacionBoletaBalance = await _boletaBalanceEnergiaServicio.ObtenerAsync(idUsuario);
            if (operacionBoletaBalance.Completado)
            {
                boletaBalance = operacionBoletaBalance.Resultado;
            }

            var flareGnaConsumo = boletaBalance?.GnsAEnel?.Where(e => e.Item == 5).FirstOrDefault();
            if (flareGnaConsumo != null)
            {
                dto.FlareGna = flareGnaConsumo.Volumen;
            }
            dto.VolumenTotalGns = dto.VolumenTotalGnsEnMs + dto.FlareGna;


            dto.FactoresDistribucionGasNaturalSeco = FactoresDistribucionGasNaturalSeco(entidadLotes, dto.VolumenTotalGnsEnMs);


            #region Cuadro N° 2. Asignación de Gas Combustible al GNA Adicional del Lote X
            var volGasnorp = await _gnsVolumeMsYPcBrutoRepositorio.ObtenerPorTipoYNombreDiaOperativoAsync(TiposTablasSupervisorPgt.VolumenMsGnsAgpsa, TiposGnsVolumeMsYPcBruto.GnsParaConsumoPropio, diaOperativo);
            if (volGasnorp != null)
            {
                dto.VolumenTotalGasCombustible = volGasnorp.VolumeMs;
            }


            dto.FactoresDistribucionGasDeCombustible = FactoresDistribucionGasNatural(entidadLotes, dto.VolumenTotalGasCombustible ?? 0);
            #endregion


            #region Cuadro N° 3. Asignación de LGN al GNA Adicional del Lote X

            var fiscalizacionProductos = new FiscalizacionProductosDto();
            var producto = await _fiscalizacionProductosServicio.ObtenerAsync(idUsuario);
            if (producto.Completado)
            {
                fiscalizacionProductos = producto.Resultado;
            }
            var produccionGlp = fiscalizacionProductos?.ProductoGlpCgn?.Where(e => e.Producto == TiposProducto.GLP).FirstOrDefault();
            if (produccionGlp != null)
            {
                dto.VolumenProduccionTotalGlp = produccionGlp.Produccion;
            }
            var produccionCgn = fiscalizacionProductos?.ProductoGlpCgn?.Where(e => e.Producto == TiposProducto.CGN).FirstOrDefault();
            if (produccionCgn != null)
            {
                dto.VolumenProduccionTotalCgn = produccionCgn.Produccion;
            }

            dto.VolumenProduccionTotalLgn = dto.VolumenProduccionTotalGlp + dto.VolumenProduccionTotalCgn;
            dto.FactoresDistribucionLiquidoGasNatural = FactoresDistribucionLiquidoGasNatural(entidadLotes, dto.VolumenProduccionTotalLgn ?? 0);
            dto.GravedadEspecifica = 0.5359;
            dto.VolumenProduccionTotalGlpCnpc = 0;
            dto.VolumenProduccionTotalCgnCnpc = 0;
            #endregion
            dto.Tabla1.GlpBls = Math.Round(dto.VolumenProduccionTotalGlpCnpc ?? 0, 2);
            dto.Tabla1.CgnBls = Math.Round(dto.VolumenProduccionTotalCgnCnpc ?? 0, 2);
            var cnsMpcEntidad = dto.FactoresDistribucionGasNaturalSeco.Where(e => e.Item == 6).FirstOrDefault();
            if (cnsMpcEntidad != null)
            {
                dto.Tabla1.CnsMpc = Math.Round(cnsMpcEntidad.AsignacionGns, 2);
            }
            var cgMpcEntidad = dto.FactoresDistribucionGasDeCombustible.Where(e => e.Item == 6).FirstOrDefault();
            if (cgMpcEntidad != null)
            {
                dto.Tabla1.CgMpc = Math.Round(cgMpcEntidad.AsignacionGns, 2);
            }


            await _boletaCnpcRepositorio.EliminarPorFechaAsync(diaOperativo, diaOperativo);
            var boletaCnpc = new Data.Reporte.Entidades.BoletaCnpc
            {
                Fecha = diaOperativo,
                CgnBls = dto.Tabla1.CgnBls,
                GasMpcd = dto.Tabla1.GasMpcd,
                GcMpc = dto.Tabla1.CgMpc,
                GlpBls = dto.Tabla1.GlpBls,
                GnsMpc = dto.Tabla1.CnsMpc,
                Actualizado = DateTime.UtcNow
            };
            await _boletaCnpcRepositorio.InsertarAsync(boletaCnpc);


            await GuardarAsync(dto, false);

            return new OperacionDto<BoletaCnpcDto>(dto);
        }


        // tabla N 01 - Factores de Distribución de Gas Natural Seco
        private List<FactoresDistribucionGasNaturalDto> FactoresDistribucionGasNaturalSeco(List<BoletaCnpcFactoresDistribucionDeGasCombustible> entidad, double volumenTotalGns)
        {


            List<int> lotes = new List<int> { (int)TiposLote.LoteZ69, (int)TiposLote.LoteVI, (int)TiposLote.LoteI, (int)TiposLote.LoteIv };

            List<FactoresDistribucionGasNaturalDto> lista = entidad.Select(e => new FactoresDistribucionGasNaturalDto
            {
                Item = e.IdLote,
                Suministrador = e.Lote,
                Volumen = Math.Round(e.Volumen, 0),
                AsignacionGns = e.AsignacionGns,
                ConcentracionC1 = e.ConcentracionC1,
                FactoresDistribucion = e.FactoresDistribucion,
            }).ToList();
            lista.ForEach(e => e.Volumen = lotes.Contains(e.Item) ? 0 : e.Volumen);




            lista.ForEach(e => e.VolumenC1 = Math.Round(e.Volumen * (e.ConcentracionC1 / 100), 0));
            double totalVolumenC1 = lista.Sum(e => e.VolumenC1);
            lista.ForEach(e => e.FactoresDistribucion = Math.Round((e.VolumenC1 / totalVolumenC1) * 100, 4));
            lista.ForEach(e => e.AsignacionGns = Math.Round(volumenTotalGns * e.FactoresDistribucion / 100, 2));

            lista.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = lista.Count + 1,
                Suministrador = "Total",
                Volumen = lista.Sum(e => e.Volumen),
                ConcentracionC1 = Math.Round(lista.Sum(e => e.VolumenConcentracionC1) / lista.Sum(e => e.Volumen), 4),
                VolumenC1 = Math.Round(lista.Sum(e => e.VolumenC1), 2),
                FactoresDistribucion = Math.Round(lista.Sum(e => e.FactoresDistribucion), 4),
                AsignacionGns = Math.Round(lista.Sum(e => e.AsignacionGns), 2),
            });
            for (var i = 0; i < lista.Count; i++)
            {
                lista[i].Item = (i + 1);
            }
            return lista;
        }



        // Cuadro N° 2. Asignación de Gas Combustible al GNA Adicional del Lote X
        private List<FactoresDistribucionGasNaturalDto> FactoresDistribucionGasNatural(List<BoletaCnpcFactoresDistribucionDeGasCombustible> entidad, double volumenTotalGas)
        {

            var lista = new List<FactoresDistribucionGasNaturalDto>();
            lista = entidad.Select(e => new FactoresDistribucionGasNaturalDto
            {
                Item = e.IdLote,
                Suministrador = e.Lote,
                Volumen = Math.Round(e.Volumen, 0),
                ConcentracionC1 = e.ConcentracionC1,
                VolumenC1 = Math.Round(e.Volumen * e.ConcentracionC1 / 100, 0),
                AsignacionGns = e.AsignacionGns,
                FactoresDistribucion = e.FactoresDistribucion,
            }).ToList();

            double totalVolumen = lista.Sum(e => e.Volumen);
            lista.ForEach(e => e.FactoresDistribucion = ((e.Volumen * e.ConcentracionC1 / 100) / lista.Sum(e => e.Volumen * e.ConcentracionC1 / 100) * 100));
            lista.ForEach(e => e.AsignacionGns = volumenTotalGas * e.FactoresDistribucion / 100);

            lista.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = (lista.Count + 1),
                Suministrador = "Total",
                Volumen = lista.Sum(e => e.Volumen),
                ConcentracionC1 = Math.Round(lista.Sum(e => e.VolumenConcentracionC1) / lista.Sum(e => e.Volumen), 4),
                VolumenC1 = Math.Round(lista.Sum(e => e.Volumen * e.ConcentracionC1 / 100), 0),
                FactoresDistribucion = Math.Round(lista.Sum(e => e.FactoresDistribucion), 4),
                AsignacionGns = Math.Round(lista.Sum(e => e.AsignacionGns), 2),
            });
            lista.ForEach(e => e.FactoresDistribucion = Math.Round(e.FactoresDistribucion, 4));
            lista.ForEach(e => e.AsignacionGns = Math.Round(e.AsignacionGns, 2));
            for (var i = 0; i < lista.Count; i++)
            {
                lista[i].Item = (i + 1);
            }
            return lista;
        }


        // Cuadro  Cuadro N° 3. Asignación de LGN al GNA Adicional del Lote X
        private List<FactoresDistribucionLiquidoGasNaturalDto> FactoresDistribucionLiquidoGasNatural(List<BoletaCnpcFactoresDistribucionDeGasCombustible> entidad, double volumenProduccionTotalLgn)
        {

            var lista = new List<FactoresDistribucionLiquidoGasNaturalDto>();
            lista = entidad.Select(e => new FactoresDistribucionLiquidoGasNaturalDto
            {
                Item = e.IdLote,
                Suministrador = e.Lote,
                Volumen = Math.Round(e.Volumen, 0),
                Riqueza = e.Riqueza,
                Contenido = Math.Round(e.Volumen * e.Riqueza, 0)
            }).ToList();

            double sumaContenido = lista.Sum(e => e.Volumen * e.Riqueza);

            lista.ForEach(e => e.FactoresDistribucion = Math.Round((e.Volumen * e.Riqueza / sumaContenido) * 100, 4));
            lista.ForEach(e => e.AsignacionGns = Math.Round(volumenProduccionTotalLgn * e.FactoresDistribucion / 100, 2));

            lista.Add(new FactoresDistribucionLiquidoGasNaturalDto
            {
                Suministrador = "Total",
                Volumen = lista.Sum(e => e.Volumen),
                Riqueza = Math.Round(lista.Where(e => e.Item != (int)TiposLote.LoteIv).Sum(e => e.VolumenRiqueza) / lista.Sum(e => e.Volumen), 4),
                Contenido = Math.Round(lista.Sum(e => e.Contenido), 4),
                FactoresDistribucion = Math.Round(lista.Sum(e => e.FactoresDistribucion), 2),
                AsignacionGns = Math.Round(lista.Sum(e => e.AsignacionGns), 2),
            });

            for (var i = 0; i < lista.Count; i++)
            {
                lista[i].Item = (i + 1);
            }
            return lista;
        }







        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletaCnpcDto peticion, bool esEditado)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            DateTime fecha = FechasUtilitario.ObtenerDiaOperativo();
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletaCnpc),
                Fecha = fecha,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                EsEditado = esEditado
            };

            await _boletaCnpcRepositorio.EliminarPorFechaAsync(fecha, fecha);
            var boletaCnpc = new Data.Reporte.Entidades.BoletaCnpc
            {
                Fecha = fecha,
                CgnBls = peticion?.Tabla1?.CgnBls,
                GasMpcd = peticion?.Tabla1?.GasMpcd,
                GcMpc = peticion?.Tabla1?.CgMpc,
                GlpBls = peticion?.Tabla1?.GlpBls,
                GnsMpc = peticion?.Tabla1?.CnsMpc,
                Actualizado = DateTime.UtcNow
            };
            await _boletaCnpcRepositorio.InsertarAsync(boletaCnpc);
            return await _impresionServicio.GuardarAsync(dto);

        }

    }
}
