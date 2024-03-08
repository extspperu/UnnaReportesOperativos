using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Configuracion.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Servicios.Implementaciones
{
    public class ReporteExistenciaServicio : IReporteExistenciaServicio
    {

        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IImpresionServicio _impresionServicio;
        public ReporteExistenciaServicio(
            IEmpresaRepositorio empresaRepositorio,
            IReporteServicio reporteServicio,
            IImpresionServicio impresionServicio
            )
        {
            _empresaRepositorio = empresaRepositorio;
            _reporteServicio = reporteServicio;
            _impresionServicio = impresionServicio;
        }

        public async Task<OperacionDto<ReporteExistenciaDto>> ObtenerAsync(long idUsuario)
        {          

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.ReporteExistencias, FechasUtilitario.ObtenerDiaOperativo());
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<ReporteExistenciaDto>(operacionImpresion.Resultado.Datos);
                return new OperacionDto<ReporteExistenciaDto>(rpta);
            }
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.ReporteExistencias, idUsuario);
            if (!operacionGeneral.Completado || operacionGeneral.Resultado == null)
            {
                return new OperacionDto<ReporteExistenciaDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            DateTime Fecha = DateTime.Now;
            var dto = new ReporteExistenciaDto
            {
                Fecha = Fecha.ToString("dd/MM/yyyy"),
                NombreReporte = operacionGeneral.Resultado.NombreReporte,
                Detalle = operacionGeneral.Resultado.Detalle,
                Compania = operacionGeneral.Resultado.Nombre,
            };

            var item = new ReporteExistenciaDetalleDto();
            var empresa = await _empresaRepositorio.BuscarPorIdAsync((int)TiposEmpresas.UnnaEnergiaSa);

            double existenciaGlpBls = 0;// Este valor proviene del repor de fiscalización de productos celda F44

            double existenciaDiaria = (existenciaGlpBls * TiposValoresFijos.Conversion42 * TiposValoresFijos.ConversionFExistencia * TiposValoresFijos.ConversionEExistencia) / 1000;
            if (empresa != null)
            {
                item.Item = 1;
                item.RazonSocial = empresa.RazonSocial;
                item.CodigoOsinergmin = empresa.CodigoOsinergmin;
                item.NroRegistroHidrocarburo = empresa.NroRegistroHidrocarburo;
                item.Direccion = empresa.Direccion;
                item.CapacidadInstalada = TiposValoresFijos.CapacidadInstaladaM3Existencia;
                item.ExistenciaDiaria = Math.Round(existenciaDiaria,2);


            }
            List<ReporteExistenciaDetalleDto> Datos = new List<ReporteExistenciaDetalleDto>();
            Datos.Add(item);
            dto.Datos = Datos;
            return new OperacionDto<ReporteExistenciaDto>(dto);
        }


        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarAsync(ReporteExistenciaDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            if (peticion.Datos == null || peticion.Datos.Count == 0)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.Invalido, "No tiene datos completos para guardar");
            }
            if (peticion.Datos.Where(e => e.CapacidadInstalada.HasValue).Count() == 0)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.Invalido, "El campo capacidad instalada no deben ser vacios");
            }
            if (peticion.Datos.Where(e => e.ExistenciaDiaria.HasValue).Count() == 0)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.Invalido, "El campo existencia diaria no deben ser vacios");
            }
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.ReporteExistencias),
                Fecha = FechasUtilitario.ObtenerDiaOperativo(),
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion)
            };
            return await _impresionServicio.GuardarAsync(dto);
        }


    }
}

