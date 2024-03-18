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
        public BoletaCnpcServicio(
            IDiaOperativoRepositorio diaOperativoRepositorio,
            IRegistroRepositorio registroRepositorio,
            IBoletaCnpcVolumenComposicionGnaEntradaRepositorio boletaCnpcVolumenComposicionGnaEntradaRepositorio,
            IReporteServicio reporteServicio,
            IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio,
            IImpresionServicio impresionServicio,
            IBoletaBalanceEnergiaServicio boletaBalanceEnergiaServicio
            )
        {
            _diaOperativoRepositorio = diaOperativoRepositorio;
            _registroRepositorio = registroRepositorio;
            _boletaCnpcVolumenComposicionGnaEntradaRepositorio = boletaCnpcVolumenComposicionGnaEntradaRepositorio;
            _reporteServicio = reporteServicio;
            _gnsVolumeMsYPcBrutoRepositorio = gnsVolumeMsYPcBrutoRepositorio;
            _impresionServicio = impresionServicio;
            _boletaBalanceEnergiaServicio = boletaBalanceEnergiaServicio;
        }

        public async Task<OperacionDto<BoletaCnpcDto>> ObtenerAsync(long idUsuario)
        {

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaCnpc, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletaCnpcDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaCnpc, FechasUtilitario.ObtenerDiaOperativo());
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<BoletaCnpcDto>(operacionImpresion.Resultado.Datos);
                rpta.General = operacionGeneral.Resultado;
                return new OperacionDto<BoletaCnpcDto>(rpta);
            }


            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            BoletaCnpcTabla1Dto tabla1 = new BoletaCnpcTabla1Dto();

            double gasMpcd1 = 0;
            double gasMpcd2 = 0;
            var primerDato = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync((int)TiposLote.LoteIv, FechasUtilitario.ObtenerDiaOperativo(), (int)TipoGrupos.FiscalizadorRegular, (int)TiposNumeroRegistro.PrimeroRegistro);
            if (primerDato != null)
            {
                var dato = await _registroRepositorio.ObtenerPorIdDatoYDiaOperativoAsync((int)TiposDatos.VolumenMpcd, primerDato.IdDiaOperativo);
                if (dato != null)
                {
                    gasMpcd1 = dato.Valor ?? 0;
                }
            }
            var segundoDato = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync((int)TiposLote.LoteX, FechasUtilitario.ObtenerDiaOperativo(), (int)TipoGrupos.FiscalizadorEnel, (int)TiposNumeroRegistro.SegundoRegistro);
            if (segundoDato != null)
            {
                var dato = await _registroRepositorio.ObtenerPorIdDatoYDiaOperativoAsync((int)TiposDatos.CnpcPeruGnaRecibido, segundoDato.IdDiaOperativo);
                if (dato != null)
                {
                    gasMpcd2 = dato.Valor ?? 0;
                }
            }

            var dto = new BoletaCnpcDto
            {
                Fecha = diaOperativo.ToString("dd/MM/yyyy")
            };

            dto.General = operacionGeneral.Resultado;

            // tabla N° 01

            tabla1.Fecha = dto.Fecha;
            tabla1.GasMpcd = Math.Round(gasMpcd1, 0) - Math.Round(gasMpcd2);
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

            var flareGnaConsumo = boletaBalance?.ConsumoPropio?.Where(e => e.Item == 1).FirstOrDefault();
            if (flareGnaConsumo != null)
            {
                dto.FlareGna = flareGnaConsumo.Volumen;
            }
            dto.VolumenTotalGns = dto.VolumenTotalGnsEnMs + dto.FlareGna;

            var entidadLotes = await _registroRepositorio.BoletaCnpcFactoresDistribucionDeGasCombustibleAsync(diaOperativo);

            dto.FactoresDistribucionGasNaturalSeco = FactoresDistribucionGasNaturalSeco(entidadLotes,dto.VolumenTotalGnsEnMs);


            #region Cuadro N° 2. Asignación de Gas Combustible al GNA Adicional del Lote X


            dto.FactoresDistribucionGasDeCombustible = FactoresDistribucionGasNatural(entidadLotes, dto.VolumenTotalGasCombustible??0);
            #endregion


            #region Cuadro N° 3. Asignación de LGN al GNA Adicional del Lote X

            dto.VolumenProduccionTotalGlp = 0;
            dto.VolumenProduccionTotalCgn = 0;
            dto.VolumenProduccionTotalLgn = 0;
            dto.FactoresDistribucionLiquidoGasNatural = FactoresDistribucionLiquidoGasNatural(entidadLotes, dto.VolumenProduccionTotalLgn ?? 0);
            dto.GravedadEspecifica = 0.5359;
            dto.VolumenProduccionTotalGlpCnpc = 0;
            dto.VolumenProduccionTotalCgnCnpc = 0;
            #endregion
            dto.Tabla1.GlpBls = Math.Round(dto.VolumenProduccionTotalGlpCnpc ?? 0, 2);
            dto.Tabla1.CgnBls = Math.Round(dto.VolumenProduccionTotalCgnCnpc ?? 0, 2);
            var cnsMpcEntidad = dto.FactoresDistribucionGasNaturalSeco.Where(e=>e.Item==6).FirstOrDefault();
            if (cnsMpcEntidad != null)
            {
                dto.Tabla1.CnsMpc = Math.Round(cnsMpcEntidad.AsignacionGns, 2);
            }
            var cgMpcEntidad = dto.FactoresDistribucionGasDeCombustible.Where(e => e.Item == 6).FirstOrDefault();
            if (cgMpcEntidad != null)
            {
                dto.Tabla1.CgMpc = Math.Round(cgMpcEntidad.AsignacionGns, 2);
            }

            return new OperacionDto<BoletaCnpcDto>(dto);
        }


        // tabla N 01 - Factores de Distribución de Gas Natural Seco
        private List<FactoresDistribucionGasNaturalDto> FactoresDistribucionGasNaturalSeco(List<BoletaCnpcFactoresDistribucionDeGasCombustible> entidad,double volumenTotalGns)
        {

            //var entidades = await _boletaCnpcVolumenComposicionGnaEntradaRepositorio.ListarAsync();

            List<int> lotes = new List<int> { (int)TiposLote.LoteZ69, (int)TiposLote.LoteVI , (int)TiposLote.LoteI , (int)TiposLote.LoteIv };

            List<FactoresDistribucionGasNaturalDto> lista = entidad.Select(e => new FactoresDistribucionGasNaturalDto
            {
                Item = e.IdLote,
                Suministrador = e.Lote,
                Volumen = e.Volumen ,
                AsignacionGns = e.AsignacionGns,
                ConcentracionC1 = e.ConcentracionC1,
                FactoresDistribucion = e.FactoresDistribucion,
            }).ToList();
            lista.ForEach(e => e.Volumen = lotes.Contains(e.Item) ? 0 : e.Volumen);



            double totalVolumenC1 = lista.Sum(e => e.Volumen);
            lista.ForEach(e => e.VolumenC1 = Math.Round(e.Volumen * e.ConcentracionC1, 4));
            lista.ForEach(e => e.FactoresDistribucion = Math.Round((e.VolumenC1 / totalVolumenC1), 4));
            lista.ForEach(e => e.AsignacionGns = Math.Round(volumenTotalGns * e.FactoresDistribucion, 2));

            lista.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = (lista.Count + 1),
                Suministrador = "Total",
                Volumen = totalVolumenC1,
                ConcentracionC1 = Math.Round(lista.Sum(e => e.VolumenConcentracionC1) / totalVolumenC1, 4),
                VolumenC1 = Math.Round(lista.Sum(e => e.VolumenC1), 2),
                FactoresDistribucion = Math.Round(lista.Sum(e => e.FactoresDistribucion), 4),
                AsignacionGns = Math.Round(lista.Sum(e => e.AsignacionGns), 2),
            });

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
                Volumen = e.Volumen,
                ConcentracionC1 = e.ConcentracionC1,
                VolumenC1 = Math.Round(e.Volumen * e.ConcentracionC1,4),
                AsignacionGns = e.AsignacionGns,                
                FactoresDistribucion = e.FactoresDistribucion,
            }).ToList();

            double totalVolumenC1 = lista.Sum(e => e.Volumen);
            lista.ForEach(e => e.FactoresDistribucion = Math.Round((e.VolumenC1 / totalVolumenC1), 4));
            lista.ForEach(e => e.AsignacionGns = Math.Round(volumenTotalGas * e.FactoresDistribucion, 2));

            lista.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = (lista.Count + 1),
                Suministrador = "Total",
                Volumen = totalVolumenC1,
                ConcentracionC1 = Math.Round(lista.Sum(e => e.VolumenConcentracionC1) / totalVolumenC1, 4),
                VolumenC1 = Math.Round(lista.Sum(e => e.VolumenC1), 2),
                FactoresDistribucion = Math.Round(lista.Sum(e => e.FactoresDistribucion), 4),
                AsignacionGns = Math.Round(lista.Sum(e => e.AsignacionGns),2),
            });
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
                Volumen = e.Volumen,
                Riqueza = e.Riqueza,
                Contenido = Math.Round(e.Volumen * e.Riqueza, 3)
            }).ToList();

            double sumaContenido = lista.Sum(e => e.Contenido);

            lista.ForEach(e => e.FactoresDistribucion = Math.Round((e.Contenido / sumaContenido) * 100, 4));
            lista.ForEach(e => e.AsignacionGns = Math.Round(volumenProduccionTotalLgn * e.FactoresDistribucion, 2));

            lista.Add(new FactoresDistribucionLiquidoGasNaturalDto
            {
                Item = (lista.Count + 1),
                Suministrador = "Total",
                Volumen = lista.Sum(e => e.Volumen),
                Riqueza = Math.Round(lista.Sum(e => e.VolumenRiqueza) / lista.Sum(e => e.Volumen), 4),
                Contenido = Math.Round(lista.Sum(e => e.Contenido),4),
                FactoresDistribucion = Math.Round(lista.Sum(e => e.FactoresDistribucion), 2),
                AsignacionGns = lista.Sum(e => e.AsignacionGns),
            });
            return lista;
        }







        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(BoletaCnpcDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            peticion.General = null;
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletaCnpc),
                Fecha = FechasUtilitario.ObtenerDiaOperativo(),
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion)
            };
            return await _impresionServicio.GuardarAsync(dto);

        }

    }
}
