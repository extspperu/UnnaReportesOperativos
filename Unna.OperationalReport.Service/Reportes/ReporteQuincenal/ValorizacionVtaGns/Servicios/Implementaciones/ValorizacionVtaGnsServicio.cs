using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Repositorios.Implementaciones;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Servicios.Implementaciones
{
    public class ValorizacionVtaGnsServicio : IValorizacionVtaGnsServicio
    {
        private readonly IImpresionServicio _impresionServicio;
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly IMensualRepositorio _mensualRepositorio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IValoresDefectoReporteServicio _valoresDefectoReporteServicio;

        public ValorizacionVtaGnsServicio(IRegistroRepositorio registroRepositorio,
            IImpresionServicio impresionServicio, IImprimirRepositorio imprimirRepositorio,
            IMensualRepositorio mensualRepositorio,
            IReporteServicio reporteServicio,
            IValoresDefectoReporteServicio valoresDefectoReporteServicio
            )
        {
            //_registroRepositorio = registroRepositorio;
            _impresionServicio = impresionServicio;
            _imprimirRepositorio = imprimirRepositorio;
            _mensualRepositorio = mensualRepositorio;
            _reporteServicio = reporteServicio;
            _valoresDefectoReporteServicio = valoresDefectoReporteServicio;
        }
        public async Task<OperacionDto<ValorizacionVtaGnsDto>> ObtenerAsync(long idUsuario, string? grupo)
        {
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.ValorizacionVentaGNSGasNORP, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<ValorizacionVtaGnsDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            DateTime? desde = default(DateTime?);
            DateTime? hasta = default(DateTime?);
            switch (grupo)
            {
                case GruposReportes.Quincenal:
                    if (diaOperativo.Day < 16) diaOperativo = diaOperativo.AddMonths(-1);
                    desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
                    hasta = new DateTime(diaOperativo.Year, diaOperativo.Month, 15);
                    break;
                case GruposReportes.Mensual:
                    DateTime mensual = diaOperativo.AddMonths(-1);
                    desde = new DateTime(mensual.Year, mensual.Month, 16);
                    hasta = new DateTime(mensual.Year, mensual.Month, 1).AddMonths(1).AddDays(-1);
                    break;
            }
            if (!desde.HasValue || !hasta.HasValue)
            {
                return new OperacionDto<ValorizacionVtaGnsDto>(CodigosOperacionDto.NoExiste, "No se obtuvo la fecha de inicio");
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.ValorizacionVentaGNSGasNORP, desde.Value);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos) && operacionImpresion.Resultado.EsEditado)
            {
                var rpta = JsonConvert.DeserializeObject<ValorizacionVtaGnsDto>(operacionImpresion.Resultado.Datos);
                return new OperacionDto<ValorizacionVtaGnsDto>(rpta);
            }

            var valorizacionVta = await _mensualRepositorio.BuscarValorizacionQuincenalVentaGnsLoteIvAsync(desde, hasta);

            if (valorizacionVta == null)
            {
                return new OperacionDto<ValorizacionVtaGnsDto>(CodigosOperacionDto.NoExiste, "No se pudo obtener registros");
            }

            var valorizacionVtaDto = valorizacionVta.Select(e => new ValorizacionVtaGnsDetDto
            {
                Item = e.Fecha.HasValue ? e.Fecha.Value.Day : new int?(),
                Fecha = e.Fecha.HasValue ? e.Fecha.Value.ToString("dd/MM/yyyy") : null,
                Energia = e.Energia,
                Costo = e.Costo,
                PoderCal = e.Calorifico,
                Precio = e.Precio,
                Volumen = e.Volumen
            }).ToList();

            var dto = new ValorizacionVtaGnsDto
            {
                Periodo = $"Del {desde.Value.Day} al {hasta.Value.Day} de {FechasUtilitario.ObtenerNombreMes(hasta.Value)} {desde.Value.Year}",
                PuntoFiscal = "MS-9225",
                RutaFirma = operacionGeneral?.Resultado?.RutaFirma,
                UrlFirma = operacionGeneral?.Resultado?.UrlFirma,
                NombreReporte = operacionGeneral?.Resultado?.NombreReporte
            };

            dto.TotalVolumen = Math.Round(valorizacionVtaDto.Sum(d => d.Volumen) ?? 0.0, 2);
            dto.TotalPoderCal = Math.Round(valorizacionVtaDto.Average(d => d.PoderCal) ?? 0.0, 2);
            dto.TotalEnergia = Math.Round(valorizacionVtaDto.Sum(d => d.Energia) ?? 0.0, 2);
            dto.TotalPrecio = Math.Round(valorizacionVtaDto.Average(d => d.Precio) ?? 0.0, 2);
            dto.TotalCosto = Math.Round(valorizacionVtaDto.Sum(d => d.Costo) ?? 0.0, 2);

            valorizacionVtaDto.ForEach(e => e.Energia = Math.Round(e.Energia ?? 0, 2));
            valorizacionVtaDto.ForEach(e => e.Costo = Math.Round(e.Costo ?? 0, 2));
            valorizacionVtaDto.ForEach(e => e.PoderCal = Math.Round(e.PoderCal ?? 0, 2));
            valorizacionVtaDto.ForEach(e => e.Precio = Math.Round(e.Precio ?? 0, 2));
            valorizacionVtaDto.ForEach(e => e.Volumen = Math.Round(e.Volumen ?? 0, 2));


            dto.ValorizacionVtaGnsDet = valorizacionVtaDto;

            double? igv = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.Igv);
            dto.SubTotalFact = dto.TotalCosto ?? 0;
            dto.EnerVolTransM = dto.TotalEnergia ?? 0;

            dto.IgvCentaje = igv ?? 0;
            if (igv.HasValue)
            {
                dto.Igv = Math.Round(igv.Value / 100 * dto.SubTotalFact, 2);
            }
            dto.TotalFact = Math.Round(dto.SubTotalFact + dto.Igv, 2);

            return new OperacionDto<ValorizacionVtaGnsDto>(dto);
        }



        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(ValorizacionVtaGnsDto peticion)
        {

            var diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            DateTime? desde = default(DateTime?);
            DateTime? hasta = default(DateTime?);
            switch (peticion.Grupo)
            {
                case GruposReportes.Quincenal:
                    desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
                    hasta = new DateTime(diaOperativo.Year, diaOperativo.Month, 15);
                    break;
                case GruposReportes.Mensual:
                    DateTime mensual = diaOperativo.AddMonths(-1);
                    desde = new DateTime(mensual.Year, mensual.Month, 16);
                    hasta = new DateTime(mensual.Year, mensual.Month, 1).AddMonths(1).AddDays(-1);
                    break;
            }
            if (!desde.HasValue || !hasta.HasValue)
            {
                return new OperacionDto<RespuestaSimpleDto<string>>(CodigosOperacionDto.NoExiste, "No se obtuvo la fecha de inicio");
            }

            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ValorizacionVentaGNSGasNORP),
                Fecha = desde.Value,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                Comentario = peticion.Comentario,
                EsEditado = true,
            };

            return await _impresionServicio.GuardarAsync(dto);
        }
    }
}
