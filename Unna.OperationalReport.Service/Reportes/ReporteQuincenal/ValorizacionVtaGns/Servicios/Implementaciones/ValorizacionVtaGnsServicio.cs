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
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using static Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Implementaciones.ResBalanceEnergLIVServicio;

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

            return new OperacionDto<ValorizacionVtaGnsDto>(dto);
        }
        private string GetDayFromID(string id)
        {
            var parts = id.Split('_');
            return parts.Length > 1 ? parts[1] : "01"; // Default to "01" if no day is found
        }
        public class RootObjectVal
        {
            public int IdUsuario { get; set; }
            public string Mes { get; set; }
            public string Anio { get; set; }
            public string Comentario { get; set; }
            public double EnerVolTransM { get; set; }
            public double SubTotalFact { get; set; }
            public double Igv { get; set; }
            public double TotalFact { get; set; }
            public List<MedicionVal> Mediciones { get; set; }
        }
        public class MedicionVal
        {
            public string ID { get; set; }
            public double Valor { get; set; }
        }
        private async Task<List<ValorizacionVtaGnsDetDto>> ValorizacionVtaGnsDet()
        {
            var imprimir = await _imprimirRepositorio.BuscarPorIdConfiguracionYFechaAsync(12, DateTime.UtcNow.Date);
            ValorizacionVtaGnsPost dto = null;
            List<ValorizacionVtaGnsDetDto> ValorizacionVtaGnsDet = new List<ValorizacionVtaGnsDetDto>();

            dto.ValorizacionVtaGnsDet = valorizacionVtaDto;

                DateTime fechaActual = DateTime.Now;

            dto.IgvCentaje = igv ?? 0;
            if (igv.HasValue && dto.SubTotalFact.HasValue)
            {
                dto.Igv = Math.Round(igv.Value / 100 * dto.SubTotalFact.Value, 2);
            }
            dto.TotalFact = Math.Round(dto.SubTotalFact ?? 0 + dto.Igv ?? 0, 2);

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
