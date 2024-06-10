using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
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
    public class FiscalizacionProductosServicio : IFiscalizacionProductosServicio
    {
        private readonly IReporteServicio _reporteServicio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IDatoDeltaVRepositorio _datoDeltaVRepositorio;
        private readonly IFiscalizacionProductoProduccionRepositorio _fiscalizacionProductoProduccionRepositorio;
        private readonly IDatoCgnRepositorio _datoCgnRepositorio;
        private readonly IReporteDiariaDatosRepositorio _reporteDiariaDatosRepositorio;
        public FiscalizacionProductosServicio(
            IReporteServicio reporteServicio,
            IImpresionServicio impresionServicio,
            IDatoDeltaVRepositorio datoDeltaVRepositorio,
            IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio,
            IDatoCgnRepositorio datoCgnRepositorio,
            IReporteDiariaDatosRepositorio reporteDiariaDatosRepositorio
            )
        {
            _reporteServicio = reporteServicio;
            _impresionServicio = impresionServicio;
            _datoDeltaVRepositorio = datoDeltaVRepositorio;
            _fiscalizacionProductoProduccionRepositorio = fiscalizacionProductoProduccionRepositorio;
            _datoCgnRepositorio = datoCgnRepositorio;
            _reporteDiariaDatosRepositorio = reporteDiariaDatosRepositorio;
        }
        public async Task<OperacionDto<FiscalizacionProductosDto>> ObtenerAsync(long? idUsuario)
        {
            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.ResumenDiarioFiscalizacionProductos, idUsuario);
            if (!operacionGeneral.Completado || operacionGeneral.Resultado == null)
            {
                return new OperacionDto<FiscalizacionProductosDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }
            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.ResumenDiarioFiscalizacionProductos, diaOperativo);
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<FiscalizacionProductosDto>(operacionImpresion.Resultado.Datos);
                rpta.General = operacionGeneral.Resultado;
                return new OperacionDto<FiscalizacionProductosDto>(rpta);
            }

            var impresionDto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ResumenDiarioFiscalizacionProductos),
                Fecha = diaOperativo,
                IdUsuario = idUsuario,
                Datos = null,
                EsEditado = false
            };
            var impresion = await _impresionServicio.GuardarAsync(impresionDto);
            if (!impresion.Completado)
            {
                return new OperacionDto<FiscalizacionProductosDto>(CodigosOperacionDto.NoExiste, "No existe dato");
            }
            long idImpresion = RijndaelUtilitario.DecryptRijndaelFromUrl<long>(impresion.Resultado.Id);

            CultureInfo ci = CultureInfo.CreateSpecificCulture("es-ES");
            DateTimeFormatInfo dtfi = ci.DateTimeFormat;
            dtfi.AbbreviatedMonthNames = new string[] { "Ene.", "Feb.", "Mar.",
                                                  "Abr.", "May.", "Jun.",
                                                  "Jul.", "Ago.", "Sep.",
                                                  "Oct.", "Nov.", "Dic.", "" };
            dtfi.AbbreviatedMonthGenitiveNames = dtfi.AbbreviatedMonthNames;            
            var data1 = diaOperativo.ToString("dd-MMM-yy", dtfi);


            var dto = new FiscalizacionProductosDto()
            {
                Fecha = data1,
                General = operacionGeneral.Resultado,
            };

            var datosDeltaV = await _datoDeltaVRepositorio.BuscarDatosDeltaVPorDiaOperativoAsync(diaOperativo);
            
            
            #region PRODUCTOS PARA PROCESO
            dto.ProductoParaReproceso = ProductosParaProceso();
            #endregion


            #region PRODUCTO GLP
            dto.ProductoGlp = await ProductoGlpAsync(diaOperativo);
            #endregion

            #region PRODUCTO CGN
            dto.ProductoCgn = await ProductoCgnAsync(datosDeltaV, diaOperativo);
            #endregion


            #region 

            //productoCgn.Add(new FiscalizacionProductoTanqueDto()
            //{
            //    Producto = "CGN",
            //    Tanque = TiposTanques.T_4601,
            //    Nivel = datosDeltaV.Where(e => e.Tanque.Equals(TiposTanques.T_4601)).FirstOrDefault() != null ? datosDeltaV.Where(e => e.Tanque.Equals(TiposTanques.T_4601)).FirstOrDefault().Nivel : null,
            //    Inventario = 0,
            //});

            //dto.ProductoCgn = productoCgn;


            #endregion

            #region PRODUCTO GLP Y CGN
            var productoGlpCgn = new List<FiscalizacionProductoGlpCgnDto>();
            var fiscalizacionGlpCgn = await _fiscalizacionProductoProduccionRepositorio.FiscalizacionProductosGlpCgnAsycn(diaOperativo);
            if (fiscalizacionGlpCgn != null)
            {
                productoGlpCgn = fiscalizacionGlpCgn.Select(e => new FiscalizacionProductoGlpCgnDto
                {
                    Producto = e.Producto,
                    Produccion = e.Produccion,
                    Despacho = e.Despacho
                }).ToList();
            }

            var entidadGl = dto.ProductoGlp.Where(e => e.Tanque.Equals("TOTAL")).FirstOrDefault();
            var entidadCgn = dto.ProductoCgn.Where(e => e.Tanque.Equals("TOTAL")).FirstOrDefault();
            productoGlpCgn.Where(e => e.Producto.Equals("GLP")).ToList().ForEach(e => e.Inventario = entidadGl != null ? entidadGl.Inventario : 0);
            productoGlpCgn.Where(e => e.Producto.Equals("CGN")).ToList().ForEach(e => e.Inventario = entidadCgn != null ? entidadCgn.Inventario : 0);


            await _fiscalizacionProductoProduccionRepositorio.EliminarPorFechaAsync(diaOperativo);

            foreach (var item in productoGlpCgn)
            {
                await _fiscalizacionProductoProduccionRepositorio.GuardarAsync(new FiscalizacionProductoProduccion
                {
                    Fecha = diaOperativo,
                    Inventario = item.Inventario,
                    Produccion = item.Produccion,
                    Despacho = item.Despacho,
                    IdImprimir = idImpresion,
                    IdUsuario = idUsuario,
                    Producto = item.Producto,
                });
            }
            

            productoGlpCgn.Add(new FiscalizacionProductoGlpCgnDto
            {
                Producto = "TOTAL",
                Produccion = Math.Round(productoGlpCgn.Sum(e=>e.Produccion??0),2),
                Despacho = Math.Round(productoGlpCgn.Sum(e=>e.Despacho??0),2),
                Inventario = Math.Round(productoGlpCgn.Sum(e=>e.Inventario??0),2)
            });

            dto.ProductoGlpCgn = productoGlpCgn;
            dto.Observacion = "Los valores reportados son la producción total (Bls) de productos terminados de GLP/CGN, el volumen correspondiente a ENEL Generación Piura S.A. se cuantifica en el documento \"Balance de Energía Diaria\".";
            #endregion

            return new OperacionDto<FiscalizacionProductosDto>(dto);
        }


        public List<FiscalizacionProductoTanqueDto> ProductosParaProceso()
        {
            var lista = new List<FiscalizacionProductoTanqueDto>();
            lista.Add(new FiscalizacionProductoTanqueDto
            {
                Item = 1,
                Producto = "PRODUCTO PARA REPROCESO",
                Tanque = $"TK-{TiposTanques.T_4601}",
                Nivel = 0,
                Inventario = 0
            });
            lista.Add(new FiscalizacionProductoTanqueDto
            {
                Item = 2,
                Producto = "PRODUCTO PARA REPROCESO",
                Tanque = $"TK-{TiposTanques.T_4605}",
                Nivel = 0,
                Inventario = 0
            });
            lista.Add(new FiscalizacionProductoTanqueDto
            {
                Item = (lista.Count + 1),
                Tanque = $"TOTAL",
                Nivel = lista.Sum(e=>e.Nivel),
                Inventario = Math.Round(lista.Sum(e => e.Inventario),2),
            });
            return lista;
        }


        public async Task<List<FiscalizacionProductoTanqueDto>> ProductoGlpAsync(DateTime DiaOperativo)
        {            
            var datosDeltaV = await _datoDeltaVRepositorio.BuscarDatosDeltaVPorDiaOperativoGlpFisProdAsync(DiaOperativo, TiposProducto.GLP);
            var lista = datosDeltaV.Select(e => new FiscalizacionProductoTanqueDto
            {
                Producto = e.Producto,
                Nivel = e.Nivel,
                Tanque = e.Tanque,
                Inventario = e.Inventario??0
            }).ToList();

            lista.Add(new FiscalizacionProductoTanqueDto
            {
                Tanque = $"TOTAL",
                Inventario = Math.Round(lista.Sum(e=>e.Inventario),2)
            });
            return lista;
        }


        public async Task<List<FiscalizacionProductoTanqueDto>> ProductoCgnAsync(List<DatoDeltaV> datoDeltaV, DateTime diaOperativo)
        {
            await Task.Delay(0);
            string? producto = "CGN";
            var lista = datoDeltaV.Where(e => e.Producto == producto).Select(e => new FiscalizacionProductoTanqueDto
            {
                Producto = producto,
                Nivel = e.Nivel,
                Tanque = e.Tanque,
            }).ToList();

            foreach (var item in lista)
            {
                item.Inventario = await _reporteDiariaDatosRepositorio.ObtenerProductoCgnInventarioCgnAsync(diaOperativo, item.Tanque)??0;
            }

            var datoCgn = await _datoCgnRepositorio.BuscarDatoCgnPorDiaOperativoAsync(diaOperativo);
            lista.AddRange(datoCgn.Select(e => new FiscalizacionProductoTanqueDto
            {
                Producto = producto,
                Nivel = e.Centaje,
                Tanque = e.Tanque,
                Inventario = e.Volumen??0
            }).ToList());

            lista.Add(new FiscalizacionProductoTanqueDto
            {
                Tanque = $"TOTAL",
                Inventario = Math.Round(lista.Sum(e => e.Inventario),2)
            });
            return lista;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(FiscalizacionProductosDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
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
