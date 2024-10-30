using ClosedXML.Excel;
using ClosedXML.Report;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Unna.OperationalReport.Service.Reportes.BoletaFiscalizacion.Dtos;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.Reporte
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoletaFiscalizacionController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BoletaFiscalizacionController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("downloadVoucher")]
        public IActionResult DownloadVoucher()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Boleta Diaria de Fiscalización");

                var image = worksheet.AddPicture("path_to_image.jpg")
                   .MoveTo(worksheet.Cell("B2"))
                   .Scale(0.5); 

                worksheet.Cell("B2").Value = "UNNA ENERGÍA";
                worksheet.Cell("E2").Value = "BOLETA DIARIA DE FISCALIZACIÓN";
                worksheet.Cell("E3").Value = "PETROPERU";
                worksheet.Range("B2:D3").Merge(); 
                worksheet.Range("E2:H3").Style
                    .Font.SetBold()
                    .Font.SetFontSize(14)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Range("E2:H3").Merge();

                worksheet.Cell("I2").Value = "UNNA ENERGÍA";
                worksheet.Cell("I3").Value = "Versión / Fecha:";
                worksheet.Cell("J3").Value = "0 / 28-11-23";
                worksheet.Cell("I4").Value = "Preparado por:";
                worksheet.Cell("J4").Value = "JPG";
                worksheet.Cell("I5").Value = "Aprobado por:";
                worksheet.Cell("J5").Value = "GOG";
                worksheet.Range("I2:J5").Style
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    .Font.SetFontSize(11);

                worksheet.Cell("K3").Value = "FECHA:";
                worksheet.Cell("L3").Value = "31/12/2023";
                worksheet.Range("K3:L3").Style
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    .Font.SetFontSize(11);

                worksheet.Range("B2:L5").Style
                    .Border.SetTopBorder(XLBorderStyleValues.Medium)
                    .Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    .Border.SetRightBorder(XLBorderStyleValues.Medium)
                    .Border.SetLeftBorder(XLBorderStyleValues.Medium);

                worksheet.Row(2).Height = 20;
                worksheet.Row(3).Height = 20;
                worksheet.Row(4).Height = 20;
                worksheet.Row(5).Height = 20; 

                worksheet.Column("A").Width = 3.43; 
                worksheet.Column("B").Width = 15;
                worksheet.Column("C").Width = worksheet.Column("B").Width; // Asumiendo que el ancho es el mismo que el de B
                worksheet.Column("D").Width = 10; 
                worksheet.Column("E").Width = 20; 
                worksheet.Column("F").Width = 20; 
                worksheet.Column("G").Width = 10; 
                worksheet.Column("H").Width = worksheet.Column("E").Width; // Asumiendo que el ancho es el mismo que el de E
                worksheet.Column("I").Width = 15; 
                worksheet.Column("J").Width = 15; 
                worksheet.Column("K").Width = 10; 
                worksheet.Column("L").Width = 15; 
                worksheet.Column("M").Width = 10; 

                // Ajustar las filas y columnas para que coincidan con la imagen proporcionada
                worksheet.Row(2).Height = 30; 
                worksheet.Row(3).Height = 20; 
                worksheet.Row(4).Height = 20; 
                worksheet.Row(5).Height = 20; 

                worksheet.Range("B2:L5").Style
                    .Border.SetTopBorder(XLBorderStyleValues.Medium)
                    .Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    .Border.SetRightBorder(XLBorderStyleValues.Medium)
                    .Border.SetLeftBorder(XLBorderStyleValues.Medium);

                
                // Guardar el documento
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BoletaFiscalizacion.xlsx");
                }

            }
        }

        [HttpGet("GenerateDailyReport")]
        public IActionResult GenerateDailyReport()
        {
            var additionalTableData = new
            {
                Items = new List<FirstTableDataFiscalizacion>
                {
                    new FirstTableDataFiscalizacion { Item = 1, Supplier = "PETROPERU (LOTE Z69)", VolumeGNA = 100.00m, Richness = 10.00m, LGNContent = 5.00m, AssignmentFactor = 0.10m, LGNAssignment = 50.00m },
                    new FirstTableDataFiscalizacion { Item = 2, Supplier = "PETROPERU (LOTE VI)", VolumeGNA = 200.00m, Richness = 20.00m, LGNContent = 10.00m, AssignmentFactor = 0.20m, LGNAssignment = 100.00m },
                    new FirstTableDataFiscalizacion { Item = 3, Supplier = "PETROPERU (LOTE I)", VolumeGNA = 300.00m, Richness = 30.00m, LGNContent = 15.00m, AssignmentFactor = 0.30m, LGNAssignment = 150.00m },
                    new FirstTableDataFiscalizacion { Item = 4, Supplier = "PETROPERU (LOTE I)", VolumeGNA = 300.00m, Richness = 30.00m, LGNContent = 15.00m, AssignmentFactor = 0.30m, LGNAssignment = 150.00m },
                    new FirstTableDataFiscalizacion { Item = 5, Supplier = "PETROPERU (LOTE I)", VolumeGNA = 300.00m, Richness = 30.00m, LGNContent = 15.00m, AssignmentFactor = 0.30m, LGNAssignment = 150.00m },
                }

            };
            var secondTableData = new
            {
                Items = new List<SecondTableDataFiscalizacion>
                {
                    new SecondTableDataFiscalizacion { Item = 1, Supplier = "PETROPERU (LOTE Z69)", VolumeGNA = 0.00000m, CalorificValue = 1.47m, VolumeGNS = 0.00000m, VolumeGNSd = 0.00000m },
                    new SecondTableDataFiscalizacion { Item = 2, Supplier = "PETROPERU (LOTE VI)", VolumeGNA = 0.00000m, CalorificValue = 2.29m, VolumeGNS = 0.00000m, VolumeGNSd = 0.00000m },
                    new SecondTableDataFiscalizacion { Item = 3, Supplier = "PETROPERU (LOTE I)", VolumeGNA = 0.00000m, CalorificValue = 1.89m, VolumeGNS = 0.00000m, VolumeGNSd = 0.00000m },
                    new SecondTableDataFiscalizacion { Item = 4, Supplier = "PETROPERU (LOTE I)", VolumeGNA = 0.00000m, CalorificValue = 1.89m, VolumeGNS = 0.00000m, VolumeGNSd = 0.00000m },
                    new SecondTableDataFiscalizacion { Item = 5, Supplier = "PETROPERU (LOTE I)", VolumeGNA = 0.00000m, CalorificValue = 1.89m, VolumeGNS = 0.00000m, VolumeGNSd = 0.00000m },

                },
                TotalCalorificValue = 5.6432m

            };
            var thirdTableData = new
            {
                Items = new List<ThirdTableDataFiscalizacion>
                    {
                        new ThirdTableDataFiscalizacion { Item = 1, Supplier = "PETROPERU (LOTE Z69)", VolumeGNS = 0.0000m, FlareVolume = 1.0000m, TransferredGNSVolume = 1.0000m },
                        new ThirdTableDataFiscalizacion { Item = 2, Supplier = "PETROPERU (LOTE VI)", VolumeGNS = 0.0000m, FlareVolume = 2.0000m, TransferredGNSVolume = 2.0000m },
                        new ThirdTableDataFiscalizacion { Item = 3, Supplier = "PETROPERU (LOTE I)", VolumeGNS = 0.0000m, FlareVolume = 3.0000m, TransferredGNSVolume = 3.0000m },
                    }
            };
            var complexData = new
            {
                ReportName = "BOLETA DIARIA DE FISCALIZACIÓN PETROPERU",
                CompanyName = "UNNA Energía",
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                PreparedBy = "JPG",
                ApprovedBy = "GOG",
                Signature = "Representante UNNA ENERGIA-PGT",
                VolumeData = new
                {
                    TotalProductionVolume = 999.90m,
                    Efficiency = 89.80m,
                    TotalContentLGN = 1113.51m
                },
                ConversionFactors = new
                {
                    ConversionFactorLotZ69 = 0.00m,
                    ConversionFactorLotVI = 0.00m,
                    ConversionFactorLotI = 0.00m,
                },
                GNSData = new
                {
                    TotalGNSVolume = 10324.71m,
                    GNSFlareVolume = -10324.71m
                },
                AdditionalTable = additionalTableData,
                SecondTable = secondTableData,
                ThirdTable = thirdTableData

            };

            var tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "report_daily.xlsx");



            using (var template = new XLTemplate(@"DailyReport.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }

            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DailyReport.xlsx");
        }

        [HttpGet("downloadPdf")]
        public IActionResult DownloadPDF()
        {
            return new ViewAsPdf("/Pages/Admin/FiscalizadorPetroPeru/Reporte/IndexPDF.cshtml")
            {
                FileName = "BoletaFiscalizacion.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                Model = null 
            };
        }
    }
}
