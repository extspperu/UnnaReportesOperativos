﻿//using Aspose.Cells;
using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class BoletaVentaGnsController : ControladorBaseWeb
    {
        string nombreArchivo = $"BOLETA DE VENTA DEL GAS NATURAL SECO DE UNNA LOTE IV A ENEL - {FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MM-yyyy")}";
        private readonly IBoletaVentaGnsServicio _boletaVentaGnsServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly IImpresionServicio _impresionServicio;

        public BoletaVentaGnsController(
            IBoletaVentaGnsServicio boletaVentaGnsServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IImpresionServicio impresionServicio
            )
        {
            _boletaVentaGnsServicio = boletaVentaGnsServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _impresionServicio = impresionServicio;
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
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(BoletaVentaGnsDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaVentaGnsServicio.GuardarAsync(peticion,true);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
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
                IdReporte = (int)TiposReportes.BoletaVentaGasNaturalSecoUnnaLoteIVEnel,
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
                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);

            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.BoletaVentaGasNaturalSecoUnnaLoteIVEnel,
                RutaPdf = tempFilePathPdf,
            });

            return File(bytes, "application/pdf", Path.GetFileName(tempFilePathPdf));
        }
        
        
        

        private async Task<string?> GenerarAsync()
        {
            var operativo = await _boletaVentaGnsServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null || operativo.Resultado.General == null)
            {
                return null;
            }
            var dato = operativo.Resultado;

            object cuerpo = new
            {
                NombreReporte = dato.General.NombreReporte,
                Compania = dato.General.Nombre,
                Version = $"Versión {dato.General.Version}",
                Fecha = dato.Fecha,
                Mpcs = dato.Mpcs,
                BtuPcs = dato.BtuPcs,
                Mmbtu = dato.Mmbtu,
                RazonSocial = dato.Empresa,
                Abreviatura = dato.Abreviatura,
            };
            var tempFilePath = $"{_general.RutaArchivos}{nombreArchivo}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaDeVentaGns.xlsx"))
            {
                if (!string.IsNullOrWhiteSpace(dato?.General?.RutaFirma))
                {
                    using (var stream = new FileStream(dato.General.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("D16")).WithSize(120, 70);
                    }
                }
                template.AddVariable(cuerpo);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }


        

    }
}
