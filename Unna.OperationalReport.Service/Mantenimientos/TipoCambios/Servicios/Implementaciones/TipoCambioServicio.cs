using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Auth.Entidades;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Mantenimiento.Entidades;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Enums;
using Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Dtos;
using Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.CalculoFacturaCpgnaFee50.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Servicios.Implementaciones
{
    public class TipoCambioServicio : ITipoCambioServicio
    {

        private readonly ITipoCambioRepositorio _tipoCambioRepositorio;
        private readonly GeneralDto _general;
        public TipoCambioServicio(
            ITipoCambioRepositorio tipoCambioRepositorio,
            GeneralDto general
            )
        {
            _tipoCambioRepositorio = tipoCambioRepositorio;
            _general = general;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> GuardarMasivaAsync(List<TipoCambioDto> parametros)
        {

            foreach (var item in parametros)
            {
                var existe = await _tipoCambioRepositorio.BuscarPorFechasAsync(item.Fecha, (int)TiposMonedas.Soles);
                if (existe == null)
                {

                    await _tipoCambioRepositorio.InsertarAsync(new TipoCambio
                    {
                        Fecha = item.Fecha,
                        Cambio = item.Cambio,
                        IdTipoMoneda = (int)TiposMonedas.Soles,
                        EstaBorrado = false
                    });
                }
                else
                {
                    existe.Cambio = item.Cambio;
                    existe.EstaBorrado = false;
                    await _tipoCambioRepositorio.InsertarAsync(existe);
                }
            }

            return new OperacionDto<RespuestaSimpleDto<bool>>(new RespuestaSimpleDto<bool>() { Id = true, Mensaje = "Correcto" });
        }


        public async Task<OperacionDto<List<TipoCambioDto>>> ListarPorFechasAsync(DateTime desde, DateTime hasta, int idTipoMoneda)
        {
            var tipoCambios = await _tipoCambioRepositorio.ListarPorFechasAsync(desde, hasta, idTipoMoneda);
            var dto = tipoCambios.Select(e => new TipoCambioDto
            {
                Cambio = e.Cambio,
                Fecha = e.Fecha,
                IdTipoMoneda = e.IdTipoMoneda
            }).ToList();
            return new OperacionDto<List<TipoCambioDto>>(dto);
        }

        public async Task<OperacionDto<List<TipoCambioDto>>> ListarDelMesAync(DateTime fecha, int idTipoMoneda)
        {
            DateTime desde = new DateTime(fecha.Year, fecha.Month, 1);
            DateTime hasta = new DateTime(fecha.Year, fecha.Month, 1).AddMonths(1).AddDays(-1);
            return await ListarPorFechasAsync(desde, hasta, idTipoMoneda);
        }



        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> ProcesarArchivoAsync(IFormFile file, int idTipoMoneda)
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


            for (int i = 1; i <= hoja.LastRowNum; i++)
            {
                IRow fila = hoja.GetRow(i);
                if (fila != null)
                {
                    string fechaCadena = fila.GetCell(0, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila.GetCell(0, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString() : "";
                    var numString = fila.GetCell(3, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila.GetCell(3, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";
                    double valor = 0;
                    bool canConvert = double.TryParse(numString, out valor);
                    if (!canConvert) continue;

                    if (string.IsNullOrWhiteSpace(fechaCadena))
                    {
                        continue;
                    }
                    //DateTime date = DateTime.ParseExact(fechaCadena, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var cultureInfo = new CultureInfo("de-DE");
                    
                    var date = DateTime.Parse(fechaCadena, cultureInfo,DateTimeStyles.NoCurrentDateDefault);
                    var tipoMoneda = await _tipoCambioRepositorio.BuscarPorFechasAsync(date, idTipoMoneda);
                    if (tipoMoneda == null)
                    {
                        tipoMoneda = new TipoCambio()
                        {
                            IdTipoMoneda = idTipoMoneda,
                            Fecha = date,
                            Cambio = valor,
                        };
                        await _tipoCambioRepositorio.InsertarAsync(tipoMoneda);
                        continue;
                    }
                    tipoMoneda.Fecha = date;
                    tipoMoneda.Cambio = valor;
                    await _tipoCambioRepositorio.EditarAsync(tipoMoneda);
                    
                }
            }

            return new OperacionDto<RespuestaSimpleDto<bool>>(
               new RespuestaSimpleDto<bool>()
               {
                   Id = true,
                   Mensaje = "Se guardo correctamente"
               }
               );
        }


    }
}
