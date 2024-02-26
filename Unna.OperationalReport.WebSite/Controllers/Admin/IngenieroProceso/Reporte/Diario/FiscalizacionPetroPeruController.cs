using ClosedXML.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionPetroPeru.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class FiscalizacionPetroPeruController : ControladorBaseWeb
    {
        private readonly IFiscalizacionPetroPeruServicio _fiscalizacionPetroPeruServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public FiscalizacionPetroPeruController(
            IFiscalizacionPetroPeruServicio fiscalizacionPetroPeruServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _fiscalizacionPetroPeruServicio = fiscalizacionPetroPeruServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }


        [HttpGet("DescarPdf")]
        public async Task<IActionResult> GenerarExcelAsync()
        {

            var operativo = await _fiscalizacionPetroPeruServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }


            var dato = operativo.Resultado;

            //var additionalTableData = new
            //{
            //    Items = new List<FirstTableDataFiscalizacion>
            //    {
            //        new FirstTableDataFiscalizacion { Item = 1, Supplier = "PETROPERU (LOTE Z69)", VolumeGNA = 100.00m, Richness = 10.00m, LGNContent = 5.00m, AssignmentFactor = 0.10m, LGNAssignment = 50.00m },
            //        new FirstTableDataFiscalizacion { Item = 2, Supplier = "PETROPERU (LOTE VI)", VolumeGNA = 200.00m, Richness = 20.00m, LGNContent = 10.00m, AssignmentFactor = 0.20m, LGNAssignment = 100.00m },
            //        new FirstTableDataFiscalizacion { Item = 3, Supplier = "PETROPERU (LOTE I)", VolumeGNA = 300.00m, Richness = 30.00m, LGNContent = 15.00m, AssignmentFactor = 0.30m, LGNAssignment = 150.00m },
            //        new FirstTableDataFiscalizacion { Item = 4, Supplier = "PETROPERU (LOTE I)", VolumeGNA = 300.00m, Richness = 30.00m, LGNContent = 15.00m, AssignmentFactor = 0.30m, LGNAssignment = 150.00m },
            //        new FirstTableDataFiscalizacion { Item = 5, Supplier = "PETROPERU (LOTE I)", VolumeGNA = 300.00m, Richness = 30.00m, LGNContent = 15.00m, AssignmentFactor = 0.30m, LGNAssignment = 150.00m },
            //    }
            //};


            //var factoresDistribucionGasNaturalSeco = new
            //{
            //    Items = dato.FactoresDistribucionGasNaturalSeco
            //};

            var complexData = new
            {
                DiaOperativo = dato.Fecha,
                //GasMpcd = dato.Tabla1.GasMpcd,
                //GlpBls = dato.Tabla1.GlpBls,
                //CgnBls = dato.Tabla1.CgnBls,
                //CnsMpc = dato.Tabla1.CnsMpc,
                //CgMpc = dato.Tabla1.CgMpc,
                //VolumenTotalDeGnsEnMs = dato.VolumenTotalGnsEnMs,
                //FlareGnaPertecienteEnel = dato.VolumenTotalGns,
                //VolumenTotalDeGns = dato.FlareGna,
                //FactoresDistribucionGasNaturalSeco = factoresDistribucionGasNaturalSeco,

            };


            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaDiariaDeFiscalizacionPetroperu.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }

            //var workbook = new Workbook(tempFilePath);
            string tempFilePathPdf = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";
            ////workbook.Save(tempFilePathPdf);            

            //Application excel = new Application();


            ////Func<LoadOptions> loadOptions = () => new SpreadsheetLoadOptions
            ////{
            ////    OnePagePerSheet = true
            ////};
            //using (var converter = new GroupDocs.Conversion.Converter(tempFilePath))
            //{
            //    // Convierta y guarde la hoja de cálculo en formato PDF
            //    converter.Convert(tempFilePathPdf, new PdfConvertOptions());
            //}


            //var renderer = new ChromePdfRenderer();
            //var document = renderer.RenderRtfStringAsPdf(tempFilePath);
            //document.SaveAs(tempFilePathPdf);


            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Workbooks.Add();
            excel.Visible = true;

            //Application excel = new Application();
            //Excel.Application xlApp = new Excel.Application();
            Workbook workbook = excel.Workbooks.Open(tempFilePath);
            workbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, tempFilePathPdf);
            workbook.Close();
            excel.Quit();



            //object misValue = System.Reflection.Missing.Value;
            //string paramExportFilePath = @"C:\Test2.pdf";
            //Excel.XlFixedFormatType paramExportFormat = Excel.XlFixedFormatType.xlTypePDF;
            //Excel.XlFixedFormatQuality paramExportQuality = Excel.XlFixedFormatQuality.xlQualityStandard;
            //bool paramOpenAfterPublish = false;
            //bool paramIncludeDocProps = true;
            //bool paramIgnorePrintAreas = true;
            //if (xlWorkBook != null)//save as pdf
            //    xlWorkBook.ExportAsFixedFormat(paramExportFormat, paramExportFilePath, paramExportQuality, paramIncludeDocProps, paramIgnorePrintAreas, 1, 1, paramOpenAfterPublish, misValue);



            var bytes = System.IO.File.ReadAllBytes(tempFilePathPdf);
            System.IO.File.Delete(tempFilePath);

            return File(bytes, "application/pdf", $"BoletaDiariaDeFiscalizacionPetroperu-{dato.Fecha.Replace("/", "-")}.pdf");
        }



        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<FiscalizacionPetroPeruDto?> ObtenerAsync()
        {
            var operacion = await _fiscalizacionPetroPeruServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(FiscalizacionPetroPeruDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.idUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _fiscalizacionPetroPeruServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


    }
}
