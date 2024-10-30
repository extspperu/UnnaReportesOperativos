using ClosedXML.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteExistencias.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class ReporteExistenciasController : ControladorBaseWeb
    {

        string nombreArchivo = $"Reporte de Existencia - {FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MM-yyyy")}";

        private readonly IReporteExistenciaServicio _reporteExistenciaServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly IImpresionServicio _impresionServicio;

        public ReporteExistenciasController(
            IReporteExistenciaServicio reporteExistenciaServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IImpresionServicio impresionServicio
            )
        {
            _reporteExistenciaServicio = reporteExistenciaServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _impresionServicio = impresionServicio;
        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<ReporteExistenciaDto?> ObtenerAsync()
        {
            var operacion = await _reporteExistenciaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(ReporteExistenciaDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _reporteExistenciaServicio.GuardarAsync(peticion,true);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("GenerarExcel")]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            var operativo = await _reporteExistenciaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }

            var datos = new
            {
                Items = operativo.Resultado.Datos
            };

            object cuerpo = new
            {
                Fecha = operativo.Resultado.Fecha,
                NombreReporte = operativo.Resultado.NombreReporte,
                Compania = operativo.Resultado.Compania,
                Detalle = operativo.Resultado.Detalle,
                Datos = datos,
            };
            var tempFilePath = $"{_general.RutaArchivos}{nombreArchivo}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\ReporteExistencia.xlsx"))
            {
                template.AddVariable(cuerpo);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.ReporteExistencias,
                RutaExcel = tempFilePath,
            });            
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(tempFilePath));
        }


    }
}
