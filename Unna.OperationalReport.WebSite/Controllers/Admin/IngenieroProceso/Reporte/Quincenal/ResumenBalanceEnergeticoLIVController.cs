using ClosedXML.Report;
using DocumentFormat.OpenXml.Spreadsheet;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Quincenal
{
    [Route("api/admin/ingenieroProceso/reporte/quincenal/[controller]")]
    [ApiController]
    public class ResumenBalanceEnergeticoLIVController : ControladorBaseWeb
    {
        private readonly GeneralDto _general;
        private readonly IResBalanceEnergLIVServicio _resBalanceEnergLIVServicio; 
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IImpresionServicio _impresionServicio;

        public ResumenBalanceEnergeticoLIVController(
            IResBalanceEnergLIVServicio resBalanceEnergLIVServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IConfiguration configuration,
            IImpresionServicio impresionServicio
            )
        {
            _resBalanceEnergLIVServicio = resBalanceEnergLIVServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _configuration = configuration;
            _impresionServicio = impresionServicio;
        }
        [HttpGet("GenerarExcel")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            string? url = await GenerarAsync();
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);
            //System.IO.File.Delete(url);
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.ResumenBalanceEnergiaLIVQuincenal,
                RutaExcel = url,
            });

            string fechaEmisionArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Resumen Balance Energético UNNA Lote IV - {fechaEmisionArchivo}.xlsx");

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

            var workbook = ExcelFile.Load(excelFilePath);

            foreach (var worksheet in workbook.Worksheets)
            {
                worksheet.PrintOptions.LeftMargin = Length.From(0.002, LengthUnit.Inch);
                worksheet.PrintOptions.RightMargin = Length.From(0.002, LengthUnit.Inch);
                worksheet.PrintOptions.TopMargin = Length.From(0.002, LengthUnit.Inch);
                worksheet.PrintOptions.BottomMargin = Length.From(0.002, LengthUnit.Inch);

                worksheet.PrintOptions.PaperType = PaperType.A2;
                worksheet.PrintOptions.Portrait = false;

                worksheet.PrintOptions.FitWorksheetWidthToPages = 1;
                worksheet.PrintOptions.FitWorksheetHeightToPages = 1;
            }

            var pdfSaveOptions = new PdfSaveOptions()
            {
                SelectionType = SelectionType.EntireFile
            };
            workbook.Save(pdfFilePath, pdfSaveOptions);

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
            System.IO.File.Delete(url);
            //System.IO.File.Delete(tempFilePathPdf);
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.ResumenBalanceEnergiaLIVQuincenal,
                RutaPdf = tempFilePathPdf,
            });
            string fechaEmisionArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"Resumen Balance Energético UNNA Lote IV - {fechaEmisionArchivo}.pdf");
        }

        private async Task<string?> GenerarAsync()
        {
            var operativo = await _resBalanceEnergLIVServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0,1);

            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            var resBalanceEnergLIVDetMedGas = new
            {
                Items = dato.ResBalanceEnergLIVDetMedGas
            };
            var resBalanceEnergLIVDetGnaFisc = new
            {
                Items = dato.ResBalanceEnergLIVDetGnaFisc
            };

            var generalResult = new
            {
                Items = dato
            };
            double gnsEnergia1Q = operativo.Resultado.ResBalanceEnergLIVDetMedGas
                .Where(d => d.Dia >= 1 && d.Dia <= 15)
                .Sum(d => d.MedGasGasCombSecoMedEnergia ?? 0.0);

            double gnsEnergia2Q = operativo.Resultado.ResBalanceEnergLIVDetMedGas
                .Where(d => d.Dia >= 16 && d.Dia <= 30)
                .Sum(d => d.MedGasGasCombSecoMedEnergia ?? 0.0);
            var complexData = new
            {
                dataResult = operativo.Resultado.ResBalanceEnergLIVDetMedGas,
                dataResultGNA = operativo.Resultado.ResBalanceEnergLIVDetGnaFisc,
                dataResult2 = operativo.Resultado.ResBalanceEnergLgnLIV_2DetLgnDto,
                dataResultResumen = operativo.Resultado,
                ResBalanceEnergLIVDetMedGas = resBalanceEnergLIVDetMedGas,
                ResBalanceEnergLIVDetGnaFisc = resBalanceEnergLIVDetGnaFisc,
                GeneralResult = generalResult,

                GNSEnergia1Q = gnsEnergia1Q,
                GNSEnergia2Q = gnsEnergia2Q
            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            try
            {
                using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\quincenal\\ResumenBalanceEnergLIV.xlsx"))
                {

                    template.AddVariable(complexData);
                    template.Generate();
                    template.SaveAs(tempFilePath);

                    using (var workbook = new ClosedXML.Excel.XLWorkbook(tempFilePath))
                    {
                        foreach (var worksheet in workbook.Worksheets)
                        {
                            foreach (var row in worksheet.RowsUsed())
                            {
                                for (int col = 1; col <= 3; col++) 
                                {
                                    var cell = row.Cell(col);
                                    if (cell.Value.ToString() == "16")
                                    {
                                        cell.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(0, 176, 80); 
                                    }
                                    else if (cell.Value.ToString() == "17")
                                    {
                                        cell.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromArgb(0, 0, 0); 
                                        cell.Style.Font.FontColor = ClosedXML.Excel.XLColor.FromArgb(146, 208, 80); 
                                    }
                                }
                            }
                        }

                        workbook.SaveAs(tempFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            return tempFilePath;
        }

        private async Task<string?> GenerarFirmaAsync()
        {
            var operativo = await _resBalanceEnergLIVServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, 1);

            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            var resBalanceEnergLIVDetMedGas = new
            {
                Items = dato.ResBalanceEnergLIVDetMedGas
            };
            var resBalanceEnergLIVDetGnaFisc = new
            {
                Items = dato.ResBalanceEnergLIVDetGnaFisc
            };

            var generalResult = new
            {
                Items = dato
            };
            double gnsEnergia1Q = operativo.Resultado.ResBalanceEnergLIVDetMedGas
                    .Where(d => d.Dia >= 1 && d.Dia <= 15)
                    .Sum(d => d.MedGasGasCombSecoMedEnergia ?? 0.0);

            double gnsEnergia2Q = operativo.Resultado.ResBalanceEnergLIVDetMedGas
                    .Where(d => d.Dia >= 16 && d.Dia <= 30)
                    .Sum(d => d.MedGasGasCombSecoMedEnergia ?? 0.0);
            var complexData = new
            {
                dataResult = operativo.Resultado.ResBalanceEnergLIVDetMedGas,
                dataResultGNA = operativo.Resultado.ResBalanceEnergLIVDetGnaFisc,
                dataResult2 = operativo.Resultado.ResBalanceEnergLgnLIV_2DetLgnDto,
                dataResultResumen = operativo.Resultado,
                ResBalanceEnergLIVDetMedGas = resBalanceEnergLIVDetMedGas,
                ResBalanceEnergLIVDetGnaFisc = resBalanceEnergLIVDetGnaFisc,
                GeneralResult = generalResult,

                GNSEnergia1Q = gnsEnergia1Q,
                GNSEnergia2Q = gnsEnergia2Q
            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            try
            {
                using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\quincenal\\ResumenBalanceEnergLIV_Firma.xlsx"))
                {
                    template.AddVariable(complexData);
                    template.Generate();

                    string imagePath1 = $"{_hostingEnvironment.WebRootPath}\\images\\firmas\\FIRMA JV UNNA.png";
                    string imagePath2 = $"{_hostingEnvironment.WebRootPath}\\images\\firmas\\FIRMA JV UNNA.png";

                    var workbook = template.Workbook;
                    var worksheet = workbook.Worksheets.First();

                    var imageCell1 = worksheet.Cell("Firma");

                    imageCell1.Value = string.Empty;

                    var image1 = worksheet.AddPicture(imagePath1)
                                         .MoveTo(imageCell1)
                                         .Scale(1);

                    var worksheet2 = workbook.Worksheet("LGN");
                    var imageCell2 = worksheet2.Cell("Firma2");

                    imageCell2.Value = string.Empty;

                    var image2 = worksheet2.AddPicture(imagePath2)
                                         .MoveTo(imageCell2)
                                         .Scale(1);

                    template.SaveAs(tempFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating template: {ex.Message}");
                throw;
            }
            return tempFilePath;
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(ResBalanceEnergLIVPost resumenBalanceEnergeticoLIV)
        {
            Console.WriteLine("JSON recibido:");
            Console.WriteLine(resumenBalanceEnergeticoLIV);

            VerificarIfEsBuenJson(resumenBalanceEnergeticoLIV);
            resumenBalanceEnergeticoLIV.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _resBalanceEnergLIVServicio.GuardarAsync(resumenBalanceEnergeticoLIV);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("GenerarLGNExcel")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarLGNExcelAsync()
        {
            string? url = await GenerarLGNAsync();
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);
            System.IO.File.Delete(url);

            string fechaEmisionArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Resumen Balance Energético LGN UNNA Lote IV - {fechaEmisionArchivo}.xlsx");

        }

        [HttpGet("GenerarLGNPdf")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarLGNPdfAsync()
        {
            string? url = await GenerarLGNAsync();
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
                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
            System.IO.File.Delete(url);
            System.IO.File.Delete(tempFilePathPdf);
            string fechaEmisionArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"Resumen Balance Energético LGN UNNA Lote IV - {fechaEmisionArchivo}.pdf");
        }
        private async Task<string?> GenerarLGNAsync()
        {
            var operativo = await _resBalanceEnergLIVServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, 1);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            var resBalanceEnergLIVDetMedGas = new
            {
                Items = dato.ResBalanceEnergLIVDetMedGas
            };
            var resBalanceEnergLIVDetGnaFisc = new
            {
                Items = dato.ResBalanceEnergLIVDetGnaFisc
            };

            var generalResult = new
            {
                Items = dato
            };

            var complexData = new
            {
                //Compania = dato?.General?.Nombre,
                //PreparadoPör = $"{dato?.General?.PreparadoPör}",
                //AprobadoPor = $"{dato?.General?.AprobadoPor}",
                //VersionFecha = $"{dato?.General?.Version} / {dato?.General?.Fecha}",
                dataResult = operativo.Resultado.ResBalanceEnergLIVDetMedGas,

                dataResultResumen = operativo.Resultado,

                ResBalanceEnergLIVDetMedGas = resBalanceEnergLIVDetMedGas,
                ResBalanceEnergLIVDetGnaFisc = resBalanceEnergLIVDetGnaFisc,
                GeneralResult = generalResult

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\quincenal\\ResumenBalanceLGNEnergIV.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }
    }
}
