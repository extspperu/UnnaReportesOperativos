using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Implementaciones
{
    public class BoletaSuministroGNSdelLoteIVaEnelServicio : IBoletaSuministroGNSdelLoteIVaEnelServicio
    {
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IMensualRepositorio _mensualRepositorio;
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;

        public BoletaSuministroGNSdelLoteIVaEnelServicio
        (
            IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio,
            IMensualRepositorio mensualRepositorio,
            ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio
        )
        {
            _impresionServicio = impresionServicio;
            _reporteServicio = reporteServicio;
            _mensualRepositorio = mensualRepositorio;
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
        }

        public async Task<OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>> ObtenerAsync(long idUsuario)
        {

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaSuministroGNSdelLoteIVaEnel, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }


            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddDays(1).AddMonths(-1);
            DateTime desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
            DateTime hasta = new DateTime(diaOperativo.Year, diaOperativo.Month, 1).AddMonths(1).AddDays(-1);

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaSuministroGNSdelLoteIVaEnel, desde);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos) && operacionImpresion.Resultado.EsEditado)
            {
                var rpta = JsonConvert.DeserializeObject<BoletaSuministroGNSdelLoteIVaEnelDto>(operacionImpresion.Resultado.Datos);
                if (rpta != null)
                {
                    rpta.UrlFirma = operacionGeneral.Resultado?.UrlFirma;
                    rpta.RutaFirma = operacionGeneral.Resultado?.RutaFirma;
                    return new OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>(rpta);
                }
            }

            var registros = await _mensualRepositorio.BuscarBoletaSuministroGnsDeLoteIvAEnelAsync(desde, hasta);
            if (registros == null)
            {
                return new OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>(CodigosOperacionDto.NoExiste, "No se puede obtener");
            }

            var boleta = registros.Select(e => new BoletaSuministroGNSdelLoteIVaEnelDetDto
            {
                Id = e.Fecha.HasValue ? e.Fecha.Value.Day : null,
                Fecha = e.Fecha.HasValue ? $"{e.Fecha.Value.Day}-{FechasUtilitario.ObtenerNombreMesAbrev(e.Fecha.Value)}-{e.Fecha.Value.Year}" : null,
                Volumen = e.Volumen,
                PoderCalorifico = e.PoderCalorifico,
                Energia = e.Energia
            }).ToList();


            var dto = new BoletaSuministroGNSdelLoteIVaEnelDto
            {
                Periodo = $"Del {desde.Day} al {hasta.Day} del {FechasUtilitario.ObtenerNombreMes(desde)} del {desde.Year}",
                TotalVolumen = boleta.Sum(e => e.Volumen),
                TotalPoderCalorifico = boleta.Average(e => e.PoderCalorifico),
                TotalEnergia = boleta.Sum(e => e.Energia),
                TotalEnergiaTransferido = boleta.Sum(e => e.Energia),
                NombreReporte = operacionGeneral.Resultado?.NombreReporte,
                UrlFirma = operacionGeneral.Resultado?.UrlFirma,
                RutaFirma = operacionGeneral.Resultado?.RutaFirma,
                BoletaSuministroGNSdelLoteIVaEnelDet = boleta
            };

            await GuardarAsync(dto, false);

            return new OperacionDto<BoletaSuministroGNSdelLoteIVaEnelDto>(dto);
        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletaSuministroGNSdelLoteIVaEnelDto peticion, bool esEditado)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddDays(1).AddMonths(-1);
            DateTime desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);

            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletaSuministroGNSdelLoteIVaEnel),
                Fecha = desde,
                IdUsuario = peticion.IdUsuario,
                Datos = esEditado ? JsonConvert.SerializeObject(peticion) : null,
                Comentario = peticion.Comentarios,
                EsEditado = esEditado,
            };

            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(35, 1);
            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(45, 1);
            return await _impresionServicio.GuardarAsync(dto);

        }
    }
}