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

                //Insertar una imagen en la cabecera si es necesario
                var image = worksheet.AddPicture("path_to_image.jpg")
                   .MoveTo(worksheet.Cell("B2"))
                   .Scale(0.5); // Ajustar la escala según sea necesario

                worksheet.Cell("B2").Value = "UNNA ENERGÍA";
                worksheet.Cell("E2").Value = "BOLETA DIARIA DE FISCALIZACIÓN";
                worksheet.Cell("E3").Value = "PETROPERU";
                worksheet.Range("B2:D3").Merge(); // Ajustar si se incluye una imagen
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
                worksheet.Row(5).Height = 20; // Ajusta esta altura para que coincida con la imagen si es necesario

                worksheet.Column("A").Width = 3.43; // Ajustar el ancho de la columna A si es necesario
                worksheet.Column("B").Width = 15;
                worksheet.Column("C").Width = worksheet.Column("B").Width; // Asumiendo que el ancho es el mismo que el de B
                worksheet.Column("D").Width = 10; // Ajusta este valor según sea necesario
                worksheet.Column("E").Width = 20; // Ajusta este valor según sea necesario
                worksheet.Column("F").Width = 20; // Ajusta este valor según sea necesario
                worksheet.Column("G").Width = 10; // Ajusta este valor según sea necesario
                worksheet.Column("H").Width = worksheet.Column("E").Width; // Asumiendo que el ancho es el mismo que el de E
                worksheet.Column("I").Width = 15; // Ajusta este valor según sea necesario
                worksheet.Column("J").Width = 15; // Ajusta este valor según sea necesario
                worksheet.Column("K").Width = 10; // Ajusta este valor según sea necesario
                worksheet.Column("L").Width = 15; // Ajusta este valor según sea necesario
                worksheet.Column("M").Width = 10; // Ajusta este valor según sea necesario, o elimina si no es necesario

                // Ajustar las filas y columnas para que coincidan con la imagen proporcionada
                worksheet.Row(2).Height = 30; // Ajusta la altura de la fila para el logo si es necesario
                worksheet.Row(3).Height = 20; // Ajusta esta altura para que coincida con la imagen si es necesario
                worksheet.Row(4).Height = 20; // Ajusta esta altura para que coincida con la imagen si es necesario
                worksheet.Row(5).Height = 20; // Ajusta esta altura para que coincida con la imagen si es necesario

                // Asegurarse de que el estilo de bordes se aplique a toda la cabecera
                worksheet.Range("B2:L5").Style
                    .Border.SetTopBorder(XLBorderStyleValues.Medium)
                    .Border.SetBottomBorder(XLBorderStyleValues.Medium)
                    .Border.SetRightBorder(XLBorderStyleValues.Medium)
                    .Border.SetLeftBorder(XLBorderStyleValues.Medium);

                // Aplicar cualquier estilo adicional necesario para la cabecera
                // Por ejemplo, si hay un fondo de color, alineación de texto específica, etc.

                //var worksheet = workbook.Worksheets.Add("Boleta Diaria de Fiscalización");

                //worksheet.Cell("B2").Value = "UNNA ENERGÍA";
                //worksheet.Cell("B3").Value = "BOLETA DE FISCALIZACIÓN";
                //worksheet.Cell("B4").Value = "PETROPERU";
                //worksheet.Range("B2:B4").Style
                //    .Font.SetBold()
                //    .Font.SetFontSize(14)
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //    .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                //worksheet.Range("B2:B4").Merge();

                //worksheet.Cell("G2").Value = "Versión / Fecha:";
                //worksheet.Cell("H2").Value = "0 / 28-11-23";
                //worksheet.Range("G2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //worksheet.Cell("G3").Value = "Preparado por:";
                //worksheet.Cell("H3").Value = "JPG";
                //worksheet.Cell("G4").Value = "Aprobado por:";
                //worksheet.Cell("H4").Value = "GOG";
                //worksheet.Range("G2:H4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                //worksheet.Cell("K2").Value = "FECHA:";
                //worksheet.Cell("L2").Value = DateTime.Now.ToString("dd/MM/yyyy");
                //worksheet.Range("K2:L2").Style
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //    .Font.SetBold();

                //worksheet.Range("B2:L4").Style
                //    .Border.SetTopBorder(XLBorderStyleValues.Medium)
                //    .Border.SetBottomBorder(XLBorderStyleValues.Medium)
                //    .Border.SetRightBorder(XLBorderStyleValues.Medium)
                //    .Border.SetLeftBorder(XLBorderStyleValues.Medium);

                //worksheet.Row(2).Height = 20;
                //worksheet.Row(3).Height = 20;
                //worksheet.Row(4).Height = 20;
                //worksheet.Row(5).Height = 15; // Reduce el espacio antes del Cuadro N° 1

                //worksheet.Column("A").Width = 4.86;
                //worksheet.Column("B").Width = 20.71;
                //worksheet.Column("C").Width = 14.29;
                //worksheet.Column("D").Width = 14.29;
                //worksheet.Column("E").Width = 14.29;
                //worksheet.Column("F").Width = 14.29;
                //worksheet.Column("G").Width = 14.29;
                //worksheet.Column("H").Width = 14.29;
                //worksheet.Column("I").Width = 14.29;
                //worksheet.Column("J").Width = 14.29;
                //worksheet.Column("K").Width = 14.29;
                //worksheet.Column("L").Width = 14.29;
                //worksheet.Column("M").Width = 14.29;

                //worksheet.Cell("A6").Value = "Cuadro N° 1. Asignación de Volumen de Líquidos del Gas Natural (LGN) desglosado por lotes de PETROPERU";
                //worksheet.Range("A6:M6").Merge().Style
                //    .Alignment.SetWrapText(true)
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //    .Font.SetBold()
                //    .Font.SetFontSize(11);
                //worksheet.Range("A6:M6").Style
                //    .Border.SetBottomBorder(XLBorderStyleValues.Thin);
                //worksheet.Cell("A8").Value = "Item";
                //worksheet.Cell("B8").Value = "Suministrador";
                //worksheet.Cell("C8").Value = "Volumen de GNA (Mpcs)";
                //worksheet.Cell("D8").Value = "Riqueza (C3+) del GNA";
                //worksheet.Cell("E8").Value = "Contenido de LGN en el GNA (Gal)";
                //worksheet.Cell("F8").Value = "Factor de Asignación FdLGN (%)";
                //worksheet.Cell("G8").Value = "Asignación de LGN (Bls)";

                //var headersRange = worksheet.Range("A8:G8");
                //headersRange.Style
                //    .Font.SetBold()
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //    .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                //    .Border.SetTopBorder(XLBorderStyleValues.Thin)
                //    .Border.SetRightBorder(XLBorderStyleValues.Thin)
                //    .Border.SetLeftBorder(XLBorderStyleValues.Thin);

                //worksheet.Column("A").Width = 5;
                //worksheet.Column("B").Width = 20;
                //worksheet.Column("C").Width = 18;
                //worksheet.Column("D").Width = 25;
                //worksheet.Column("E").Width = 25;
                //worksheet.Column("F").Width = 20;
                //worksheet.Column("G").Width = 18;

                //worksheet.Cell("A9").Value = "1";
                //worksheet.Cell("B9").Value = "PETROPERU (LOTE Z69)";
                //worksheet.Cell("C9").Value = 4999.9700;
                //worksheet.Cell("D9").Value = 1.2774;
                //worksheet.Cell("E9").Value = 6386.96;
                //worksheet.Cell("F9").Value = "13.6563%";
                //worksheet.Cell("G9").Value = 136.55;

                //int totalRow = 12;
                //worksheet.Cell($"A{totalRow}").Value = "Total";
                //worksheet.Cell($"C{totalRow}").FormulaA1 = $"SUM(C9:C{totalRow - 1})";
                //worksheet.Cell($"D{totalRow}").FormulaA1 = $"SUM(D9:D{totalRow - 1})";
                //worksheet.Cell($"E{totalRow}").FormulaA1 = $"SUM(E9:E{totalRow - 1})";
                //worksheet.Cell($"G{totalRow}").FormulaA1 = $"SUM(G9:G{totalRow - 1})";

                //var totalRange = worksheet.Range($"A{totalRow}:G{totalRow}");
                //totalRange.Style
                //    .Font.SetBold()
                //    .Fill.SetBackgroundColor(XLColor.LightGray)
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //    .Border.SetTopBorder(XLBorderStyleValues.Thin)
                //    .Border.SetBottomBorder(XLBorderStyleValues.Medium)
                //    .Border.SetRightBorder(XLBorderStyleValues.Medium)
                //    .Border.SetLeftBorder(XLBorderStyleValues.Medium);
                //// Espaciado después del Cuadro N° 1 antes de los factores de conversión
                //worksheet.Row(totalRow + 1).Height = 15; // Asegúrese de que esta fila esté en blanco para el espaciado

                //// Factores de conversión después de Cuadro N° 1
                //int notesRowStart = totalRow + 2; // Dos filas después del total del Cuadro N° 1
                //worksheet.Cell($"A{notesRowStart}").Value = "Factor de conversión LOTE Z-69 (PCSD/Gal)";
                //worksheet.Cell($"B{notesRowStart}").Value = 34.96;
                //worksheet.Cell($"A{notesRowStart + 1}").Value = "Factor de conversión LOTE VI (PCSD/Gal)";
                //worksheet.Cell($"B{notesRowStart + 1}").Value = 34.62;
                //worksheet.Cell($"A{notesRowStart + 2}").Value = "Factor de conversión LOTE I (PCSD/Gal)";
                //worksheet.Cell($"B{notesRowStart + 2}").Value = 34.30;

                //// Estilos para las filas de factores de conversión
                //var notesRange = worksheet.Range($"A{notesRowStart}:B{notesRowStart + 2}");
                //notesRange.Style
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                //    .Font.SetFontSize(10);

                //// Ajustes de bordes para la tabla del Cuadro N° 1
                //var tableRange = worksheet.Range($"A8:G{totalRow}");
                //tableRange.Style
                //    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                //    .Border.SetInsideBorder(XLBorderStyleValues.Thin);

                //// Ajustes de altura de fila para el Cuadro N° 1
                //for (int i = 9; i <= totalRow; i++)
                //{
                //    worksheet.Row(i).Height = 15; // Altura estándar para las filas de datos
                //}
                //worksheet.Row(totalRow).Height = 20; // Altura mayor para la fila de totales

                //// Espaciado antes del Cuadro N° 2
                //int spaceBeforeCuadro2 = notesRowStart + 3; // Espacio después de los factores de conversión
                //worksheet.Row(spaceBeforeCuadro2).Height = 15; // Fila vacía para espacio

                //// Inicio del Cuadro N° 2
                //int cuadro2StartRow = spaceBeforeCuadro2 + 1;
                //worksheet.Cell($"A{cuadro2StartRow}").Value = "Cuadro N° 2. Volumen GNS disponible por lotes de PETROPERU";
                //worksheet.Range($"A{cuadro2StartRow}:I{cuadro2StartRow}").Merge().Style
                //    .Alignment.SetWrapText(true)
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //    .Font.SetBold()
                //    .Font.SetFontSize(11);
                //worksheet.Range($"A{cuadro2StartRow}:I{cuadro2StartRow}").Style
                //    .Border.SetBottomBorder(XLBorderStyleValues.Thin);


                //worksheet.Cell("A43").Value = "Cuadro N° 3. Volumen transferido a Refinería por lotes de PETROPERU";
                //worksheet.Range("A43:G43").Merge().Style
                //    .Alignment.SetWrapText(true)
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //    .Font.SetBold()
                //    .Font.SetFontSize(11);
                //worksheet.Range("A43:G43").Style
                //    .Border.SetBottomBorder(XLBorderStyleValues.Thin);

                //worksheet.Cell("B45").Value = "Volumen Total de GNS (M³ - S⁻²¹⁵)";
                //worksheet.Cell("E45").Value = "10,324.71";
                //worksheet.Cell("E45").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //worksheet.Range("B45:F45").Merge();

                //worksheet.Cell("A47").Value = "Item";
                //worksheet.Cell("B47").Value = "Suministrador";
                //worksheet.Cell("C47").Value = "Volumen de GNS (Mpcs)";
                //worksheet.Cell("D47").Value = "Volumen Flare (Mpcs)";
                //worksheet.Cell("E47").Value = "Volumen de GNS transferido (Mpcs)";
                //worksheet.Range("A47:E47").Style
                //    .Font.SetBold()
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //    .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                //    .Border.SetTopBorder(XLBorderStyleValues.Thin)
                //    .Border.SetRightBorder(XLBorderStyleValues.Thin)
                //    .Border.SetLeftBorder(XLBorderStyleValues.Thin);

                //worksheet.Column("A").Width = 5;
                //worksheet.Column("B").Width = 20;
                //worksheet.Column("C").Width = 18;
                //worksheet.Column("D").Width = 15;
                //worksheet.Column("E").Width = 18;

                //worksheet.Cell("A48").Value = "1";
                //worksheet.Cell("B48").Value = "PETROPERU (LOTE Z69)";
                //worksheet.Cell("C48").Value = 4799.4709;
                //worksheet.Cell("D48").Value = 0.0019;
                //worksheet.Cell("E48").Value = 4799.4690;

                //worksheet.Cell("A49").Value = "2";
                //worksheet.Cell("B49").Value = "PETROPERU (LOTE VI)";
                //worksheet.Cell("C49").Value = 3011.4782;
                //worksheet.Cell("D49").Value = 0.0013;
                //worksheet.Cell("E49").Value = 3011.4769;

                //worksheet.Cell("A50").Value = "3";
                //worksheet.Cell("B50").Value = "PETROPERU (LOTE I)";
                //worksheet.Cell("C50").Value = 2513.7652;
                //worksheet.Cell("D50").Value = 0.0011;
                //worksheet.Cell("E50").Value = 2513.7641;

                //int totalRowGNSRef = 51; // Asumiendo que 51 es la fila del total
                //worksheet.Cell($"A{totalRowGNSRef}").Value = "Total";
                //worksheet.Cell($"C{totalRowGNSRef}").FormulaA1 = $"SUM(C48:C{totalRowGNSRef - 1})";
                //worksheet.Cell($"D{totalRowGNSRef}").FormulaA1 = $"SUM(D48:D{totalRowGNSRef - 1})";
                //worksheet.Cell($"E{totalRowGNSRef}").FormulaA1 = $"SUM(E48:E{totalRowGNSRef - 1})";

                //var totalRangeGNSRef = worksheet.Range($"A{totalRowGNSRef}:E{totalRowGNSRef}");
                //totalRangeGNSRef.Style
                //    .Font.SetBold()
                //    .Fill.SetBackgroundColor(XLColor.LightGray)
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                //    .Border.SetTopBorder(XLBorderStyleValues.Thin)
                //    .Border.SetBottomBorder(XLBorderStyleValues.Medium)
                //    .Border.SetRightBorder(XLBorderStyleValues.Medium)
                //    .Border.SetLeftBorder(XLBorderStyleValues.Medium);

                //worksheet.Cell("B56").Value = "Volumen total de GNS flare (VGNSRF)";
                //worksheet.Cell("E56").Value = 0.0043;
                //worksheet.Cell("E56").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //worksheet.Range("B56:D56").Merge();

                //worksheet.Cell("A58").Value = "Representante UNNA ENERGÍA-PGT";
                //worksheet.Range("A58:E58").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //var tableRangeGNSRef = worksheet.Range($"A47:E{totalRowGNSRef}");
                //tableRangeGNSRef.Style
                //    .Border.SetOutsideBorder(XLBorderStyleValues.Medium)
                //    .Border.SetInsideBorder(XLBorderStyleValues.Thin);

                //for (int i = 48; i <= totalRowGNSRef; i++)
                //{
                //    worksheet.Row(i).Height = 15;
                //}

                //worksheet.Row(totalRowGNSRef).Height = 20;
                //// Ajustar la altura de las filas del Cuadro N° 3
                //for (int i = 47; i < totalRowGNSRef; i++)
                //{
                //    worksheet.Row(i).Height = 20; // Aumentar la altura para las filas de datos
                //}
                //worksheet.Row(totalRowGNSRef).Height = 25; // Aumentar la altura para la fila de totales

                //// Ajustar la altura de las filas para la firma y agregar un espacio después del Cuadro N° 3
                //worksheet.Row(totalRowGNSRef + 1).Height = 15; // Espacio vacío después del Cuadro N° 3
                //worksheet.Row(totalRowGNSRef + 2).Height = 15; // Espacio vacío adicional si es necesario
                //worksheet.Row(totalRowGNSRef + 3).Height = 40; // Aumentar la altura para la firma
                //worksheet.Row(totalRowGNSRef + 4).Height = 15; // Espacio vacío después de la firma

                //// Ajustar el tamaño del cuadro de firma para que se alinee con los bordes de la hoja
                //worksheet.Range($"B{totalRowGNSRef + 3}:E{totalRowGNSRef + 3}").Style
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //    .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //// Asegurar que todas las columnas tienen el mismo ancho que el Cuadro N° 1 para uniformidad
                //for (int col = 1; col <= 13; col++)
                //{
                //    worksheet.Column(col).Width = worksheet.Column("D").Width; // Tomar como referencia el ancho de la columna D
                //}

                //// Asegurar que el pie de página esté centrado respecto a la hoja entera
                //worksheet.Range($"A{totalRowGNSRef + 5}:M{totalRowGNSRef + 5}").Merge().Style
                //    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                //    .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //// Ajustar la altura de las filas al final de la hoja para agregar espacio adicional si es necesario
                //worksheet.Row(totalRowGNSRef + 6).Height = 15; // Espacio vacío al final de la hoja
                //worksheet.Row(totalRowGNSRef + 7).Height = 15; // Continuación del espacio vacío al final


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
