using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Report;
using GemBox.Spreadsheet;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class BoletaDeterminacionVolumenController : ControladorBaseWeb
    {
        private readonly IBoletaDeterminacionVolumenGnaServicio _boletaDeterminacionVolumenGnaServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public BoletaDeterminacionVolumenController(
        IBoletaDeterminacionVolumenGnaServicio boletaDeterminacionVolumenGnaServicio,
        IWebHostEnvironment hostingEnvironment,
        GeneralDto general
        )
        {
            _boletaDeterminacionVolumenGnaServicio = boletaDeterminacionVolumenGnaServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }
        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<BoletaDeterminacionVolumenGnaDto?> ObtenerAsync()
        {
            var operacion = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(BoletaDeterminacionVolumenGnaDto boletaDeterminacionVolumenGna)
        {
            VerificarIfEsBuenJson(boletaDeterminacionVolumenGna);
            boletaDeterminacionVolumenGna.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaDeterminacionVolumenGnaServicio.GuardarAsync(boletaDeterminacionVolumenGna);
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
            System.IO.File.Delete(url);

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BOLETA DE DETERMINACION DE VOLUMEN DE GNA FISCALIZADO - {nombreArchivo}.xlsx");

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
            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/pdf", $"BOLETA DE DETERMINACION DE VOLUMEN DE GNA FISCALIZADO - {nombreArchivo}.pdf");
        }



        private async Task<string?> GenerarAsync()
        {
            var operativo = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return null;
            }
            var dato = operativo.Resultado;
            if (dato.FactoresAsignacionGasCombustible != null)
            {
                dato.FactoresAsignacionGasCombustible.ForEach(e => e.FactorAsignacion = (e.FactorAsignacion / 100));
            }
            if (dato.FactorAsignacionGns != null)
            {
                dato.FactorAsignacionGns.ForEach(e => e.FactorAsignacion = (e.FactorAsignacion / 100));
            }

            if (dato.FactorAsignacionLiquidosGasNatural != null)
            {
                dato.FactorAsignacionLiquidosGasNatural.ForEach(e => e.FactorAsignacion = (e.FactorAsignacion / 100));
            }


            var factoresAsignacionGasCombustible = new
            {
                Items = dato.FactoresAsignacionGasCombustible
            };

            var factorAsignacionGns = new
            {
                Items = dato.FactorAsignacionGns
            };

            var factorAsignacionLiquidosGasNatural = new
            {
                Items = dato.FactorAsignacionLiquidosGasNatural
            };

            var complexData = new
            {
                Compania = dato?.General?.Nombre,
                PreparadoPör = $"{dato?.General?.PreparadoPör}",
                AprobadoPor = $"{dato?.General?.AprobadoPor}",
                VersionFecha = $"{dato?.General?.Version} / {dato?.General?.Fecha}",

                DiaOperativo = dato?.Fecha,
                VolumenTotalGasCombustible = dato?.VolumenTotalGasCombustible,
                VolumenTotalGns = dato?.VolumenTotalGns,

                FactoresAsignacionGasCombustible = factoresAsignacionGasCombustible,
                FactorAsignacionGns = factorAsignacionGns,
                VolumenProduccionTotalGlp = dato?.VolumenProduccionTotalGlp,
                VolumenProduccionTotalCgn = dato?.VolumenProduccionTotalCgn,
                VolumenProduccionTotalLgn = dato?.VolumenProduccionTotalLgn,
                FactorAsignacionLiquidosGasNatural = factorAsignacionLiquidosGasNatural,
                DistribucionGasNaturalAsociado = dato?.DistribucionGasNaturalAsociado,
                VolumenProduccionTotalGlpLoteIv = dato?.VolumenProduccionTotalGlpLoteIv,
                VolumenProduccionTotalCgnLoteIv = dato?.VolumenProduccionTotalCgnLoteIv,
                FactorCoversion = dato?.FactorCoversion,

                VolumenGnsVentaVgnsvTotal = dato?.VolumenGnsVentaVgnsvTotal ?? 0,
                VolumenGnsVentaVgnsvEnel = dato?.VolumenGnsVentaVgnsvEnel ?? 0,
                VolumenGnsVentaVgnsvGasnorp = dato?.VolumenGnsVentaVgnsvGasnorp ?? 0,
                VolumenGnsVentaVgnsvLimagas = dato?.VolumenGnsVentaVgnsvLimagas ?? 0,
                VolumenGnsFlareVgnsrf = dato?.VolumenGnsFlareVgnsrf ?? 0,
                SumaVolumenGasCombustibleVolumen = double.IsNaN(dato?.SumaVolumenGasCombustibleVolumen ?? 0) ? 0 : dato.SumaVolumenGasCombustibleVolumen,
                VolumenGnaFiscalizado = dato?.VolumenGnaFiscalizado ?? 0,

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaDeterminacionVolGNA.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }

        //[HttpGet("GenerarPDF2")]
        //public async Task<IActionResult> ConvertirExcelAPdfYDescargar()
        //{
        //    var operativo = await _boletaDeterminacionVolumenGnaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
        //    if (!operativo.Completado || operativo.Resultado == null)
        //    {
        //        return File(new byte[0], "application/octet-stream");
        //    }
        //    var dato = operativo.Resultado;
        //    var factoresAsignacionGasCombustible = new
        //    {
        //        Items = dato.FactoresAsignacionGasCombustible
        //    };

        //    var factorAsignacionGns = new
        //    {
        //        Items = dato.FactorAsignacionGns
        //    };

        //    var factorAsignacionLiquidosGasNatural = new
        //    {
        //        Items = dato.FactorAsignacionLiquidosGasNatural
        //    };

        //    var complexData = new
        //    {
        //        Compania = dato?.General?.Nombre,
        //        PreparadoPör = $"Preparado por: {dato?.General?.PreparadoPör}",
        //        AprobadoPor = $"Aprobado por: {dato?.General?.AprobadoPor}",

        //        DiaOperativo = dato?.Fecha,
        //        VolumenTotalGasCombustible = dato?.VolumenTotalGasCombustible,
        //        VolumenTotalGns = dato?.VolumenTotalGns,

        //        FactoresAsignacionGasCombustible = factoresAsignacionGasCombustible,
        //        FactorAsignacionGns = factorAsignacionGns,
        //        VolumenProduccionTotalGlp = dato?.VolumenProduccionTotalGlp,
        //        VolumenProduccionTotalCgn = dato?.VolumenProduccionTotalCgn,
        //        VolumenProduccionTotalLgn = dato?.VolumenProduccionTotalLgn,
        //        FactorAsignacionLiquidosGasNatural = factorAsignacionLiquidosGasNatural,
        //        DistribucionGasNaturalAsociado = dato?.DistribucionGasNaturalAsociado,
        //        VolumenProduccionTotalGlpLoteIv = dato?.VolumenProduccionTotalGlpLoteIv,
        //        VolumenProduccionTotalCgnLoteIv = dato?.VolumenProduccionTotalCgnLoteIv,
        //        FactorCoversion = dato?.FactorCoversion,

        //        VolumenGnsVentaVgnsvTotal = dato?.VolumenGnsVentaVgnsvTotal ?? 0,
        //        VolumenGnsVentaVgnsvEnel = dato?.VolumenGnsVentaVgnsvEnel ?? 0,
        //        VolumenGnsVentaVgnsvGasnorp = dato?.VolumenGnsVentaVgnsvGasnorp ?? 0,
        //        VolumenGnsVentaVgnsvLimagas = dato?.VolumenGnsVentaVgnsvLimagas ?? 0,
        //        VolumenGnsFlareVgnsrf = dato?.VolumenGnsFlareVgnsrf ?? 0,
        //        SumaVolumenGasCombustibleVolumen = double.IsNaN(dato?.SumaVolumenGasCombustibleVolumen ?? 0) ? 0 : dato.SumaVolumenGasCombustibleVolumen,
        //        VolumenGnaFiscalizado = dato?.VolumenGnaFiscalizado ?? 0,

        //    };

        //    var memoryStream = new MemoryStream();
        //    var writer = new PdfWriter(memoryStream);
        //    writer.SetCloseStream(false);

        //    var pdf = new PdfDocument(writer);
        //    var document = new Document(pdf, PageSize.A4);
        //    document.SetMargins(30, 30, 30, 30);

        //    PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        //    PdfFont normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        //    Table mainTable = new Table(UnitValue.CreatePercentArray(new float[] { 4, 3, 4 })).UseAllAvailableWidth();

        //    float widthInPoints = 80; 
        //    float heightInPoints = 40; 
        //    String imagePath = "wwwroot/images/logo.png";
        //    ImageData imageData = ImageDataFactory.Create(imagePath);
        //    Image logo = new Image(imageData).SetWidth(widthInPoints).SetHeight(heightInPoints);
        //    Cell imageCell = new Cell().Add(logo)
        //        .SetPadding(10)
        //        .SetBorder(Border.NO_BORDER)
        //        .SetVerticalAlignment(VerticalAlignment.MIDDLE);
        //    mainTable.AddCell(imageCell);

        //    Cell titleDetailsCell = new Cell(1, 2)
        //        .Add(new Paragraph("BOLETA DE DETERMINACION DE VOLUMEN DE GNA FISCALIZADO")
        //             .SetFont(bold).SetFontSize(14))
        //        .Add(new Paragraph("UNNA ENERGIA - LOTE IV")
        //             .SetFont(bold));
        //    mainTable.AddCell(titleDetailsCell);

        //    Cell versionCell = new Cell().Add(new Paragraph("Versión/Fecha\nv1.0 / 2024-03-24")
        //        .SetFont(normal));
        //    mainTable.AddCell(versionCell);

        //    Cell preparedByCell = new Cell().Add(new Paragraph(complexData.PreparadoPör)
        //        .SetFont(normal));
        //    mainTable.AddCell(preparedByCell);

        //    Cell approvedByCell = new Cell().Add(new Paragraph(complexData.AprobadoPor)
        //        .SetFont(normal)
        //        .SetTextAlignment(TextAlignment.RIGHT));
        //    mainTable.AddCell(approvedByCell);

        //    document.Add(mainTable);

        //    Table floatRightTable = new Table(2)
        //        .UseAllAvailableWidth()
        //        ;

        //    floatRightTable.AddCell(new Cell().Add(new Paragraph("FECHA").SetBold()));
        //    floatRightTable.AddCell(new Cell().Add(new Paragraph(complexData.DiaOperativo).SetBold()));  

        //    document.Add(floatRightTable);

        //    Paragraph title = new Paragraph("Cuadro N° 1. Asignación de Volumen de Gas Combustible (GC) - LOTE IV")
        //        .SetTextAlignment(TextAlignment.CENTER)
        //        .SetBold()
        //        .SetFontSize(12);
        //    document.Add(title);

        //    Table inputTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1, 1 }))
        //        .UseAllAvailableWidth();
        //    inputTable.AddCell(new Cell().Add(new Paragraph("Volumen Total de Gas Combustible")).SetBold());
        //    inputTable.AddCell(new Cell().Add(new Paragraph(complexData.VolumenTotalGasCombustible.ToString()))); 
        //    inputTable.AddCell(new Cell().Add(new Paragraph("(MPCS)")));
        //    document.Add(inputTable);

        //    Table Table1 = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3, 2, 4, 2, 2, 2 }))
        //        .UseAllAvailableWidth();

        //    Table1.AddHeaderCell(new Cell(3, 1).Add(new Paragraph("ITEM")));
        //    Table1.AddHeaderCell(new Cell(3, 1).Add(new Paragraph("SUMINISTRADOR")));
        //    Table1.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("VOLUMEN DE GNA (Mpcs)")));
        //    Table1.AddHeaderCell(new Cell(1, 4).Add(new Paragraph("Factores de Asignación de Gas Combustible")));

        //    Table1.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("PODER CALORIFICO DEL GNA EN BTU/PC (De cromatografia de gases)")));
        //    Table1.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("ENERGIA MMBTU")));
        //    Table1.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("FACTOR DE ASIGNACIÓN DE GNS [FdGNS] (%)")));
        //    Table1.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Asignación de Gas Combustible (Mpcs)")));

        //    Table1.AddHeaderCell(new Cell().Add(new Paragraph("(Fi)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    Table1.AddHeaderCell(new Cell().Add(new Paragraph("(Gi)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    Table1.AddHeaderCell(new Cell().Add(new Paragraph("(Hi=FixGi)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    Table1.AddHeaderCell(new Cell().Add(new Paragraph("(Ii=Hi/Ht)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    Table1.AddHeaderCell(new Cell().Add(new Paragraph("(Ji=IixGC Total)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));

        //    foreach (var item in complexData.FactoresAsignacionGasCombustible.Items)
        //    {
        //        Table1.AddCell(new Cell().Add(new Paragraph(item.Item.ToString())));
        //        Table1.AddCell(new Cell().Add(new Paragraph(item.Suministrador)));
        //        Table1.AddCell(new Cell().Add(new Paragraph(item.Volumen.ToString()))); 
        //        Table1.AddCell(new Cell().Add(new Paragraph(item.VolumenCalorifico.ToString()))); 
        //        Table1.AddCell(new Cell().Add(new Paragraph(item.EnergiaMmbtu.ToString()))); 
        //        Table1.AddCell(new Cell().Add(new Paragraph(item.FactorAsignacion.ToString())));
        //        Table1.AddCell(new Cell().Add(new Paragraph(item.Asignacion.ToString()))); 
        //    }

        //    document.Add(Table1);

        //    Paragraph title2 = new Paragraph("Cuadro N° 2. Asignación de Volumen de Gas Natural Seco (GNS) - LOTE IV")
        //        .SetTextAlignment(TextAlignment.CENTER)
        //        .SetBold()
        //        .SetFontSize(12);
        //    document.Add(title2);

        //    Table gnsInputTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1, 1 }))
        //        .UseAllAvailableWidth();
        //    gnsInputTable.AddCell(new Cell().Add(new Paragraph("Volumen Total de GNS")).SetBold());
        //    gnsInputTable.AddCell(new Cell().Add(new Paragraph(complexData.VolumenTotalGns.ToString()))); 
        //    gnsInputTable.AddCell(new Cell().Add(new Paragraph("(MPCS)")));
        //    document.Add(gnsInputTable);

        //    Paragraph title3 = new Paragraph("")
        //        .SetTextAlignment(TextAlignment.CENTER)
        //        .SetBold()
        //        .SetFontSize(12);
        //    document.Add(title3);

        //    Table mainTable2 = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3, 2, 4, 2, 2, 2 }))
        //        .UseAllAvailableWidth();

        //    mainTable2.AddHeaderCell(new Cell(3, 1).Add(new Paragraph("ITEM")));
        //    mainTable2.AddHeaderCell(new Cell(3, 1).Add(new Paragraph("SUMINISTRADOR")));
        //    mainTable2.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("VOLUMEN DE GNA (Mpcs)")));
        //    mainTable2.AddHeaderCell(new Cell(1, 4).Add(new Paragraph("Factores de Asignación de Gas Combustible")));

        //    mainTable2.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("PODER CALORIFICO DEL GNA EN BTU/PC (De cromatografia de gases)")));
        //    mainTable2.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("ENERGIA MMBTU")));
        //    mainTable2.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("FACTOR DE ASIGNACIÓN DE GNS [FdGNS] (%)")));
        //    mainTable2.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Asignación de Gas Combustible (Mpcs)")));

        //    mainTable2.AddHeaderCell(new Cell().Add(new Paragraph("(Fi)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    mainTable2.AddHeaderCell(new Cell().Add(new Paragraph("(Gi)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    mainTable2.AddHeaderCell(new Cell().Add(new Paragraph("(Hi=FixGi)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    mainTable2.AddHeaderCell(new Cell().Add(new Paragraph("(Ii=Hi/Ht)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    mainTable2.AddHeaderCell(new Cell().Add(new Paragraph("(Ji=IixGC Total)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));

        //    foreach (var item in complexData.FactorAsignacionGns.Items)
        //    {
        //        mainTable2.AddCell(new Cell().Add(new Paragraph(item.Item.ToString())));
        //        mainTable2.AddCell(new Cell().Add(new Paragraph(item.Suministrador)));
        //        mainTable2.AddCell(new Cell().Add(new Paragraph(item.Volumen.ToString()))); 
        //        mainTable2.AddCell(new Cell().Add(new Paragraph(item.Calorifico.ToString()))); 
        //        mainTable2.AddCell(new Cell().Add(new Paragraph(item.EnergiaMmbtu.ToString()))); 
        //        mainTable2.AddCell(new Cell().Add(new Paragraph(item.FactorAsignacion.ToString()))); 
        //        mainTable2.AddCell(new Cell().Add(new Paragraph(item.Asignacion.ToString()))); 
        //    }

        //    document.Add(mainTable2);

        //    Paragraph note = new Paragraph("Nota: Para el Cuadro N° 2, cuando no se concrete una venta a ENEL del Gas Natural Seco del Lote IV, para efectos de cálculo se considera el volumen 0.")
        //        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
        //        .SetFontSize(8)
        //        .SetTextAlignment(TextAlignment.LEFT);
        //    document.Add(note);

        //    Paragraph title4 = new Paragraph("Cuadro N° 3. Asignación de Volumen de Líquidos del Gas Natural (LGN) - LOTE IV")
        //        .SetTextAlignment(TextAlignment.CENTER)
        //        .SetBold()
        //        .SetFontSize(12);
        //    document.Add(title4);

        //    Table gnsInputTable4 = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1, 1 }))
        //        .UseAllAvailableWidth();
        //    gnsInputTable.AddCell(new Cell().Add(new Paragraph("Volumen Total de GNS")).SetBold());
        //    gnsInputTable.AddCell(new Cell().Add(new Paragraph(complexData.VolumenProduccionTotalGlp.ToString()))); 
        //    gnsInputTable.AddCell(new Cell().Add(new Paragraph("(MPCS)")));
        //    document.Add(gnsInputTable4);

        //    Table inputTable4 = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1, 1 }))
        //        .UseAllAvailableWidth();
        //    inputTable4.AddCell(new Cell().Add(new Paragraph("Volumen Total de Gas Combustible")).SetBold());
        //    inputTable4.AddCell(new Cell().Add(new Paragraph(complexData.VolumenProduccionTotalCgn.ToString()))); 
        //    inputTable4.AddCell(new Cell().Add(new Paragraph("(MPCS)")));
        //    document.Add(inputTable4);

        //    Table inputTable55 = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1, 1 }))
        //        .UseAllAvailableWidth();
        //    inputTable4.AddCell(new Cell().Add(new Paragraph("Volumen Total de Gas Combustible")).SetBold());
        //    inputTable4.AddCell(new Cell().Add(new Paragraph(complexData.VolumenProduccionTotalLgn.ToString()))); 
        //    inputTable4.AddCell(new Cell().Add(new Paragraph("(MPCS)")));
        //    document.Add(inputTable55);

        //    Paragraph title5 = new Paragraph("")
        //        .SetTextAlignment(TextAlignment.CENTER)
        //        .SetBold()
        //        .SetFontSize(12);
        //    document.Add(title5);

        //    Table mainTable6 = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3, 2, 4, 2, 2, 2 }))
        //        .UseAllAvailableWidth();

        //    mainTable6.AddHeaderCell(new Cell(3, 1).Add(new Paragraph("ITEM")));
        //    mainTable6.AddHeaderCell(new Cell(3, 1).Add(new Paragraph("SUMINISTRADOR")));
        //    mainTable6.AddHeaderCell(new Cell(2, 1).Add(new Paragraph("VOLUMEN DE GNA (Mpcs)")));
        //    mainTable6.AddHeaderCell(new Cell(1, 4).Add(new Paragraph("Factores de Asignación de Gas Combustible")));

        //    mainTable6.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("PODER CALORIFICO DEL GNA EN BTU/PC (De cromatografia de gases)")));
        //    mainTable6.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("ENERGIA MMBTU")));
        //    mainTable6.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("FACTOR DE ASIGNACIÓN DE GNS [FdGNS] (%)")));
        //    mainTable6.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Asignación de Gas Combustible (Mpcs)")));

        //    mainTable6.AddHeaderCell(new Cell().Add(new Paragraph("(Fi)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    mainTable6.AddHeaderCell(new Cell().Add(new Paragraph("(Gi)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    mainTable6.AddHeaderCell(new Cell().Add(new Paragraph("(Hi=FixGi)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    mainTable6.AddHeaderCell(new Cell().Add(new Paragraph("(Ii=Hi/Ht)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    mainTable6.AddHeaderCell(new Cell().Add(new Paragraph("(Ji=IixGC Total)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));

        //    foreach (var item in complexData.FactorAsignacionLiquidosGasNatural.Items)
        //    {
        //        mainTable6.AddCell(new Cell().Add(new Paragraph(item.Item.ToString())));
        //        mainTable6.AddCell(new Cell().Add(new Paragraph(item.Suministrador)));
        //        mainTable6.AddCell(new Cell().Add(new Paragraph(item.Volumen.ToString()))); 
        //        mainTable6.AddCell(new Cell().Add(new Paragraph(item.Riqueza.ToString()))); 
        //        mainTable6.AddCell(new Cell().Add(new Paragraph(item.Contenido.ToString()))); 
        //        mainTable6.AddCell(new Cell().Add(new Paragraph(item.FactorAsignacion.ToString()))); 
        //        mainTable6.AddCell(new Cell().Add(new Paragraph(item.Asignacion.ToString()))); 
        //    }

        //    document.Add(mainTable6);

        //    Table tableBot = new Table(UnitValue.CreatePercentArray(new float[] { 5, 3, 2 }))
        //        .UseAllAvailableWidth();
        //    tableBot.AddCell(new Cell().Add(new Paragraph("Volumen de Producción Total de GLP LOTE IV"))
        //        .SetBold().SetTextAlignment(TextAlignment.LEFT));
        //    tableBot.AddCell(new Cell().Add(new Paragraph(complexData.VolumenProduccionTotalGlpLoteIv.ToString()))
        //        .SetTextAlignment(TextAlignment.RIGHT));
        //    tableBot.AddCell(new Cell().Add(new Paragraph("Bls"))
        //        .SetTextAlignment(TextAlignment.LEFT));

        //    tableBot.AddCell(new Cell().Add(new Paragraph("Volumen de Producción Total de CGN LOTE IV"))
        //        .SetBold().SetTextAlignment(TextAlignment.LEFT));
        //    tableBot.AddCell(new Cell().Add(new Paragraph(complexData.VolumenProduccionTotalCgnLoteIv.ToString()))
        //        .SetTextAlignment(TextAlignment.RIGHT));
        //    tableBot.AddCell(new Cell().Add(new Paragraph("Bls"))
        //        .SetTextAlignment(TextAlignment.LEFT));

        //    float pageWidth = pdf.GetDefaultPageSize().GetWidth();
        //    float rightMargin = 150; 
        //    float tableWidth = 190;

        //    float leftMargin = pageWidth - tableWidth - rightMargin - document.GetLeftMargin() - document.GetRightMargin();
        //    tableBot.SetMarginLeft(leftMargin);
        //    document.Add(tableBot);

        //    Table tableBot2 = new Table(UnitValue.CreatePercentArray(new float[] { 5, 3, 2 }))
        //        .UseAllAvailableWidth();
        //    tableBot2.AddCell(new Cell().Add(new Paragraph("Factor de conversión"))
        //        .SetBold().SetTextAlignment(TextAlignment.LEFT));
        //    tableBot2.AddCell(new Cell().Add(new Paragraph(complexData.FactorCoversion.ToString()))
        //        .SetTextAlignment(TextAlignment.RIGHT));
        //    tableBot2.AddCell(new Cell().Add(new Paragraph("PCSD/Gal"))
        //        .SetTextAlignment(TextAlignment.LEFT));

        //    float pageWidth2 = pdf.GetDefaultPageSize().GetWidth();
        //    float rightMargin2 = 150;
        //    float tableWidth2 = 190;

        //    float leftMargin2 = pageWidth2 - tableWidth2 - rightMargin2 - document.GetLeftMargin() - document.GetRightMargin();
        //    tableBot2.SetMarginLeft(leftMargin2);
        //    document.Add(tableBot2);

        //    Paragraph title04 = new Paragraph("Cuadro N° 4. Volumen Fiscalizado del Gas Natural Asociado (GNA) - LOTE IV")
        //        .SetTextAlignment(TextAlignment.CENTER)
        //        .SetBold()
        //        .SetFontSize(12);
        //    document.Add(title04);

        //    Table table004 = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3,  2, 2, 2, 2 }))
        //        .UseAllAvailableWidth();

        //    table004.AddHeaderCell(new Cell(3, 1).Add(new Paragraph("ITEM")));
        //    table004.AddHeaderCell(new Cell(3, 1).Add(new Paragraph("SUMINISTRADOR")));
        //    table004.AddHeaderCell(new Cell(1, 4).Add(new Paragraph("Factores de Asignación de Gas Combustible")));

        //    table004.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("PODER CALORIFICO DEL GNA EN BTU/PC (De cromatografia de gases)")));
        //    table004.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("ENERGIA MMBTU")));
        //    table004.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("FACTOR DE ASIGNACIÓN DE GNS [FdGNS] (%)")));
        //    table004.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Asignación de Gas Combustible (Mpcs)")));

        //    table004.AddHeaderCell(new Cell().Add(new Paragraph("(Gi)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    table004.AddHeaderCell(new Cell().Add(new Paragraph("(Hi=FixGi)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    table004.AddHeaderCell(new Cell().Add(new Paragraph("(Ii=Hi/Ht)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));
        //    table004.AddHeaderCell(new Cell().Add(new Paragraph("(Ji=IixGC Total)").SetFontSize(8).SetFontColor(ColorConstants.GRAY)));

        //    table004.AddCell(new Cell().Add(new Paragraph(complexData.DistribucionGasNaturalAsociado.Item.ToString())));
        //    table004.AddCell(new Cell().Add(new Paragraph(complexData.DistribucionGasNaturalAsociado.Suministrador.ToString())));
        //    table004.AddCell(new Cell().Add(new Paragraph(complexData.DistribucionGasNaturalAsociado.VolumenGnsd.ToString()))); 
        //    table004.AddCell(new Cell().Add(new Paragraph(complexData.DistribucionGasNaturalAsociado.GasCombustible.ToString()))); 
        //    table004.AddCell(new Cell().Add(new Paragraph(complexData.DistribucionGasNaturalAsociado.VolumenGns.ToString()))); 
        //    table004.AddCell(new Cell().Add(new Paragraph(complexData.DistribucionGasNaturalAsociado.VolumenGna.ToString())));           

        //    document.Add(table004);

        //    Table ventaTotalTable = new Table(new float[] { 1, 2, 1 })
        //        .UseAllAvailableWidth();

        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Volumen de GNS a venta (VGNSv) - Total")));
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph(complexData.VolumenGnsVentaVgnsvTotal.ToString())));  
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Mpcsd")));

        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Volumen de GNS a venta (VGNSv) - ENEL")));
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph(complexData.VolumenGnsVentaVgnsvEnel.ToString())));  
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Mpcsd")));

        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Volumen de GNS a venta (VGNSv)-GASNORP")));
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph(complexData.VolumenGnsVentaVgnsvGasnorp.ToString())));  
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Mpcsd")));

        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Volumen de GNS a venta (VGNSv)- Limagas")));
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph(complexData.VolumenGnsVentaVgnsvLimagas.ToString())));  
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Mpcsd")));

        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Volumen de GNS flare (VGNSRF)")));
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph(complexData.VolumenGnsFlareVgnsrf.ToString())));  
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Mpcsd")));

        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Suma de volumen de Gas Combustible y Volumen GNS equiv. de LGN (VGC+VGL)")));
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph(complexData.SumaVolumenGasCombustibleVolumen.ToString()))); 
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Mpcsd")));

        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Volumen de GNA fiscalizado (VGNAF)")));
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph(complexData.VolumenGnaFiscalizado.ToString())));  
        //    ventaTotalTable.AddCell(new Cell().Add(new Paragraph("Mpcsd")));

        //    document.Add(ventaTotalTable);

        //    document.Close();
        //    memoryStream.Position = 0;

        //    return new FileStreamResult(memoryStream, "application/pdf")
        //    {
        //        FileDownloadName = "ReporteFicticio.pdf"
        //    };
        //}
    }
}
