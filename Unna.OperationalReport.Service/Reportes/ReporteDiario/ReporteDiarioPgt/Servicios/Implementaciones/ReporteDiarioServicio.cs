using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Servicios.Implementaciones
{


    public class ReporteDiarioServicio : IReporteDiarioServicio
    {

        private readonly IReporteServicio _reporteServicio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IFiscalizacionPetroPeruServicio _fiscalizacionPetroPeruServicio;
        private readonly IFiscalizacionProductoProduccionRepositorio _fiscalizacionProductoProduccionRepositorio;
        private readonly IVolumenDespachoRepositorio _volumenDespachoRepositorio;
        private readonly IBoletaCnpcServicio _boletaCnpcServicio;
        private readonly IBoletaBalanceEnergiaServicio _boletaBalanceEnergiaServicio;
        public ReporteDiarioServicio(
            IReporteServicio reporteServicio,
            IImpresionServicio impresionServicio,
            IRegistroRepositorio registroRepositorio,
            IFiscalizacionPetroPeruServicio fiscalizacionPetroPeruServicio,
            IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio,
            IVolumenDespachoRepositorio volumenDespachoRepositorio,
            IBoletaCnpcServicio boletaCnpcServicio,
            IBoletaBalanceEnergiaServicio boletaBalanceEnergiaServicio
            )
        {
            _reporteServicio = reporteServicio;
            _impresionServicio = impresionServicio;
            _registroRepositorio = registroRepositorio;
            _fiscalizacionPetroPeruServicio = fiscalizacionPetroPeruServicio;
            _fiscalizacionProductoProduccionRepositorio = fiscalizacionProductoProduccionRepositorio;
            _volumenDespachoRepositorio = volumenDespachoRepositorio;
            _boletaCnpcServicio = boletaCnpcServicio;
            _boletaBalanceEnergiaServicio = boletaBalanceEnergiaServicio;
        }


        public async Task<OperacionDto<ReporteDiarioDto>> ObtenerAsync(long? idUsuario)
        {

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.ReporteDiarioPgt, FechasUtilitario.ObtenerDiaOperativo());
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<ReporteDiarioDto>(operacionImpresion.Resultado.Datos);
                return new OperacionDto<ReporteDiarioDto>(rpta);
            }
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.ReporteExistencias, idUsuario);
            if (!operacionGeneral.Completado || operacionGeneral.Resultado == null)
            {
                return new OperacionDto<ReporteDiarioDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            var dto = new ReporteDiarioDto
            {
                Fecha = diaOperativo.ToString("dd/MM/yyyy"),
                General = operacionGeneral.Resultado
            };

            #region 1. RECEPCION DE GAS NATURAL ASOCIADO (GNA)

            var gasNaturalAsociado = await _registroRepositorio.ListarReporteDiarioGasNaturalAsociadoAsync(diaOperativo);
            var dtoGas = gasNaturalAsociado.Select(e => new GasNaturalAsociadoDto
            {
                IdLote = e.IdLote,
                Lote = e.Lote,
                Volumen = e.Volumen,
                Calorifico = e.Calorifico,
                EnergiaDiaria = e.EnergiaDiaria,
                Riqueza = e.Riqueza,
                RiquezaBls = e.RiquezaBls,
                VolumenPromedio = e.VolumenPromedio
            }).ToList();

            double totalGasProcesado = dtoGas.Sum(e => e.Volumen ?? 0);
            dtoGas.Add(new GasNaturalAsociadoDto
            {
                Lote = "TOTAL",
                Volumen = totalGasProcesado,
                Calorifico = Math.Round(dtoGas.Sum(e => e.VolumenPorderCalorifico ?? 0) / dtoGas.Sum(e => e.Volumen ?? 0), 2),
                Riqueza = Math.Round(dtoGas.Sum(e => e.VolumenRiqueza ?? 0) / dtoGas.Sum(e => e.Volumen ?? 0), 4),
                RiquezaBls = Math.Round(dtoGas.Sum(e => e.VolumenRiquezaBls ?? 0) / dtoGas.Sum(e => e.Volumen ?? 0), 4),
                EnergiaDiaria = dtoGas.Sum(e => e.EnergiaDiaria),
                VolumenPromedio = dtoGas.Sum(e => e.VolumenPromedio)
            });
            dto.GasNaturalAsociado = dtoGas;
            dto.HoraPlantaFs = 0; // cero por defecto
            dto.GasNoProcesado = Math.Round(totalGasProcesado / 24 * dto.HoraPlantaFs ?? 0, 0);
            dto.GasProcesado = totalGasProcesado - dto.GasNoProcesado;
            dto.UtilizacionPlantaParinias = Math.Round(dto.GasProcesado ?? 0 / 44000, 2);


            #endregion


            #region 3. PRODUCCIÓN Y VENTA DE LÍQUIDOS DE GAS NATURAL (LGN)

            var operacionPetroperu = await _fiscalizacionPetroPeruServicio.ObtenerAsync(idUsuario ?? 0);
            if (operacionPetroperu.Completado && operacionPetroperu.Resultado != null)
            {
                dto.EficienciaRecuperacionLgn = operacionPetroperu.Resultado.Eficiencia;
            }

            var fiscalizacionProducto = await _fiscalizacionProductoProduccionRepositorio.ListarReporteDiarioGasNaturalAsociadoAsync(diaOperativo);
            dto.LiquidosGasNaturalProduccionVentas = fiscalizacionProducto.Select(e => new LiquidosGasNaturalProduccionVentasDto
            {
                ProduccionDiaria = e.ProduccionDiaria,
                ProduccionMensual = e.ProduccionMensual,
                VentaDiaria = e.VentaDiaria,
                VentaMensual = e.VentaMensual,
                Producto = e.Producto
            }).ToList();


            var volumenDespacho = await _volumenDespachoRepositorio.ListarPorDiaOperativoAsync(diaOperativo);

            var volumenDespachoGlp = new List<VolumenDespachoDto>();
            var entidadVolumenDespachoGlp = volumenDespacho?.Where(e => e.Tipo.Equals(TiposTablasSupervisorPgt.DespachoGlp)).ToList();
            for (var i = 0; i < entidadVolumenDespachoGlp?.Count; i++)
            {
                volumenDespachoGlp.Add(new VolumenDespachoDto
                {
                    Id = (i + 1),
                    Dato = $"{(i + 1)}do despacho",
                    Cliente = entidadVolumenDespachoGlp[i].Cliente,
                    Placa = entidadVolumenDespachoGlp[i].Placa,
                    Tanque = entidadVolumenDespachoGlp[i].Tanque,
                    Volumen = entidadVolumenDespachoGlp[i].Volumen
                });
            }
            volumenDespachoGlp.Add(new VolumenDespachoDto
            {
                Dato = $"Total gal. GLP",
                Tanque = volumenDespachoGlp.Count.ToString(),
                Volumen = volumenDespachoGlp.Sum(e => e.Volumen)
            });
            dto.VolumenDespachoGlp = volumenDespachoGlp;

            var volumenDespachoCgn = new List<VolumenDespachoDto>();
            var entidadVolumenDespachoCgn = volumenDespacho?.Where(e => e.Tipo.Equals(TiposTablasSupervisorPgt.DespachoCgn)).ToList();
            for (var i = 0; i < entidadVolumenDespachoCgn?.Count; i++)
            {
                volumenDespachoCgn.Add(new VolumenDespachoDto
                {
                    Id = (i + 1),
                    Dato = $"{(i + 1)}do despacho",
                    Cliente = entidadVolumenDespachoCgn[i].Cliente,
                    Placa = entidadVolumenDespachoCgn[i].Placa,
                    Tanque = entidadVolumenDespachoCgn[i].Tanque,
                    Volumen = entidadVolumenDespachoCgn[i].Volumen
                });
            }
            volumenDespachoCgn.Add(new VolumenDespachoDto
            {
                Dato = $"Total gal. GLP",
                Tanque = volumenDespachoCgn.Count.ToString(),
                Volumen = volumenDespachoCgn.Sum(e => e.Volumen)
            });
            dto.VolumenDespachoCgn = volumenDespachoCgn;


            #endregion 4.  VOLUMEN DE GAS Y PRODUCCIÓN DE GNA ADICIONAL DEL LOTE X (CNPC PERÚ):

            double produccionLoteXGnaTotalCnpcVolumen = 0;
            var volumenGnsVentaVgnsvEnel = await _registroRepositorio.ObtenerValorAsync((int)TiposDatos.GnsVentaUnnaLoteIv, (int)TiposLote.LoteX, diaOperativo, (int)TiposNumeroRegistro.SegundoRegistro);
            if (volumenGnsVentaVgnsvEnel != null)
            {
                produccionLoteXGnaTotalCnpcVolumen = volumenGnsVentaVgnsvEnel.Valor ?? 0;
            }

            var volumenProduccionLoteXGnaTotalCnpc = new List<VolumenGasProduccionDto>();
            volumenProduccionLoteXGnaTotalCnpc.Add(new VolumenGasProduccionDto
            {
                Nombre = "VOLUMEN NOMINADO POR EGPSA",
                Volumen = produccionLoteXGnaTotalCnpcVolumen,
            });

            var cnpcLoteX = gasNaturalAsociado.Where(e => e.IdLote == (int)TiposLote.LoteX).FirstOrDefault();
            volumenProduccionLoteXGnaTotalCnpc.Add(new VolumenGasProduccionDto
            {
                Nombre = "VOLUMEN ADICIONAL DISPONIBLE",
                Volumen = ((cnpcLoteX != null ? cnpcLoteX.Volumen : 0) - produccionLoteXGnaTotalCnpcVolumen),
            });
            dto.VolumenProduccionLoteXGnaTotalCnpc = volumenProduccionLoteXGnaTotalCnpc;

            var boletaCnpcDto = new BoletaCnpcDto();
            var operacionCnpc = await _boletaCnpcServicio.ObtenerAsync(idUsuario ?? 0);
            if (operacionCnpc.Completado && operacionCnpc.Resultado != null)
            {
                boletaCnpcDto = operacionCnpc.Resultado;
            }
            var volumenProduccionLoteXLiquidoGasNatural = new List<VolumenGasProduccionDto>();
            volumenProduccionLoteXLiquidoGasNatural.Add(new VolumenGasProduccionDto
            {
                Nombre = "GLP (BLS)",
                Volumen = boletaCnpcDto.Tabla1?.GlpBls,
            });
            volumenProduccionLoteXLiquidoGasNatural.Add(new VolumenGasProduccionDto
            {
                Nombre = "CGN (BLS)",
                Volumen = boletaCnpcDto.Tabla1?.CgnBls,
            });
            volumenProduccionLoteXLiquidoGasNatural.Add(new VolumenGasProduccionDto
            {
                Nombre = "TOTAL",
                Volumen = (boletaCnpcDto.Tabla1?.GlpBls) + (boletaCnpcDto.Tabla1?.CgnBls),
            });
            dto.VolumenProduccionLoteXLiquidoGasNatural = volumenProduccionLoteXLiquidoGasNatural;

            #region 5.  VOLUMEN DE GAS Y PRODUCCIÓN DE ENEL:

            dto.VolumenProduccionEnel = await VolumenProduccionEnelAsync(idUsuario??0, produccionLoteXGnaTotalCnpcVolumen);

            #endregion


            #region 7.  VOLUMEN DE GAS Y PRODUCCIÓN UNNA ENERGIA LOTE IV:



            #endregion



            return new OperacionDto<ReporteDiarioDto>(dto);
        }

        private async Task<List<VolumenGasProduccionDto>> VolumenProduccionEnelAsync(long idUsuario, double? volumenGnsVentaVgnsvEnel)
        {
            var lista = new List<VolumenGasProduccionDto>();
       
            lista.Add(new VolumenGasProduccionDto
            {
                Nombre = "RECEPCIÓN DE GNA (RENOMINADO LOTE X)",
                Volumen = volumenGnsVentaVgnsvEnel,
            });

            var entregaGna = new List<DistribucionVolumenPorderCalorificoDto>();
            var operacion = await _boletaBalanceEnergiaServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado)
            {
                entregaGna = operacion.Resultado?.GnsAEnel;
            }

            lista.Add(new VolumenGasProduccionDto
            {
                Nombre = "GNS A ENEL",
                Volumen = entregaGna.Count > 0 ? entregaGna.First().Volumen :0,
            });

            lista.Add(new VolumenGasProduccionDto
            {
                Nombre = "HUMEDAD DE GNA",
                Volumen = entregaGna.Where(e=>e.Item == 4).FirstOrDefault() != null? entregaGna.Where(e => e.Item == 4).FirstOrDefault().Volumen:0,
            });
            
            lista.Add(new VolumenGasProduccionDto
            {
                Nombre = "GAS AL FLARE",
                Volumen = entregaGna.Where(e => e.Item == 5).FirstOrDefault() != null ? entregaGna.Where(e => e.Item == 5).FirstOrDefault().Volumen : 0,
            });


            lista.Add(new VolumenGasProduccionDto
            {
                Nombre = "GAS COMBUSTIBLE (VGC)",
                Volumen = lista.Sum(e => e.Volumen),
            });
            lista.Add(new VolumenGasProduccionDto
            {
                Nombre = "TOTAL",
                Volumen = lista.Sum(e => e.Volumen),
            });

            return lista;
        }



        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(ReporteExistenciaDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ReporteDiarioPgt),
                Fecha = FechasUtilitario.ObtenerDiaOperativo(),
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion)
            };
            return await _impresionServicio.GuardarAsync(dto);
        }


    }
}
