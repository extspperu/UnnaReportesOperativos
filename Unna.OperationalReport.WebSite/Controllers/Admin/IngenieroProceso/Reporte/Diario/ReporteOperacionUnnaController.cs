using ClosedXML.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Servicios.Implementaciones;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class ReporteOperacionUnnaController : ControladorBaseWeb
    {

        private readonly IReporteOperacionUnna _reporteOperacionUnna;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public ReporteOperacionUnnaController(
            IReporteOperacionUnna reporteOperacionUnna,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _reporteOperacionUnna = reporteOperacionUnna;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }


        [HttpGet("GenerarExcel")]
        public async Task<IActionResult> GenerarExcelAsync()
        {

            var operativo = await _reporteOperacionUnna.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }

            var dato = operativo.Resultado;


            var complexData = new
            {
                ReporteNro = dato.ReporteNro,
                EmpresaNombre = dato.Empresa,
                FechaEmision = dato.FechaEmision,
                DiaOperativo = dato.DiaOperativo,
                CapacidadDisPlanta = dato.PlantaSepGasNat.CapacidadDisPlanta,
                VolumenGasNatHumedo = dato.PlantaSepGasNat.VolumenGasNatHumedo,
                VolumenGasNatSecoReinyFlare = dato.PlantaSepGasNat.VolumenGasNatSecoReinyFlare,
                VolumenGasNatSecoVentas = dato.PlantaSepGasNat.VolumenGasNatSecoVentas,
                ProcGasNatSecoTotal = dato.PlantaSepGasNat.ProcGasNatSecoTotal,
                CapacidadDisPlVolumenLgnProducidoPlantaanta = dato.PlantaSepGasNat.VolumenLgnProducidoPlanta,
                VolumenLgnProcesado = dato.PlantaFracLiqGasNat.VolumenLgnProcesado,
                VolumenLgnProducidoCgn = dato.PlantaFracLiqGasNat.VolumenLgnProducidoCgn,
                VolumenLgnProducidoGlp = dato.PlantaFracLiqGasNat.VolumenLgnProducidoGlp,
                VolumenLgnProducidoTotal = dato.PlantaFracLiqGasNat.VolumenLgnProducidoTotal,
                VolumenProductosCondensadosLgn = dato.PlantaFracLiqGasNat.VolumenProductosCondensadosLgn,
                VolumenProductosGlp = dato.PlantaFracLiqGasNat.VolumenProductosGlp,
                VolumenProductosTotal = dato.PlantaFracLiqGasNat.VolumenProductosTotal,
                EventosOperativos = dato.PlantaFracLiqGasNat.EventosOperativos
            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\ReporteOSINERGMIN.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
                
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ReporteOSINERGMIN.xlsx");
        }

    }
}
