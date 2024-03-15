using ClosedXML.Report;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class BoletaCnpcController : ControladorBaseWeb
    {

        private readonly IBoletaCnpcServicio _boletaCnpcServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;
        public BoletaCnpcController(
            IBoletaCnpcServicio boletaCnpcServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _boletaCnpcServicio = boletaCnpcServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }

        [HttpGet("GenerarExcel")]
        public async Task<IActionResult> GenerarExcelAsync()
        {

            var operativo = await _boletaCnpcServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
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

            var factoresDistribucionGasNaturalSeco = new
            {
                Items = dato.FactoresDistribucionGasNaturalSeco
            };
            
            var factoresDistribucionGasDeCombustible = new
            {
                Items = dato.FactoresDistribucionGasDeCombustible
            }; 

            var factoresDistribucionLiquidoGasNatural = new
            {
                Items = dato.FactoresDistribucionLiquidoGasNatural
            };

            var complexData = new
            {
                Compania = dato?.General?.Nombre,
                PreparadoPör = $"Preparado por: {dato?.General?.PreparadoPör}",
                AprobadoPor = $"Aprobado por: {dato?.General?.AprobadoPor}",

                DiaOperativo = dato?.Fecha,
                GasMpcd = dato.Tabla1?.GasMpcd,
                GlpBls = dato.Tabla1?.GlpBls,
                CgnBls = dato.Tabla1?.CgnBls,
                CnsMpc = dato.Tabla1?.CnsMpc,
                CgMpc = dato.Tabla1?.CgMpc,

                VolumenTotalGnsEnMs = dato?.VolumenTotalGnsEnMs,
                VolumenTotalGns = dato?.VolumenTotalGns,
                FlareGna = dato?.FlareGna,

                FactoresDistribucionGasNaturalSeco = factoresDistribucionGasNaturalSeco,
                VolumenTotalGasCombustible=dato?.VolumenTotalGasCombustible,
                FactoresDistribucionGasDeCombustible = factoresDistribucionGasDeCombustible,
                VolumenProduccionTotalGlp = dato?.VolumenProduccionTotalGlp,
                VolumenProduccionTotalCgn = dato?.VolumenProduccionTotalCgn,
                VolumenProduccionTotalLgn=dato?.VolumenProduccionTotalLgn,
                FactoresDistribucionLiquidoGasNatural = factoresDistribucionLiquidoGasNatural,
                GravedadEspecifica = dato?.GravedadEspecifica,
                VolumenProduccionTotalGlpCnpc = dato?.VolumenProduccionTotalGlpCnpc,
                VolumenProduccionTotalCgnCnpc = dato?.VolumenProduccionTotalCgnCnpc,
            };


            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaCnpc.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);

            //var workbook = new Workbook(tempFilePath);
            //string tempFilePathPdf = $"{_general.RutaArchivos}{Guid.NewGuid()}.pdf";
            //workbook.Save(tempFilePathPdf);

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BoletaCnpc-{dato.Fecha.Replace("/", "-")}.xlsx");
        }



        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<BoletaCnpcDto?> ObtenerAsync()
        {
            var operacion = await _boletaCnpcServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<bool>?> GuardarAsync(BoletaCnpcDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _boletaCnpcServicio.GuardarAsync(peticion);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
