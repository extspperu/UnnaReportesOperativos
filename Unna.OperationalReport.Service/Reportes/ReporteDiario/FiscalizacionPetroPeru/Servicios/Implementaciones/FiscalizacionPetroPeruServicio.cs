using DocumentFormat.OpenXml.Drawing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Auth.Enums;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Procedimientos;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Implementaciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Implementaciones
{
    public class FiscalizacionPetroPeruServicio : IFiscalizacionPetroPeruServicio
    {

        private readonly IReporteServicio _reporteServicio;
        private readonly IBoletaDiariaFiscalizacionRepositorio _boletaDiariaFiscalizacionRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteDiariaDatosRepositorio _reporteDiariaDatosRepositorio;
        private readonly IBoletaDeterminacionVolumenGnaServicio _boletaDeterminacionVolumenGnaServicio;
        private readonly IRegistroRepositorio _registrorepositorio;
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;

        public FiscalizacionPetroPeruServicio(
            IReporteServicio reporteServicio,
            IBoletaDiariaFiscalizacionRepositorio boletaDiariaFiscalizacionRepositorio,
            IImpresionServicio impresionServicio,
            IReporteDiariaDatosRepositorio reporteDiariaDatosRepositorio,
            IBoletaDeterminacionVolumenGnaServicio boletaDeterminacionVolumenGnaServicio,
            IRegistroRepositorio registrorepositorio,
            ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio
            )
        {
            _reporteServicio = reporteServicio;
            _boletaDiariaFiscalizacionRepositorio = boletaDiariaFiscalizacionRepositorio;
            _impresionServicio = impresionServicio;
            _reporteDiariaDatosRepositorio = reporteDiariaDatosRepositorio;
            _boletaDeterminacionVolumenGnaServicio = boletaDeterminacionVolumenGnaServicio;
            _registrorepositorio = registrorepositorio;
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
        }

        public async Task<OperacionDto<FiscalizacionPetroPeruDto>> ObtenerAsync(long idUsuario)
        {
            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaDiariaFiscalizacionPetroPeru, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<FiscalizacionPetroPeruDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaDiariaFiscalizacionPetroPeru, diaOperativo);
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos) && operacionImpresion.Resultado.EsEditado)
            {
                var rpta = JsonConvert.DeserializeObject<FiscalizacionPetroPeruDto>(operacionImpresion.Resultado.Datos);
                rpta.General = operacionGeneral.Resultado;
                return new OperacionDto<FiscalizacionPetroPeruDto>(rpta);
            }

            var dto = new FiscalizacionPetroPeruDto
            {
                Fecha = diaOperativo.ToString("dd/MM/yyyy"),
                IdUsuario = idUsuario
            };

            dto.General = operacionGeneral.Resultado;

            //var operacionReporte = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaDiariaFiscalizacionPetroPeru, FechasUtilitario.ObtenerDiaOperativo());

            #region Cuadro N° 1. Asignación de Volumen de Líquidos del Gas Natural (LGN) disgregado por lotes de PETROPERU

            var boletaLoteIv = new BoletaDeterminacionVolumenGnaDto();
            var operacionBoletaLoteIv = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(idUsuario);
            if (operacionBoletaLoteIv.Completado && operacionBoletaLoteIv.Resultado != null)
            {
                boletaLoteIv = operacionBoletaLoteIv.Resultado;
            }
            dto.VolumenTotalProduccion = boletaLoteIv.VolumenProduccionTotalLgn ?? 0;
            if (boletaLoteIv.FactorAsignacionLiquidosGasNatural != null)
            {
                var factorAsignacionLiquidosGasNatural = boletaLoteIv.FactorAsignacionLiquidosGasNatural.Where(e => e.Suministrador.Equals("Total")).FirstOrDefault();
                dto.ContenidoLgn = factorAsignacionLiquidosGasNatural != null ? factorAsignacionLiquidosGasNatural.Contenido / 42:0;
            }
            if (dto.ContenidoLgn > 0)
            {
                dto.Eficiencia = Math.Round((dto.VolumenTotalProduccion / dto.ContenidoLgn) * 100, 2);
            }            

            dto.FactorAsignacionLiquidoGasNatural = await ObtenerFactorAsignacionLiquidoGasNatural(dto.VolumenTotalProduccion, dto.ContenidoLgn);
            dto.ContenidoLgn = Math.Round(dto.ContenidoLgn, 2);
            var factorConversionZ69 = await _reporteDiariaDatosRepositorio.ObtenerFactorConversionPorLotePetroperuAsync(diaOperativo, (int)TiposLote.LoteZ69, (int)TiposDatos.VolumenMpcd, dto.Eficiencia);
            if (factorConversionZ69 != null)
            {
                dto.FactorConversionZ69 = Math.Round(factorConversionZ69.Value, 2);
            }

            var factorConversionVi = await _reporteDiariaDatosRepositorio.ObtenerFactorConversionPorLotePetroperuAsync(diaOperativo, (int)TiposLote.LoteVI, (int)TiposDatos.VolumenMpcd, dto.Eficiencia);
            if (factorConversionVi != null)
            {
                dto.FactorConversionVi = factorConversionVi.Value;
            }

            var factorConversionI = await _reporteDiariaDatosRepositorio.ObtenerFactorConversionPorLotePetroperuAsync(diaOperativo, (int)TiposLote.LoteI, (int)TiposDatos.VolumenMpcd, dto.Eficiencia);
            if (factorConversionI != null)
            {
                dto.FactorConversionI = factorConversionI.Value;
            }


            #endregion




            #region Cuadro N° 2. Volumen GNS disponible por lotes de PETROPERU

            dto.DistribucionGasNaturalSeco = await ObtenerDistribucionGasNaturalSecoAsync(dto);

            #endregion



            #region Cuadro N° 3. Volumen transferido a Refinería por lotes de PETROPERU

            var distribucionGasNaturalSeco = dto.DistribucionGasNaturalSeco.Where(e => e.Suministrador.Equals("Total")).FirstOrDefault();
            dto.VolumenTotalGns = await _registrorepositorio.ObtenerVolumenGNSManualAsync();// 9791.75;// Es valor fijo, agregar mantenedor 
            if (distribucionGasNaturalSeco != null)
            {
                dto.VolumenTotalGnsFlare = Math.Round((distribucionGasNaturalSeco.VolumenGnsd - dto.VolumenTotalGns ?? 0), 4);
            }
            dto.VolumenTransferidoRefineriaPorLote = VolumenTransferidoRefineriaPorLoteAsync(dto, boletaLoteIv);

            #endregion

            await GuardarAsync(dto, false);

            return new OperacionDto<FiscalizacionPetroPeruDto>(dto);
        }


        private async Task<List<FactorAsignacionLiquidoGasNaturalDto>> ObtenerFactorAsignacionLiquidoGasNatural(double volumenTotalProduccion, double contenidoLgn)
        {
            var lista = new List<FactorAsignacionLiquidoGasNaturalDto>();
            var entidades = await _boletaDiariaFiscalizacionRepositorio.ListarFactorAsignacionLiquidoGasNaturalAsync(FechasUtilitario.ObtenerDiaOperativo(), (int)TiposDatos.VolumenMpcd, (int)TiposDatos.Riqueza, (int)TiposDatos.PoderCalorifico);
            lista = entidades.Select(e => new FactorAsignacionLiquidoGasNaturalDto()
            {
                Item = e.Item,
                Suministrador = e.Suministrador,
                Volumen = e.Volumen,
                Riqueza = e.Riqueza,
            }).ToList();

            lista.ForEach(e => e.VolumenRiqueza = Math.Round(e.Volumen * e.Riqueza, 2));
            
            if (contenidoLgn > 0)
            {
                lista.ForEach(e => e.Factor = ((e.VolumenRiqueza / contenidoLgn) / 42) * 100);// 42 es fijo
                lista.ForEach(e => e.Asignacion = Math.Round((volumenTotalProduccion * ((e.VolumenRiqueza / contenidoLgn) / 42) * 100) / 100, 2));
            }
            
            
            lista.Add(new FactorAsignacionLiquidoGasNaturalDto
            {
                Item = (lista.Count + 1),
                Suministrador = "Total",
                Volumen = Math.Round(lista.Sum(e => e.Volumen), 4),
                Riqueza = Math.Round(lista.Sum(e => e.VolumenRiqueza) / lista.Sum(e => e.Volumen), 4),
                Asignacion = Math.Round(lista.Sum(e => e.Asignacion), 4),
                Factor = lista.Sum(e => e.Factor),
                VolumenRiqueza = Math.Round(lista.Sum(e => e.VolumenRiqueza), 2)
            });
            lista.ForEach(e => e.Factor = Math.Round(e.Factor, 4));
            return lista;
        }

        // Cuadro N° 2
        private async Task<List<DistribucionGasNaturalSecoDto>> ObtenerDistribucionGasNaturalSecoAsync(FiscalizacionPetroPeruDto parametros)
        {
            var lista = new List<DistribucionGasNaturalSecoDto>();
            var entidades = await _boletaDiariaFiscalizacionRepositorio.ListarFactorAsignacionLiquidoGasNaturalAsync(FechasUtilitario.ObtenerDiaOperativo(), (int)TiposDatos.VolumenMpcd, (int)TiposDatos.Riqueza, (int)TiposDatos.PoderCalorifico);
            lista = entidades.Select(e => new DistribucionGasNaturalSecoDto()
            {
                Item = e.Item,
                Suministrador = e.Suministrador,
                VolumenGna = e.Volumen,
                PoderCalorifico = e.Calorifico,
            }).ToList();

            if (parametros.FactorAsignacionLiquidoGasNatural != null && parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item == 1).FirstOrDefault() != null)
            {
                double volumenGns = Math.Round((parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item == 1).First().Asignacion * 42 * parametros.FactorConversionZ69 / 1000), 4);
                lista.Where(w => w.Item == 1).ToList().ForEach(s => s.VolumenGns = volumenGns);
            }

            if (parametros.FactorAsignacionLiquidoGasNatural != null && parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item == 2).FirstOrDefault() != null)
            {
                double volumenGns = Math.Round((parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item == 2).First().Asignacion * 42 * parametros.FactorConversionVi / 1000), 4);
                lista.Where(w => w.Item == 2).ToList().ForEach(s => s.VolumenGns = volumenGns);
            }

            if (parametros.FactorAsignacionLiquidoGasNatural != null && parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item == 3).FirstOrDefault() != null)
            {
                double volumenGns = Math.Round((parametros.FactorAsignacionLiquidoGasNatural.Where(e => e.Item == 3).First().Asignacion * 42 * parametros.FactorConversionI / 1000), 4);
                lista.Where(w => w.Item == 3).ToList().ForEach(s => s.VolumenGns = volumenGns);
            }

            lista.ForEach(e => e.VolumenGnsd = Math.Round(e.VolumenGna - e.VolumenGns,4));

            lista.Add(new DistribucionGasNaturalSecoDto
            {
                Item = (lista.Count + 1),
                Suministrador = "Total",
                VolumenGna = lista.Sum(e => e.VolumenGna),
                PoderCalorifico = Math.Round(lista.Sum(e => e.VolumenGnaPoderCalorifico) / lista.Sum(e => e.VolumenGna), 2),
                VolumenGns = Math.Round(lista.Sum(e => e.VolumenGns), 4),
                VolumenGnsd = Math.Round(lista.Sum(e => e.VolumenGnsd), 4)
            });

            return lista;
        }


        // Cuadro N° 3
        private List<VolumenTransferidoRefineriaPorLoteDto> VolumenTransferidoRefineriaPorLoteAsync(FiscalizacionPetroPeruDto parametros, BoletaDeterminacionVolumenGnaDto boletaLoteIv)
        {
            var lista = new List<VolumenTransferidoRefineriaPorLoteDto>();

            if (parametros.DistribucionGasNaturalSeco == null || parametros.DistribucionGasNaturalSeco.Count == 0)
            {
                return lista;
            }

            lista = parametros.DistribucionGasNaturalSeco.Select(e => new VolumenTransferidoRefineriaPorLoteDto()
            {
                Item = e.Item,
                Suministrador = e.Suministrador,
                VolumenGns = e.VolumenGnsd,
            }).ToList();
            double LoteI = 0;
            double LoteIv = 0;
            double LoteVi = 0;
            if (boletaLoteIv?.FactoresAsignacionGasCombustible?.Where(e => e.Item == 1).FirstOrDefault() != null)
            {
                LoteI = boletaLoteIv.FactoresAsignacionGasCombustible.Where(e => e.Item == 1).FirstOrDefault().EnergiaMmbtu;
            }
            if (boletaLoteIv?.FactoresAsignacionGasCombustible?.Where(e => e.Item == 3).FirstOrDefault() != null)
            {
                LoteIv = boletaLoteIv.FactoresAsignacionGasCombustible.Where(e => e.Item == 3).FirstOrDefault().EnergiaMmbtu;
            }
            if (boletaLoteIv?.FactoresAsignacionGasCombustible?.Where(e => e.Item == 4).FirstOrDefault() != null)
            {
                LoteVi = boletaLoteIv.FactoresAsignacionGasCombustible.Where(e => e.Item == 4).FirstOrDefault().EnergiaMmbtu;
            }

            foreach (var item in lista)
            {
                switch (item.Item)
                {
                    case 1:
                        item.VolumenFlare = (LoteI / (LoteI + LoteIv + LoteVi));
                        break;
                    case 2:
                        item.VolumenFlare = (LoteIv / (LoteI + LoteIv + LoteVi));
                        break;
                    case 3:
                        item.VolumenFlare = (LoteVi / (LoteI + LoteIv + LoteVi));
                        break;
                }                    
            }
            lista.ForEach(e => e.VolumenFlare = Math.Round(e.VolumenFlare*parametros.VolumenTotalGnsFlare??0, 4));

            lista = lista.Where(e => !e.Suministrador.Equals("Total")).ToList();
            lista.ForEach(e => e.VolumenGnsTransferido = Math.Round(e.VolumenGns - e.VolumenFlare, 4));

            lista.Add(new VolumenTransferidoRefineriaPorLoteDto
            {
                Item = (lista.Count + 1),
                Suministrador = "Total",
                VolumenGns = Math.Round(lista.Sum(e => e.VolumenGns), 4),
                VolumenFlare = Math.Round(lista.Sum(e => e.VolumenFlare), 4),
                VolumenGnsTransferido = Math.Round(lista.Sum(e => e.VolumenGnsTransferido), 4)
            });

            return lista;
        }






        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(FiscalizacionPetroPeruDto peticion, bool esEditado)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            if (esEditado)
            {
                var distribucionGasNaturalSeco = peticion.DistribucionGasNaturalSeco.Where(e => e.Suministrador.Equals("Total")).FirstOrDefault();
                if (peticion.VolumenTotalGns.HasValue)
                {
                    peticion.VolumenTotalGnsFlare = Math.Round((distribucionGasNaturalSeco.VolumenGnsd - peticion.VolumenTotalGns ?? 0), 4);
                }

                var boletaLoteIv = new BoletaDeterminacionVolumenGnaDto();
                var operacionBoletaLoteIv = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(peticion.IdUsuario ?? 0);
                if (operacionBoletaLoteIv.Completado && operacionBoletaLoteIv.Resultado != null)
                {
                    boletaLoteIv = operacionBoletaLoteIv.Resultado;
                }
                peticion.VolumenTransferidoRefineriaPorLote = VolumenTransferidoRefineriaPorLoteAsync(peticion, boletaLoteIv);

            }

            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletaDiariaFiscalizacionPetroPeru),
                Fecha = FechasUtilitario.ObtenerDiaOperativo(),
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                EsEditado = esEditado
            };
            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(11,3);
            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(21, 3);
            return await _impresionServicio.GuardarAsync(dto);
        }
    }
}
