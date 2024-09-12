using ClosedXML.Report;
using DocumentFormat.OpenXml.Drawing;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class FiscalizacionProductoController : ControladorBaseWeb
    {
        private readonly IFiscalizacionProductosServicio _fiscalizacionProductosServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly IImpresionServicio _impresionServicio;
        public FiscalizacionProductoController(
            IFiscalizacionProductosServicio fiscalizacionProductosServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IImpresionServicio impresionServicio
            )
        {
            _fiscalizacionProductosServicio = fiscalizacionProductosServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _impresionServicio = impresionServicio;
        }

        [HttpGet("GenerarPdf")]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            var operativo = await _fiscalizacionProductosServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }
            var dato = operativo.Resultado;
            var productoParaReproceso = new
            {
                Items = dato.ProductoParaReproceso
            };
            
            var datosGlp = dato.ProductoGlp?.Where(e => e.Tanque != "TOTAL").ToList();
            if (datosGlp != null && datosGlp.Count > 0)
            {
                datosGlp.ForEach(e => e.Producto = "");
                double totalColumenas = dato.ProductoGlp.Count / 2;
                int total = (int)Math.Round(totalColumenas, 0);
                datosGlp[total-1].Producto = TiposProducto.GLP;
            }           

            var productoGlp = new
            {
                
                Items = datosGlp
            };

            var datosCgn = dato.ProductoCgn?.Where(e => e.Tanque != "TOTAL").ToList();
            if (datosCgn != null && datosCgn.Count > 0)
            {
                datosCgn.ForEach(e => e.Producto = "");
                double totalColumenas = dato.ProductoCgn.Count / 2;
                int total = (int)Math.Round(totalColumenas, 0);
                datosCgn[total - 1].Producto = TiposProducto.CGN;
            }
            var productoCgn = new
            {
                Items = datosCgn
            };
            var nombreGlp = new
            {
                Items = dato.ProductoGlp.Select(e => e.Producto).ToList()
            };

            var procesoTanque1 = dato?.ProductoParaReproceso?.Where(e => e.Item == 1).FirstOrDefault();
            var procesoTanque2 = dato?.ProductoParaReproceso?.Where(e => e.Item == 2).FirstOrDefault();
            var procesoTotal = dato?.ProductoParaReproceso?.Where(e => e.Tanque == "TOTAL").FirstOrDefault();
            var produccionGlp = dato?.ProductoGlpCgn?.Where(e => e.Producto == TiposProducto.GLP).FirstOrDefault();
            var produccionCgn = dato?.ProductoGlpCgn?.Where(e => e.Producto == TiposProducto.CGN).FirstOrDefault();
            var produccionTotal = dato?.ProductoGlpCgn?.Where(e => e.Producto == "TOTAL").FirstOrDefault();
            var productoGlpTotal = dato?.ProductoGlp?.Where(e => e.Tanque == "TOTAL").FirstOrDefault();
            var productoCgnTotal = dato?.ProductoCgn?.Where(e => e.Tanque == "TOTAL").FirstOrDefault();


         
            var complexData = new
            {
                DiaOperativo = dato.Fecha,
                Compania = dato?.General?.Nombre,
                VersionFecha = $"{dato?.General?.Version} / {dato?.General?.FechaCadena}",
                PreparadoPor = $"{dato?.General?.PreparadoPor}",
                AprobadoPor = $"{dato?.General?.AprobadoPor}",
                ProcesoTanque1 = procesoTanque1?.Tanque,
                ProcesoTanque2 = procesoTanque2?.Tanque,
                ProcesoNivel1 = procesoTanque1?.Nivel,
                ProcesoNivel2 = procesoTanque2?.Nivel,
                ProcesoInventario1 = procesoTanque1?.Inventario,
                ProcesoInventario2 = procesoTanque2?.Inventario,
                TotalPRoceso = procesoTotal?.Inventario,
                ProductoGlp = productoGlp,
                productoCgn = productoCgn,
                Observacion = dato?.Observacion,
                GlpProduccion = produccionGlp != null ? produccionGlp.Produccion : 0,
                GlpDespacho = produccionGlp != null ? produccionGlp.Despacho : 0,
                GlpInventario = produccionGlp != null ? produccionGlp.Inventario : 0,
                CgnProduccion = produccionCgn != null ? produccionCgn.Produccion : 0,
                CgnDespacho = produccionCgn != null ? produccionCgn.Despacho : 0,
                CgnInventario = produccionCgn != null ? produccionCgn.Inventario : 0,
                TotalProduccion = produccionTotal != null ? produccionTotal.Produccion : 0,
                TotalDespacho = produccionTotal != null ? produccionTotal.Despacho : 0,
                TotalInventario = produccionTotal != null ? produccionTotal.Inventario : 0,
                TotalGlpNivel = productoGlpTotal?.Nivel,
                TotalGlpInventario = productoGlpTotal?.Inventario,
                TotalCgnNivel = productoCgnTotal?.Nivel,
                TotalCgnInventario = productoCgnTotal?.Inventario,
            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";


            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\FisProductos.xlsx"))
            {

                if (!string.IsNullOrWhiteSpace(dato?.General?.RutaFirma))
                {
                    using (var stream = new FileStream(dato.General.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                        var picture = worksheet.AddPicture(stream).MoveTo(worksheet.Cell("B39")).WithSize(120, 70);
                    }
                }

                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }

            var tempFilePathPdf = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            string excelFilePath = tempFilePath;
            string pdfFilePath = tempFilePathPdf;

            using (var excelPackage = new OfficeOpenXml.ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelFile workbook = ExcelFile.Load(excelFilePath);
                workbook.Save(pdfFilePath, SaveOptions.PdfDefault);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);

            System.IO.File.Delete(tempFilePath);

            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.ResumenDiarioFiscalizacionProductos,
                RutaPdf = tempFilePathPdf,
            });

            string nombreArchivo = FechasUtilitario.ObtenerDiaOperativo().ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"{dato.General?.NombreReporte} - {nombreArchivo}.pdf");
        }

    


        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<FiscalizacionProductosDto?> ObtenerAsync()
        {
            var operacion = await _fiscalizacionProductosServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(FiscalizacionProductosDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _fiscalizacionProductosServicio.GuardarAsync(peticion,true);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
