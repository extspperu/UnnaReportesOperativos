using Newtonsoft.Json;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Servicios.Implementaciones
{
    public class BoletaVentaGnsUnnaEnergiaLimagasServicio : IBoletaVentaGnsUnnaEnergiaLimagasServicio
    {

        private readonly IServicioCompresionGnaLimaGasRepositorio _servicioCompresionGnaLimaGasRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IValoresDefectoReporteServicio _valoresDefectoReporteServicio;
        private readonly UrlConfiguracionDto _urlConfiguracion;
        private readonly GeneralDto _general;

        public BoletaVentaGnsUnnaEnergiaLimagasServicio(
            IServicioCompresionGnaLimaGasRepositorio servicioCompresionGnaLimaGasRepositorio,
            IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio,
            IValoresDefectoReporteServicio valoresDefectoReporteServicio,
            UrlConfiguracionDto urlConfiguracion,
            GeneralDto general
            )
        {
            _servicioCompresionGnaLimaGasRepositorio = servicioCompresionGnaLimaGasRepositorio;
            _impresionServicio = impresionServicio;
            _reporteServicio = reporteServicio;
            _valoresDefectoReporteServicio = valoresDefectoReporteServicio;
            _urlConfiguracion = urlConfiguracion;
            _general = general;
        }

        public async Task<OperacionDto<BoletaVentaGnsUnnaEnergiaLimagasDto>> ObtenerAsync(long idUsuario)
        {

            //DateTime fecha = FechasUtilitario.ObtenerDiaOperativo();
            
            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddDays(1).AddMonths(-1);
            DateTime fecha = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaMensualVentaGnsUnnaEnergiaLimagas, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletaVentaGnsUnnaEnergiaLimagasDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaMensualVentaGnsUnnaEnergiaLimagas, fecha);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                if (fecha == operacionImpresion.Resultado.Fecha)
                {
                    var rpta = JsonConvert.DeserializeObject<BoletaVentaGnsUnnaEnergiaLimagasDto>(operacionImpresion.Resultado.Datos);
                    return new OperacionDto<BoletaVentaGnsUnnaEnergiaLimagasDto>(rpta);
                }
            }


            var entidad = await _servicioCompresionGnaLimaGasRepositorio.BuscarPorFechaAsync(fecha);
            if (entidad == null)
            {
                return new OperacionDto<BoletaVentaGnsUnnaEnergiaLimagasDto>(CodigosOperacionDto.NoExiste, "No existe dato cargado para el periodo");
            }

            //var entidades = await _boletaCnpcRepositorio.ListarPorFechaAsync(Inicio, fecha);
            List<BoletaVentaMensualDto> lista = new List<BoletaVentaMensualDto>();
            if (entidad.ServicioCompresionGnaLimaGasVentas != null)
            {
                lista = entidad.ServicioCompresionGnaLimaGasVentas.Select(e => new BoletaVentaMensualDto
                {
                    Fecha = e.FechaDespacho,
                    Placa = e.Placa,
                    FechaInicioCarga = e.FechaInicioCarga.HasValue ? e.FechaInicioCarga.Value.ToString("yyyy-MM-dd") : null,
                    FechaFinCarga = e.FechaFinCarga.HasValue ? e.FechaFinCarga.Value.ToString("yyyy-MM-dd") : null,
                    NroConstanciaDespacho = e.NroConstanciaDespacho,
                    Volumen = e.VolumenSm3,
                    PoderCalorifico = e.PoderCalorifico,
                    Energia = e.Energia
                }).ToList();
                for (var i = 0; i < lista.Count;i++)
                {
                    lista[i].Id = i;
                }
            }

            double? igvCentaje = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.Igv);
            double? precioUsdMmbtu = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.PrecioUsdMmbtu);
            double? cpio = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.CPiOIndicePrecioConsumidorEeUu);
            double? cpii = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.CPIiFactorAjuste);

            var dto = new BoletaVentaGnsUnnaEnergiaLimagasDto
            {
                NombreReporte = operacionGeneral.Resultado.NombreReporte,
                Periodo = entidad.Periodo,
                TotalVolumen = Math.Round(lista.Sum(e => e.Volumen ?? 0),2),
                TotalEnergia = Math.Round(lista.Sum(e => e.Energia ?? 0),2),
                TotalPoderCalorifico = Math.Round(lista.Sum(e => e.PoderCalorifico ?? 0), 2),
                EnergiaVolumenSuministrado = Math.Round(lista.Sum(e => e.Energia ?? 0), 2),
                IgvCentaje = igvCentaje ?? 0,
                Comentario = entidad.Comentario,                
                BoletaVentaMenensual = lista,
                PrecioBase = precioUsdMmbtu??0,
                CPIi = Math.Round(cpii ??0,2),
                CPIo = Math.Round(cpio ??0,2),
                
            };
            dto.Fac = Math.Round(dto.CPIi / dto.CPIo, 2);
            dto.SubTotal = Math.Round(dto.EnergiaVolumenSuministrado * Math.Round(dto.PrecioBase * dto.Fac, 2),2);
            dto.Igv = Math.Round(dto.SubTotal * dto.IgvCentaje / 100, 2);
            dto.Total = dto.SubTotal + dto.Igv;
            if (!string.IsNullOrWhiteSpace(operacionGeneral.Resultado.UrlFirma))
            {
                dto.UrlFirma = $"{_urlConfiguracion.UrlBase}{operacionGeneral.Resultado.UrlFirma.Replace("~/", "")}";
            }
            return new OperacionDto<BoletaVentaGnsUnnaEnergiaLimagasDto>(dto);
        }


        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletaVentaGnsUnnaEnergiaLimagasDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            DateTime fecha = new DateTime(FechasUtilitario.ObtenerDiaOperativo().Year, FechasUtilitario.ObtenerDiaOperativo().Month, 1);
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletaMensualVentaGnsUnnaEnergiaLimagas),
                Fecha = fecha,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                Comentario = peticion.Comentario
            };

            return await _impresionServicio.GuardarAsync(dto);

        }


    }




}
