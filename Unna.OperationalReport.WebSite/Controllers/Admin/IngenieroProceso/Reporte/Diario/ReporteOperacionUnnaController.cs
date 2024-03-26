using ClosedXML.Report;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class ReporteOperacionUnnaController : ControladorBaseWeb
    {

        private readonly IReporteOperacionUnnaServicio _reporteOperacionUnnaServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public ReporteOperacionUnnaController(
            IReporteOperacionUnnaServicio reporteOperacionUnnaServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _reporteOperacionUnnaServicio = reporteOperacionUnnaServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }


        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<ReporteOperacionUnnaDto?> ObtenerAsync()
        {
            var operacion = await _reporteOperacionUnnaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(ReporteOperacionUnnaDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _reporteOperacionUnnaServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("GenerarExcel")]
        public async Task<IActionResult> GenerarExcelAsync()
        {

            var operativo = await _reporteOperacionUnnaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }

            var dato = operativo.Resultado;


            var complexData = new
            {
                NombreReporte = dato.General?.NombreReporte,
                ReporteNro = dato.ReporteNro,
                EmpresaNombre = dato.Empresa,
                FechaEmision = dato.FechaEmision,
                DiaOperativo = dato.DiaOperativo,

                CapacidadDisPlanta = dato.CapacidadDisenio,
                VolumenGasNatHumedo = dato.ProcesamientoGasNatural?.Volumen,
                                
                VolumenGasNatSecoReinyFlare = dato.ProcesamientoGasNaturalSeco?.Count > 0 ? dato.ProcesamientoGasNaturalSeco[0].Volumen:0,
                VolumenGasNatSecoVentas = dato.ProcesamientoGasNaturalSeco?.Count > 1 ? dato.ProcesamientoGasNaturalSeco[1].Volumen:0,
                ProcGasNatSecoTotal = dato.ProcesamientoGasNaturalSeco?.Count > 2 ? dato.ProcesamientoGasNaturalSeco[2].Volumen:0,

                volumenLgnProducidoPlanta = dato.ProduccionLgn?.Volumen,

                VolumenLgnProcesado = dato.ProcesamientoLiquidos?.Volumen,

                VolumenLgnProducidoCgn = dato.ProductosObtenido?.Count > 0 ? dato.ProductosObtenido[0].Volumen:0,
                VolumenLgnProducidoGlp = dato.ProductosObtenido?.Count > 1 ? dato.ProductosObtenido[1].Volumen : 0,
                VolumenLgnProducidoTotal = dato.ProductosObtenido?.Count > 2 ? dato.ProductosObtenido[2].Volumen : 0,

                VolumenProductosCondensadosLgn = dato.Almacenamiento?.Count > 0 ? dato.Almacenamiento[0].Volumen : 0,
                VolumenProductosGlp = dato.Almacenamiento?.Count > 1 ? dato.Almacenamiento[1].Volumen : 0,
                VolumenProductosTotal = dato.Almacenamiento?.Count > 2 ? dato.Almacenamiento[2].Volumen : 0,
                EventosOperativos = dato.EventoOperativo

            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\ReporteOSINERGMIN.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
                
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ReporteOSINERGMIN.xlsx");
        }

    }
}
