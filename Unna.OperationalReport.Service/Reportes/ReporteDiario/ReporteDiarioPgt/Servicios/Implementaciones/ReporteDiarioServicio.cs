using Newtonsoft.Json;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
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
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos;
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
        private readonly IReporteDiariaDatosRepositorio _reporteDiariaDatosRepositorio;
        private readonly IBoletaDeterminacionVolumenGnaServicio _boletaDeterminacionVolumenGnaServicio;
        private readonly IGnsVolumeMsYPcBrutoRepositorio _gnsVolumeMsYPcBrutoRepositorio;
        private readonly IBoletaEnelRepositorio _boletaEnelRepositorio;
        private readonly IDiarioPgtProduccionRepositorio _diarioPgtProduccionRepositorio;

        public ReporteDiarioServicio(
            IReporteServicio reporteServicio,
            IImpresionServicio impresionServicio,
            IRegistroRepositorio registroRepositorio,
            IFiscalizacionPetroPeruServicio fiscalizacionPetroPeruServicio,
            IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio,
            IVolumenDespachoRepositorio volumenDespachoRepositorio,
            IBoletaCnpcServicio boletaCnpcServicio,
            IBoletaBalanceEnergiaServicio boletaBalanceEnergiaServicio,
            IReporteDiariaDatosRepositorio reporteDiariaDatosRepositorio,
            IBoletaDeterminacionVolumenGnaServicio boletaDeterminacionVolumenGnaServicio,
            IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio,
            IBoletaEnelRepositorio boletaEnelRepositorio,
            IDiarioPgtProduccionRepositorio diarioPgtProduccionRepositorio
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
            _reporteDiariaDatosRepositorio = reporteDiariaDatosRepositorio;
            _boletaDeterminacionVolumenGnaServicio = boletaDeterminacionVolumenGnaServicio;
            _gnsVolumeMsYPcBrutoRepositorio = gnsVolumeMsYPcBrutoRepositorio;
            _diarioPgtProduccionRepositorio = diarioPgtProduccionRepositorio;
        }


        public async Task<OperacionDto<ReporteDiarioDto>> ObtenerAsync(long? idUsuario)
        {
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.ReporteDiarioPgt, idUsuario);
            if (!operacionGeneral.Completado || operacionGeneral.Resultado == null)
            {
                return new OperacionDto<ReporteDiarioDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }
            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.ReporteDiarioPgt, FechasUtilitario.ObtenerDiaOperativo());
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<ReporteDiarioDto>(operacionImpresion.Resultado.Datos);
                rpta.General = operacionGeneral.Resultado;
                return new OperacionDto<ReporteDiarioDto>(rpta);
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
                Volumen = e.Volumen ?? 0,
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
                EnergiaDiaria =dtoGas.Sum(e => e.EnergiaDiaria ?? 0),
                VolumenPromedio = Math.Round(dtoGas.Sum(e => e.VolumenPromedio ?? 0), 2)
            });
            dtoGas.ForEach(e => e.Volumen = Math.Round(e.Volumen ?? 0, 0));
            dtoGas.ForEach(e => e.VolumenPromedio = Math.Round(e.VolumenPromedio ?? 0, 0));
            dtoGas.ForEach(e => e.EnergiaDiaria = Math.Round(e.EnergiaDiaria ?? 0, 2));
            dto.GasNaturalAsociado = dtoGas;
            dto.HoraPlantaFs = 0; // cero por defecto 
            dto.GasNoProcesado = Math.Round(totalGasProcesado / 24 * dto.HoraPlantaFs ?? 0, 0);
            dto.GasProcesado = Math.Round(totalGasProcesado - dto.GasNoProcesado ?? 0, 0);
            if (dto.GasProcesado.HasValue)
            {
                dto.UtilizacionPlantaParinias = Math.Round((dto.GasProcesado.Value / 44000) * 100, 2);
            }



            #endregion
            var fiscalizacionPetroPeruDto = new FiscalizacionPetroPeruDto();
            var operacionPetroperu = await _fiscalizacionPetroPeruServicio.ObtenerAsync(idUsuario ?? 0);
            if (operacionPetroperu.Completado && operacionPetroperu.Resultado != null)
            {
                dto.EficienciaRecuperacionLgn = operacionPetroperu.Resultado.Eficiencia;
                fiscalizacionPetroPeruDto = operacionPetroperu.Resultado;
            }

            #region 2. DISTRIBUCIÓN DE GAS NATURAL SECO TOTAL (GNS):
            dto.GasNaturalSeco = await GasNaturalSecoDtoAsync(diaOperativo, fiscalizacionPetroPeruDto.VolumenTotalGns ?? 0);


            #endregion

            #region 3. PRODUCCIÓN Y VENTA DE LÍQUIDOS DE GAS NATURAL (LGN)



            var fiscalizacionProducto = await _fiscalizacionProductoProduccionRepositorio.ListarReporteDiarioGasNaturalAsociadoAsync(diaOperativo);
            dto.LiquidosGasNaturalProduccionVentas = fiscalizacionProducto.Select(e => new LiquidosGasNaturalProduccionVentasDto
            {
                ProduccionDiaria = Math.Round(e.ProduccionDiaria ?? 0, 2),
                ProduccionMensual = Math.Round(e.ProduccionMensual ?? 0, 2),
                VentaDiaria = Math.Round(e.VentaDiaria ?? 0, 2),
                VentaMensual = Math.Round(e.VentaMensual ?? 0, 2),
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
            dto.VolumenDespachoCgn = volumenDespachoCgn;


            #endregion 4.  VOLUMEN DE GAS Y PRODUCCIÓN DE GNA ADICIONAL DEL LOTE X (CNPC PERÚ):

            double produccionLoteXGnaTotalCnpcVolumen = 0;
            var volumenGnsVentaVgnsvEnel = await _registroRepositorio.ObtenerValorAsync((int)TiposDatos.GnsVentaUnnaLoteIv, (int)TiposLote.LoteX, diaOperativo, (int)TiposNumeroRegistro.SegundoRegistro);
            if (volumenGnsVentaVgnsvEnel != null)
            {
                produccionLoteXGnaTotalCnpcVolumen = volumenGnsVentaVgnsvEnel.Valor ?? 0;
            }
            var cnpcLoteX = gasNaturalAsociado.Where(e => e.IdLote == (int)TiposLote.LoteX).FirstOrDefault();
            var volumenProduccionLoteXGnaTotalCnpc = new List<VolumenGasProduccionDto>();
            volumenProduccionLoteXGnaTotalCnpc.Add(new VolumenGasProduccionDto
            {
                Item = 1,
                Nombre = "VOLUMEN NOMINADO POR EGPSA",
                Volumen = ((cnpcLoteX != null ? cnpcLoteX.Volumen : 0) - produccionLoteXGnaTotalCnpcVolumen),
            });


            volumenProduccionLoteXGnaTotalCnpc.Add(new VolumenGasProduccionDto
            {
                Item = 2,
                Nombre = "VOLUMEN ADICIONAL DISPONIBLE",
                Volumen = produccionLoteXGnaTotalCnpcVolumen,
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

            var entregaGna = new List<DistribucionVolumenPorderCalorificoDto>();

            var boletaBalanceEnergia = new BoletaBalanceEnergiaDto();
            var operacion = await _boletaBalanceEnergiaServicio.ObtenerAsync(idUsuario ?? 0);
            if (operacion.Completado)
            {
                boletaBalanceEnergia = operacion.Resultado;
                entregaGna = operacion.Resultado?.GnsAEnel;
            }
            double CnpcPeruGnaRecibidoVolumen = 0;
            var volumenGnsVentaVgnsv2Enel = await _registroRepositorio.ObtenerValorAsync((int)TiposDatos.CnpcPeruGnaRecibido, (int)TiposLote.LoteX, diaOperativo, (int)TiposNumeroRegistro.SegundoRegistro);
            if (volumenGnsVentaVgnsv2Enel != null)
            {
                CnpcPeruGnaRecibidoVolumen = volumenGnsVentaVgnsv2Enel.Valor ?? 0;
            }
            dto.VolumenProduccionEnel = await VolumenProduccionEnelAsync(entregaGna, CnpcPeruGnaRecibidoVolumen, diaOperativo);
            dto.VolumenProduccionGasNaturalEnel = VolumenProduccionGasNaturalEnelAsync(boletaBalanceEnergia?.LiquidosBarriles);

            #endregion
            #region 6.  VOLUMEN DE GAS Y PRODUCCIÓN DE PETROPERU (LOTE I, VI y Z-69):


            dto = await VolumenGasYProduccionPetroPeruAsync(dto, fiscalizacionPetroPeruDto, diaOperativo);


            dto.GasAlfare = fiscalizacionPetroPeruDto.VolumenTotalGnsFlare;

            #endregion

            #region 7.  VOLUMEN DE GAS Y PRODUCCIÓN UNNA ENERGIA LOTE IV:

            dto = await VolumenGasYProduccionUnnaEnergiaLoteIvAsync(dto, idUsuario ?? 0, diaOperativo);

            #endregion

            //await _diarioPgtProduccionRepositorio.BuscarPorIdYNoBorradoAsync();


            return new OperacionDto<ReporteDiarioDto>(dto);
        }

        private async Task<List<VolumenGasProduccionDto>> VolumenProduccionEnelAsync(List<DistribucionVolumenPorderCalorificoDto>? entregaGna, double? volumenGnsVentaVgnsvEnel, DateTime diaOperativo)
        {
            var lista = new List<VolumenGasProduccionDto>();

            lista.Add(new VolumenGasProduccionDto
            {
                Item = 1,
                Nombre = "RECEPCIÓN DE GNA (RENOMINADO LOTE X)",
                Volumen = volumenGnsVentaVgnsvEnel,
            });

            lista.Add(new VolumenGasProduccionDto
            {
                Item = 2,
                Nombre = "GNS A ENEL",
                Volumen = entregaGna?.Count > 0 ? entregaGna.First().Volumen : 0,
            });

            lista.Add(new VolumenGasProduccionDto
            {
                Item = 4,
                Nombre = "HUMEDAD DE GNA",
                Volumen = entregaGna.Where(e => e.Item == 4).FirstOrDefault() != null ? entregaGna?.Where(e => e.Item == 4).FirstOrDefault().Volumen : 0,
            });

            lista.Add(new VolumenGasProduccionDto
            {
                Item = 5,
                Nombre = "GAS AL FLARE",
                Volumen = entregaGna.Where(e => e.Item == 5).FirstOrDefault() != null ? entregaGna.Where(e => e.Item == 5).FirstOrDefault().Volumen : 0,
            });

            var hotoil = await _boletaEnelRepositorio.ObtenerGnsAEnelAsync(diaOperativo);
            var valorVgc = hotoil.Where(e => e.Id == 1 && e.Nombre == "Consumo Propio UNNA ENERGIA").FirstOrDefault();
            lista.Add(new VolumenGasProduccionDto
            {
                Item = 6,
                Nombre = "GAS COMBUSTIBLE (VGC)",
                Volumen = valorVgc != null ? valorVgc.Volumen : 0,
            });
            lista.Add(new VolumenGasProduccionDto
            {
                Item = lista.Count() + 1,
                Nombre = "TOTAL",
                Volumen = lista.Sum(e => e.Volumen),
            });

            return lista;
        }

        private List<VolumenGasProduccionDto> VolumenProduccionGasNaturalEnelAsync(List<LiquidosBarrilesDto>? liquidosBarriles)
        {
            var lista = new List<VolumenGasProduccionDto>();

            var glpBls = liquidosBarriles?.Where(e => e.Id == 2).FirstOrDefault();
            var cgnBls = liquidosBarriles?.Where(e => e.Id == 3).FirstOrDefault();


            lista.Add(new VolumenGasProduccionDto
            {
                Item = 1,
                Nombre = "GLP (BLS)",
                Volumen = glpBls != null ? glpBls.Enel : 0,
            });
            lista.Add(new VolumenGasProduccionDto
            {
                Item = 2,
                Nombre = "CGN (BLS)",
                Volumen = cgnBls != null ? cgnBls.Enel : 0,
            });
            lista.Add(new VolumenGasProduccionDto
            {
                Item = 3,
                Nombre = "TOTAL",
                Volumen = lista.Sum(e => e.Volumen),
            });
            return lista;
        }



        private async Task<List<GasNaturalSecoDto>> GasNaturalSecoDtoAsync(DateTime diaOperativo, double volumenTotalGns)
        {

            var entidad = await _reporteDiariaDatosRepositorio.ObtenerGasNaturalSecoAsync(diaOperativo, volumenTotalGns);

            var lista = entidad.Select(e => new GasNaturalSecoDto
            {
                Item = e.Id,
                Distribucion = e.Distribucion,
                Calorifico = e.PoderCalorifico,
                EnergiaDiaria = Math.Round(e.EnergiaDiaria ?? 0, 2),
                Volumen = e.VolumenDiaria ?? 0,
                VolumenPromedio = e.PromedioMensual ?? 0
            }).ToList();

            await _reporteDiariaDatosRepositorio.EliminarDistribucionGasNaturalSecoPorFechaAsync(diaOperativo);
            foreach (var item in entidad)
            {
                await _reporteDiariaDatosRepositorio.GuardarDistribucionGasNaturalSecoAsync(item);
            }


            lista.Add(new GasNaturalSecoDto
            {
                Item = (lista.Count + 1),
                Distribucion = "TOTAL",
                Volumen = lista.Sum(e => e.Volumen),
                VolumenPromedio = Math.Round(lista.Sum(e => e.VolumenPromedio), 2),
                EnergiaDiaria = Math.Round(lista.Sum(e => e.EnergiaDiaria), 2)
            });
            lista.ForEach(e => e.Volumen = Math.Round(e.Volumen, 0));
            return lista;
        }


        private async Task<ReporteDiarioDto> VolumenGasYProduccionPetroPeruAsync(ReporteDiarioDto dto, FiscalizacionPetroPeruDto petroPeru, DateTime diaOperativo)
        {
            await Task.Delay(0);
            var lista = new List<VolumenGasProduccionPetroperuDto>();

            var loteZ69Gna = petroPeru.FactorAsignacionLiquidoGasNatural?.Where(e => e.Item == 1).FirstOrDefault();
            var loteViGna = petroPeru.FactorAsignacionLiquidoGasNatural?.Where(e => e.Item == 2).FirstOrDefault();
            var loteIGna = petroPeru.FactorAsignacionLiquidoGasNatural?.Where(e => e.Item == 3).FirstOrDefault();


            var loteZ69Transferido = petroPeru.VolumenTransferidoRefineriaPorLote?.Where(e => e.Item == 1).FirstOrDefault();
            var loteViTransferido = petroPeru.VolumenTransferidoRefineriaPorLote?.Where(e => e.Item == 2).FirstOrDefault();
            var loteITransferido = petroPeru.VolumenTransferidoRefineriaPorLote?.Where(e => e.Item == 3).FirstOrDefault();

            lista.Add(new VolumenGasProduccionPetroperuDto
            {
                Item = 1,
                Suministrador = "LOTE Z-69",
                GnaRecibido = loteZ69Gna != null ? loteZ69Gna.Volumen : 0,
                GnsTrasferido = loteZ69Transferido != null ? loteZ69Transferido.VolumenGnsTransferido : 0
            });

            lista.Add(new VolumenGasProduccionPetroperuDto
            {
                Item = 2,
                Suministrador = "LOTE VI",
                GnaRecibido = loteViGna != null ? loteViGna.Volumen : 0,
                GnsTrasferido = loteViTransferido != null ? loteViTransferido.VolumenGnsTransferido : 0
            });
            lista.Add(new VolumenGasProduccionPetroperuDto
            {
                Item = 3,
                Suministrador = "LOTE I",
                GnaRecibido = loteIGna != null ? loteIGna.Volumen : 0,
                GnsTrasferido = loteITransferido != null ? loteITransferido.VolumenGnsTransferido : 0
            });
            lista.Add(new VolumenGasProduccionPetroperuDto
            {
                Item = 4,
                Suministrador = "TOTAL",
                GnaRecibido = Math.Round(lista.Sum(e => e.GnaRecibido), 4),
                GnsTrasferido = Math.Round(lista.Sum(e => e.GnsTrasferido), 4),
            });

            dto.VolumenProduccionPetroperu = lista;


            var secoProducto = await _reporteDiariaDatosRepositorio.DiarioVolumenLiquidosGasNaturalAsync(diaOperativo, loteZ69Gna != null ? loteZ69Gna.Asignacion : 0, loteViGna != null ? loteViGna.Asignacion : 0, loteIGna != null ? loteIGna.Asignacion : 0);
            var volumenProduccionLoteIvLiquidoGasNatural = secoProducto?.Select(e => new VolumenProduccionLiquidoGasNaturalDto
            {

                Produccion = e.Distribucion,
                LoteZ69 = e.LoteZ69,
                LoteI = e.LoteI,
                LoteVi = e.LoteVi,
                Item = e.Id
            }).ToList();
            volumenProduccionLoteIvLiquidoGasNatural?.Add(new VolumenProduccionLiquidoGasNaturalDto
            {
                Item = (volumenProduccionLoteIvLiquidoGasNatural.Count + 1),
                Produccion = "TOTAL",
                LoteZ69 = Math.Round(volumenProduccionLoteIvLiquidoGasNatural.Sum(e => e.LoteZ69), 2),
                LoteI = Math.Round(volumenProduccionLoteIvLiquidoGasNatural.Sum(e => e.LoteI), 2),
                LoteVi = Math.Round(volumenProduccionLoteIvLiquidoGasNatural.Sum(e => e.LoteVi), 2),
            });
            dto.VolumenProduccionLiquidoGasNatural = volumenProduccionLoteIvLiquidoGasNatural;

            return dto;
        }


        private async Task<ReporteDiarioDto> VolumenGasYProduccionUnnaEnergiaLoteIvAsync(ReporteDiarioDto dto, long idUsuario, DateTime diaOperativo)
        {
            var boletaDeterminacion = new BoletaDeterminacionVolumenGnaDto();
            var operacion = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado)
            {
                boletaDeterminacion = operacion.Resultado;
            }

            var volumenDeGna = boletaDeterminacion?.FactoresAsignacionGasCombustible?.Where(e => e.Item == 5).FirstOrDefault();
            var lista = new List<VolumenGasProduccionDto>();
            lista.Add(new VolumenGasProduccionDto
            {
                Item = 1,
                Nombre = "VOLUMEN DE GNA",
                Volumen = volumenDeGna != null ? volumenDeGna.Volumen : 0
            });

            var volLimaGas = await _gnsVolumeMsYPcBrutoRepositorio.ObtenerPorTipoYNombreDiaOperativoAsync(TiposTablasSupervisorPgt.VolumenVolLimaGas, TiposGnsVolumeMsYPcBruto.VolLimaGas, diaOperativo);
            lista.Add(new VolumenGasProduccionDto
            {
                Item = 2,
                Nombre = "VENTA DE GNS A LIMAGAS",
                Volumen = volLimaGas != null ? volLimaGas.VolumeMs : 0,
            });

            var volGasnorp = await _gnsVolumeMsYPcBrutoRepositorio.ObtenerPorTipoYNombreDiaOperativoAsync(TiposTablasSupervisorPgt.VolumenVolLimaGas, TiposGnsVolumeMsYPcBruto.VolGasnorp, diaOperativo);
            lista.Add(new VolumenGasProduccionDto
            {
                Item = 3,
                Nombre = "VENTA DE GNS A GASNORP",
                Volumen = volGasnorp != null ? volGasnorp.VolumeMs : 0,
            });

            var ventaDeGnsAEnel = await _registroRepositorio.ObtenerValorAsync((int)TiposDatos.GnsVentaUnnaLoteIv, (int)TiposLote.LoteX, diaOperativo, (int)TiposNumeroRegistro.SegundoRegistro);

            lista.Add(new VolumenGasProduccionDto
            {
                Item = 4,
                Nombre = "VENTA DE GNS A ENEL",
                Volumen = ventaDeGnsAEnel != null ? ventaDeGnsAEnel.Valor : 0
            });
            lista.Add(new VolumenGasProduccionDto
            {
                Item = 5,
                Nombre = "GAS COMBUSTIBLE (VGC)",
                Volumen = boletaDeterminacion?.DistribucionGasNaturalAsociado?.GasCombustible
            });
            lista.Add(new VolumenGasProduccionDto
            {
                Item = 6,
                Nombre = "VOLUMEN de GNS equiv. de LGN (VGL)",
                Volumen = boletaDeterminacion?.DistribucionGasNaturalAsociado?.VolumenGns
            });
            lista.Add(new VolumenGasProduccionDto
            {
                Item = 7,
                Nombre = "FLARE",
                Volumen = boletaDeterminacion?.VolumenGnsFlareVgnsrf
            });




            dto.VolumenProduccionLoteIvUnnaEnegia = lista;
            var volumenProduccionLoteIvLiquidoGasNatural = new List<VolumenGasProduccionDto>();
            volumenProduccionLoteIvLiquidoGasNatural.Add(new VolumenGasProduccionDto
            {
                Item = 1,
                Nombre = "GLP (BLS)",
                Volumen = boletaDeterminacion?.VolumenProduccionTotalGlpLoteIv
            });
            volumenProduccionLoteIvLiquidoGasNatural.Add(new VolumenGasProduccionDto
            {
                Item = 2,
                Nombre = "CGN (BLS)",
                Volumen = boletaDeterminacion?.VolumenProduccionTotalCgnLoteIv
            });
            volumenProduccionLoteIvLiquidoGasNatural.Add(new VolumenGasProduccionDto
            {
                Item = volumenProduccionLoteIvLiquidoGasNatural.Count() + 1,
                Nombre = "TOTAL",
                Volumen = Math.Round(volumenProduccionLoteIvLiquidoGasNatural.Sum(e => e.Volumen ?? 0), 2)
            });

            await _diarioPgtProduccionRepositorio.GuardarProduccionAsync( new DiarioPgtProduccion
            {
                Fecha = diaOperativo,
                IdLote = (int)TiposLote.LoteIv,
                Glp = boletaDeterminacion?.VolumenProduccionTotalGlpLoteIv,
                Cgn = boletaDeterminacion?.VolumenProduccionTotalCgnLoteIv
            });
            dto.VolumenProduccionLoteIvLiquidoGasNatural = volumenProduccionLoteIvLiquidoGasNatural;

            return dto;
        }


        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ReporteDiarioDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            peticion.General = null;

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();

            await _reporteDiariaDatosRepositorio.EliminarDistribucionGasNaturalSecoPorFechaAsync(diaOperativo);

            var datos = peticion?.GasNaturalSeco?.Where(e => e.Distribucion != "TOTAL").ToList();

            var entidades = datos.Select(e => new DiarioPgtDistribucionGasNaturalSeco
            {
                Distribucion = e.Distribucion,
                EnergiaDiaria = e.EnergiaDiaria,
                Fecha = diaOperativo,
                Id = e.Item,
                PoderCalorifico = e.Calorifico,
                PromedioMensual = e.VolumenPromedio,
                VolumenDiaria = e.Volumen,

            }).ToList();
            foreach (var item in entidades)
            {
                await _reporteDiariaDatosRepositorio.GuardarDistribucionGasNaturalSecoAsync(item);
            }
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ReporteDiarioPgt),
                Fecha = diaOperativo,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion)
            };
            return await _impresionServicio.GuardarAsync(dto);
        }


    }
}
