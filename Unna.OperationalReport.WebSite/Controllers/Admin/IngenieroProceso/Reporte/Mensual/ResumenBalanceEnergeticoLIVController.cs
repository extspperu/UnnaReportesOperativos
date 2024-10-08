﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Servicios.Abstracciones;
using ClosedXML.Report;
using GemBox.Spreadsheet.Drawing;
using GemBox.Spreadsheet;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Mensual
{
    [Route("api/admin/ingenieroProceso/reporte/mensuales/[controller]")]
    [ApiController]
    public class ResumenBalanceEnergeticoLIVController : ControladorBaseWeb
    {
        string nombreArchivo = $"Resumen Balance Energético UNNA Lote IV - {FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MM-yyyy")}";

        private readonly GeneralDto _general;
        private readonly IResBalanceEnergLIVServicio _resBalanceEnergLIVServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IImpresionServicio _impresionServicio;

        public ResumenBalanceEnergeticoLIVController(
            IResBalanceEnergLIVServicio resBalanceEnergLIVServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IImpresionServicio impresionServicio
            )
        {
            _resBalanceEnergLIVServicio = resBalanceEnergLIVServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
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
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.ResumenBalanceEnergiaLIVMensual,
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
         
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.ResumenBalanceEnergiaLIVMensual,
                RutaPdf = tempFilePathPdf,
            });            
            return File(bytes, "application/pdf", Path.GetFileName(tempFilePathPdf));

        }

        private async Task<string?> GenerarAsync()
        {
            var operativo = await _resBalanceEnergLIVServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, 2);

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




            var tempFilePath = $"{_general.RutaArchivos}{nombreArchivo}.xlsx";
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


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(ResBalanceEnergLIVPost resumenBalanceEnergeticoLIV)
        {
            VerificarIfEsBuenJson(resumenBalanceEnergeticoLIV);
            resumenBalanceEnergeticoLIV.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            resumenBalanceEnergeticoLIV.IdReporte = (int)TiposReportes.ResumenBalanceEnergiaLIVMensual;
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
            
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.ResumenBalanceEnergiaLIVMensual,
                RutaExcel = url,
            });            
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(url));
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
            var tempFilePathPdf = $"{_general.RutaArchivos}{nombreArchivo}.pdf";
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
            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.ResumenBalanceEnergiaLIVMensual,
                RutaPdf = tempFilePathPdf,
            });
            return File(bytes, "application/pdf", Path.GetFileName(tempFilePathPdf));
        }
        private async Task<string?> GenerarLGNAsync()
        {
            var operativo = await _resBalanceEnergLIVServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, 2);
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
                dataResult = operativo.Resultado.ResBalanceEnergLIVDetMedGas,

                dataResultResumen = operativo.Resultado,

                ResBalanceEnergLIVDetMedGas = resBalanceEnergLIVDetMedGas,
                ResBalanceEnergLIVDetGnaFisc = resBalanceEnergLIVDetGnaFisc,
                GeneralResult = generalResult

            };
            var tempFilePath = $"{_general.RutaArchivos}{nombreArchivo}.xlsx";
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
