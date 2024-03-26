using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
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
        public FiscalizacionProductosServicio(
            IReporteServicio reporteServicio,
            IImpresionServicio impresionServicio,
            IDatoDeltaVRepositorio datoDeltaVRepositorio,
            IFiscalizacionProductoProduccionRepositorio fiscalizacionProductoProduccionRepositorio
            )
        {
            _reporteServicio = reporteServicio;
            _impresionServicio = impresionServicio;
            _datoDeltaVRepositorio = datoDeltaVRepositorio;
            _fiscalizacionProductoProduccionRepositorio = fiscalizacionProductoProduccionRepositorio;
        }
        public async Task<OperacionDto<FiscalizacionProductosDto>> ObtenerAsync(long? idUsuario)
        {
            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.ResumenDiarioFiscalizacionProductos, FechasUtilitario.ObtenerDiaOperativo());
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<FiscalizacionProductosDto>(operacionImpresion.Resultado.Datos);
                return new OperacionDto<FiscalizacionProductosDto>(rpta);
            }
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.ReporteExistencias, idUsuario);
            if (!operacionGeneral.Completado || operacionGeneral.Resultado == null)
            {
                return new OperacionDto<FiscalizacionProductosDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }
            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            var dto = new FiscalizacionProductosDto();

            var datosDeltaV = await _datoDeltaVRepositorio.BuscarDatosDeltaVPorDiaOperativoAsync(diaOperativo);


            #region PRODUCTOS PARA PROCESO
            dto.ProductoParaReproceso = ProductosParaProceso();

            #endregion


            #region 
            var productoCgn = new List<FiscalizacionProductoTanqueDto>();

            var t4605B = new FiscalizacionProductoTanqueDto
            {
                Producto = "CGN",
                Tanque = TiposTanques.T_4605B,
                Nivel = 0,
                Inventario = 0,
            };
            var t4606B = new FiscalizacionProductoTanqueDto
            {
                Producto = "CGN",
                Tanque = TiposTanques.T_4606B,
                Nivel = 0,
                Inventario = 0,
            };
            productoCgn.Add(new FiscalizacionProductoTanqueDto()
            {
                Producto = "CGN",
                Tanque = TiposTanques.T_4601,
                Nivel = datosDeltaV.Where(e => e.Tanque.Equals(TiposTanques.T_4601)).FirstOrDefault() != null ? datosDeltaV.Where(e => e.Tanque.Equals(TiposTanques.T_4601)).FirstOrDefault().Nivel : null,
                Inventario = 0,
            });



            dto.ProductoCgn = productoCgn;


            #endregion 

            #region PRODUCTO GLP Y CGN
            var productoGlpCgn  = new List<FiscalizacionProductoGlpCgnDto>();
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
            dto.ProductoGlpCgn = productoGlpCgn;
            #endregion

            dto.General = operacionGeneral.Resultado;

            return new OperacionDto<FiscalizacionProductosDto>(dto);
        }


        public List<FiscalizacionProductoTanqueDto> ProductosParaProceso()
        {
            var lista = new List<FiscalizacionProductoTanqueDto>();            
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
