using ClosedXML.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Servicios.Implementaciones;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using Unna.OperationalReport.Tools.WebComunes.ApiWeb.Auth.Atributos;
using Unna.OperationalReport.Tools.WebComunes.WebSite.Base;

namespace Unna.OperationalReport.WebSite.Controllers.Admin.IngenieroProceso.Reporte.Diario
{
    [Route("api/admin/ingenieroProceso/reporte/diario/[controller]")]
    [ApiController]
    public class ReporteDiarioPgtController : ControladorBaseWeb
    {
        private readonly IReporteDiarioServicio _reporteDiarioServicio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly GeneralDto _general;

        public ReporteDiarioPgtController(
            IReporteDiarioServicio reporteDiarioServicio,
            IWebHostEnvironment hostingEnvironment,
            GeneralDto general
            )
        {
            _reporteDiarioServicio = reporteDiarioServicio;
            _hostingEnvironment = hostingEnvironment;
            _general = general;
        }

        [HttpGet("GenerarExcel")]
        public async Task<IActionResult> GenerarExcelAsync()
        {
            var operativo = await _reporteDiarioServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            if (!operativo.Completado || operativo.Resultado == null)
            {
                return File(new byte[0], "application/octet-stream");
            }

            var dato = operativo.Resultado;

            var gasNaturalAsociado = new
            {
                Items = dato.GasNaturalAsociado
            };

            var gasNaturalSeco = new
            {
                Items = dato.GasNaturalSeco
            };

            var liquidosGasNaturalProduccionVentas = new
            {
                Items = dato.LiquidosGasNaturalProduccionVentas
            };

            var volumenDespachoGlp = new
            {
                Items = dato.VolumenDespachoGlp
            };

            var volumenDespachoCgn = new
            {
                Items = dato.VolumenDespachoCgn
            };

            var volumenProduccionLoteXGnaTotalCnpc = new
            {
                Items = dato.VolumenProduccionLoteXGnaTotalCnpc
            };

            var volumenProduccionLoteXLiquidoGasNatural = new
            {
                Items = dato.VolumenProduccionLoteXLiquidoGasNatural
            };

            //var volumenProduccionEnel = new
            //{
            //    Items = dato.VolumenProduccionEnel
            //};

            var volumenProduccionPetroperu = new
            {
                Items = dato.VolumenProduccionPetroperu
            };

            var volumenProduccionLiquidoGasNatural = new
            {
                Items = dato.VolumenProduccionLiquidoGasNatural
            };
                
            var tanqueDespachoGalGlp = new
            {
                Items = TanqueDespachoGal(dato?.VolumenDespachoGlp)
            };
            var tanqueDespachoGalCgn = new
            {
                Items = TanqueDespachoGal(dato?.VolumenDespachoCgn)
            };

            var volumenNominado4 = dato.VolumenProduccionLoteXGnaTotalCnpc?.Where(e => e.Item == 1).FirstOrDefault();
            var volumenNominadoAdicional4 = dato.VolumenProduccionLoteXGnaTotalCnpc?.Where(e => e.Item == 2).FirstOrDefault();

            //6.VOLUMEN DE GAS Y PRODUCCIÓN DE PETROPERU(LOTE I, VI y Z - 69):
            var filaLoteZ69 = dato.VolumenProduccionPetroperu?.Where(e => e.Item == 1).FirstOrDefault();
            var filaLoteVi = dato.VolumenProduccionPetroperu?.Where(e => e.Item == 2).FirstOrDefault();
            var filaLoteI = dato.VolumenProduccionPetroperu?.Where(e => e.Item == 3).FirstOrDefault();
            var filaLotesTotal = dato.VolumenProduccionPetroperu?.Where(e => e.Item == 4).FirstOrDefault();
            
            
            var produccionGlp6 = dato.VolumenProduccionLiquidoGasNatural?.Where(e => e.Item == 1).FirstOrDefault();
            var produccionCgn6 = dato.VolumenProduccionLiquidoGasNatural?.Where(e => e.Item == 2).FirstOrDefault();
            var produccionTotal6 = dato.VolumenProduccionLiquidoGasNatural?.Where(e => e.Item == 3).FirstOrDefault();

            var complexData = new
            {
                NomCompania = dato.General.Nombre,
                Revisado = dato.General.PreparadoPör,
                Aprobado = dato.General.AprobadoPor,
                VersionFecha = $"{dato.General.Version} / {dato.General.Fecha}",
                NombreReporte = dato.General.NombreReporte,

                Fecha = dato?.Fecha,
                GasProcesado = dato?.GasProcesado,
                GasNoProcesado = dato?.GasNoProcesado,
                UtilizacionPlantaParinias = dato?.UtilizacionPlantaParinias/100,
                HoraPlantaFs = dato?.HoraPlantaFs,
                EficienciaRecuperacionLgn = dato?.EficienciaRecuperacionLgn/100,
                
                GasNaturalAsociado = gasNaturalAsociado,
                //GasNaturalAsociado2 = gasNaturalAsociado,
                GasNaturalSeco = gasNaturalSeco,
                LiquidosGasNaturalProduccionVentas = liquidosGasNaturalProduccionVentas,

                VolumenNominadoEgpsa4 = volumenNominado4 != null ? volumenNominado4.Volumen : 0,
                VolumenNominadoAdicional4 = volumenNominadoAdicional4 != null ? volumenNominadoAdicional4.Volumen : 0,
                LiquidoGlp4 = dato?.VolumenProduccionLoteXLiquidoGasNatural?.Where(e => e.Item == 1).FirstOrDefault() != null ? dato?.VolumenProduccionLoteXLiquidoGasNatural?.Where(e => e.Item == 1)?.FirstOrDefault()?.Volumen : 0,
                LiquidoCgn4 = dato?.VolumenProduccionLoteXLiquidoGasNatural?.Where(e => e.Item == 2).FirstOrDefault() != null ? dato?.VolumenProduccionLoteXLiquidoGasNatural?.Where(e => e.Item == 2)?.FirstOrDefault()?.Volumen : 0,
                LiquidoTotal4 = dato?.VolumenProduccionLoteXLiquidoGasNatural?.Where(e => e.Item == 3).FirstOrDefault() != null ? dato?.VolumenProduccionLoteXLiquidoGasNatural?.Where(e => e.Item == 3)?.FirstOrDefault()?.Volumen : 0,
                //VolumenProduccionLoteXGnaTotalCnpc = volumenProduccionLoteXGnaTotalCnpc,


                //VolumenProduccionEnel = volumenProduccionEnel,
                //VolumenProduccionGasNaturalEnel = volumenProduccionGasNaturalEnel,
                LiquidoGlp5 = dato?.VolumenProduccionGasNaturalEnel?.Where(e => e.Item == 1).FirstOrDefault() != null ? dato?.VolumenProduccionGasNaturalEnel?.Where(e => e.Item == 1)?.FirstOrDefault()?.Volumen : 0,
                LiquidoCgn5 = dato?.VolumenProduccionGasNaturalEnel?.Where(e => e.Item == 2).FirstOrDefault() != null ? dato?.VolumenProduccionGasNaturalEnel?.Where(e => e.Item == 2)?.FirstOrDefault()?.Volumen : 0,
                LiquidoTotal5 = dato?.VolumenProduccionGasNaturalEnel?.Where(e => e.Item == 3).FirstOrDefault() != null ? dato?.VolumenProduccionGasNaturalEnel?.Where(e => e.Item == 3)?.FirstOrDefault()?.Volumen : 0,

                RecepcionDeGna5 = dato?.VolumenProduccionEnel?.Where(e => e.Item == 1).FirstOrDefault() != null ? dato?.VolumenProduccionEnel?.Where(e => e.Item == 1)?.FirstOrDefault()?.Volumen : 0,
                GnsAEnel5 = dato?.VolumenProduccionEnel?.Where(e => e.Item == 2).FirstOrDefault() != null ? dato?.VolumenProduccionEnel?.Where(e => e.Item == 2)?.FirstOrDefault()?.Volumen : 0,
                HumedadAgua5 = dato?.VolumenProduccionEnel?.Where(e => e.Item == 3).FirstOrDefault() != null ? dato?.VolumenProduccionEnel?.Where(e => e.Item == 3)?.FirstOrDefault()?.Volumen : 0,
                GasFlare5 = dato?.VolumenProduccionEnel?.Where(e => e.Item == 4).FirstOrDefault() != null ? dato?.VolumenProduccionEnel?.Where(e => e.Item == 4)?.FirstOrDefault()?.Volumen : 0,
                GasCombustible5 = dato?.VolumenProduccionEnel?.Where(e => e.Item == 5).FirstOrDefault() != null ? dato?.VolumenProduccionEnel?.Where(e => e.Item == 5)?.FirstOrDefault()?.Volumen : 0,
                TotalDistribucion5 = dato?.VolumenProduccionEnel?.Where(e => e.Item == 6).FirstOrDefault() != null ? dato?.VolumenProduccionEnel?.Where(e => e.Item == 6)?.FirstOrDefault()?.Volumen : 0,

                //6.VOLUMEN DE GAS Y PRODUCCIÓN DE PETROPERU(LOTE I, VI y Z - 69):
                LoteZ69Gna = filaLoteZ69 != null ? filaLoteZ69.GnaRecibido:0,
                LoteZ69Gns = filaLoteZ69 != null ? filaLoteZ69.GnsTrasferido : 0,
                LoteViGna = filaLoteVi != null ? filaLoteVi.GnaRecibido : 0,
                LoteViGns = filaLoteVi != null ? filaLoteVi.GnsTrasferido : 0,
                LoteIGna = filaLoteI != null ? filaLoteI.GnaRecibido : 0,
                LoteIGns = filaLoteI != null ? filaLoteI.GnsTrasferido : 0,
                TotalGnaLotes = filaLotesTotal != null ? filaLotesTotal.GnaRecibido : 0,
                TotalGnsLotes = filaLotesTotal != null ? filaLotesTotal.GnsTrasferido : 0,

                LoteZ69Glp = produccionGlp6 != null ? produccionGlp6.LoteZ69 : 0,
                LoteZ69Cgn = produccionCgn6 != null ? produccionCgn6.LoteZ69 : 0,
                LoteZ69Total = produccionTotal6 != null ? produccionTotal6.LoteZ69 : 0,

                LoteViGlp = produccionGlp6 != null ? produccionGlp6.LoteVi : 0,
                LoteViCgn = produccionCgn6 != null ? produccionCgn6.LoteVi : 0,
                LoteViTotal = produccionTotal6 != null ? produccionTotal6.LoteVi : 0,

                LoteIGlp = produccionGlp6 != null ? produccionGlp6.LoteI : 0,
                LoteICgn = produccionCgn6 != null ? produccionCgn6.LoteI : 0,
                LoteITotal = produccionTotal6 != null ? produccionTotal6.LoteI : 0,

                GasAlfare = dato?.GasAlfare,



                VolumenVentaGna7 = dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e=>e.Item == 1).FirstOrDefault() != null ? dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e => e.Item == 1).FirstOrDefault()?.Volumen:0,
                VentaGnsLimaGas7 = dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e=>e.Item == 2).FirstOrDefault() != null ? dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e => e.Item == 2).FirstOrDefault()?.Volumen:0,
                VentaGnsGasnorp7 = dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e=>e.Item == 3).FirstOrDefault() != null ? dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e => e.Item == 3).FirstOrDefault()?.Volumen:0,
                VentaAEnel7 = dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e=>e.Item == 4).FirstOrDefault() != null ? dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e => e.Item == 4).FirstOrDefault()?.Volumen:0,
                GasCombustible7 = dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e=>e.Item == 5).FirstOrDefault() != null ? dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e => e.Item == 5).FirstOrDefault()?.Volumen:0,
                VolumenEquivalente7 = dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e=>e.Item == 6).FirstOrDefault() != null ? dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e => e.Item == 6).FirstOrDefault()?.Volumen:0,
                Flare7 = dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e=>e.Item == 7).FirstOrDefault() != null ? dato?.VolumenProduccionLoteIvUnnaEnegia?.Where(e => e.Item == 7).FirstOrDefault()?.Volumen:0,


                VolumenGlp7 = dato?.VolumenProduccionLoteIvLiquidoGasNatural?.Where(e=>e.Item == 1).FirstOrDefault() != null ? dato?.VolumenProduccionLoteIvLiquidoGasNatural?.Where(e => e.Item == 1).FirstOrDefault()?.Volumen:0,
                VolumenCgn7 = dato?.VolumenProduccionLoteIvLiquidoGasNatural?.Where(e=>e.Item == 2).FirstOrDefault() != null ? dato?.VolumenProduccionLoteIvLiquidoGasNatural?.Where(e => e.Item == 2).FirstOrDefault()?.Volumen:0,
                VolumenTotal7 = dato?.VolumenProduccionLoteIvLiquidoGasNatural?.Where(e=>e.Item == 3).FirstOrDefault() != null ? dato?.VolumenProduccionLoteIvLiquidoGasNatural?.Where(e => e.Item == 3).FirstOrDefault()?.Volumen:0,



                VolumenProduccionPetroperu = volumenProduccionPetroperu,
                VolumenProduccionLiquidoGasNatural = volumenProduccionLiquidoGasNatural,


                TanqueDespachoGalGlp = tanqueDespachoGalGlp,
                TanqueDespachoGalCgn = tanqueDespachoGalCgn,
                Comentario = dato?.Comentario
            };

            var tempFilePath = $"{_general.RutaArchivos}{Guid.NewGuid()}.xlsx";

            using (var template = new XLTemplate($"{_hostingEnvironment.WebRootPath}\\plantillas\\reporte\\diario\\BoletaReporteDiario.xlsx"))
            {
                template.AddVariable(complexData);
                template.Generate();
                template.SaveAs(tempFilePath);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BoletaReporteDiario-{dato?.Fecha?.Replace("/", "-")}.xlsx");
        }

        private List<TanqueDespachoGalDto> TanqueDespachoGal(List<VolumenDespachoDto> lista)
        {
            var listaGlp = new List<TanqueDespachoGalDto>();

            listaGlp.Add(new TanqueDespachoGalDto
            {
                Datos = "Tanque N°",
                Despacho1 = lista.Count > 0 ? lista[0].Tanque : null,
                Despacho2 = lista.Count > 1 ? lista[1].Tanque : null,
                Despacho3 = lista.Count > 2 ? lista[2].Tanque : null,
                Despacho4 = lista.Count > 3 ? lista[3].Tanque : null,
                Despacho5 = lista.Count > 4 ? lista[4].Tanque : null,
                Despacho6 = lista.Count > 5 ? lista[5].Tanque : null,
                Despacho7 = lista.Count > 6 ? lista[6].Tanque : null,
                Despacho8 = lista.Count > 7 ? lista[7].Tanque : null,
                Total = lista?.Count
            });
            listaGlp.Add(new TanqueDespachoGalDto
            {
                Datos = "Cliente",
                Despacho1 = lista.Count > 0 ? lista[0].Cliente : null,
                Despacho2 = lista.Count > 1 ? lista[1].Cliente : null,
                Despacho3 = lista.Count > 2 ? lista[2].Cliente : null,
                Despacho4 = lista.Count > 3 ? lista[3].Cliente : null,
                Despacho5 = lista.Count > 4 ? lista[4].Cliente : null,
                Despacho6 = lista.Count > 5 ? lista[5].Cliente : null,
                Despacho7 = lista.Count > 6 ? lista[6].Cliente : null,
                Despacho8 = lista.Count > 7 ? lista[7].Cliente : null,
                
            });
            listaGlp.Add(new TanqueDespachoGalDto
            {
                Datos = "Placa de Cisterna",
                Despacho1 = lista.Count > 0 ? lista[0].Placa : "",
                Despacho2 = lista.Count > 1 ? lista[1].Placa : "",
                Despacho3 = lista.Count > 2 ? lista[2].Placa : "",
                Despacho4 = lista.Count > 3 ? lista[3].Placa : "",
                Despacho5 = lista.Count > 4 ? lista[4].Placa : "",
                Despacho6 = lista.Count > 5 ? lista[5].Placa : "",
                Despacho7 = lista.Count > 6 ? lista[6].Placa : "",
                Despacho8 = lista.Count > 7 ? lista[7].Placa : "",
                
            });
            listaGlp.Add(new TanqueDespachoGalDto
            {
                Datos = "Volumen",
                Despacho1 = lista.Count > 0 ? lista[0].Volumen.ToString() : "",
                Despacho2 = lista.Count > 1 ? lista[1].Volumen.ToString() : "",
                Despacho3 = lista.Count > 2 ? lista[2].Volumen.ToString() : "",
                Despacho4 = lista.Count > 3 ? lista[3].Volumen.ToString() : "",
                Despacho5 = lista.Count > 4 ? lista[4].Volumen.ToString() : "",
                Despacho6 = lista.Count > 5 ? lista[5].Volumen.ToString() : "",
                Despacho7 = lista.Count > 6 ? lista[6].Volumen.ToString() : "",
                Despacho8 = lista.Count > 7 ? lista[7].Volumen.ToString() : "",
                Total = lista?.Sum(e => e.Volumen)
            });
            return listaGlp;
        }

        [HttpGet("Obtener")]
        [RequiereAcceso()]
        public async Task<ReporteDiarioDto?> ObtenerAsync()
        {
            var operacion = await _reporteDiarioServicio.ObtenerAsync(ObtenerIdUsuarioActual() ?? 0);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }


        [HttpPost("Guardar")]
        [RequiereAcceso()]
        public async Task<RespuestaSimpleDto<string>?> GuardarAsync(ReporteDiarioDto peticion)
        {
            VerificarIfEsBuenJson(peticion);
            peticion.IdUsuario = ObtenerIdUsuarioActual() ?? 0;
            var operacion = await _reporteDiarioServicio.GuardarAsync(peticion,true);
            return ObtenerResultadoOGenerarErrorDeOperacion(operacion);
        }

    }
}
