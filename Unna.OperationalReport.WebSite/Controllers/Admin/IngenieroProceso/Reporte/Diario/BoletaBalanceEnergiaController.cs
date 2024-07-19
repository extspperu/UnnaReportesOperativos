using ClosedXML.Report;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class BoletaBalanceEnergiaController : ControladorBaseWeb
    {
        private readonly IBoletaBalanceEnergiaServicio _boletaBalanceEnergiaServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        private readonly IImpresionServicio _impresionServicio;

        public BoletaBalanceEnergiaController(
            IBoletaBalanceEnergiaServicio boletaBalanceEnergiaServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general,
            IImpresionServicio impresionServicio
            )
        {
            _boletaBalanceEnergiaServicio = boletaBalanceEnergiaServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
            _impresionServicio = impresionServicio;
        }

        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(BoletaBalanceEnergiaDto boletaBalanceEnergia)
        {
            VerificarIfEsBuenJson(boletaBalanceEnergia);
            boletaBalanceEnergia.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaBalanceEnergiaServicio.GuardarAsync(boletaBalanceEnergia,true);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<BoletaBalanceEnergiaDto?> ObtenerAsync()
        {
            var operacion = await _boletaBalanceEnergiaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

        [HttpGet("GenerarPdf")]
        public async Task<IActionResult> GenerarExcelAsync()
        {

            var operativo = await _boletaBalanceEnergiaServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }
            var dato = operativo.Resultado;


            var comPesado = dato.LiquidosBarriles?.Where(e => e.Id == 1).FirstOrDefault();
            var produccionGlp = dato.LiquidosBarriles?.Where(e => e.Id == 2).FirstOrDefault();
            var produccionCgn = dato.LiquidosBarriles?.Where(e => e.Id == 3).FirstOrDefault();



            var gns = dato.ConsumoPropioGnsVendioEnel?.Where(e => e.Item == 1).FirstOrDefault();
            var gnsTotal = dato.ConsumoPropioGnsVendioEnel?.Where(e => e.Item == 2).FirstOrDefault();


            var gnsMalacas = dato.GnsAEnel?.Where(e => e.Item == 1).FirstOrDefault();
            var gnsRefineria = dato.GnsAEnel?.Where(e => e.Item == 2).FirstOrDefault();
            var gnsBalance = dato.GnsAEnel?.Where(e => e.Item == 3).FirstOrDefault();
            var gnsHumedad = dato.GnsAEnel?.Where(e => e.Item == 4).FirstOrDefault();
            var gnsGasFlare = dato.GnsAEnel?.Where(e => e.Item == 5).FirstOrDefault();
            var gnsGasTotal = dato.GnsAEnel?.Where(e => e.Item == 6).FirstOrDefault();

            var consumoPropioGns = dato.ConsumoPropio?.Where(e => e.Item == 1).FirstOrDefault();
            var consumoPropioTotal = dato.ConsumoPropio?.Where(e => e.Item == 2).FirstOrDefault();
            


            var complexData = new
            {
                //NS a ENEL
                gnsMalacasVolumen = gnsMalacas != null ? gnsMalacas.Volumen :0,
                gnsMalacasPc = gnsMalacas != null ? gnsMalacas.PoderCalorifico :0,
                gnsMalacasEnergia = gnsMalacas != null ? gnsMalacas.Energia :0,
                gnsRefineriaVolumen = gnsRefineria != null ? gnsRefineria.Energia :0,
                gnsRefineriaPc = gnsRefineria != null ? gnsRefineria.PoderCalorifico : 0,
                gnsRefineriaEnergia = gnsRefineria != null ? gnsRefineria.Energia :0,
                gnsBalanceVolumen = gnsBalance != null ? gnsBalance.Volumen :0,
                gnsBalancePc = gnsBalance != null ? gnsBalance.PoderCalorifico : 0,
                gnsBalanceEnergia = gnsBalance != null ? gnsBalance.Energia :0,
                gnsHumendadVolumen = gnsHumedad != null ? gnsHumedad.Volumen : 0,
                gnsHumendadPc = gnsHumedad != null ? gnsHumedad.PoderCalorifico : 0,
                gnsHumendadEnergia = gnsHumedad != null ? gnsHumedad.Energia : 0,
                gnsGasFlareVolumen = gnsGasFlare != null ? gnsGasFlare.Volumen : 0,
                gnsGasFlarePc = gnsGasFlare != null ? gnsGasFlare.PoderCalorifico : 0,
                gnsGasFlareEnergia = gnsGasFlare != null ? gnsGasFlare.Energia : 0,
                gnsGasTotalVolumen = gnsGasTotal != null ? gnsGasTotal.Volumen : 0,
                gnsGasTotalPc = gnsGasTotal != null ? gnsGasTotal.PoderCalorifico : 0,
                gnsGasTotalEnergia = gnsGasTotal != null ? gnsGasTotal.Energia : 0,

                //Consumo Propio UNNA ENERGIA
                ConsumoPropioGnsVolumen = consumoPropioGns != null ? consumoPropioGns.Volumen : 0,
                ConsumoPropioGnsPc = consumoPropioGns != null ? consumoPropioGns.PoderCalorifico : 0,
                ConsumoPropioGnsEnergia = consumoPropioGns != null ? consumoPropioGns.Energia : 0,
                ConsumoPropioTotalVolumen = consumoPropioTotal != null ? consumoPropioTotal.Volumen : 0,
                ConsumoPropioTotalPc = consumoPropioTotal != null ? consumoPropioTotal.PoderCalorifico : 0,
                ConsumoPropioTotalEnergia = consumoPropioTotal != null ? consumoPropioTotal.Energia : 0,


                NombreReporte = dato?.General?.NombreReporte,
                Compania = dato?.General?.Nombre,
                VersionFecha = $"{dato?.General?.Version} / {dato?.General?.Fecha}",
                Revisor = dato?.General?.PreparadoPor,
                AprobadoPor = dato?.General?.AprobadoPor,

                Fecha = dato?.Fecha,

                Entrega = dato?.GnaEntregaUnna?.Entrega,
                Volumen = dato?.GnaEntregaUnna?.Volumen,
                PoderCalorifico = dato?.GnaEntregaUnna?.PoderCalorifico,
                Energia = dato?.GnaEntregaUnna?.Energia,
                Riqueza = dato?.GnaEntregaUnna?.Riqueza,


                //GNS Vendido a ENEL
                GnsVolumen = gns != null ? gns.Volumen:0,
                GnsPc = gns != null ? gns.PoderCalorifico : 0,
                GnsEnergia = gns != null ? gns.Energia : 0,
                GnsVolumenTotal = gnsTotal != null ? gnsTotal.Volumen : 0,
                GnsPcTotal = gnsTotal != null ? gnsTotal.PoderCalorifico : 0,
                GnsEnergiaTotal = gnsTotal != null ? gnsTotal.Energia : 0,

                ComPesadoEnel = comPesado != null ? comPesado.Enel : 0,
                ComPesadoBlTotal = comPesado != null ? comPesado.Blsd : 0,
                ProduccionGlpEnel = produccionGlp != null ? produccionGlp.Enel : 0,
                ProduccionGlpTotal = produccionGlp != null ? produccionGlp.Blsd : 0,
                ProduccionCgnEnel = produccionCgn != null ? produccionCgn.Enel : 0,
                ProduccionCgnTotal = produccionCgn != null ? produccionCgn.Blsd : 0,

                ComPesadosGna = dato?.ComPesadosGna,
                PorcentajeEficiencia = dato?.PorcentajeEficiencia/100,

                //Contenido Calórico promedio del  LGN
                ContenidoCalorificoPromLgn = dato?.ContenidoCalorificoPromLgn,

                EntregaGna = dato?.EntregaGna?.Mpcsd,
                EntregaGnaEnergia = dato?.EntregaGna?.Energia,

                GnsRestituido = dato?.GnsRestituido?.Mpcsd,
                GnsRestituidoEnergia = dato?.GnsRestituido?.Energia,               

                DiferenciaEnergetica = dato?.DiferenciaEnergetica,
                ExesoConsumoPropio = dato?.ExesoConsumoPropio,

                GnsConsumoPropio = dato?.GnsConsumoPropio?.Mpcsd,
                GnsConsumoPropioEnergia = dato?.GnsConsumoPropio?.Energia,

                RecuperacionBarriles = dato?.Recuperacion?.Barriles,
                RecuperacionEnergia = dato?.Recuperacion?.Energia,

                Comentario = dato?.Comentario
            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaBalanceEnergia.xlsx"))
            {
                if (!string.IsNullOrWhiteSpace(dato?.General?.RutaFirma))
                {
                    using (var stream = new FileStream(dato.General.RutaFirma, FileMode.Open))
                    {
                        var worksheet = template.Workbook.Worksheets.Worksheet(1);
                        worksheet.AddPicture(stream).MoveTo(worksheet.Cell("D47")).WithSize(220, 110);
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
            //System.IO.File.Delete(tempFilePathPdf);

            await _impresionServicio.GuardarRutaArchivosAsync(new GuardarRutaArchivosDto
            {
                IdReporte = (int)TiposReportes.BoletaBalanceEnergiaDiaria,
                RutaPdf = tempFilePathPdf,
            });

            return File(bytes, "application/pdf", $"BoletaBalanceEnergia-{dato.Fecha.Replace("/", "-")}.pdf");


        }
    }
}
