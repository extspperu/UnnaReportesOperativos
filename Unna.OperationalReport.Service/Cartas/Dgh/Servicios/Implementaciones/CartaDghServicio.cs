using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.CustomUI;
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
                Osinergmin2 = await Osinergmin2Async(desde, hasta),
                Osinergmin4 = await Osinergmin4Async(diaOperativo)

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

            dto.Glp = new List<VolumenVendieronProductosDto>
            {
                new VolumenVendieronProductosDto { Item = 1, Producto = "PIURA GAS S.A.C.", Bls = 1583.31 },
                new VolumenVendieronProductosDto { Item = 2, Producto = "MEGA GAS S.A.C", Bls = 1382.48 },
                new VolumenVendieronProductosDto { Item = 3, Producto = "LIMA GAS S.A", Bls = 5212.48 },
                new VolumenVendieronProductosDto { Item = 4, Producto = "PERUANA DE COMBUSTIBLES S.A", Bls = 0.00 },
                new VolumenVendieronProductosDto { Item = 5, Producto = "SOLGAS S.A.", Bls = 4095.24 },
                new VolumenVendieronProductosDto { Item = 6, Producto = "CORPORACION PRIMAX S.A.", Bls = 2809.55 },
                new VolumenVendieronProductosDto { Item = 7, Producto = "PUNTO GAS S.A.C", Bls = 3116.36 },
                new VolumenVendieronProductosDto { Item = 8, Producto = "AERO GAS DEL NORTE SAC", Bls = 3358.17 }
            };
                    dto.Cgn = new List<VolumenVendieronProductosDto>
            {
                new VolumenVendieronProductosDto { Item = 1, Producto = "CORPORACION GTM DEL PERU S.A.", Bls = 423.33 },
                new VolumenVendieronProductosDto { Item = 2, Producto = "AERO GAS DEL NORTE SAC", Bls = 2531.31 },
                new VolumenVendieronProductosDto { Item = 3, Producto = "SUCROALCOLERA DEL CHIRA S.A.", Bls = 0.00 },
                new VolumenVendieronProductosDto { Item = 4, Producto = "SHERIDAN ENTERPRISES S.A.C.", Bls = 0.00 },
                new VolumenVendieronProductosDto { Item = 5, Producto = "GM & G HIDROCARBUROS S.A.", Bls = 1949.67 },
                new VolumenVendieronProductosDto { Item = 6, Producto = "KURESA S.A.", Bls = 0.00 },
                new VolumenVendieronProductosDto { Item = 7, Producto = "HERCO COMBUSTIBLES S.A.", Bls = 0.00 },
                new VolumenVendieronProductosDto { Item = 8, Producto = "JEBICORP SAC", Bls = 0.00 },
                new VolumenVendieronProductosDto { Item = 9, Producto = "BRENNTAG DEL PERU SAC", Bls = 0.00 }
            };

            return dto;
        }

        //Carta 4
        private async Task<Osinergmin4Dto> Osinergmin4Async(DateTime diaOperativo)
        {
            string nombreMes = FechasUtilitario.ObtenerNombreMes(diaOperativo) ?? "";
            string? periodo = $"{nombreMes.ToUpper()} DEL {diaOperativo.Year}";
            int mes = diaOperativo.Month;
            string mescadena;
            if (mes < 10) {
                mescadena = "0" + mes.ToString(); 
            } else {
                mescadena = mes.ToString(); 
            }
            string fech = ("01/" + mescadena + "/" + diaOperativo.Year.ToString());
            DateTime desde = DateTime.Parse(fech);
            var dto = new Osinergmin4Dto
            {
                Periodo = fech
            };

            //primera tabla
            var entidadProduccionLiquidosGasNatural = await _informeMensualRepositorio.ProduccionLiquidosGasNaturalAsync(desde,diaOperativo);
            var produccionLiquidosGasNatural = new ProduccionLiquidosGasNaturalDto
            {
                Glp = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 1).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 1)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
               // PropanoSaturado = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 2).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 2)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                //ButanoSaturado = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 3).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 3)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                //Hexano = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 4).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 4)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                Condensados = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 5).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 5)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
                //PromedioLiquidos = entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 6).FirstOrDefault() != null ? entidadProduccionLiquidosGasNatural?.Where(e => e.Id == 6)?.FirstOrDefault()?.MpcMes ?? 0 : 0,
            };
            dto.ProduccionLiquidosGasNatural = produccionLiquidosGasNatural;

            //Segunda tabla
            var liquidos = await _informeMensualRepositorio.VentaLiquidosGasNaturalAsync(desde,diaOperativo);
            if (liquidos != null)
            {
                dto.VentaLiquidoGasNatural = new VentaLiquidosGasNaturalDto
                {
                    //ButanoSaturado = liquidos.ButanoSaturado,
                    CondensadoGasNatural = liquidos.CondensadoGasNatural,
                    //CondensadoGasolina = liquidos.CondensadoGasolina,
                    Glp = liquidos.Glp,
                    //Hexano = liquidos.Hexano,
                    //PropanoSaturado = liquidos.PropanoSaturado,
                    //Total = (liquidos.ButanoSaturado + liquidos.CondensadoGasNatural + liquidos.CondensadoGasolina + liquidos.Glp + liquidos.Hexano + liquidos.PropanoSaturado)
                };
            }

            var inventario = await _informeMensualRepositorio.InventarioLiquidoGasNaturalAsync(desde,diaOperativo);

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
