﻿using ClosedXML.Report;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Mensual
{
    [Route("api/admin/ingenieroProceso/reporte/mensual/[controller]")]
    [ApiController]
    public class CalculoFacturaCpgnaFee50Controller : ControladorBaseWeb
    {
        string nombreArchivo = $"Cálculo factura CPGNA - Con FEE 50.0 % - {FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MM-yyyy")}";


        private readonly ICalculoFacturaCpgnaFee50Servicio _calculoFacturaCpgnaFee50Servicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly IImpresionServicio _impresionServicio;
        public CalculoFacturaCpgnaFee50Controller(
            ICalculoFacturaCpgnaFee50Servicio calculoFacturaCpgnaFee50Servicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IImpresionServicio impresionServicio
            )
        {
            _calculoFacturaCpgnaFee50Servicio = calculoFacturaCpgnaFee50Servicio;
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
                IdReporte = (int)TiposReportes.CalculoFacturaCpgnaFee50,
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

                worksheet.PrintOptions.PaperType = PaperType.A4;
                worksheet.PrintOptions.Portrait = true;

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
                IdReporte = (int)TiposReportes.CalculoFacturaCpgnaFee50,
                RutaPdf = tempFilePathPdf,
            });
            return File(bytes, "application/pdf", Path.GetFileName(tempFilePathPdf));
        }

        private async Task<string?> GenerarAsync()
        {
            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddDays(1).AddMonths(-1);
            var operativo = await _calculoFacturaCpgnaFee50Servicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, diaOperativo);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }

            var dato = operativo.Resultado;

            var precioGlp = new
            {
                Items = dato.PrecioGlp
            };
            var tipoCambio = new
            {
                Items = dato.TipoCambio
            };

            var resumenEntrega = new
            {
                Items = dato.ResumenEntrega
            };
            var barrilesProducto = new
            {
                Items = dato.BarrilesProducto
            };

            var complexData = new
            {
                NombreReporte = $"{dato?.NombreReporte}",
                PrecioGlp = precioGlp,
                TipoCambio = tipoCambio,
                GravedadEspecifica = dato.GravedadEspecifica,
                PrefPromedioPeriodo = dato.PrefPromedioPeriodo,
                Factor = dato.Factor,
                TipoCambioPromedio = dato.TipoCambioPromedio,
                PrefPeríodo = dato.PrefPeríodo,
                Pref = dato.Pref,
                Vglp = dato.Vglp,
                Vhas = dato.Vhas,
                Precio = dato.Precio,
                PrecioDeterminacion = dato.PrecioDeterminacion,
                VolumenProcesamientoGna = dato.VolumenProcesamientoGna,
                Cm = dato.Cm,
                PrecioFacturacion = dato.PrecioFacturacion,
                PSec = dato.PSec,
                Vtotal = dato.Vtotal,
                CmPrecioPsec = dato.CmPrecioPsec,
                ImporteCmEep = dato.ImporteCmEep,
                IgvCmEep = dato.IgvCmEep,
                MontoTotalCmEep = dato.MontoTotalCmEep,
                PrecioSecado = dato.PrecioSecado,
                Igv = dato.Igv,
                Total = dato.Total,
                ResumenEntrega = resumenEntrega,
                BarrilesProducto = barrilesProducto,
                CentajeTotal = dato.CentajeTotal / 100,
                CentajeCgn = dato.CentajeCgn / 100,
                CentajeGlp = dato.CentajeGlp / 100,
                NumeroDiasPeriodo = dato.NumeroDiasPeriodo,
                Mmpcd = dato.Mmpcd,

            };

            var tempFilePath = $"{_general.RutaArchivos}{nombreArchivo}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\CalculoFacturaCpgnaConFee50.xlsx"))
            {
                if (!string.IsNullOrWhiteSpace(dato?.RutaFirma))
                {
                    using (var stream = new FileStream(dato.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(2);
                        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("D17")).WithSize(120, 70);

                        var worksheetLote1 = template.Workbook.Worksheets.Worksheet(3);
                        worksheetLote1.AddPicture(stream).MoveTo(worksheetLote1.Cell("C17")).WithSize(120, 70);

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
        public async Task<CalculoFacturaCpgnaFee50Dto?> ObtenerAsync()
        {
            DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddDays(1).AddMonths(-1);
            var operacion = await _calculoFacturaCpgnaFee50Servicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, diaOperativo);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(CalculoFacturaCpgnaFee50Dto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _calculoFacturaCpgnaFee50Servicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("GuardarPrecio")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarPrecioAsync(PrecioGlpPeriodo peticion)
        {
            VerificarIfEsBuenJson(peticion);
            if (string.IsNullOrWhiteSpace(peticion.Id))
            {
                peticion.DiaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddDays(1).AddMonths(-1);
            }
            var operacion = await _calculoFacturaCpgnaFee50Servicio.GuardarPrecioAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpDelete("EliminarPrecio/{id}")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> EliminarPrecioAsync(string id)
        {
            var operacion = await _calculoFacturaCpgnaFee50Servicio.EliminarPrecioAsync(id);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


    }
}
