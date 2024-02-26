using Aspose.Cells;
using ClosedXML.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class BoletaVentaGnsController : ControladorBaseWeb
    {

        private readonly IBoletaVentaGnsServicio _boletaVentaGnsServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;

        public BoletaVentaGnsController(
            IBoletaVentaGnsServicio boletaVentaGnsServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _boletaVentaGnsServicio = boletaVentaGnsServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<BoletaVentaGnsDto?> ObtenerAsync()
        {
            var operacion = await _boletaVentaGnsServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(BoletaVentaGnsDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaVentaGnsServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("GenerarExcel")]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            var operativo = await _boletaVentaGnsServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }
            var dato = operativo.Resultado;
                 
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaDeVentaGns.xlsx"))
            {
                template.AddVariable(dato);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
           
            var tempFilePathPdf = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";
            var workbook = new Workbook(tempFilePath);
            workbook.Save(tempFilePathPdf);

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);


            System.IO.File.Delete(tempFilePath);
            System.IO.File.Delete(tempFilePathPdf);

            return File(bytes, "application/pdf", $"BoletaVentaGns-{dato.Fecha.Replace("/", "-")}.pdf");
            //return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BoletaCnpc-{dato.Fecha.Replace("/", "-")}.xlsx");
        }

        [HttpGet("Pdf")]
        public async Task<IActionResult> Pdf()
        {
            var dto = new BoletaVentaGnsDto();
            var operacion = await _boletaVentaGnsServicio.ObtenerAsync(ObtenerIdUsuarioActual()??0);
            if (operacion.Completado)
            {
                dto = operacion.Resultado;
            }
            return new ViewAsPdf("/Pages/Admin/IngenieroProceso/Reporte/Diario/ReporteExistencias/Pdf.cshtml")
            {
                FileName = "BoletaVentaDeGasNatural.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                Model = dto
            };
        }

    }
}
