using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
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


        //[HttpGet("GenerarPdf/{idCarta}")]
        //[RequiereAcceso()]
        //public async Task<IActionResult> GenerarPdfAsync(string idCarta)
        //{
        //    var operativo = await _mercaptanoServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, FechasUtilitario.ObtenerDiaOperativo(), idCarta);
        //    if (!operativo.Completado || operativo.Resultado == null)
        //    {
        //        return File(new byte[0], "application/octet-stream");
        //    }

        //    string? url = await GenerarAsync(operativo.Resultado);
        //    if (string.IsNullOrWhiteSpace(url))
        //    {
        //        return File(new byte[0], "application/octet-stream");
        //    }
        //    var tempFilePathPdf = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";

        //    SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
        //    string excelFilePath = url;
        //    string pdfFilePath = tempFilePathPdf;

        //    var workbook = ExcelFile.Load(excelFilePath);

        //    foreach (var worksheet in workbook.Worksheets)
        //    {

        //        worksheet.PrintOptions.PaperType = PaperType.A4;
        //        worksheet.PrintOptions.Portrait = true;

        //        worksheet.PrintOptions.FitWorksheetWidthToPages = 1;
        //        worksheet.PrintOptions.FitWorksheetHeightToPages = 1;
        //    }

        //    var pdfSaveOptions = new PdfSaveOptions()
        //    {
        //        SelectionType = SelectionType.EntireFile
        //    };
        //    workbook.Save(pdfFilePath, pdfSaveOptions);

        //    var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
        //    System.IO.File.Delete(url);
        //    System.IO.File.Delete(tempFilePathPdf);
        //    return File(bytes, "application/pdf", $"s.pdf");
        //}

        //[HttpGet("Obtener/{idCarta}")]
        //[RequiereAcceso()]
        //public async Task<CartaDto?> ObtenerAsync(string idCarta)
        //{
        //    var operacion = await _mercaptanoServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, FechasUtilitario.ObtenerDiaOperativo(), idCarta);
        //    return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        //}


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
            //if (!File.Exists(tempFilePathPdf))
            //{
            //    return File(new byte[0], "application/octet-stream");
            //}
            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);            
            System.IO.File.Delete(tempFilePathPdf);
            return File(bytes, "application/pdf", $"UNNA ENERGIA-OSINERGMIN MERCAPTANO.pdf");

        }



    }
}
