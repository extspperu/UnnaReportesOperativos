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
    public class ReporteExistenciaServicio: IReporteExistenciaServicio
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
            DateTime Fecha = DateTime.Now;

            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.ReporteExistencias, idUsuario);
            if (!operacionGeneral.Completado || operacionGeneral.Resultado == null)
            {
                return new OperacionDto<ReporteExistenciaDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.ReporteExistencias, Fecha);
            if (operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos))
            {
                var rpta = JsonConvert.DeserializeObject<ReporteExistenciaDto>(operacionImpresion.Resultado.Datos);                
                return new OperacionDto<ReporteExistenciaDto>(rpta);
            }

            var dto = new ReporteExistenciaDto
            {
                Fecha = Fecha.ToString("dd/MM/yyyy"),
                NombreReporte = operacionGeneral.Resultado.NombreReporte,
                Detalle = operacionGeneral.Resultado.Detalle,
                Compania = operacionGeneral.Resultado.Nombre,
                            };

            var item = new ReporteExistenciaDetalleDto();
            var empresa = await _empresaRepositorio.BuscarPorIdAsync((int)TiposEmpresas.UnnaEnergiaSa);
            if (empresa !=null)
            {
                item.Item = 1;
                item.RazonSocial = empresa.RazonSocial;
                item.CodigoOsinergmin = empresa.CodigoOsinergmin;
                item.NroRegistroHidrocarburo = empresa.NroRegistroHidrocarburo;
                item.Direccion = empresa.Direccion;
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

