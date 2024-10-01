using ClosedXML.Report;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Security.Policy;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;


namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Mensual
{
    [Route("api/admin/ingenieroProceso/reporte/mensual/[controller]")]
    [ApiController]
    public class BoletaVentaGnsUnnaEnergiaLimagasController : ControladorBaseWeb
    {
        string nombreArchivo = $"Boleta mensual de Suministro de GNS de UNNA ENERGIA a LIMAGAS NATURAL - {FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MM-yyyy")}";

        private readonly IBoletaVentaGnsUnnaEnergiaLimagasServicio _boletaVentaGnsUnnaEnergiaLimagasServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly IImpresionServicio _impresionServicio;
        public BoletaVentaGnsUnnaEnergiaLimagasController(
        IBoletaVentaGnsUnnaEnergiaLimagasServicio boletaVentaGnsUnnaEnergiaLimagasServicio,
        IWebHostEnvironment hostingEnvironment,
        GeneralDto general,
        IImpresionServicio impresionServicio
            )
        {
            _boletaVentaGnsUnnaEnergiaLimagasServicio = boletaVentaGnsUnnaEnergiaLimagasServicio; ;
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
                IdReporte = (int)TiposReportes.BoletaMensualVentaGnsUnnaEnergiaLimagas,
                RutaExcel = url,
            });
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(url));

        }

        [HttpGet("GenerarPdf")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarPdfAsync()
        {
            var operativo = await _boletaVentaGnsUnnaEnergiaLimagasServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            var boletaVentaMenensual = new
            {
                Items = dato.BoletaVentaMenensual
            };
            var boletaVentaMenensual1 = new
            {
                Items = dato.BoletaVentaMenensual?.Take(121).ToList()
            };
            var boletaVentaMenensual2 = new
            {
                Items = dato.BoletaVentaMenensual?.Skip(121).Take(121).ToList()
            };

            var complexData = new
            {
                NombreReporte = $"{dato?.NombreReporte}",
                Periodo = dato?.Periodo,
                TotalVolumen = dato?.TotalVolumen,
                TotalPoderCalorifico = dato?.TotalPoderCalorifico,
                TotalEnergia = dato?.TotalEnergia,

                EnergiaVolumenSuministrado = dato?.EnergiaVolumenSuministrado,
                PrecioBase = dato?.PrecioBase,
                Fac = dato?.Fac,
                CPIo = dato?.CPIo,
                CPIi = dato?.CPIi,
                SubTotal = dato?.SubTotal,
                Igv = dato?.Igv,
                IgvCentaje = $"IGV {dato?.IgvCentaje}%",
                Total = dato?.Total,
                Comentario = dato?.Comentario,
                BoletaVentaMenensual = boletaVentaMenensual,
                BoletaVentaMenensual1 = boletaVentaMenensual1,
                BoletaVentaMenensual2 = boletaVentaMenensual2,

            };
            if (dato.BoletaVentaMenensual.Count < 122)
            {
                var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
                using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\BoletaVentaGnsUnnaEnergiaLimagas.xlsx"))
                {
                    if (!string.IsNullOrWhiteSpace(dato?.RutaFirma))
                    {
                        using (var stream = new FileStream(dato.RutaFirma, FileMode.Open))
                        {
                            var worksheet = template.Workbook.Worksheets.Worksheet(1);
                            worksheet.AddPicture(stream).MoveTo(worksheet.Cell("C31")).WithSize(120, 70);
                        }
                    }
                    template.AddVariable(complexData);
                    template.Generate();
                    template.SaveAs(tempFilePath);
                }
                var tempFilePathPdf = $"{_general.RutaArchivos}{nombreArchivo}.pdf";

                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");                
                

                using (var excelPackage = new OfficeOpenXml.ExcelPackage(new FileInfo(tempFilePath)))
                {
                    ExcelFile workbook = ExcelFile.Load(tempFilePath);
                    workbook.Worksheets[0].PrintOptions.PaperType = PaperType.A4;
                    workbook.Worksheets[0].PrintOptions.Portrait = true;
                    workbook.Worksheets[0].PrintOptions.LeftMargin = 0.2;
                    workbook.Worksheets[0].PrintOptions.RightMargin = 0.2;
                    workbook.Worksheets[0].PrintOptions.TopMargin = 1;
                    workbook.Worksheets[0].PrintOptions.BottomMargin = 1;
                    workbook.Save(tempFilePathPdf, SaveOptions.PdfDefault);
                }
                var bytesPdfFile0 = System.IO.File.ReadAllBytes(tempFilePathPdf);
                await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
                {
                    IdReporte = (int)TiposReportes.BoletaMensualVentaGnsUnnaEnergiaLimagas,
                    RutaPdf = tempFilePathPdf,
                });
                return File(bytesPdfFile0, "application/pdf", Path.GetFileName(tempFilePathPdf));
            }

            List<PdfDocument> list = new List<PdfDocument>();

            var tempFilePath1 = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\Pdf\\BoletaVentaGnsUnnaEnergiaLimagasV1.xlsx"))
            {
               
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath1);
            }
            var tempFilePathPdf1 = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            using (var excelPackage = new OfficeOpenXml.ExcelPackage(new FileInfo(tempFilePath1)))
            {
                ExcelFile workbook = ExcelFile.Load(tempFilePath1);
                workbook.Save(tempFilePathPdf1, SaveOptions.PdfDefault);
            }
            var bytesPdfFile1 = System.IO.File.ReadAllBytes(tempFilePathPdf1);
            //System.IO.File.Delete(tempFilePath1);
            //System.IO.File.Delete(tempFilePathPdf1);
            list.Add(PdfReader.Open(new MemoryStream(bytesPdfFile1), PdfDocumentOpenMode.Import));

            var tempFilePath2 = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\Pdf\\BoletaVentaGnsUnnaEnergiaLimagasV2.xlsx"))
            {
                if (!string.IsNullOrWhiteSpace(dato?.RutaFirma))
                {
                    using (var stream = new FileStream(dato.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("C23")).WithSize(120, 70);
                    }
                }
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath2);
            }
            var tempFilePathPdf2 = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            using (var excelPackage = new OfficeOpenXml.ExcelPackage(new FileInfo(tempFilePath2)))
            {
                ExcelFile workbook = ExcelFile.Load(tempFilePath2);
                workbook.Save(tempFilePathPdf2, SaveOptions.PdfDefault);
            }
            var bytesPdfFile2 = System.IO.File.ReadAllBytes(tempFilePathPdf2);
            System.IO.File.Delete(tempFilePath2);
            
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.BoletaMensualVentaGnsUnnaEnergiaLimagas,
                RutaPdf = tempFilePathPdf2,
            });
            list.Add(PdfReader.Open(new MemoryStream(bytesPdfFile2), PdfDocumentOpenMode.Import));

            using (PdfSharp.Pdf.PdfDocument outPdf = new PdfSharp.Pdf.PdfDocument())
            {
                for (int i = 1; i <= list.Count; i++)
                {
                    foreach (PdfSharp.Pdf.PdfPage page in list[i - 1].Pages)
                    {
                        outPdf.AddPage(page);
                    }
                }

                MemoryStream stream = new MemoryStream();
                outPdf.Save(stream, false);
                byte[] bytes = stream.ToArray();

                return File(bytes, "application/pdf", $"{nombreArchivo}.pdf");
            }




            
        }

        private async Task<string?> GenerarAsync()
        {
            var operativo = await _boletaVentaGnsUnnaEnergiaLimagasServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            var boletaVentaMenensual = new
            {
                Items = dato.BoletaVentaMenensual
            };

            var complexData = new
            {
                NombreReporte = $"{dato?.NombreReporte}",
                Periodo = dato?.Periodo,
                TotalVolumen = dato?.TotalVolumen,
                TotalPoderCalorifico = dato?.TotalPoderCalorifico,
                TotalEnergia = dato?.TotalEnergia,

                EnergiaVolumenSuministrado = dato?.EnergiaVolumenSuministrado,
                PrecioBase = dato?.PrecioBase,
                Fac = dato?.Fac,
                CPIo = dato?.CPIo,
                CPIi = dato?.CPIi,
                SubTotal = dato?.SubTotal,
                Igv = dato?.Igv,
                IgvCentaje = $"IGV {dato?.IgvCentaje}%",
                Total = dato?.Total,
                Comentario = dato?.Comentario,
                BoletaVentaMenensual = boletaVentaMenensual,

            };
            var tempFilePath = $"{_general.RutaArchivos}{nombreArchivo}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\BoletaVentaGnsUnnaEnergiaLimagas.xlsx"))
            {
                if (!string.IsNullOrWhiteSpace(dato?.RutaFirma))
                {
                    using (var stream = new FileStream(dato.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("C31")).WithSize(120, 70);
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
        public async Task<BoletaVentaGnsUnnaEnergiaLimagasDto?> ObtenerAsync()
        {
            var operacion = await _boletaVentaGnsUnnaEnergiaLimagasServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(BoletaVentaGnsUnnaEnergiaLimagasDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaVentaGnsUnnaEnergiaLimagasServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("ProcesarArchivo")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> ProcesarArchivoAsync(IFormFile file)
        {
            var operacion = await _boletaVentaGnsUnnaEnergiaLimagasServicio.ProcesarArchivoAsync(file, ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


    }
}
