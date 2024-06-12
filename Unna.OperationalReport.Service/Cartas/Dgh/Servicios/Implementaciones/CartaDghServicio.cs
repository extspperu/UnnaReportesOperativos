using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
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
        private readonly IInformeMensualRepositorio _informeMensualRepositorio;
        public CartaDghServicio(
            ICartaRepositorio cartaRepositorio,
            IEmpresaRepositorio empresaRepositorio,
            IUsuarioServicio usuarioServicio,
            UrlConfiguracionDto urlConfiguracion,
            IInformeMensualRepositorio informeMensualRepositorio
            )
        {
            _cartaRepositorio = cartaRepositorio;
            _empresaRepositorio = empresaRepositorio;
            _usuarioServicio = usuarioServicio;
            _urlConfiguracion = urlConfiguracion;
            _informeMensualRepositorio = informeMensualRepositorio;
        }

        public async Task<OperacionDto<CartaDto>> ObtenerAsync(long idUsuario, DateTime diaOperativo, string idCarta)
        {
            int id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idCarta);
            var cartaEntidad = await _cartaRepositorio.BuscarPorIdAsync(id);
            if (cartaEntidad == null)
            {
                return new OperacionDto<CartaDto>(CodigosOperacionDto.NoExiste, "No existe carta");
            }

            DateTime desde = new DateTime(diaOperativo.Year, diaOperativo.Month, 1);

            string? nombreMes = FechasUtilitario.ObtenerNombreMes(desde)?.ToUpper();
            var dto = new CartaDto
            {
                Solicitud = await SolicitudAsync(desde, id, idUsuario),
                Osinergmin1 = await Osinergmin1Async(desde)
            };





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

        private async Task<Osinerg1Dto> Osinergmin1Async(DateTime diaOperativo)
        {
            string nombreMes = FechasUtilitario.ObtenerNombreMes(diaOperativo) ?? "";
            string? periodo = $"{nombreMes.ToUpper()} {diaOperativo.Year}";

            DateTime hasta = diaOperativo.AddMonths(1).AddDays(-1);
            var dto = new Osinerg1Dto
            {
                Periodo = periodo,
                PlantaDestilacion= "2,000 BARRILES",// Valor fijo
                PlantaAbsorcion = "40 MMPCD",//Valor fijo                
            };

            var isunergmin = await _informeMensualRepositorio.RecepcionGasNaturalAsync(diaOperativo, hasta);
            var recepcionGasNatural = isunergmin.Select(e => new GasNaturalPrincipalDto
            {
                Item = e.Id,
                Nombre = e.Nombre,
                MpcMes = e.MpcMes,
                PropiedadCalidad = e.PropiedadCalidad
            }).ToList();
            dto.RecepcionGasNatural = recepcionGasNatural;
            return dto;
        }




    }
}
