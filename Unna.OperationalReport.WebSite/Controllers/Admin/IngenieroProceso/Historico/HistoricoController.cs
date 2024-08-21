using ClosedXML.Excel;
using ClosedXML.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Data.Reporte.Procedimientos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Historico
{
    [Route("api/admin/ingenieroProceso/[controller]")]
    [ApiController]
    public class HistoricoController : ControladorBaseWeb
    {
        private readonly GeneralDto _general;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HistoricoController(GeneralDto general, IWebHostEnvironment hostingEnvironment)
        {
            _general = general;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("GenerarExcel_3_GNA")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            string? url = await Excel_3_GNA();
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{nombreArchivo}.xlsx");
        }

        [HttpGet("GenerarExcel_1b_Tks")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarExcel_1b_TksAsync()
        {
            string? url = await Excel_1b_Tks();
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{nombreArchivo}.xlsx");
        }

        [HttpGet("GenerarExcel_2_Liquidos")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarExcel_2_LiquidosAsync()
        {
            string? url = await Excel_2_Liquidos();
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{nombreArchivo}.xlsx");
        }

        [HttpGet("GenerarExcel_3_Gas")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarExcel_3_GasAsync()
        {
            string? url = await Excel_3_Gas();
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{nombreArchivo}.xlsx");
        }

        [HttpGet("GenerarExcel_Existencias")]
        [RequiereAcceso()]
        public async Task<IActionResult> GenerarExcel_ExistenciasAsync()
        {
            string? url = await Excel_Existencias();
            if (string.IsNullOrWhiteSpace(url))
            {
                return File(new byte[0], "application/octet-stream");
            }
            var bytes = System.IO.File.ReadAllBytes(url);

            string nombreArchivo = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow).ToString("dd-MM-yyyy");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{nombreArchivo}.xlsx");
        }

        private async Task<string?> Excel_3_GNA()
        {
            
            var complexData = new
            {
                dataResult = 0,

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\Consultas\\3_GNA.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }
        private async Task<string?> Excel_1b_Tks()
        {

            var complexData = new
            {
                dataResult = 0,

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\Consultas\\1b_Tks.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }
        private async Task<string?> Excel_2_Liquidos()
        {

            var complexData = new
            {
                dataResult = 0,

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\Consultas\\2_Liquidos.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }
        private async Task<string?> Excel_3_Gas()
        {

            var complexData = new
            {
                dataResult = 0,

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\Consultas\\3_Gas.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }
        private async Task<string?> Excel_Existencias()
        {

            var complexData = new
            {
                dataResult = 0,

            };
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\Consultas\\Existencias.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            return tempFilePath;
        }
        private async Task<string?> GenerarAsync(string tipoReporte)
        {
            var complexData = new
            {

                dataResult = 0,

            };
            
            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";
            string plantillaExcel = tipoReporte switch
            {
                "1b_Tks" => $"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\Consultas\\1b_Tks.xlsx",
                "2_Líquidos" => $"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\Consultas\\2_Líquidos.xlsx",
                "3_Gas" => $"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\Consultas\\3_Gas.xlsx",
                "3_GNA" => $"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\Consultas\\3_GNA.xlsx",
                "Existencias" => $"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\Consultas\\Existencias.xlsx",
            };
            try
            {
                using (var template = new XLTemplate(plantillaExcel))
                {
                    template.AddVariable(complexData);
                    template.Generate();
                    template.SaveAs(tempFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return tempFilePath;
        }
    }
}
