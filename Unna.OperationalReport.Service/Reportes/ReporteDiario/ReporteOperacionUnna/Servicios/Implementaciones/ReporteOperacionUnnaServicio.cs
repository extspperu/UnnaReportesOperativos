using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Servicios.Implementaciones
{
    public class ReporteOperacionUnnaServicio : IReporteOperacionUnnaServicio
    {
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IBoletaDeterminacionVolumenGnaServicio _boletaDeterminacionVolumenGnaServicio;
        private readonly IFiscalizacionProductosServicio _fiscalizacionProductosServicio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IEmpresaRepositorio _empresaRepositorio;
        public ReporteOperacionUnnaServicio(
            IRegistroRepositorio registroRepositorio,
            IBoletaDeterminacionVolumenGnaServicio boletaDeterminacionVolumenGnaServicio,
            IFiscalizacionProductosServicio fiscalizacionProductosServicio,
            IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio,
            IEmpresaRepositorio empresaRepositorio
            )
        {
            _registroRepositorio = registroRepositorio;
            _boletaDeterminacionVolumenGnaServicio = boletaDeterminacionVolumenGnaServicio;
            _fiscalizacionProductosServicio = fiscalizacionProductosServicio;
            _impresionServicio = impresionServicio;
            _reporteServicio = reporteServicio;
            _empresaRepositorio = empresaRepositorio;
        }

        public async Task<OperacionDto<ReporteOperacionUnnaDto>> ObtenerAsync(long idUsuario)
        {
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.ReporteOperacionUnna, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<ReporteOperacionUnnaDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.ReporteOperacionUnna, FechasUtilitario.ObtenerDiaOperativo());
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<ReporteOperacionUnnaDto>(operacionImpresion.Resultado.Datos);
                rpta.General = operacionGeneral.Resultado;
                return new OperacionDto<ReporteOperacionUnnaDto>(rpta);
            }

            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            var dto = new ReporteOperacionUnnaDto
            {
                ReporteNro = $"{diaOperativo.Day}{diaOperativo.Month}{diaOperativo.Year}-UNNA",
                FechaEmision = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd/MM/yyyy"),
                DiaOperativo = diaOperativo.ToString("dd/MM/yyyy"),
                General = operacionGeneral.Resultado
            };
            dto.CapacidadDisenio = 44.0;//Es un dato fijo


            var empresa = await _empresaRepositorio.BuscarPorIdAsync((int)TiposEmpresas.UnnaEnergiaSa);
            if (empresa != null)
            {
                dto.Empresa = empresa.RazonSocial;
            }

            var registrosDatos = await _registroRepositorio.ListarDatosPorFechaAsync(diaOperativo);

            #region PROCESAMIENTO DE GAS NATURAL
            dto.ProcesamientoGasNatural = new ProcesamientoVolumenDto
            {
                Nombre = "GAS NATURAL HÚMEDO",
                Volumen = Math.Round(registrosDatos.Sum(e => e.Volumen), 2)
            };

            var procesamientoGasNaturalSeco = new List<ProcesamientoVolumenDto>();
            procesamientoGasNaturalSeco.Add(new ProcesamientoVolumenDto
            {
                Item = 1,
                Nombre = "REINYECTADO - FLARE",
                Volumen = 0
            });
            procesamientoGasNaturalSeco.Add(new ProcesamientoVolumenDto
            {
                Item = 2,
                Nombre = "VENTAS (ENEL + PETROPERU + LIMAGAS + GASNORP)",
                Volumen = 0
            });
            procesamientoGasNaturalSeco.Add(new ProcesamientoVolumenDto
            {
                Item = 3,
                Nombre = "TOTAL",
                Volumen = procesamientoGasNaturalSeco.Sum(e => e.Volumen)
            });

            dto.ProcesamientoGasNaturalSeco = procesamientoGasNaturalSeco;

            #endregion

            var boletaLoteIv = new BoletaDeterminacionVolumenGnaDto();
            var operacionBoletaLoteIv = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(idUsuario);
            if (operacionBoletaLoteIv.Completado && operacionBoletaLoteIv.Resultado != null)
            {
                boletaLoteIv = operacionBoletaLoteIv.Resultado;
            }

            #region PRODUCCIÓN DE LGN EN PLANTA UNNA
            dto.ProduccionLgn = new ProcesamientoVolumenDto
            {
                Nombre = "LGN",
                Volumen = boletaLoteIv.VolumenProduccionTotalLgn
            };

            #endregion

            #region PROCESAMIENTO DE LÍQUIDOS DE GAS NATURAL (LGN) - UNNA
            dto.ProcesamientoLiquidos = new ProcesamientoVolumenDto
            {
                Nombre = "LGN",
                Volumen = boletaLoteIv.VolumenProduccionTotalLgn
            };
            #endregion


            #region PRODUCTOS OBTENIDOS EN PLANTA - UNNA



            var productosObtenido = new List<ProcesamientoVolumenDto>();
            productosObtenido.Add(new ProcesamientoVolumenDto
            {
                Item = 1,
                Nombre = "CGN(4)",
                Volumen = boletaLoteIv.VolumenProduccionTotalGlp
            });
            productosObtenido.Add(new ProcesamientoVolumenDto
            {
                Item = 2,
                Nombre = "GLP",
                Volumen = boletaLoteIv.VolumenProduccionTotalCgn
            });
            productosObtenido.Add(new ProcesamientoVolumenDto
            {
                Item = 3,
                Nombre = "TOTAL",
                Volumen = productosObtenido.Sum(e => e.Volumen)
            });
            dto.ProductosObtenido = productosObtenido;

            #endregion


            #region ALMACENAMIENTO DE PRODUCTOS LÍQUIDOS - STOCK

            var producto = new FiscalizacionProductosDto();
            var operacionProducto = await _fiscalizacionProductosServicio.ObtenerAsync(idUsuario);
            if (operacionProducto.Completado && operacionProducto.Resultado != null)
            {
                producto = operacionProducto.Resultado;
            }
            var almacenamiento = new List<ProcesamientoVolumenDto>();
            var productoProduccionCgn = producto.ProductoGlpCgn?.Where(e => e.Produccion.Equals("CGN")).FirstOrDefault();

            almacenamiento.Add(new ProcesamientoVolumenDto
            {
                Item = 1,
                Nombre = "CONDENSADOS DE LGN",
                Volumen = productoProduccionCgn != null ? Math.Round(productoProduccionCgn.Inventario ?? 0, 0) : 0,
            });

            var productoProduccionGlp = producto.ProductoGlpCgn?.Where(e => e.Produccion.Equals("GLP")).FirstOrDefault();

            almacenamiento.Add(new ProcesamientoVolumenDto
            {
                Item = 2,
                Nombre = "GLP",
                Volumen = productoProduccionGlp != null ? Math.Round(productoProduccionGlp.Inventario ?? 0, 0) : 0,
            });

            almacenamiento.Add(new ProcesamientoVolumenDto
            {
                Nombre = "TOTAL",
                Volumen = almacenamiento.Sum(e => e.Volumen)
            });
            dto.Almacenamiento = almacenamiento;

            #endregion


            return new OperacionDto<ReporteOperacionUnnaDto>(dto);
        }


        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(ReporteOperacionUnnaDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            peticion.General = null;
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ReporteOperacionUnna),
                Fecha = FechasUtilitario.ObtenerDiaOperativo(),
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion)
            };
            return await _impresionServicio.GuardarAsync(dto);

        }


    }
}
