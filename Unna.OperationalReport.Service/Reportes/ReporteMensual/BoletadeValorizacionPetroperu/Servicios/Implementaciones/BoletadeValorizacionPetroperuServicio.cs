using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperu.Servicios.Implementaciones
{
    public class BoletadeValorizacionPetroperuServicio : IBoletadeValorizacionPetroperuServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IFiscalizacionProductoProduccionRepositorio _fiscalizacionProductoProduccionRepositorio;
        private readonly IGnsVolumeMsYPcBrutoRepositorio _gnsVolumeMsYPcBrutoRepositorio;
        private readonly IPreciosGLPRepositorio _preciosGLPRepositorio;
        private readonly ITipodeCambioRepositorio _tipodeCambioRepositorio;
        private readonly IImprimirRepositorio _imprimirRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IMensualRepositorio _mensualRepositorio;


        public BoletadeValorizacionPetroperuServicio
       (
           IRegistroRepositorio registroRepositorio,
           IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio,
           IGnsVolumeMsYPcBrutoRepositorio gnsVolumeMsYPcBrutoRepositorio,
           IPreciosGLPRepositorio preciosGLPRepositorio,
           ITipodeCambioRepositorio tipodeCambioRepositorio,
           IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio,
            IImprimirRepositorio imprimirRepositorio,
            IMensualRepositorio mensualRepositorio
       )
        {
            _registroRepositorio = registroRepositorio;
            _fiscalizacionProductoProduccionRepositorio = fiscalizacionProductoProduccionRepositorio;
            _gnsVolumeMsYPcBrutoRepositorio = gnsVolumeMsYPcBrutoRepositorio;
            _preciosGLPRepositorio = preciosGLPRepositorio;
            _tipodeCambioRepositorio = tipodeCambioRepositorio;
            _impresionServicio = impresionServicio;
            _reporteServicio = reporteServicio;
            _imprimirRepositorio = imprimirRepositorio;
            _mensualRepositorio = mensualRepositorio;
        }
        public async Task<OperacionDto<BoletadeValorizacionPetroperuDto>> ObtenerAsync(long idUsuario)
        {

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletadeValorizacionPetroperu, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletadeValorizacionPetroperuDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddMonths(-1);

            DateTime desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);
            DateTime hasta = new DateTime(diaOperativo.Year, diaOperativo.Month, 1).AddMonths(1).AddDays(-1);

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletadeValorizacionPetroperu, desde);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<BoletadeValorizacionPetroperuDto>(operacionImpresion.Resultado.Datos);
                if (rpta != null)
                {
                    
                    return new OperacionDto<BoletadeValorizacionPetroperuDto>(rpta);
                }                
                
            }


            var datos = await _mensualRepositorio.BoletaValorizacionPetroPeruAsync(desde, hasta);
            var boleta = datos.Select(e => new BoletadeValorizacionPetroperuDetDto
            {
                Dia = e.Fecha.Day,
                
                
            }).ToList();

            var dto = new BoletadeValorizacionPetroperuDto
            {

            };

            return new OperacionDto<BoletadeValorizacionPetroperuDto>(dto);
        }

        //private async void BoletadeValorizacionPetroperuDet1()
        //{
        //    var registrosVol = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 3, diaOperativo);
        //    var registrosVolLoteIV = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 4, diaOperativo);
        //    var registrosVolLoteVI = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 2, diaOperativo);
        //    var registrosVolLoteZ69 = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 1, diaOperativo);
        //    var registrosVolLoteX = await _registroRepositorio.ObtenerValorMensualGNSAsync(1, 5, diaOperativo);
        //    var registrosPC = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 3, diaOperativo);
        //    var registrosPCLoteVI = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 2, diaOperativo);
        //    var registrosPCLoteZ69 = await _registroRepositorio.ObtenerValorMensualGNSAsync(2, 1, diaOperativo);
        //    var registroRiq = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 3, diaOperativo);
        //    var registroRiqLoteIV = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 4, diaOperativo);
        //    var registroRiqLoteVI = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 2, diaOperativo);
        //    var registroRiqLoteZ69 = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 1, diaOperativo);
        //    var registroRiqLoteX = await _registroRepositorio.ObtenerValorMensualGNSAsync(3, 5, diaOperativo);
        //    var fiscalizacionGlpCgn = await _fiscalizacionProductoProduccionRepositorio.FiscalizacionProductosGlpCgnMensualAsync(diaOperativo);
        //    var gnsVolumeMsYPcBruto = await _gnsVolumeMsYPcBrutoRepositorio.ObtenerPorTipoYNombreDiaOperativoMensualAsync("VolumenMsGnsAgpsa", "GNS A REFINERÍA", diaOperativo);
        //    var preciosGLP = await _preciosGLPRepositorio.ObtenerPreciosGLPMensualAsync(diaOperativo);
        //    var tipoCambio = await _tipodeCambioRepositorio.ObtenerTipodeCambioMensualAsync(diaOperativo, 1);
        //    var registroVolGNSTransf = await _imprimirRepositorio.ObtenerVolumenGnsTransferidoAsync(7, diaOperativo);

        //}

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletadeValorizacionPetroperuDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            DateTime fecha = FechasUtilitario.ObtenerDiaOperativo();
            peticion.General = null;
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletadeValorizacionPetroperu),
                Fecha = fecha,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                Comentario = null
            };


            return await _impresionServicio.GuardarAsync(dto);

        }
    }
}
