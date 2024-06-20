using DocumentFormat.OpenXml.Drawing.Charts;
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
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Cartas.Dgh.Dtos;
using Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
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
        private readonly IRegistroCromatografiaRepositorio _registroCromatografiaRepositorio;
        public CartaDghServicio(
            ICartaRepositorio cartaRepositorio,
            IEmpresaRepositorio empresaRepositorio,
            IUsuarioServicio usuarioServicio,
            UrlConfiguracionDto urlConfiguracion,
            IInformeMensualRepositorio informeMensualRepositorio,
            IRegistroCromatografiaRepositorio registroCromatografiaRepositorio
            )
        {
            _cartaRepositorio = cartaRepositorio;
            _empresaRepositorio = empresaRepositorio;
            _usuarioServicio = usuarioServicio;
            _urlConfiguracion = urlConfiguracion;
            _informeMensualRepositorio = informeMensualRepositorio;
            _registroCromatografiaRepositorio = registroCromatografiaRepositorio;
        }

        public async Task<OperacionDto<CartaDto>> ObtenerAsync(long idUsuario, DateTime diaOperativo, string idCarta)
        {
            var empresa = await _empresaRepositorio.BuscarPorIdAsync((int)TiposEmpresas.UnnaEnergiaSa);

            int id = RijndaelUtilitario.DecryptRijndaelFromUrl<int>(idCarta);
            var carta = await _cartaRepositorio.BuscarPorIdAsync(id);
            if (carta == null)
            {
                return new OperacionDto<CartaDto>(CodigosOperacionDto.NoExiste, "No existe carta");
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

            var dto = new CartaDto
            {
                Periodo = periodo,
                SitioWeb = empresa?.SitioWeb,
                Telefono = empresa?.Telefono,
                Direccion = empresa?.Direccion,

                UrlFirma = $"{_urlConfiguracion.UrlBase}{urlFirma?.Replace("~", "")}",
                Solicitud = await SolicitudAsync(desde, id, idUsuario),
                Osinergmin1 = await Osinergmin1Async(desde),
                CalidadProducto = await ObteneCalidadProductoAsync(desde, hasta),
                AnalisisCromatografico = await ObtenerAnalisisCromatograficoAsync(desde)
            };
            dto.NombreArchivo = $"{carta.Sumilla}-{dto.Solicitud.Numero}-{desde.Year}-{carta.Tipo}";


            return new OperacionDto<CartaDto>(dto);
        }

        private async Task<CartaSolicitudDto> SolicitudAsync(DateTime diaOperativo, int idCarta, long? idUsuario)
        {


            var entidad = await _cartaRepositorio.BuscarPorIdAsync(idCarta);
            if (entidad == null)
            {
                return new CartaSolicitudDto();
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

            };


            return dto;
        }

        private async Task<Osinergmin1Dto> Osinergmin1Async(DateTime diaOperativo)
        {
            string nombreMes = FechasUtilitario.ObtenerNombreMes(diaOperativo) ?? "";
            string? periodo = $"{nombreMes.ToUpper()} DEL {diaOperativo.Year}";

            DateTime hasta = diaOperativo.AddMonths(1).AddDays(-1);
            var dto = new Osinergmin1Dto
            {
                Periodo = periodo,
            };

            var osinergmin = await _informeMensualRepositorio.RecepcionGasNaturalAsync(diaOperativo, hasta);

            var recepcionGasNaturalAsociado = new RecepcionGasNaturalAsociadoDto
            {
                LoteI = osinergmin?.Where(e => e.Id == (int)TiposLote.LoteI).FirstOrDefault() != null ? osinergmin?.Where(e => e.Id == (int)TiposLote.LoteI)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                LoteIV = osinergmin?.Where(e => e.Id == (int)TiposLote.LoteIv).FirstOrDefault() != null ? osinergmin?.Where(e => e.Id == (int)TiposLote.LoteIv)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                LoteX = osinergmin?.Where(e => e.Id == (int)TiposLote.LoteX).FirstOrDefault() != null ? osinergmin?.Where(e => e.Id == (int)TiposLote.LoteX)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                LoteVI = osinergmin?.Where(e => e.Id == (int)TiposLote.LoteVI).FirstOrDefault() != null ? osinergmin?.Where(e => e.Id == (int)TiposLote.LoteVI)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                LoteZ69 = osinergmin?.Where(e => e.Id == (int)TiposLote.LoteZ69).FirstOrDefault() != null ? osinergmin?.Where(e => e.Id == (int)TiposLote.LoteZ69)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                Total = osinergmin.Sum(e => e.MpcMes ?? 0),
            };
            dto.RecepcionGasNaturalAsociado = recepcionGasNaturalAsociado;

            var usoGasEntidad = await _informeMensualRepositorio.ReporteMensualUsoDeGasAsync(diaOperativo, hasta);
            var usoGas = new UsoGasDto
            {
                GasNaturalRestituido = usoGasEntidad?.Where(e => e.Id == 1).FirstOrDefault() != null ? usoGasEntidad?.Where(e => e.Id == 1)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                ConsumoPropio = usoGasEntidad?.Where(e => e.Id == 2).FirstOrDefault() != null ? usoGasEntidad?.Where(e => e.Id == 2)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                ConvertidoEnLgn = usoGasEntidad?.Where(e => e.Id == 3).FirstOrDefault() != null ? usoGasEntidad?.Where(e => e.Id == 3)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                Total = usoGasEntidad.Sum(e => e.MpcMes ?? 0),
            };
            dto.UsoGas = usoGas;

            var entidadProduccionLiquidosGasNatural = await _informeMensualRepositorio.ProduccionLiquidosGasNaturalAsync(diaOperativo, hasta);
            var produccionLiquidosGasNatural = new ProduccionLiquidosGasNaturalDto
            {
                Glp = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 1).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 1)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                PropanoSaturado = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 2).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 2)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                ButanoSaturado = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 3).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 3)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                Hexano = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 4).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 4)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                Condensados = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 5).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 5)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                PromedioLiquidos = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 6).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 6)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
            };
            dto.ProduccionLiquidosGasNatural = produccionLiquidosGasNatural;

            return dto;
        }



        private async Task<Osinergmin2Dto> Osinergmin2Async(DateTime desde, DateTime hasta)
        {
            var dto = new Osinergmin2Dto();

            var liquidos = await _informeMensualRepositorio.VentaLiquidosGasNaturalAsync(desde, hasta);
            if (liquidos != null)
            {
                dto.VentaLiquidoGasNatural = new VentaLiquidosGasNaturalDto
                {
                    ButanoSaturado = liquidos.ButanoSaturado,
                    CondensadoGasNatural = liquidos.CondensadoGasNatural,
                    CondensadoGasolina = liquidos.CondensadoGasolina,
                    Glp = liquidos.Glp,
                    Hexano = liquidos.Hexano,
                    PropanoSaturado = liquidos.PropanoSaturado,
                    Total = (liquidos.ButanoSaturado + liquidos.CondensadoGasNatural + liquidos.CondensadoGasolina + liquidos.Glp + liquidos.Hexano + liquidos.PropanoSaturado)
                };
            }
            var productos = await _informeMensualRepositorio.VolumenVendieronProductosAsync(desde, hasta);
            if (productos != null && productos.Count > 0)
            {
                dto.Glp = productos.Where(e => e.Producto == TiposProducto.GLP).ToList().Select(e => new VolumenVendieronProductosDto
                {
                    Item = e.Id,
                    Producto = e.Nombre,
                    Bls = e.Bls ?? 0
                }).ToList();
                dto.Cgn = productos.Where(e => e.Producto == TiposProducto.CGN).ToList().Select(e => new VolumenVendieronProductosDto
                {
                    Item = e.Id,
                    Producto = e.Nombre,
                    Bls = e.Bls ?? 0
                }).ToList();
            }
            var inventario = await _informeMensualRepositorio.InventarioLiquidoGasNaturalAsync(desde, hasta);

            if (inventario != null && inventario.Count > 0)
            {
                dto.InventarioLiquidoGasNatural = inventario.Select(e => new VolumenVendieronProductosDto
                {
                    Item = e.Id,
                    Producto = e.Nombre,
                    Bls = e.Bls
                }).ToList();
            }


            return dto;
        }

        //Carta 4
        private async Task<Osinergmin4Dto> Osinergmin4Async(DateTime diaOperativo)
        {
            string nombreMes = FechasUtilitario.ObtenerNombreMes(diaOperativo) ?? "";
            string? periodo = $"{nombreMes.ToUpper()} DEL {diaOperativo.Year}";

            DateTime hasta = diaOperativo.AddMonths(1).AddDays(-1);
            var dto = new Osinergmin4Dto
            {
                Periodo = periodo,
            };

            //primera tabla
            var entidadProduccionLiquidosGasNatural = await _informeMensualRepositorio.ProduccionLiquidosGasNaturalAsync(diaOperativo, hasta);
            var produccionLiquidosGasNatural = new ProduccionLiquidosGasNaturalDto
            {
                Glp = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 1).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 1)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                PropanoSaturado = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 2).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 2)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                ButanoSaturado = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 3).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 3)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                Hexano = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 4).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 4)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                Condensados = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 5).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 5)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                PromedioLiquidos = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 6).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 6)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
            };
            dto.ProduccionLiquidosGasNatural = produccionLiquidosGasNatural;

            //Segunda tabla
            var liquidos = await _informeMensualRepositorio.VentaLiquidosGasNaturalAsync(diaOperativo, hasta);
            if (liquidos != null)
            {
                dto.VentaLiquidoGasNatural = new VentaLiquidosGasNaturalDto
                {
                    ButanoSaturado = liquidos.ButanoSaturado,
                    CondensadoGasNatural = liquidos.CondensadoGasNatural,
                    CondensadoGasolina = liquidos.CondensadoGasolina,
                    Glp = liquidos.Glp,
                    Hexano = liquidos.Hexano,
                    PropanoSaturado = liquidos.PropanoSaturado,
                    Total = (liquidos.ButanoSaturado + liquidos.CondensadoGasNatural + liquidos.CondensadoGasolina + liquidos.Glp + liquidos.Hexano + liquidos.PropanoSaturado)
                };
            }

            var inventario = await _informeMensualRepositorio.InventarioLiquidoGasNaturalAsync(diaOperativo, hasta);

            if (inventario != null && inventario.Count > 0)
            {
                dto.InventarioLiquidoGasNatural = inventario.Select(e => new VolumenVendieronProductosDto
                {
                    Item = e.Id,
                    Producto = e.Nombre,
                    Bls = e.Bls
                }).ToList();
            }

            return dto;
        }



        private async Task<ReporteAnalisisCromatograficoDto> ObtenerAnalisisCromatograficoAsync(DateTime periodo)
        {

            var dto = new ReporteAnalisisCromatograficoDto();
            var comoponentes = await _registroCromatografiaRepositorio.ListarReporteCromatograficoPorLotesAsync(periodo);
            var componentes1 = comoponentes.Where(e => e.Grupo == "COMPONENTES1");
            var componentes2 = comoponentes.Where(e => e.Grupo == "COMPONENTES2");

            var componente = componentes1.Select(e => new CompoisicionModalDto
            {
                Item = e.Id,
                Componente = e.Componente,
                LoteI = e.LoteI,
                LoteIv = e.LoteIv,
                LoteVi = e.LoteVi,
                LoteX = e.LoteX,
                LoteZ69 = e.LoteZ69,
                MetodoAstm = e.MetodoAstm,
            }).ToList();
            componente.Add(new CompoisicionModalDto
            {
                Item = componente.Count + 1,
                Componente = "TOTAL",
                LoteI = componente.Sum(e => e.LoteI),
                LoteIv = componente.Sum(e => e.LoteIv),
                LoteVi = componente.Sum(e => e.LoteVi),
                LoteX = componente.Sum(e => e.LoteX),
                LoteZ69 = componente.Sum(e => e.LoteZ69),
            });

            dto.Componente = componente;

            dto.ComponentePromedio = componentes2.Select(e => new CompoisicionModalDto
            {
                Item = e.Id,
                Componente = e.Componente,
                LoteI = e.LoteI,
                LoteIv = e.LoteIv,
                LoteVi = e.LoteVi,
                LoteX = e.LoteX,
                LoteZ69 = e.LoteZ69,
                MetodoAstm = e.MetodoAstm,
            }).ToList();

            return dto;
        }


        private async Task<CalidadProductoDto> ObteneCalidadProductoAsync(DateTime periodo)
        {

            var dto = new CalidadProductoDto();
            var comoponentes = await _registroCromatografiaRepositorio.ListarReporteCromatograficoPorLotesAsync(periodo);
            var componentes1 = comoponentes.Where(e => e.Grupo == "COMPONENTES1");
            var componentes2 = comoponentes.Where(e => e.Grupo == "COMPONENTES2");

            var componente = componentes1.Select(e => new CompoisicionModalDto
            {
                Item = e.Id,
                Componente = e.Componente,
                LoteI = e.LoteI,
                LoteIv = e.LoteIv,
                LoteVi = e.LoteVi,
                LoteX = e.LoteX,
                LoteZ69 = e.LoteZ69,
                MetodoAstm = e.MetodoAstm,
            }).ToList();
            componente.Add(new CompoisicionModalDto
            {
                Item = componente.Count + 1,
                Componente = "TOTAL",
                LoteI = componente.Sum(e => e.LoteI),
                LoteIv = componente.Sum(e => e.LoteIv),
                LoteVi = componente.Sum(e => e.LoteVi),
                LoteX = componente.Sum(e => e.LoteX),
                LoteZ69 = componente.Sum(e => e.LoteZ69),
            });


            return dto;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(CartaDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            await Task.Delay(0);
            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool> { Id = true, Mensaje = "Se guardó correctamente" });
        }

    }
}
