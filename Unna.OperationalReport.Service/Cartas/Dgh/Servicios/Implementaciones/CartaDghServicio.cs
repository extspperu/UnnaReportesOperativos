using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Cartas.Dgh.Dtos;
using Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Dtos;
using Unna.OperationalReport.Service.Usuarios.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Implementaciones
{
    public class CartaDghServicio : ICartaDghServicio
    {

        private readonly ICartaRepositorio _cartaRepositorio;
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly UrlConfiguracionDto _urlConfiguracion;
        public CartaDghServicio(
            ICartaRepositorio cartaRepositorio,
            IEmpresaRepositorio empresaRepositorio,
            IUsuarioServicio usuarioServicio,
            UrlConfiguracionDto urlConfiguracion
            )
        {
            _cartaRepositorio = cartaRepositorio;
            _empresaRepositorio = empresaRepositorio;
            _usuarioServicio = usuarioServicio;
            _urlConfiguracion = urlConfiguracion;
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

            string? nombreMes = FechasUtilitario.ObtenerNombreMes(desde)?.ToUpper();
            var dto = new CartaDto
            {
                Solicitud = await SolicitudAsync(desde, id,idUsuario),
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

        private async Task<CartaSolicitudDto> SolicitudAsync(DateTime diaOperativo, int idCarta, long? idUsuario)
        {

            var empresa = await _empresaRepositorio.BuscarPorIdAsync((int)TiposEmpresas.UnnaEnergiaSa);

            var entidad = await _cartaRepositorio.BuscarPorIdAsync(idCarta);
            if (entidad == null)
            {
                return new CartaSolicitudDto();
            }

            string? urlFirma = default(string?); 
            var usuarioOperacion = await _usuarioServicio.ObtenerAsync(idUsuario ?? 0);
            if (usuarioOperacion.Completado && usuarioOperacion.Resultado != null && !string.IsNullOrWhiteSpace(usuarioOperacion.Resultado.UrlFirma))
            {
                urlFirma = usuarioOperacion.Resultado.UrlFirma;
            }

            string nombreMes = FechasUtilitario.ObtenerNombreMes(diaOperativo) ?? "";

            string? periodo = $"{nombreMes.ToUpper()} {diaOperativo.Year}";

            DateTime fechaActual = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow);

            var dto = new CartaSolicitudDto
            {
                Fecha = $"Talara, {fechaActual.Day} de {FechasUtilitario.ObtenerNombreMes(fechaActual)} de {fechaActual.Year}",
                Periodo = periodo,
                Asunto = entidad.Asunto,
                Destinatario = entidad.Destinatario,
                Cuerpo = entidad.Cuerpo.Replace("{{periodo}}", periodo),
                Sumilla = entidad.Sumilla,
                Anio = diaOperativo.Year.ToString(),
                Numero = "2319",
                Pie = entidad.Pie,
                Direccion = empresa?.Direccion,
                SitioWeb = empresa?.SitioWeb,
                Telefono = empresa?.Telefono,
                UrlFirma = $"{_urlConfiguracion.UrlBase}{urlFirma?.Replace("~", "")}",                
            };
            dto.NombreArchivo = $"{entidad.Sumilla}-{dto.Numero}-{dto.Anio}-{entidad.Tipo}";

            return dto;
        }


     



    }
}
