using Newtonsoft.Json;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Mensual.Entidades;
using Unna.OperationalReport.Data.Mensual.Procedimientos;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Servicios.Implementaciones
{
    public class CalculoFacturaCpgnaFee50Servicio : ICalculoFacturaCpgnaFee50Servicio
    {
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IValoresDefectoReporteServicio _valoresDefectoReporteServicio;
        private readonly ITipoCambioServicio _tipoCambioServicio;
        private readonly IPeriodoPrecioGlpRepositorio _periodoPrecioGlpRepositorio;
        private readonly IBalanceEnergiaDiariaRepositorio _balanceEnergiaDiariaRepositorio;
        private readonly IMensualRepositorio _mensualRepositorio;
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;
        public CalculoFacturaCpgnaFee50Servicio(
            IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio,
            IValoresDefectoReporteServicio valoresDefectoReporteServicio,
            ITipoCambioServicio tipoCambioServicio,
            IPeriodoPrecioGlpRepositorio periodoPrecioGlpRepositorio,
            IBalanceEnergiaDiariaRepositorio balanceEnergiaDiariaRepositorio,
            IMensualRepositorio mensualRepositorio,
            ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio
            )
        {
            _impresionServicio = impresionServicio;
            _reporteServicio = reporteServicio;
            _valoresDefectoReporteServicio = valoresDefectoReporteServicio;
            _tipoCambioServicio = tipoCambioServicio;
            _periodoPrecioGlpRepositorio = periodoPrecioGlpRepositorio;
            _balanceEnergiaDiariaRepositorio = balanceEnergiaDiariaRepositorio;
            _mensualRepositorio = mensualRepositorio;
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> EliminarPrecioAsync(string id)
        {
            var idPeriodo = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(id);
            var entidad = await _periodoPrecioGlpRepositorio.BuscarPorIdAsync(idPeriodo);
            if (entidad == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "No existe el registro");
            }

            await _periodoPrecioGlpRepositorio.EliminarAsync(entidad);

            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true, Mensaje = "Se guaardó correctamente" });

        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarPrecioAsync(PrecioGlpPeriodo peticion)
        {
            var id = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(peticion.Id);
            var entidad = await _periodoPrecioGlpRepositorio.BuscarPorIdAsync(id);
            if (entidad == null)
            {
                entidad = new PeriodoPrecioGlp();
                entidad.Periodo = new DateTime(peticion.DiaOperativo.Year, peticion.DiaOperativo.Month, 1);
            }
            entidad.Desde = peticion.Desde;
            entidad.Hasta = peticion.Hasta;
            entidad.PrecioKg = peticion.PrecioKg;
            entidad.NroDia = peticion.NroDias;
            if (entidad.IdPeriodoPrecioGlp > 0)
            {
                await _periodoPrecioGlpRepositorio.EditarAsync(entidad);
            }
            else
            {
                await _periodoPrecioGlpRepositorio.InsertarAsync(entidad);
            }

            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true, Mensaje = "Se guaardó correctamente" });

        }

        public async Task<OperacionDto<List<PrecioGlpPeriodo>?>> ListarPeriodoPreciosAsync(DateTime diaOperativo)
        {
            DateTime periodo = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
            var entidadPeriodoPrecioGlp = await _periodoPrecioGlpRepositorio.ListarPorPeriodoAsync(periodo);
            var precioGlpPeriodos = entidadPeriodoPrecioGlp?.Select(e => new PrecioGlpPeriodo
            {
                Id = RijndaelUtilitario.EncryptRijndaelToUrl(e.IdPeriodoPrecioGlp),
                Desde = e.Desde,
                Hasta = e.Hasta,
                PrecioKg = e.PrecioKg,
                NroDias = e.NroDia ?? 0,
            }).ToList();
            return new OperacionDto<List<PrecioGlpPeriodo>?>(precioGlpPeriodos);

        }

        public async Task<OperacionDto<CalculoFacturaCpgnaFee50Dto>> ObtenerAsync(long idUsuario, DateTime diaOperativo)
        {

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.CalculoFacturaCpgnaFee50, diaOperativo);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<CalculoFacturaCpgnaFee50Dto>(operacionImpresion.Resultado.Datos);
                return new OperacionDto<CalculoFacturaCpgnaFee50Dto>(rpta);
            }

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.CalculoFacturaCpgnaFee50, idUsuario);
            if (!operacionGeneral.Completado && operacionGeneral.Resultado == null)
            {
                return new OperacionDto<CalculoFacturaCpgnaFee50Dto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            DateTime desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
            DateTime hasta = new DateTime(diaOperativo.Year, diaOperativo.Month, 1).AddMonths(1).AddDays(-1);

            string? nombreMes = FechasUtilitario.ObtenerNombreMes(desde).ToUpper();
            var dto = new CalculoFacturaCpgnaFee50Dto
            {
                NombreReporte = $"{operacionGeneral?.Resultado?.NombreReporte} - MES DE {nombreMes} {hasta.Year}",
                RutaFirma = operacionGeneral?.Resultado?.RutaFirma,
                GravedadEspecifica = 0.552, // Es un valor fijo
                Factor = 3.785              // Es un valor fijo
            };

            #region Segunda pestaña de registro
            var resumenEntregaLista = await _mensualRepositorio.ListarFactura50VolumenEntregadaAsync(desde, hasta);
            var resumenEntrega = resumenEntregaLista.Select(e => new ResumenEntregaDto
            {
                Item = e.Id,
                Fecha = e.Fecha,
                Cnpc = e.Cnpc,
                Total = e.Total,
                NoProcesado = e.NoProcesado,
                PlantaFs = e.PlantaFs,
                AjustePlantaFs = e.AjustePlantaFs,
                Procesado = e.Procesado,
                ProcesadoHoras = e.ProcesadoHoras
            }).ToList();

            double horasProcesado = resumenEntrega.Sum(e => e.ProcesadoHoras ?? 0);
            double procesado = resumenEntrega.Sum(e => e.Procesado ?? 0);
            resumenEntrega.Add(new ResumenEntregaDto
            {
                Item = (resumenEntrega.Count + 1),
                Fecha = "TOTAL",
                Cnpc = resumenEntrega.Sum(e => e.Cnpc),
                Total = resumenEntrega.Sum(e => e.Total),
                NoProcesado = resumenEntrega.Sum(e => e.NoProcesado),
                PlantaFs = resumenEntrega.Sum(e => e.PlantaFs),
                AjustePlantaFs = resumenEntrega.Sum(e => e.AjustePlantaFs),
                Procesado = procesado,
                ProcesadoHoras = horasProcesado
            });
            dto.ResumenEntrega = resumenEntrega;
            dto.NumeroDiasPeriodo = horasProcesado / 24;
            if (dto.NumeroDiasPeriodo > 0)
            {
                dto.Mmpcd = (procesado / dto.NumeroDiasPeriodo) / 1000;
            }


            var barrilesLista = await _mensualRepositorio.ListarFactura50BarrilesAsync(desde, hasta);
            var barriles = barrilesLista.Select(e => new BarrilesProductoDto
            {
                Item = e.Id,
                Fecha = e.Fecha,
                Glp = e.Glp,
                Cgn = e.Cgn,
                Total = e.Glp + e.Cgn,
            }).ToList();
            double totalBarriles = barriles.Sum(e => e.Total ?? 0);
            double totalGlp = barriles.Sum(e => e.Glp ?? 0);
            double totalCgn = barriles.Sum(e => e.Cgn ?? 0);
            barriles.Add(new BarrilesProductoDto
            {
                Item = (barriles.Count + 1),
                Fecha = "TOTAL",
                Glp = totalGlp,
                Cgn = totalCgn,
                Total = totalBarriles,
            });
            dto.BarrilesProducto = barriles;
            dto.CentajeTotal = 100;
            if (totalBarriles > 0)
            {
                dto.CentajeCgn = (totalCgn / totalBarriles) * 100;
                dto.CentajeGlp = (totalGlp / totalBarriles) * 100;
            }


            #endregion

            #region A) Determinación del PRef - (Precio de Lista del GLP de la Refinería de PETROPERU en Talara)

            var entidadPeriodoPrecioGlp = await ListarPeriodoPreciosAsync(diaOperativo);
            if (entidadPeriodoPrecioGlp.Completado && entidadPeriodoPrecioGlp.Resultado != null)
            {
                dto.PrecioGlp = entidadPeriodoPrecioGlp.Resultado;
                dto.PrefPromedioPeriodo = dto.PrecioGlp.Sum(e => e.PrecioKg * e.NroDias ?? 0) / dto.PrecioGlp.Sum(e => e.NroDias ?? 0);
            }

            var operacionTipoCambio = await _tipoCambioServicio.ListarPorFechasAsync(desde, hasta, (int)TiposMonedas.Soles);
            if (operacionTipoCambio.Completado && operacionTipoCambio.Resultado != null)
            {
                dto.TipoCambio = operacionTipoCambio.Resultado;
                dto.TipoCambioPromedio = Math.Round(operacionTipoCambio.Resultado.Sum(e => e.Cambio) / operacionTipoCambio.Resultado.Count, 3);
            }
            dto.PrefPeríodo = Math.Round(dto.PrefPromedioPeriodo * dto.GravedadEspecifica * dto.Factor * 42 / dto.TipoCambioPromedio, 2);

            #endregion
            #region B) Determinación del Precio de los Componentes Pesados.

            dto.Vglp = Math.Round(totalGlp, 2);
            dto.Vhas = Math.Round(totalCgn, 2);
            dto.Pref = Math.Round(dto.PrefPeríodo, 2);
            if ((dto.Vglp + dto.Vhas) > 0)
            {
                dto.Precio = Math.Round(0.95 * dto.Pref * (dto.Vglp + 1.2 * dto.Vhas) / (dto.Vglp + dto.Vhas), 2);
            }


            #endregion
            #region C) Determinación del Precio de los Componentes Pesados.

          
            dto.Vtotal = Math.Round(totalBarriles, 2);
            dto.VolumenProcesamientoGna = Math.Round(dto.Mmpcd, 2);
            dto.PrecioDeterminacion = Math.Round(dto.Precio, 2);
            dto.Cm = 50;// valor fijo
            #endregion


            double? igvCentaje = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.Igv);

            #region D) Determinación de la Facturación por la Contraprestación del Suministro de Componentes 

            dto.PrecioFacturacion = Math.Round(dto.Precio, 2);
            dto.PSec = 0.76;// valor fijo             
            dto.CmPrecioPsec = Math.Round(dto.Precio * dto.Cm / 100 + dto.PSec ?? 0, 2);

            dto.ImporteCmEep = Math.Round(dto.Vtotal * dto.CmPrecioPsec, 2);
            dto.IgvCmEep = Math.Round(dto.ImporteCmEep * (igvCentaje / 100) ?? 0, 2) ;
            dto.MontoTotalCmEep = Math.Round(dto.ImporteCmEep + dto.IgvCmEep , 2);

            dto.PrecioSecado = Math.Round(dto.PSec * dto.Vtotal, 2);
            dto.Igv = Math.Round(dto.PrecioSecado * (igvCentaje / 100) ?? 0 , 2);
            dto.Total = Math.Round(dto.PrecioSecado + dto.Igv, 2);

            #endregion

            

            return new OperacionDto<CalculoFacturaCpgnaFee50Dto>(dto);
        }




        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(CalculoFacturaCpgnaFee50Dto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            DateTime fecha = new DateTime(FechasUtilitario.ObtenerDiaOperativo().Year, FechasUtilitario.ObtenerDiaOperativo().Month, 1);
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.CalculoFacturaCpgnaFee50),
                Fecha = fecha,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
            };

            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(41,1);
            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(51,1);
            return await _impresionServicio.GuardarAsync(dto);
        }
    }
}
