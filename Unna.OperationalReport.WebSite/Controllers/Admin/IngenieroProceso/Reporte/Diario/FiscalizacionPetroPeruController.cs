
using Aspose.Cells;
using ClosedXML.Report;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class FiscalizacionPetroPeruController : ControladorBaseWeb
    {
        private readonly IFiscalizacionPetroPeruServicio _fiscalizacionPetroPeruServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public FiscalizacionPetroPeruController(
            IFiscalizacionPetroPeruServicio fiscalizacionPetroPeruServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _fiscalizacionPetroPeruServicio = fiscalizacionPetroPeruServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }


        [HttpGet("DescarPdf")]
        public async Task<IActionResult> GenerarExcelAsync()
        {

            var operativo = await _fiscalizacionPetroPeruServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }


            var dato = operativo.Resultado;

      

            var complexData = new
            {
                DiaOperativo = dato.Fecha,
                //GasMpcd = dato.Tabla1.GasMpcd,
                //GlpBls = dato.Tabla1.GlpBls,
                //CgnBls = dato.Tabla1.CgnBls,
                //CnsMpc = dato.Tabla1.CnsMpc,
                //CgMpc = dato.Tabla1.CgMpc,
                //VolumenTotalDeGnsEnMs = dato.VolumenTotalGnsEnMs,
                //FlareGnaPertecienteEnel = dato.VolumenTotalGns,
                //VolumenTotalDeGns = dato.FlareGna,
                //FactoresDistribucionGasNaturalSeco = factoresDistribucionGasNaturalSeco,

            };


            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaDiariaDeFiscalizacionPetroperu.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }

            //var workbook = new Workbook(tempFilePath);

            var tempFilePathPdf = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";

            var workbook = new Workbook(tempFilePath);
            workbook.Save(tempFilePathPdf);

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);

                                  

            System.IO.File.Delete(tempFilePath);
            System.IO.File.Delete(tempFilePathPdf);
            

            return File(bytes, "application/pdf", $"BoletaDiariaDeFiscalizacionPetroperu-{dato.Fecha.Replace("/", "-")}.pdf");
        }



        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<FiscalizacionPetroPeruDto?> ObtenerAsync()
        {
            var operacion = await _fiscalizacionPetroPeruServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(FiscalizacionPetroPeruDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _fiscalizacionPetroPeruServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


    }
}
