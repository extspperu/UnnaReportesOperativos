using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Repositorios.Implementaciones;
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
        public FiscalizacionProductosServicio(
            IReporteServicio reporteServicio,
            IImpresionServicio impresionServicio,
            IDatoDeltaVRepositorio datoDeltaVRepositorio,
            IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio,
            IDatoCgnRepositorio datoCgnRepositorio
            )
        {
            _reporteServicio = reporteServicio;
            _impresionServicio = impresionServicio;
            _datoDeltaVRepositorio = datoDeltaVRepositorio;
            _fiscalizacionProductoProduccionRepositorio = fiscalizacionProductoProduccionRepositorio;
            _datoCgnRepositorio = datoCgnRepositorio;
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
           
            
            var dto = new FiscalizacionProductosDto()
            {
                Fecha = diaOperativo.ToString("dd-MM-yyyy"),
                General = operacionGeneral.Resultado,
            };

            var datosDeltaV = await _datoDeltaVRepositorio.BuscarDatosDeltaVPorDiaOperativoAsync(diaOperativo);

            #region PRODUCTOS PARA PROCESO
            dto.ProductoParaReproceso = ProductosParaProceso();
            #endregion


            #region PRODUCTO GLP
            dto.ProductoGlp = await ProductoGlpAsync(datosDeltaV);
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


            productoGlpCgn.Add(new FiscalizacionProductoGlpCgnDto
            {
                Producto = "TOTAL",
                Produccion = productoGlpCgn.Sum(e=>e.Produccion),
                Despacho = productoGlpCgn.Sum(e=>e.Despacho),
                Inventario = productoGlpCgn.Sum(e=>e.Inventario)
            });

            dto.ProductoGlpCgn = productoGlpCgn;
            #endregion

            return new OperacionDto<FiscalizacionProductosDto>(dto);
        }


        public List<FiscalizacionProductoTanqueDto> ProductosParaProceso()
        {
            var lista = new List<FiscalizacionProductoTanqueDto>();
            lista.Add(new FiscalizacionProductoTanqueDto
            {
                Producto = "PRODUCTO PARA REPROCESO",
                Tanque = $"TK-{TiposTanques.T_4601}",
                Nivel = 0,
                Inventario = 0
            });
            lista.Add(new FiscalizacionProductoTanqueDto
            {
                Producto = "PRODUCTO PARA REPROCESO",
                Tanque = $"TK-{TiposTanques.T_4605}",
                Nivel = 0,
                Inventario = 0
            });
            lista.Add(new FiscalizacionProductoTanqueDto
            {
                Tanque = $"TOTAL",
                Nivel = lista.Sum(e=>e.Nivel),
                Inventario = lista.Sum(e => e.Inventario),
            });
            return lista;
        }


        public async Task<List<FiscalizacionProductoTanqueDto>> ProductoGlpAsync(List<DatoDeltaV> datoDeltaV)
        {
            await Task.Delay(0);
            var lista = datoDeltaV.Where(e=>e.Producto == "GLP").Select(e => new FiscalizacionProductoTanqueDto
            {
                Producto = "GLP",
                Nivel = e.Nivel,
                Tanque = e.Tanque
            }).ToList();

            lista.Add(new FiscalizacionProductoTanqueDto
            {
                Producto = "GLP",
                Tanque = $"TOTAL",
                Inventario = lista.Sum(e=>e.Inventario)
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
                Tanque = e.Tanque
            }).ToList();

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
                Producto = producto,
                Tanque = $"TOTAL",
                Inventario = lista.Sum(e => e.Inventario)
            });
            return lista;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(FiscalizacionProductosDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
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
