using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Cartas.Mercaptanos.Dtos;
using Unna.OperationalReport.Service.Cartas.Mercaptanos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Usuarios.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Cartas.Mercaptanos.Servicios.Implementaciones
{
    public class MercaptanoServicio: IMercaptanoServicio
    {
        private readonly ICartaRepositorio _cartaRepositorio;
        private readonly IUsuarioServicio _usuarioServicio;
        public MercaptanoServicio(
            ICartaRepositorio cartaRepositorio,
            IUsuarioServicio usuarioServicio
            )
        {
            _cartaRepositorio = cartaRepositorio;
            _usuarioServicio = usuarioServicio;
        }

        public async Task<OperacionDto<MercaptanoDto>> ObtenerAsync(long idUsuario, DateTime diaOperativo, string idCarta)
        {
            //var empresa = await _empresaRepositorio.BuscarPorIdAsync((int)TiposEmpresas.UnnaEnergiaSa);

            int id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idCarta);
            var carta = await _cartaRepositorio.BuscarPorIdAsync(id);
            if (carta == null)
            {
                return new OperacionDto<MercaptanoDto>(CodigosOperacionDto.NoExiste, "No existe carta");
            }

            DateTime fecha = diaOperativo.AddDays(1).AddMonths(-1);
            DateTime desde = new DateTime(fecha.Year, fecha.Month, 1);
            DateTime hasta = desde.AddMonths(1).AddDays(-1);

            string? nombreMes = FechasUtilitario.ObtenerNombreMes(desde)?.ToUpper();

            string? periodo = $"{nombreMes.ToUpper()} DEL {diaOperativo.Year}";

            string? urlFirma = default(string?);
            var usuarioOperacion = await _usuarioServicio.ObtenerAsync(idUsuario);
            if (usuarioOperacion.Completado && usuarioOperacion.Resultado != null && !string.IsNullOrWhiteSpace(usuarioOperacion.Resultado.UrlFirma))
            {
                urlFirma = usuarioOperacion.Resultado.UrlFirma;
            }

            var dto = new MercaptanoDto
            {
            };

            return new OperacionDto<MercaptanoDto>(dto);
        }


        public async Task<OperacionDto<ControlMercaptanoDto>> ControlMercaptanoAsync(long idUsuario, DateTime diaOperativo, string idCarta)
        {

            int id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idCarta);
            var carta = await _cartaRepositorio.BuscarPorIdAsync(id);
            if (carta == null)
            {
                return new OperacionDto<ControlMercaptanoDto>(CodigosOperacionDto.NoExiste, "No existe carta");
            }

            
            DateTime fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow);

            //string? periodo = $"{nombreMes.ToUpper()} DEL {diaOperativo.Year}";

            //string? urlFirma = default(string?);
            //var usuarioOperacion = await _usuarioServicio.ObtenerAsync(idUsuario);
            //if (usuarioOperacion.Completado && usuarioOperacion.Resultado != null && !string.IsNullOrWhiteSpace(usuarioOperacion.Resultado.UrlFirma))
            //{
            //    urlFirma = usuarioOperacion.Resultado.UrlFirma;
            //}

            var dto = new ControlMercaptanoDto();
            //{
            //    Periodo = periodo,
            //    SitioWeb = empresa?.SitioWeb,
            //    Telefono = empresa?.Telefono,
            //    Direccion = empresa?.Direccion,

            //    UrlFirma = $"{_urlConfiguracion.UrlBase}{urlFirma?.Replace("~", "")}",
            //    Solicitud = await SolicitudAsync(desde, id, idUsuario),
            //    Osinergmin1 = await Osinergmin1Async(desde),
            //    Osinergmin2 = await Osinergmin2Async(desde, hasta),
            //    CalidadProducto = await ObteneCalidadProductoAsync(desde),
            //    AnalisisCromatografico = await ObtenerAnalisisCromatograficoAsync(desde)
            //};
            //dto.NombreArchivo = $"{carta.Sumilla}-{dto.Solicitud.Numero}-{desde.Year}-{carta.Tipo}";


            return new OperacionDto<ControlMercaptanoDto>(dto);
        }

    }
}
