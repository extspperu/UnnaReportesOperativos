using DocumentFormat.OpenXml.Drawing;
using Newtonsoft.Json;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using static NPOI.POIFS.Crypt.CryptoFunctions;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Servicios.Implementaciones
{
    public class FacturacionGnsLIVServicio : IFacturacionGnsLIVServicio
    {

        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IValoresDefectoReporteServicio _valoresDefectoReporteServicio;
        private readonly IBoletaSuministroGNSdelLoteIVaEnelServicio _boletaSuministroGNSdelLoteIVaEnelServicio;
        public FacturacionGnsLIVServicio(
            IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio,
            IValoresDefectoReporteServicio valoresDefectoReporteServicio,
            IBoletaSuministroGNSdelLoteIVaEnelServicio boletaSuministroGNSdelLoteIVaEnelServicio
            )
        {
            _impresionServicio = impresionServicio;
            _reporteServicio = reporteServicio;
            _valoresDefectoReporteServicio = valoresDefectoReporteServicio;
            _boletaSuministroGNSdelLoteIVaEnelServicio = boletaSuministroGNSdelLoteIVaEnelServicio;
        }

        public async Task<OperacionDto<FacturacionGnsLIVDto>> ObtenerAsync(long idUsuario)
        {

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.FacturacionGasNaturalSecoLoteIv, diaOperativo);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<FacturacionGnsLIVDto>(operacionImpresion.Resultado.Datos);
                return new OperacionDto<FacturacionGnsLIVDto>(rpta);
            }

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.FacturacionGasNaturalSecoLoteIv, idUsuario);
            if (!operacionGeneral.Completado && operacionGeneral.Resultado != null)
            {
                return new OperacionDto<FacturacionGnsLIVDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            DateTime desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
            DateTime hasta = new DateTime(diaOperativo.Year, diaOperativo.Month, 1).AddMonths(1).AddDays(-1);

            string? nombreMes = FechasUtilitario.ObtenerNombreMes(desde).ToUpper();
            var dto = new FacturacionGnsLIVDto
            {
                NombreReporte = $"{operacionGeneral.Resultado.NombreReporte} - {nombreMes} {hasta.Year}",
                Periodo = $"PERIODO DEL {desde.Day} AL {hasta.Day} DE {nombreMes} DEL {hasta.Year}",
                UrlFirma = operacionGeneral.Resultado.UrlFirma
            };

            var operacion = await _boletaSuministroGNSdelLoteIVaEnelServicio.ObtenerAsync(idUsuario);
            if (operacion.Completado && operacion.Resultado != null)
            {
                dto.Mpc = Math.Round(operacion.Resultado.TotalVolumenMPC, 2);
                dto.Mmbtu = Math.Round(operacion.Resultado.TotalEnergiaMMBTU,2);
            }

            double? precioUs = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.PrecioFacturacionUsMmbtu);

            dto.Concepto = $"Suministro de Gas Natural Seco (GNS Lote IV), correspondiente al periodo del {desde.Day} al {hasta.Day} de {nombreMes} del {hasta.Year}";

            dto.PrecioUs = precioUs ?? 0;
            dto.ImporteUs = Math.Round(dto.Mmbtu * precioUs ?? 0,2);

            dto.TotalMpc = dto.Mpc;
            dto.TotalMmbtu = dto.Mmbtu;
            dto.TotalPrecioUs = dto.PrecioUs;
            dto.TotalImporteUs = dto.ImporteUs;

            return new OperacionDto<FacturacionGnsLIVDto>(dto);
        }


        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(FacturacionGnsLIVDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            DateTime fecha = new DateTime(FechasUtilitario.ObtenerDiaOperativo().Year, FechasUtilitario.ObtenerDiaOperativo().Month, 1);
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.FacturacionGasNaturalSecoLoteIv),
                Fecha = fecha,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
            };
            return await _impresionServicio.GuardarAsync(dto);

        }

    }

}