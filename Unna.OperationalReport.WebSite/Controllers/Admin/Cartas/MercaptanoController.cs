using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Cartas.Dgh.Dtos;
using Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Implementaciones;
using Unna.OperationalReport.Service.Cartas.Mercaptanos.Dtos;
using Unna.OperationalReport.Service.Cartas.Mercaptanos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Cartas
{
    [Route("api/admin/cartas/[controller]")]
    [ApiController]
    public class MercaptanoController : ControladorBase
    {
        private readonly IMercaptanoServicio _mercaptanoServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public MercaptanoController(
            IMercaptanoServicio mercaptanoServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _mercaptanoServicio = mercaptanoServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }


        [HttpPost("GenerarPdf")]
        [RequiereAcceso()]
        public async Task<MercaptanoDto?> GuardarAsync(MercaptanoDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            peticion.DiaOperativo = FechasUtilitario.ObtenerDiaOperativo();
            var operacion = await _mercaptanoServicio.ObtenerAsync(peticion);
            if (operacion.Completado && operacion.Resultado != null)
            {
                var excelFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
                using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\cartas\\OsinergminMercaptano.xlsx"))
                {
                    if (!string.IsNullOrWhiteSpace(operacion.Resultado?.RutaFirma))
                    {
                        using (var stream = new FileStream(operacion.Resultado?.RutaFirma, FileMode.Open))
                        {
                            var worksheet = template.Workbook.Worksheets.Worksheet(1);
                            var picture = worksheet.AddPicture(stream).MoveTo(worksheet.Cell("B36")).WithSize(160, 85);

                            var worksheet2 = template.Workbook.Worksheets.Worksheet(2);
                            var picture2 = worksheet2.AddPicture(stream).MoveTo(worksheet2.Cell("G42")).WithSize(160, 85);
                        }
                    }
                    template.AddVariable(operacion.Resultado);
                    template.Generate();
                    template.SaveAs(excelFilePath);
                }

                string nombrePdf = Guid.NewGuid().ToString();
                var tempFilePathPdf = $"{_general.RutaArchivos}{nombrePdf}.pdf";

                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

                var workbook = ExcelFile.Load(excelFilePath);

                foreach (var worksheet in workbook.Worksheets)
                {
                    worksheet.PrintOptions.PaperType = PaperType.A4;
                    worksheet.PrintOptions.Portrait = true;
                    worksheet.PrintOptions.FitWorksheetWidthToPages = 1;
                    worksheet.PrintOptions.FitWorksheetHeightToPages = 1;
                }
                var pdfSaveOptions = new PdfSaveOptions()
                {
                    SelectionType = SelectionType.EntireFile
                };
                workbook.Save(tempFilePathPdf, pdfSaveOptions);

                operacion.Resultado.Key = nombrePdf;
                System.IO.File.Delete(excelFilePath);
            }
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<MercaptanoDto?> ObtenerAsync()
        {
            var operacion = await _mercaptanoServicio.ObtenerAsync(new MercaptanoDto
            {
                DiaOperativo = FechasUtilitario.ObtenerDiaOperativo(),
                IdUsuario = ObtenerIdUsuarioActual()
            });
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("DecargarPdf/{key}")]
        [RequiereAcceso()]
        public IActionResult DecargarPdf(string key)
        {
            var tempFilePathPdf = $"{_general.RutaArchivos}{key}.pdf";

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
            System.IO.File.Delete(tempFilePathPdf);
            return File(bytes, "application/pdf", $"UNNA ENERGIA-OSINERGMIN MERCAPTANO.pdf");

        }

    }
}
