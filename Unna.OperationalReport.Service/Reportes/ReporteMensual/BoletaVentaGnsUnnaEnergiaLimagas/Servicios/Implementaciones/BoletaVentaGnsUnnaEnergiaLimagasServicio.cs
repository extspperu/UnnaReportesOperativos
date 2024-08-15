using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using Unna.OperationalReport.Data.Mantenimiento.Enums;
using Unna.OperationalReport.Data.Mensual.Entidades;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Generales.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.Impresiones.Dtos;
using Unna.OperationalReport.Service.Reportes.Impresiones.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVolumenesUNNAEnergiaCNPC.Dtos;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaVentaGnsUnnaEnergiaLimagas.Servicios.Implementaciones
{
    public class BoletaVentaGnsUnnaEnergiaLimagasServicio : IBoletaVentaGnsUnnaEnergiaLimagasServicio
    {

        private readonly IServicioCompresionGnaLimaGasRepositorio _servicioCompresionGnaLimaGasRepositorio;
        private readonly IImpresionServicio _impresionServicio;
        private readonly IReporteServicio _reporteServicio;
        private readonly IValoresDefectoReporteServicio _valoresDefectoReporteServicio;
        private readonly UrlConfiguracionDto _urlConfiguracion;
        private readonly GeneralDto _general;
        private readonly ISeguimientoBalanceDiarioServicio _seguimientoBalanceDiarioServicio;


        public BoletaVentaGnsUnnaEnergiaLimagasServicio(
            IServicioCompresionGnaLimaGasRepositorio servicioCompresionGnaLimaGasRepositorio,
            IImpresionServicio impresionServicio,
            IReporteServicio reporteServicio,
            IValoresDefectoReporteServicio valoresDefectoReporteServicio,
            UrlConfiguracionDto urlConfiguracion,
            GeneralDto general,
            ISeguimientoBalanceDiarioServicio seguimientoBalanceDiarioServicio
            )
        {
            _servicioCompresionGnaLimaGasRepositorio = servicioCompresionGnaLimaGasRepositorio;
            _impresionServicio = impresionServicio;
            _reporteServicio = reporteServicio;
            _valoresDefectoReporteServicio = valoresDefectoReporteServicio;
            _urlConfiguracion = urlConfiguracion;
            _general = general;
            _seguimientoBalanceDiarioServicio = seguimientoBalanceDiarioServicio;
        }

        public async Task<OperacionDto<BoletaVentaGnsUnnaEnergiaLimagasDto>> ObtenerAsync(long idUsuario)
        {

            //DateTime fecha = FechasUtilitario.ObtenerDiaOperativo();

            System.DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddMonths(-1);
            System.DateTime fecha = new System.DateTime(diaOperativo.Year, diaOperativo.Month, 1);
            var operacionGeneral = await _reporteServicio.ObtenerAsync((int)TiposReportes.BoletaMensualVentaGnsUnnaEnergiaLimagas, idUsuario);
            if (!operacionGeneral.Completado)
            {
                return new OperacionDto<BoletaVentaGnsUnnaEnergiaLimagasDto>(CodigosOperacionDto.NoExiste, operacionGeneral.Mensajes);
            }

            var operacionImpresion = await _impresionServicio.ObtenerAsync((int)TiposReportes.BoletaMensualVentaGnsUnnaEnergiaLimagas, fecha);
            if (operacionImpresion != null && operacionImpresion.Completado && operacionImpresion.Resultado != null && !string.IsNullOrWhiteSpace(operacionImpresion.Resultado.Datos) && operacionImpresion.Resultado.EsEditado)
            {
                var rpta = JsonConvert.DeserializeObject<BoletaVentaGnsUnnaEnergiaLimagasDto>(operacionImpresion.Resultado.Datos);
                return new OperacionDto<BoletaVentaGnsUnnaEnergiaLimagasDto>(rpta);

            }


            var entidad = await _servicioCompresionGnaLimaGasRepositorio.BuscarPorFechaAsync(fecha);
            if (entidad == null)
            {
                return new OperacionDto<BoletaVentaGnsUnnaEnergiaLimagasDto>(CodigosOperacionDto.NoExiste, "No existe dato cargado para el periodo");
            }

            List<BoletaVentaMensualDto> lista = new List<BoletaVentaMensualDto>();
            if (entidad.ServicioCompresionGnaLimaGasVentas != null)
            {
                lista = entidad.ServicioCompresionGnaLimaGasVentas.Select(e => new BoletaVentaMensualDto
                {
                    Id = (int)e.Id,
                    Fecha = e.FechaDespacho,
                    Placa = e.Placa,
                    //FechaInicioCarga = e.FechaInicioCarga.HasValue ? e.FechaInicioCarga.Value.ToString("yyyy-MM-dd") : null,
                    //FechaFinCarga = e.FechaFinCarga.HasValue ? e.FechaFinCarga.Value.ToString("yyyy-MM-dd") : null,
                    FechaInicioCarga = e.InicioCarga,
                    FechaFinCarga = e.FinCarga,
                    NroConstanciaDespacho = e.NroConstanciaDespacho?.Replace("?", ""),
                    Volumen = e.VolumenSm3,
                    PoderCalorifico = e.PoderCalorifico,
                    Energia = e.Energia.HasValue ? Math.Round(e.Energia.Value, 2) : e.Energia
                }).ToList();
                for (var i = 0; i < lista.Count; i++)
                {
                    lista[i].Id = i;
                }
            }

            double? igvCentaje = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.Igv);
            double? precioUsdMmbtu = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.PrecioUsdMmbtu);
            double? cpio = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.CPiOIndicePrecioConsumidorEeUu);
            double? cpii = await _valoresDefectoReporteServicio.ObtenerValorAsync(LlaveValoresDefecto.CPIiFactorAjuste);

            var dto = new BoletaVentaGnsUnnaEnergiaLimagasDto
            {
                NombreReporte = operacionGeneral.Resultado?.NombreReporte,
                UrlFirma = operacionGeneral.Resultado?.UrlFirma,
                RutaFirma = operacionGeneral.Resultado?.RutaFirma,
                Periodo = entidad.Periodo,
                TotalVolumen = Math.Round(lista.Sum(e => e.Volumen ?? 0), 2),
                TotalEnergia = Math.Round(lista.Sum(e => e.Energia ?? 0), 2),
                TotalPoderCalorifico = lista.Count > 0 ? Math.Round(lista.Sum(e => e.PoderCalorifico ?? 0) / lista.Count, 2) : 0,
                EnergiaVolumenSuministrado = Math.Round(lista.Sum(e => e.Energia ?? 0), 2),
                IgvCentaje = igvCentaje ?? 0,
                Comentario = entidad.Comentario,
                BoletaVentaMenensual = lista,
                PrecioBase = precioUsdMmbtu ?? 0,
                CPIi = Math.Round(cpii ?? 0, 2),
                CPIo = Math.Round(cpio ?? 0, 2),

            };
            dto.Fac = Math.Round(dto.CPIi / dto.CPIo, 2);
            dto.SubTotal = Math.Round(dto.EnergiaVolumenSuministrado * Math.Round(dto.PrecioBase * dto.Fac, 2), 2);
            dto.Igv = Math.Round(dto.SubTotal * dto.IgvCentaje / 100, 2);
            dto.Total = dto.SubTotal + dto.Igv;

            return new OperacionDto<BoletaVentaGnsUnnaEnergiaLimagasDto>(dto);
        }


        public async Task<OperacionDto<RespuestaSimpleDto<string>>> GuardarAsync(BoletaVentaGnsUnnaEnergiaLimagasDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<string>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }
            System.DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddMonths(-1);
            System.DateTime fecha = new System.DateTime(diaOperativo.Year, diaOperativo.Month, 1);
            var dto = new ImpresionDto()
            {
                IdConfiguracion = RijndaelUtilitario.EncryptRijndaelToUrl((int)TiposReportes.BoletaMensualVentaGnsUnnaEnergiaLimagas),
                Fecha = fecha,
                IdUsuario = peticion.IdUsuario,
                Datos = JsonConvert.SerializeObject(peticion),
                Comentario = peticion.Comentario,
                EsEditado = true
            };

            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(39,3);
            await _seguimientoBalanceDiarioServicio.ActualizarEstadoSeguimientoDiarioAsync(49,3);
            return await _impresionServicio.GuardarAsync(dto);

        }


        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> ProcesarArchivoAsync(IFormFile file, long idUsuario)
        {
            if (file == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.Invalido, "Seleccione un archivo para subir");
            }

            var extension = Path.GetExtension(file.FileName);

            var nameArchivo = $"{Guid.NewGuid()}{extension}";

            var rutaArchivo = $"{_general.RutaArchivos}{nameArchivo}";

            using (FileStream filestream = System.IO.File.Create($"{rutaArchivo}"))
            {
                await file.CopyToAsync(filestream);
                filestream.Flush();
            }

            IWorkbook MiExcel = null;
            FileStream fs = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);

            MiExcel = new XSSFWorkbook(fs);

            ISheet hoja = MiExcel.GetSheetAt(0);
            if (hoja == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Documento de excel no es válido");
            }

            System.DateTime diaOperativo = FechasUtilitario.ObtenerDiaOperativo().AddMonths(-1);
            System.DateTime fecha = new System.DateTime(diaOperativo.Year, diaOperativo.Month, 1);

            var entidad = await _servicioCompresionGnaLimaGasRepositorio.BuscarPorFechaAsync(fecha);
            if (entidad == null)
            {
                entidad = new ServicioCompresionGnaLimaGas();
            }
            IRow periodo = hoja.GetRow(5);
            entidad.Periodo = periodo.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? periodo.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString() : "";
            entidad.IdUsuario = idUsuario;
            entidad.Fecha = fecha;
            entidad.Creado = System.DateTime.UtcNow;
            entidad.Actualizado = System.DateTime.UtcNow;
            if (entidad.Id > 0)
            {
                _servicioCompresionGnaLimaGasRepositorio.Editar(entidad);
            }
            else
            {
                _servicioCompresionGnaLimaGasRepositorio.Insertar(entidad);
            }

            await _servicioCompresionGnaLimaGasRepositorio.UnidadDeTrabajo.GuardarCambiosAsync();


            XLWorkbook archivoExcel = new XLWorkbook(rutaArchivo);
            
            
            await InsertarVentasDetalleAsync(archivoExcel, entidad.Id ?? 0, idUsuario);

          


            return new OperacionDto<RespuestaSimpleDto<bool>>(
               new RespuestaSimpleDto<bool>()
               {
                   Id = true,
                   Mensaje = "Se guardo correctamente"
               }
               );
        }




        private async Task<bool> InsertarVentasDetalleAsync(XLWorkbook archivoExcel, long id, long idUsuario)
        {
            await _servicioCompresionGnaLimaGasRepositorio.EliminarPorIdVentasAsync(id);

            var HojaExcel = archivoExcel.Worksheet(1);

            for (int i = 10; i <= 150; i++)
            {
                try
                {
                    IXLRow fila = HojaExcel.Row(i);
                    var venta = new ServicioCompresionGnaLimaGasVentas();

                    string? fechaDespacho = fila.Cell(2) != null ? fila.Cell(2).GetValue<string>() : null;
                    if (string.IsNullOrWhiteSpace(fechaDespacho))
                    {
                         return false;
                    }
                    try
                    {
                        venta.FechaDespacho = System.DateTime.FromOADate(int.Parse(fechaDespacho)).ToString("dd/MM/yyyy");
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                    

                    venta.Placa = fila.Cell(3) != null ? fila.Cell(3).GetValue<string>() : null;
                    var fechaInicioCarga = fila.Cell(4) != null ? fila.Cell(4).GetValue<string>() : null;
                    var fechaFinCarga = fila.Cell(5) != null ? fila.Cell(5).GetValue<string>() : null;

                    venta.InicioCarga = fechaInicioCarga;
                    venta.FinCarga = fechaFinCarga;
                    //try
                    //{
                    //    venta.FechaInicioCarga = !string.IsNullOrEmpty(fechaInicioCarga) ? System.DateTime.ParseExact(fechaInicioCarga, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) : null;
                    //    venta.FechaFinCarga = !string.IsNullOrEmpty(fechaFinCarga) ? System.DateTime.ParseExact(fechaFinCarga, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) : null;                        
                    //}
                    //catch (Exception ex)
                    //{

                    //}

                    venta.NroConstanciaDespacho = fila.Cell(6) != null ? fila.Cell(6).GetValue<string>().Replace("?", "") : null;
                    var volumen = fila.Cell(7) != null ? fila.Cell(7).GetValue<string>() : null;
                    double volumenValor = 0;
                    bool canValumen = double.TryParse(volumen, out volumenValor);
                    if (canValumen) venta.VolumenSm3 = volumenValor;

                    var volumenMp = fila.Cell(8) != null ? fila.Cell(8).GetValue<string>() : null;
                    double pvolumenMpValor = 0;
                    bool canVolumenMp = double.TryParse(volumenMp, out pvolumenMpValor);
                    if (canVolumenMp) venta.VolumenMmpcs = pvolumenMpValor;

                    var poderCalorifico = fila.Cell(9) != null ? fila.Cell(9).GetValue<string>() : null;
                    double poderCalorificoValor = 0;
                    bool canpoderCalorifico = double.TryParse(poderCalorifico, out poderCalorificoValor);
                    if (canpoderCalorifico) venta.PoderCalorifico = poderCalorificoValor;

                    var energia = fila.Cell(10) != null ? fila.Cell(10).GetValue<string>() : null;
                    double energiaValor = 0;
                    bool canEnergia = double.TryParse(energia, out energiaValor);
                    if (canEnergia) venta.Energia = energiaValor;

                    var precio = fila.Cell(11) != null ? fila.Cell(11).GetValue<string>() : null;
                    double precioValor = 0;
                    bool canPrecio = double.TryParse(precio, out precioValor);
                    if (canPrecio) venta.Precio = precioValor;

                    var subTotal = fila.Cell(12) != null ? fila.Cell(12).GetValue<string>() : null;
                    double subTotalValor = 0;
                    bool canSubTotal = double.TryParse(subTotal, out subTotalValor);
                    if (canSubTotal) venta.SubTotal = subTotalValor;
                    venta.IdUsuario = idUsuario;
                    venta.Creado = System.DateTime.UtcNow;
                    venta.IdServicioCompresionGnaLimaGas = id;
                    await _servicioCompresionGnaLimaGasRepositorio.InsertarVentasAsync(venta);

                }
                catch
                {

                }



            }

            return true;

        }



    }




}
