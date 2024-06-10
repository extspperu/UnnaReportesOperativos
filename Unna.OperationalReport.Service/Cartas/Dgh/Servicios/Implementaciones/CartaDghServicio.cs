using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Cartas.Dgh.Dtos;
using Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Implementaciones
{
    public class CartaDghServicio : ICartaDghServicio
    {

        private readonly ICartaRepositorio _cartaRepositorio;
        public CartaDghServicio(ICartaRepositorio cartaRepositorio)
        {
            _cartaRepositorio = cartaRepositorio;
        }

        public async Task<OperacionDto<CartaDto>> ObtenerAsync(long idUsuario, DateTime diaOperativo, string idCarta)
        {
            int id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idCarta);
            var carta = await _cartaRepositorio.BuscarPorIdAsync(id);
            if (carta == null)
            {
                return new OperacionDto<CartaDto>(CodigosOperacionDto.NoExiste, "No existe carta");
            }

            DateTime desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);

            string? nombreMes = FechasUtilitario.ObtenerNombreMes(desde).ToUpper();
            var dto = new CartaDto
            {
                Solicitud = await SolicitudAsync(desde, id),

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

        private async Task<CartaSolicitudDto> SolicitudAsync(DateTime diaOperativo, int idCarta)
        {
            var entidad = await _cartaRepositorio.BuscarPorIdAsync(idCarta);

            string nombreMes = FechasUtilitario.ObtenerNombreMes(diaOperativo) ?? "";
            var dto = new CartaSolicitudDto
            {
                Fecha = $"Talara, {diaOperativo.Day} de {nombreMes} de {diaOperativo.Year}",
                Periodo = nombreMes.ToUpper(),
                Asunto = entidad.Asunto,
                Destinatario = entidad.Destinatario,
                Cuerpo = entidad.Cuerpo,
                Sumilla = entidad.Sumilla,
            };

            return dto;
        }

    }
}
