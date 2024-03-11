using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Servicios.Implementaciones
{
    public class FiscalizacionProductosServicio: IFiscalizacionProductosServicio
    {
        private readonly IReporteServicio _reporteServicio;
        private readonly IImpresionServicio _impresionServicio;
        public FiscalizacionProductosServicio(
            IReporteServicio reporteServicio,
            IImpresionServicio impresionServicio
            )
        {
            _reporteServicio = reporteServicio;
            _impresionServicio = impresionServicio;
        }
        public async Task<OperacionDto<FiscalizacionProductosDto>> ObtenerAsync(long? idUsuario)
        {
            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.ResumenDiarioFiscalizacionProductos, FechasUtilitario.ObtenerDiaOperativo());
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<FiscalizacionProductosDto>(operacionImpresion.Resultado.Datos);
                return new OperacionDto<FiscalizacionProductosDto>(rpta);
            }
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.ReporteExistencias, idUsuario);
            if (!operacionGeneral.Completado || operacionGeneral.Resultado == null)
            {
                return new OperacionDto<FiscalizacionProductosDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var dto = new FiscalizacionProductosDto();
            dto.General = operacionGeneral.Resultado;

            return new OperacionDto<FiscalizacionProductosDto>(dto);
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(FiscalizacionProductosDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            peticion.General = null;
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ResumenDiarioFiscalizacionProductos),
                Fecha = FechasUtilitario.ObtenerDiaOperativo(),
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion)
            };
            return await _impresionServicio.GuardarAsync(dto);
        }

    }
}
