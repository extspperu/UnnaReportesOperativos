using ClosedXML.Report;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaProcesamientoGnaLoteIv.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Mensual
{
    [Route("api/admin/ingenieroProceso/reporte/mensual/[controller]")]
    [ApiController]
    public class BoletaProcesamientoGnaLoteIvController : ControladorBaseWeb
    {
        private readonly IBoletaProcesamientoGnaLoteIvServicio _boletaProcesamientoGnaLoteIvServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public BoletaProcesamientoGnaLoteIvController(
            IBoletaProcesamientoGnaLoteIvServicio boletaProcesamientoGnaLoteIvServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _boletaProcesamientoGnaLoteIvServicio = boletaProcesamientoGnaLoteIvServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }

        [HttpGet("GenerarExcel")]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            string? url = await GenerarAsync();
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);
            System.IO.File.Delete(url);

            DateTime fecha = DateTime.UtcNow.AddDays(-1);
            string? mes = FechasUtilitario.ObtenerNombreMes(fecha);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fecha.Month} Boleta Valorización Procesamiento GNA LOTE IV {mes} {fecha.Year}.xlsx");

        }

        [HttpGet("GenerarPdf")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarPdfAsync()
        {
            string? url = await GenerarAsync();
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var tempFilePathPdf = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            string excelFilePath = url;
            string pdfFilePath = tempFilePathPdf;

            using (var excelPackage = new OfficeOpenXml.ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelFile workbook = ExcelFile.Load(excelFilePath);

                foreach (var worksheet in workbook.Worksheets)
                {

                    worksheet.PrintOptions.PaperType = PaperType.A4;
                    worksheet.PrintOptions.Portrait = true;

                    worksheet.PrintOptions.FitWorksheetWidthToPages = 1;
                    worksheet.PrintOptions.FitWorksheetHeightToPages = 1;
                }

                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);

            System.IO.File.Delete(url);
            System.IO.File.Delete(tempFilePathPdf);
            DateTime fecha = DateTime.UtcNow.AddDays(-1);
            string? mes = FechasUtilitario.ObtenerNombreMes(fecha);
            return File(bytes, "application/pdf", $"{fecha.Month} Boleta Valorización Procesamiento GNA LOTE IV {mes} {fecha.Year}.pdf");
        }




        private async Task<string?> GenerarAsync()
        {
            var operativo = await _boletaProcesamientoGnaLoteIvServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            var valores = new
            {
                Items = dato.Valores
            };
            byte[] file = Encoding.ASCII.GetBytes("C:\\Users\\Meliton\\Downloads\\Akiles.io.png");
            var imgSrc = String.Format("data:image/jpeg;base64,{0}", file);
            var complexData = new
            {
                NombreReporte = $"{dato?.NombreReporte}",                
                Anio = dato?.Anio,
                Mes = dato?.Mes,
                TotalPc = dato?.TotalPc,
                TotalVolumen = dato?.TotalVolumen,
                TotalEnergia = dato?.TotalEnergia,
                SubTotal = dato?.SubTotal,
                Igv = dato?.Igv,
                EnergiaVolumenProcesado = dato?.EnergiaVolumenProcesado,
                PrecioUsd = dato?.PrecioUsd,
                TotalFacturar = dato?.TotalFacturar,
                IgvCentaje = $"IGV {dato?.IgvCentaje}%",
                
                Valores = valores,
                Firma = imgSrc,


            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\BoletaValorizacionProcesamientoGNALoteIv.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }



        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<BoletaProcesamientoGnaLoteIvDto?> ObtenerAsync()
        {
            var operacion = await _boletaProcesamientoGnaLoteIvServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(BoletaProcesamientoGnaLoteIvDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaProcesamientoGnaLoteIvServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
