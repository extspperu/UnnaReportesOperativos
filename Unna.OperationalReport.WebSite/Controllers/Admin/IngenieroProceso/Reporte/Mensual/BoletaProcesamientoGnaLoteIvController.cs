using ClosedXML.Report;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
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
        string nombreArchivo = $"Boleta Valorización Procesamiento GNA LOTE IV - {FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MM-yyyy")}";


        private readonly IBoletaProcesamientoGnaLoteIvServicio _boletaProcesamientoGnaLoteIvServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly IImpresionServicio _impresionServicio;
        public BoletaProcesamientoGnaLoteIvController(
            IBoletaProcesamientoGnaLoteIvServicio boletaProcesamientoGnaLoteIvServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IImpresionServicio impresionServicio
            )
        {
            _boletaProcesamientoGnaLoteIvServicio = boletaProcesamientoGnaLoteIvServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _impresionServicio = impresionServicio;
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
            
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.BoletaMensualProcesamientoGnaLoteIv,
                RutaExcel = url,
            });
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(url));

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
            var tempFilePathPdf = $"{_general.RutaArchivos}{nombreArchivo}.pdf";

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
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.BoletaMensualProcesamientoGnaLoteIv,
                RutaPdf = tempFilePathPdf,
            });
            return File(bytes, "application/pdf", Path.GetFileName(tempFilePathPdf));
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


            };
            var tempFilePath = $"{_general.RutaArchivos}{nombreArchivo}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\BoletaValorizacionProcesamientoGNALoteIv.xlsx"))
            {
                if (!string.IsNullOrWhiteSpace(dato?.RutaFirma))
                {
                    using (var stream = new FileStream(dato.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("C22")).WithSize(130, 80);
                    }
                }
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
