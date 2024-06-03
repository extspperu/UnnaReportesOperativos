﻿using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ICalculoFacturaCpgnaFee50Servicio _calculoFacturaCpgnaFee50Servicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public CalculoFacturaCpgnaFee50Controller(
            ICalculoFacturaCpgnaFee50Servicio calculoFacturaCpgnaFee50Servicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _calculoFacturaCpgnaFee50Servicio = calculoFacturaCpgnaFee50Servicio;
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

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Cálculo factura CPGNA - Con FEE 50.0 %.xlsx");

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
                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }

            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);

            System.IO.File.Delete(url);
            System.IO.File.Delete(tempFilePathPdf);
            return File(bytes, "application/pdf", $"Cálculo factura CPGNA - Con FEE 50.0 %.pdf");
        }

        private async Task<string?> GenerarAsync()
        {
            var operativo = await _calculoFacturaCpgnaFee50Servicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, FechasUtilitario.ObtenerDiaOperativo());
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

            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\mensual\\CalculoFacturaCpgnaConFee50.xlsx"))
            {
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
            var operacion = await _calculoFacturaCpgnaFee50Servicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0, FechasUtilitario.ObtenerDiaOperativo());
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
                peticion.DiaOperativo = FechasUtilitario.ObtenerDiaOperativo();
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
