﻿using ClosedXML.Report;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Cartas.Dgh.Dtos;
using Unna.OperationalReport.Service.Cartas.Dgh.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Cartas
{
    [Route("api/admin/cartas/[controller]")]
    [ApiController]
    public class SolicitudController : ControladorBaseWeb
    {

        private readonly ICartaDghServicio _cartaDghServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public SolicitudController(
            ICartaDghServicio cartaDghServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _cartaDghServicio = cartaDghServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }

      
        [HttpGet("GenerarPdf/{idCarta}")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarPdfAsync(string idCarta)
        {
            var operativo = await _cartaDghServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, FechasUtilitario.ObtenerDiaOperativo(), idCarta);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }

            string? url = await GenerarAsync(operativo.Resultado);
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

                worksheet.PrintOptions.PaperType = PaperType.A4;
                worksheet.PrintOptions.Portrait = true;

                worksheet.PrintOptions.FitWorksheetWidthToPages = 1;
                worksheet.PrintOptions.FitWorksheetHeightToPages = 1;
                if(worksheet.Name.Equals("Hoja3")) {
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0, 3) == "ENE")
                    {
                        worksheet.Cells[10, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[10, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[24, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[24, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[38, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[38, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0, 3) == "FEB")
                    {
                        worksheet.Cells[11, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[11, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[25, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[25, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[39, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[39, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0, 3) == "MAR")
                    {
                        worksheet.Cells[12, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[12, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[26, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[26, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[40, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[40, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0,3) == "ABR") {
                        worksheet.Cells[13, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[13, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[27, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[27, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[41, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[41, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0, 3) == "MAY")
                    {
                        worksheet.Cells[14, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[14, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[28, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[28, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[42, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[42, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0, 3) == "JUN")
                    {
                        worksheet.Cells[15, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[15, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[29, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[29, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[43, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[43, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0, 3) == "JUL")
                    {
                        worksheet.Cells[16, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[16, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[30, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[30, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[44, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[44, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0, 3) == "AGO")
                    {
                        worksheet.Cells[17, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[17, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[31, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[31, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[45, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[45, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0, 3) == "SEP")
                    {
                        worksheet.Cells[18, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[18, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[32, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[32, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[46, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[46, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0, 3) == "OCT")
                    {
                        worksheet.Cells[19, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[19, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[33, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[33, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[47, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[47, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0, 3) == "NOV")
                    {
                        worksheet.Cells[20, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[20, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[34, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[34, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[48, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[48, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                    if (operativo?.Resultado?.Osinergmin4?.Periodo.Substring(0, 3) == "DIC")
                    {
                        worksheet.Cells[21, 2].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Glp;
                        worksheet.Cells[21, 6].Value = operativo?.Resultado?.Osinergmin4?.ProduccionLiquidosGasNatural?.Condensados;

                        worksheet.Cells[35, 2].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.Glp;
                        worksheet.Cells[35, 6].Value = operativo?.Resultado?.Osinergmin4?.VentaLiquidoGasNatural?.CondensadoGasNatural;

                        worksheet.Cells[49, 2].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Glp;
                        worksheet.Cells[49, 6].Value = operativo?.Resultado?.Osinergmin4?.InventarioLiquidoGasNatural?.Condensados;
                    }
                }
            }

            var pdfSaveOptions = new PdfSaveOptions()
            {
                SelectionType = SelectionType.EntireFile 
            };
            workbook.Save(pdfFilePath, pdfSaveOptions);

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
            System.IO.File.Delete(url);
            System.IO.File.Delete(tempFilePathPdf);
            string fechaEmisionArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"Resumen Balance Energético UNNA Lote IV - {fechaEmisionArchivo}.pdf");
        }

        private async Task<string?> GenerarAsync(CartaDto entidad)
        {
            await Task.Delay(0);

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\cartas\\solicitud.xlsx"))
            {
                template.AddVariable(entidad);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }



        [HttpGet("Obtener/{idCarta}")]
        [RequiereAcceso()]
        public async Task<CartaDto?> ObtenerAsync(string idCarta)
        {
            var operacion = await _cartaDghServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, FechasUtilitario.ObtenerDiaOperativo(), idCarta);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(CartaDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _cartaDghServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }
    }
}
