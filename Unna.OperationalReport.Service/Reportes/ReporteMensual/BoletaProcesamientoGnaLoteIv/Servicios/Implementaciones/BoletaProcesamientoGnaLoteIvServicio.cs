using DocumentFormat.OpenXml.Drawing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Servicios.Implementaciones
{
    public class BoletaProcesamientoGnaLoteIvServicio : IBoletaProcesamientoGnaLoteIvServicio
    {


        private readonly IViewValoresIngresadosPorFechaRepositorio _viewValoresIngresadosPorFechaRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IValoresDefectoReporteServicio _valoresDefectoReporteServicio;
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;
        public BoletaProcesamientoGnaLoteIvServicio(
            IViewValoresIngresadosPorFechaRepositorio viewValoresIngresadosPorFechaRepositorio,
            IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio,
            IValoresDefectoReporteServicio valoresDefectoReporteServicio,
            ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio
            )
        {
            _viewValoresIngresadosPorFechaRepositorio = viewValoresIngresadosPorFechaRepositorio;
            _impresionServicio = impresionServicio;
            _reporteServicio = reporteServicio;
            _valoresDefectoReporteServicio = valoresDefectoReporteServicio;
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
        }

        public async Task<OperacionDto<BoletaProcesamientoGnaLoteIvDto>> ObtenerAsync(long idUsuario)
        {
            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddDays(1).AddMonths(-1);

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaMensualProcesamientoGnaLoteIv, diaOperativo);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<BoletaProcesamientoGnaLoteIvDto>(operacionImpresion.Resultado.Datos);
                return new OperacionDto<BoletaProcesamientoGnaLoteIvDto>(rpta);
            }

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaMensualProcesamientoGnaLoteIv, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletaProcesamientoGnaLoteIvDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            DateTime desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
            DateTime hasta = new DateTime(diaOperativo.Year, diaOperativo.Month, 1).AddMonths(1).AddDays(-1);

            var registros = await _viewValoresIngresadosPorFechaRepositorio.ListarPorLoteYFechasAsync((int)TiposLote.LoteIv, desde, hasta);

            var lista = registros.Select(e => new DatosProcesamientoLoteiVDto
            {
                Dia = $"{e.Fecha.Day}-{FechasUtilitario.ObtenerNombreMesAbrev(e.Fecha)}-{e.Fecha.Year}",
                Volumen = e.Volumen ?? 0,
                PoderCalorifico = e.Calorifico ?? 0,
            }).ToList();
            lista.ForEach(e => e.Energia = Math.Round(e.Volumen * e.PoderCalorifico / 1000, 4));

            for (var i = 0; i < lista.Count; i++)
            {
                lista[i].Id = i + 1;
            }

            double? precioUsd = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.PrecioUsdMmbtu);
            double? igv = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.Igv);

            var dto = new BoletaProcesamientoGnaLoteIvDto
            {
                NombreReporte = operacionGeneral.Resultado?.NombreReporte,
                UrlFirma = operacionGeneral.Resultado?.UrlFirma,
                RutaFirma = operacionGeneral.Resultado.RutaFirma,
                Anio = diaOperativo.Year,
                Mes = FechasUtilitario.ObtenerNombreMes(diaOperativo),
                TotalEnergia = lista.Sum(e => e.Energia),
                TotalPc = Math.Round(lista.Sum(e => e.Energia) / lista.Sum(e => e.Volumen) * 1000, 4),
                TotalVolumen = lista.Sum(e => e.Volumen),
                Valores = lista,
                EnergiaVolumenProcesado = Math.Round(lista.Sum(e => e.Energia), 4),
                PrecioUsd = precioUsd ?? 0,
                SubTotal = precioUsd * lista.Sum(e => e.Energia) ?? 0
            };
            dto.IgvCentaje = igv ?? 0;
            dto.Igv = Math.Round((dto.IgvCentaje / 100) * dto.SubTotal, 4);
            dto.TotalFacturar = Math.Round(dto.Igv + dto.SubTotal, 4);
            dto.SubTotal = Math.Round(dto.SubTotal, 4);
            return new OperacionDto<BoletaProcesamientoGnaLoteIvDto>(dto);
        }


        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletaProcesamientoGnaLoteIvDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            DateTime fecha = FechasUtilitario.ObtenerDiaOperativo();
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletaMensualProcesamientoGnaLoteIv),
                Fecha = fecha,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
            };

            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(38, 3);
            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(48, 3);
            return await _impresionServicio.GuardarAsync(dto);
        }
    }
}
