using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Entidades;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Cartas.Dgh.Dtos;
using Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Cartas.Mercaptanos.Dtos;
using Unna.OperationalReport.Service.Cartas.Mercaptanos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Usuarios.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Cartas.Mercaptanos.Servicios.Implementaciones
{
    public class MercaptanoServicio : IMercaptanoServicio
    {
        private readonly ICartaRepositorio _cartaRepositorio;
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly ICartaDghServicio _cartaDghServicio;
        private readonly UrlConfiguracionDto _urlConfiguracion;
        public MercaptanoServicio(
            ICartaRepositorio cartaRepositorio,
            IUsuarioServicio usuarioServicio,
            IEmpresaRepositorio empresaRepositorio,
            ICartaDghServicio cartaDghServicio,
            UrlConfiguracionDto urlConfiguracion
            )
        {
            _cartaRepositorio = cartaRepositorio;
            _usuarioServicio = usuarioServicio;
            _empresaRepositorio = empresaRepositorio;
            _cartaDghServicio = cartaDghServicio;
            _urlConfiguracion = urlConfiguracion;
        }

        public async Task<OperacionDto<MercaptanoDto>> ObtenerAsync(MercaptanoDto parametro)
        {
            if (!parametro.DiaOperativo.HasValue)
            {
                return new OperacionDto<MercaptanoDto>(CodigosOperacionDto.NoExiste, "Día operativo es requerido");
            }

            var empresa = await _empresaRepositorio.BuscarPorIdAsync((int)TiposEmpresas.UnnaEnergiaSa);

            var carta = await _cartaRepositorio.BuscarPorIdAsync((int)TipoCartas.OsinergminMercaptano);
            if (carta == null)
            {
                return new OperacionDto<MercaptanoDto>(CodigosOperacionDto.NoExiste, "No existe carta");
            }

            DateTime fecha = parametro.DiaOperativo.Value.AddDays(1).AddMonths(-1);
            DateTime desde = new DateTime(fecha.Year, fecha.Month, 1);
            DateTime hasta = desde.AddMonths(1).AddDays(-1);

            string? nombreMes = FechasUtilitario.ObtenerNombreMes(desde)?.ToUpper();

            string? periodo = $"{nombreMes?.ToUpper()} DEL {fecha.Year}";

            string? urlFirma = default(string?);

            var usuarioOperacion = await _usuarioServicio.ObtenerAsync(parametro.IdUsuario ?? 0);
            if (!usuarioOperacion.Completado || usuarioOperacion.Resultado == null)
            {
                return new OperacionDto<MercaptanoDto>(CodigosOperacionDto.NoExiste, "No existe carta");
            }
            var usuario = usuarioOperacion.Resultado;

            urlFirma = usuario.UrlFirma;
            parametro.Periodo = periodo;
            parametro.SitioWeb = empresa?.SitioWeb;
            parametro.Telefono = empresa?.Telefono;
            parametro.Direccion = empresa?.Direccion;
            parametro.UrlFirma = $"{_urlConfiguracion.UrlBase}{urlFirma?.Replace("~", "")}";
            parametro.Solicitud = await _cartaDghServicio.SolicitudAsync(desde, (int)TipoCartas.OsinergminMercaptano, "1233");
            parametro.NombreArchivo = $"{carta.Sumilla}-{parametro.Solicitud.Numero}-{desde.Year}-{carta.Tipo}";

            #region Mercaptano 
            if (parametro.Mercaptano == null)
            {
                parametro.Mercaptano = new ControlMercaptanoDto();
            }
            parametro.Mercaptano.Reponsable = $"{usuario.Nombres} {usuario.Paterno} {usuario.Materno}";
            parametro.Mercaptano.PostFirma = $"Ing. {parametro.Mercaptano.Reponsable} Ingeniero de Procesos de Gas Natural Plantas de Gas Talara UNNA ENERGIA S.A.";
            parametro.Mercaptano.Periodo = periodo;

            parametro.Mercaptano = await ControlMercaptanoAsync(parametro.Mercaptano);

            parametro.Mercaptano.FechaInicial = desde.ToString("dd/MM/yyyy");
            parametro.Mercaptano.FechaFinal = hasta.ToString("dd/MM/yyyy");
            #endregion

            return new OperacionDto<MercaptanoDto>(parametro);
        }


        private async Task<ControlMercaptanoDto> ControlMercaptanoAsync(ControlMercaptanoDto parametro)
        {

            DateTime fecha = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow);
            parametro.Fecha = string.IsNullOrWhiteSpace(parametro.Fecha) ? fecha.ToString("dd/MM/yyyy") : parametro.Fecha;
            parametro.LitrosInicial = 3.1416 * Math.Pow(30, 2) * parametro.NivelInicial / 1000;
            parametro.LitrosFinal = 3.1416 * Math.Pow(30, 2) * parametro.NivelFinal / 1000;
            parametro.VolumenReposicionLitros = 3.785 * parametro.VolumenReposicionGal;



            return parametro;
        }

    }
}
