using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Cartas.Dgh.Dtos;
using Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Implementaciones
{
    public class CartaDghServicio: ICartaDghServicio
    {
        public async Task<OperacionDto<CartaDto>> ObtenerCartaAsync(long idUsuario, DateTime diaOperativo, int idCarta)
        {

            await Task.Delay(0);

            //var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.CalculoFacturaCpgnaFee50, idUsuario);
            //if (!operacionGeneral.Completado && operacionGeneral.Resultado == null)
            //{
            //    return new OperacionDto<CalculoFacturaCpgnaFee50Dto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            //}

            DateTime desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);

            string? nombreMes = FechasUtilitario.ObtenerNombreMes(desde).ToUpper();
            var dto = new CartaDto
            {
                
            };

            //#region A) Determinación del PRef - (Precio de Lista del GLP de la Refinería de PETROPERU en Talara)

            //var entidadPeriodoPrecioGlp = await ListarPeriodoPreciosAsync(diaOperativo);
            //if (entidadPeriodoPrecioGlp.Completado && entidadPeriodoPrecioGlp.Resultado != null)
            //{
            //    dto.PrecioGlp = entidadPeriodoPrecioGlp.Resultado;
            //    dto.PrefPromedioPeriodo = dto.PrecioGlp.Sum(e => e.PrecioKg ?? 0);
            //}

            //var operacionTipoCambio = await _tipoCambioServicio.ListarPorFechasAsync(desde, hasta, (int)TiposMonedas.Soles);
            //if (operacionTipoCambio.Completado && operacionTipoCambio.Resultado != null)
            //{
            //    dto.TipoCambio = operacionTipoCambio.Resultado;
            //    dto.TipoCambioPromedio = Math.Round(operacionTipoCambio.Resultado.Sum(e => e.Cambio) / operacionTipoCambio.Resultado.Count, 3);
            //}
            //dto.PrefPeríodo = Math.Round(dto.PrefPromedioPeriodo * dto.GravedadEspecifica * dto.Factor * 42 / dto.TipoCambioPromedio, 2);

            //#endregion
           



            return new OperacionDto<CartaDto>(dto);
        }

    }
}
