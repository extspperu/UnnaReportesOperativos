using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Drawing.Printing;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Fuentes.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Fuentes.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
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
        public BoletaCnpcServicio(
            IDiaOperativoRepositorio diaOperativoRepositorio,
            IRegistroRepositorio registroRepositorio,
            IBoletaCnpcVolumenComposicionGnaEntradaRepositorio boletaCnpcVolumenComposicionGnaEntradaRepositorio,
            IReporteServicio reporteServicio,
            IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio,
            IImpresionServicio impresionServicio
            )
        {
            _diaOperativoRepositorio = diaOperativoRepositorio;
            _registroRepositorio = registroRepositorio;
            _boletaCnpcVolumenComposicionGnaEntradaRepositorio = boletaCnpcVolumenComposicionGnaEntradaRepositorio;
            _reporteServicio = reporteServicio;
            _gnsVolumeMsYPcBrutoRepositorio = gnsVolumeMsYPcBrutoRepositorio;
            _impresionServicio = impresionServicio;
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
            var primerDato = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync((int)TiposLote.LoteIv, FechasUtilitario.ObtenerDiaOperativo(), (int)TipoGrupos.FiscalizadorEnel, (int)TiposNumeroRegistro.PrimeroRegistro);
            if (primerDato != null)
            {
                var dato = await _registroRepositorio.ObtenerPorIdDatoYDiaOperativoAsync((int)TiposDatos.VolumenMpcd, primerDato.IdDiaOperativo);
                if (dato != null)
                {
                    gasMpcd1 = dato.Valor ?? 0;
                }
            }
            var segundoDato = await _diaOperativoRepositorio.ObtenerPorIdLoteYFechaAsync((int)TiposLote.LoteIv, FechasUtilitario.ObtenerDiaOperativo(), (int)TipoGrupos.FiscalizadorEnel, (int)TiposNumeroRegistro.SegundoRegistro);
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
            tabla1.GlpBls = 13;
            tabla1.CgnBls = 14;
            dto.Tabla1 = tabla1;


            //Cuadro N° 1. Fiscalización de GNS del GAS Adicional del Lote X
            var volumenTotalGns = await _gnsVolumeMsYPcBrutoRepositorio.ObtenerPorTipoYNombreDiaOperativoAsync(TiposTablasSupervisorPgt.VolumenMsGnsAgpsa, TiposGnsVolumeMsYPcBruto.GnsAEgpsa, diaOperativo);
            if (volumenTotalGns != null)
            {
                dto.VolumenTotalGns = volumenTotalGns.VolumeMs;
            }
            dto.VolumenTotalGnsEnMs = 125;
            dto.FlareGna = dto.VolumenTotalGns + dto.VolumenTotalGnsEnMs;
                        
            dto.FactoresDistribucionGasNaturalSeco = await FactoresDistribucionGasNaturalSeco();

            
            #region Cuadro N° 2. Asignación de Gas Combustible al GNA Adicional del Lote X
            var factoresDistribucionGasDeCombustible = new List<FactoresDistribucionGasNaturalDto>();

            factoresDistribucionGasDeCombustible.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 1,
                Sumistrador = "LOTE Z69",
                Volumen = 500
            });
            factoresDistribucionGasDeCombustible.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 2,
                Sumistrador = "CNPC",
                Volumen = 122
            });
            dto.FactoresDistribucionGasNaturalSeco = factoresDistribucionGasDeCombustible;
            #endregion


            return new OperacionDto<BoletaCnpcDto>(dto);
        }


        // tabla N 01 - Factores de Distribución de Gas Natural Seco
        private async Task<List<FactoresDistribucionGasNaturalDto>> FactoresDistribucionGasNaturalSeco()
        {

            var entidades = await _boletaCnpcVolumenComposicionGnaEntradaRepositorio.ListarAsync();
            

            List<FactoresDistribucionGasNaturalDto> factoresDistribucionGasNaturalSeco = new List<FactoresDistribucionGasNaturalDto>();

            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 1,
                Sumistrador = "LOTE Z69",
                Volumen = 0,
                ConcentracionC1 = entidades.Where(e => e.Id == 1).FirstOrDefault() != null ? entidades.Where(e => e.Id == 1).First().ConcentracionN2HastaO2 : 0,
                VolumenC1 = 1,
                FactoresDistribucion = 1,
                AsignacionGns = 1,
            });
            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 2,
                Sumistrador = "CNPC",
                Volumen = 13,
                ConcentracionC1 = entidades.Where(e => e.Id == 1).FirstOrDefault() != null ? entidades.Where(e => e.Id == 1).First().ConcentracionN2HastaO2 : 0,
                VolumenC1 = 1,
                FactoresDistribucion = 1,
                AsignacionGns = 1,
            });
            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 3,
                Sumistrador = "LOTE VI",
                Volumen = 0,
                ConcentracionC1 = entidades.Where(e => e.Id == 3).FirstOrDefault() != null ? entidades.Where(e => e.Id == 3).First().ConcentracionN2HastaO2 : 0,
                VolumenC1 = 1,
                FactoresDistribucion = 1,
                AsignacionGns = 1,
            });
            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 4,
                Sumistrador = "LOTE I",
                Volumen = 0,
                ConcentracionC1 = entidades.Where(e => e.Id == 4).FirstOrDefault() != null ? entidades.Where(e => e.Id == 4).First().ConcentracionN2HastaO2 : 0,
                
                FactoresDistribucion = 1,
                AsignacionGns = 1,
            });
            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 5,
                Sumistrador = "LOTE IV",
                Volumen = 0,
                ConcentracionC1 = entidades.Where(e => e.Id == 5).FirstOrDefault() != null ? entidades.Where(e => e.Id == 5).First().ConcentracionN2HastaO2 : 0,
                
                FactoresDistribucion = 1,
                AsignacionGns = 1,
            });
            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Item = 6,
                Sumistrador = "CNPC ADICIONAL",
                Volumen = 353,
                ConcentracionC1 = entidades.Where(e => e.Id == 6).FirstOrDefault() != null ? entidades.Where(e => e.Id == 6).First().ConcentracionN2HastaO2 : 0,
                //VolumenC1 = Volumen + ConcentracionC1,
                FactoresDistribucion = 1,
                AsignacionGns = 1,
            });
            factoresDistribucionGasNaturalSeco.Add(new FactoresDistribucionGasNaturalDto
            {
                Sumistrador = "Total",
                Volumen = factoresDistribucionGasNaturalSeco.Sum(e => e.Volumen),
                ConcentracionC1 = factoresDistribucionGasNaturalSeco.Sum(e => e.ConcentracionC1),
                VolumenC1 = factoresDistribucionGasNaturalSeco.Sum(e => e.VolumenC1),
                FactoresDistribucion = factoresDistribucionGasNaturalSeco.Sum(e => e.FactoresDistribucion),
                AsignacionGns = factoresDistribucionGasNaturalSeco.Sum(e => e.AsignacionGns),
            });

            return factoresDistribucionGasNaturalSeco;
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
