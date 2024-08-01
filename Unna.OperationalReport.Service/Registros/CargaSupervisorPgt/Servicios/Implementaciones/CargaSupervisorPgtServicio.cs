using Azure;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Data.Reporte.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using RegexMatch = System.Text.RegularExpressions.Match;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Servicios.Implementaciones
{
    public class CargaSupervisorPgtServicio : ICargaSupervisorPgtServicio
    {
        private readonly IArchivoServicio _archivoServicio;
        private readonly IDatoDeltaVRepositorio _datoDeltaVRepositorio;
        private readonly IRegistroSupervisorRepositorio _registroSupervisorRepositorio;
        public CargaSupervisorPgtServicio(
            IArchivoServicio archivoServicio,
            IDatoDeltaVRepositorio datoDeltaVRepositorio,
            IRegistroSupervisorRepositorio registroSupervisorRepositorio
            )
        {
            _archivoServicio = archivoServicio;
            _datoDeltaVRepositorio = datoDeltaVRepositorio;
            _registroSupervisorRepositorio = registroSupervisorRepositorio;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> ProcesarDocuemtoAsync(long idArchivo, long idRegistroSupervisor)
        {
            var operacion = await _archivoServicio.ObtenerAsync(idArchivo);
            if (!operacion.Completado || operacion.Resultado == null || string.IsNullOrWhiteSpace(operacion.Resultado.Ruta))
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, operacion.Mensajes);
            }

            string rutaArchivo = operacion.Resultado.Ruta;

            return await ProcesarArchivoAsync(rutaArchivo, idRegistroSupervisor);

        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> ProcesarDocumentoTxtAsync(long idDiaOperativoArchivo, long IdDiaOperativo)
        {
            var operacion = await _archivoServicio.ObtenerAsync(idDiaOperativoArchivo);
            if (!operacion.Completado || operacion.Resultado == null || string.IsNullOrWhiteSpace(operacion.Resultado.Ruta))
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, operacion.Mensajes);
            }

            string rutaArchivo = operacion.Resultado.Ruta;

            return await ProcesarArchivoTxtAsync(rutaArchivo, IdDiaOperativo);

        }

        private async Task<OperacionDto<RespuestaSimpleDto<bool>>> ProcesarArchivoAsync(string path, long idRegistroSupervisor)
        {

            IWorkbook MiExcel = null;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            MiExcel = new XSSFWorkbook(fs);

            ISheet hoja = MiExcel.GetSheetAt(0);
            if (hoja == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.NoExiste, "Documento de excel no es válido");
            }

            int cantidadfilas = hoja.LastRowNum;

            //VOLUME_DELTA_V
            await GuardarVolumenDeltaVAsync(hoja, idRegistroSupervisor);

            XLWorkbook archivoExcel = new XLWorkbook(path);
            //PRODUCCION_DIARIA_MS
            await GuardarProduccionDiariaMsAsync(archivoExcel, idRegistroSupervisor);

            //GNS
            await GuardarVolumenMsGnsEgpsaAsync(hoja, idRegistroSupervisor);

            //VOL
            await GuardarVolumenMsVolLimaGasAsync(hoja, idRegistroSupervisor);
            
            //Almacenamiento de Lima Gas
            await GuardarAlmacenamientoLimaGasAsync(archivoExcel, idRegistroSupervisor);


            //DATOS DELTA V
            await GuardarDatosDeltaVAsync(hoja, idRegistroSupervisor);


            //DATOS CGN
            await GuardarDatosCgnAsync(hoja, idRegistroSupervisor);

            //VOLUMEN DE DESPACHO DE GLP
            int numColumnaGlp = 34;

            await GuardarVolumenDespachoGlpAsync(archivoExcel, idRegistroSupervisor, TiposTablasSupervisorPgt.DespachoGlp, numColumnaGlp);


            //VOLUMEN DE DESPACHO DE CGN
            int numColumnaCgn = 47;
            await GuardarVolumenDespachoGlpAsync(archivoExcel, idRegistroSupervisor, TiposTablasSupervisorPgt.DespachoCgn, numColumnaCgn);


            //DESPACHO GLP ENVASADO
            await GuardarDespachoGlpEnvasadoAsync(hoja, idRegistroSupervisor);



            return new OperacionDto<RespuestaSimpleDto<bool>>(
               new RespuestaSimpleDto<bool>()
               {
                   Id = true,
                   Mensaje = "Se guardo correctamente"
               }
               );
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> ProcesarArchivoTxtAsync(string path, long IdDiaOperativo)
        {
            List<GasDataDto> extractedData = ExtractData(path);

            foreach (var data in extractedData)
            {
                var volumenPromedio = new DatoComposicionUnnaEnergiaPromedio()
                {
                    idDiaOperativo = IdDiaOperativo,
                    componente = data.Name,
                    promedioComponente = int.TryParse(data.LastAverage, out int promedio) ? promedio : 0
                };

                await _datoDeltaVRepositorio.GuardarVolumenTxtAsync(volumenPromedio);
            }

            return new OperacionDto<RespuestaSimpleDto<bool>>(
                new RespuestaSimpleDto<bool>
                {
                    Id = true,
                    Mensaje = "Se guardó correctamente"
                }
            );
        }

        private List<GasDataDto> ExtractData(string filePath)
        {
            List<GasDataDto> dataList = new List<GasDataDto>();

            string content = File.ReadAllText(filePath);
            // Regex to find the sections
            Regex sectionRegex = new Regex(@"(\d+)\s+-\s+(.*?)\n\s+Average\s+Minimum\s+Maximum\s+Samples\s*\n(.*?)(?=\d+\s+-|$)", RegexOptions.Singleline);
            MatchCollection matches = sectionRegex.Matches(content);

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                string itemName = match.Groups[2].Value.Trim();
                string valuesBlock = match.Groups[3].Value.Trim();

                string[] lines = valuesBlock.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string lastAverage = null;

                foreach (string line in lines)
                {
                    string[] parts = Regex.Split(line.Trim(), @"\s+");
                    if (parts.Length >= 4)  
                    {
                        lastAverage = parts[3]; 
                    }
                }

                GasDataDto gasData = new GasDataDto
                {
                    Name = itemName,
                    LastAverage = lastAverage
                };

                dataList.Add(gasData);
            }

            return dataList;
        }

        private async Task GuardarVolumenDeltaVAsync(ISheet hoja, long idRegistroSupervisor)
        {
            await _datoDeltaVRepositorio.EliminarVolumenDeltaVAsync(idRegistroSupervisor);

            for (int i = 13; i <= 17; i++)
            {
                IRow fila = hoja.GetRow(i);
                if (fila != null)
                {
                    var volumenDeltaV = new VolumenDeltaV()
                    {
                        IdRegistroSupervisor = idRegistroSupervisor,
                        NombreLote = fila.GetCell(1, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila.GetCell(1, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString() : "",
                        Actualizado = DateTime.UtcNow
                    };
                    double valor = 0;
                    var numString = fila.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";
                    bool canConvert = double.TryParse(numString, out valor);
                    if (canConvert)
                    {
                        volumenDeltaV.Volumen = valor;
                    }
                    if (!string.IsNullOrWhiteSpace(volumenDeltaV.NombreLote))
                    {
                        await _datoDeltaVRepositorio.GuardarVolumenDeltaVAsync(volumenDeltaV);
                    }
                }
            }



        }


        private async Task GuardarProduccionDiariaMsAsync(XLWorkbook archivoExcel, long idRegistroSupervisor)
        {
            await _datoDeltaVRepositorio.EliminarProduccionDiariaMsAsync(idRegistroSupervisor);

            var HojaExcel = archivoExcel.Worksheet(1);

            for (int i = 25; i <= 26; i++)
            {
                IXLRow fila = HojaExcel.Row(i);

                string? fil1 = fila.Cell(2) != null ? fila.Cell(2).GetValue<string>() : null;
                string? fil2 = fila.Cell(3) != null ? fila.Cell(3).GetValue<string>() : null;
                if (fil1 == null)
                {
                    continue;
                }
                var produccionDiariaMs = new ProduccionDiariaMs()
                {
                    Producto = fil1,
                    IdRegistroSupervisor = idRegistroSupervisor,
                    Actualizado = DateTime.UtcNow
                };
                double valor = 0;
                if (double.TryParse(fil2, out valor)) produccionDiariaMs.MedidoresMasicos = valor;

                if (!string.IsNullOrWhiteSpace(produccionDiariaMs.Producto))
                {
                    await _datoDeltaVRepositorio.GuardarProduccionDiariaMsAsync(produccionDiariaMs);
                }
            }
        }



        //VOLUMEN MS - GNS A EGPSA
        private async Task GuardarVolumenMsGnsEgpsaAsync(ISheet hoja, long idRegistroSupervisor)
        {

            await _datoDeltaVRepositorio.EliminarGnsVolumeMsYPcBrutoAsync(idRegistroSupervisor, TiposTablasSupervisorPgt.VolumenMsGnsAgpsa);

            for (int i = 14; i <= 16; i++)
            {
                IRow fila3 = hoja.GetRow(i);

                if (fila3 == null)
                {
                    continue;
                }

                var entidad = new GnsVolumeMsYPcBruto();
                entidad.Tipo = TiposTablasSupervisorPgt.VolumenMsGnsAgpsa;
                entidad.IdRegistroSupervisor = idRegistroSupervisor;
                entidad.Nombre = fila3.GetCell(4, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(4, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString() : "";
                var valor1 = fila3.GetCell(6, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(6, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";
                var valor2 = fila3.GetCell(7, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(7, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";

                double valor = 0;
                bool canConvert = double.TryParse(valor1, out valor);
                if (canConvert)
                {
                    entidad.VolumeMs = valor;
                }

                double value2 = 0;
                bool canConvert2 = double.TryParse(valor2, out value2);
                if (canConvert2)
                {
                    entidad.PcBrutoRepCroma = value2;
                }

                await _datoDeltaVRepositorio.GuardarGnsVolumeMsYPcBrutoAsync(entidad);
            }
        }



        //VOLUMEN MS - VOL LIMAGAS
        private async Task GuardarVolumenMsVolLimaGasAsync(ISheet hoja, long idRegistroSupervisor)
        {
            await _datoDeltaVRepositorio.EliminarGnsVolumeMsYPcBrutoAsync(idRegistroSupervisor, TiposTablasSupervisorPgt.VolumenVolLimaGas);

            for (int i = 24; i <= 25; i++)
            {
                IRow fila3 = hoja.GetRow(i);

                if (fila3 == null)
                {
                    continue;
                }

                var entidad = new GnsVolumeMsYPcBruto();
                entidad.Tipo = TiposTablasSupervisorPgt.VolumenVolLimaGas;
                entidad.IdRegistroSupervisor = idRegistroSupervisor;
                entidad.Nombre = fila3.GetCell(4, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(4, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString() : "";
                var valor1 = fila3.GetCell(6, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(6, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";
                var valor2 = fila3.GetCell(7, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(7, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";

                double valor = 0;
                bool canConvert = double.TryParse(valor1, out valor);
                if (canConvert)
                {
                    entidad.VolumeMs = valor;
                }

                double value2 = 0;
                bool canConvert2 = double.TryParse(valor2, out value2);
                if (canConvert2)
                {
                    entidad.PcBrutoRepCroma = value2;
                }

                await _datoDeltaVRepositorio.GuardarGnsVolumeMsYPcBrutoAsync(entidad);
            }
        }



        //ALMACENAMIENTO DE LIMAGAS
        private async Task GuardarAlmacenamientoLimaGasAsync(XLWorkbook archivoExcel, long idRegistroSupervisor)
        {
            await _datoDeltaVRepositorio.EliminarGnsVolumeMsYPcBrutoAsync(idRegistroSupervisor, TiposTablasSupervisorPgt.AlmacenamientoLimaGas);

            var HojaExcel = archivoExcel.Worksheet(1);

            IXLRow fila = HojaExcel.Row(40);
            IXLRow fila1 = HojaExcel.Row(40);

            if (fila == null)
            {
                return;
            }

            var entidad = new GnsVolumeMsYPcBruto();
            entidad.Tipo = TiposTablasSupervisorPgt.AlmacenamientoLimaGas;
            entidad.IdRegistroSupervisor = idRegistroSupervisor;
            entidad.Nombre = "Almacenamiento LIMAGAS (BBL) TK - 4610";
            var valor = fila.Cell(9) != null ? fila.Cell(9).GetValue<string>() : null;
            double valorFinal = 0;
            bool canConvert = double.TryParse(valor, out valorFinal);
            if (canConvert)
            {
                entidad.VolumeMs = valorFinal;
            }

            await _datoDeltaVRepositorio.GuardarGnsVolumeMsYPcBrutoAsync(entidad);


        }



        //DATOS DELTA V
        private async Task GuardarDatosDeltaVAsync(ISheet hoja, long idRegistroSupervisor)
        {

            await _datoDeltaVRepositorio.EliminarDatosDeltaVAsync(idRegistroSupervisor);

            for (int i = 14; i <= 23; i++)
            {
                IRow fila3 = hoja.GetRow(i);

                if (fila3 == null)
                {
                    continue;
                }

                var entidad = new DatoDeltaV();
                entidad.IdRegistroSupervisor = idRegistroSupervisor;
                entidad.Tanque = fila3.GetCell(9, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(9, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString() : "";

                var nivel = fila3.GetCell(10, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(10, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";
                var pres = fila3.GetCell(11, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(11, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";
                var temp = fila3.GetCell(12, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(12, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";
                var api = fila3.GetCell(13, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(13, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";

                double valorNivel = 0;
                if (double.TryParse(nivel, out valorNivel)) entidad.Nivel = valorNivel;

                double valorPres = 0;
                if (double.TryParse(pres, out valorPres)) entidad.Pres = valorPres;

                double valorTemp = 0;
                if (double.TryParse(temp, out valorTemp)) entidad.Temp = valorTemp;

                double valorApi = 0;
                if (double.TryParse(api, out valorApi)) entidad.Api = valorApi;

                await _datoDeltaVRepositorio.GuardarDatosDeltaVAsync(entidad);
            }
        }

        //DATOS CGN
        private async Task GuardarDatosCgnAsync(ISheet hoja, long idRegistroSupervisor)
        {

            await _datoDeltaVRepositorio.EliminarDatosCgnAsync(idRegistroSupervisor);

            for (int i = 27; i <= 28; i++)
            {
                IRow fila3 = hoja.GetRow(i);

                if (fila3 == null)
                {
                    continue;
                }

                var entidad = new DatoCgn();
                entidad.IdRegistroSupervisor = idRegistroSupervisor;
                entidad.Tanque = fila3.GetCell(9, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(9, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString() : "";

                var centaje = fila3.GetCell(10, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(10, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";
                var volumen = fila3.GetCell(11, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(11, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";

                double valorCentaje = 0;
                if (double.TryParse(centaje, out valorCentaje)) entidad.Centaje = valorCentaje;

                double valorVolumen = 0;
                if (double.TryParse(volumen, out valorVolumen)) entidad.Volumen = valorVolumen;

                await _datoDeltaVRepositorio.GuardarDatosCgnAsync(entidad);
            }
        }



        //VOLUMEN DE DESPACHO
        private async Task GuardarVolumenDespachoGlpAsync(XLWorkbook archivoExcel, long idRegistroSupervisor, string? tipo, int columnaTanque)
        {

            await _datoDeltaVRepositorio.EliminarVolumenDeDespachoAsync(idRegistroSupervisor, tipo);


            List<string> Tanques = new List<string>();
            List<string> Clientes = new List<string>();
            List<string?> Placas = new List<string?>();
            List<double?> Volumenes = new List<double?>();


            var HojaExcel = archivoExcel.Worksheet(1);

            IXLRow fila = HojaExcel.Row(columnaTanque);
            if (fila != null)
            {
                for (int j = 3; j <= 12; j++)
                {
                    string? valor = fila.Cell(j) != null ? fila.Cell(j).GetValue<string>() : null;
                    if (!string.IsNullOrEmpty(valor)) Tanques.Add(valor.Replace("TK-", ""));
                }
            }

            IXLRow fila2 = HojaExcel.Row((columnaTanque + 1));
            if (fila2 != null)
            {
                for (int j = 3; j <= 12; j++)
                {
                    string? valor = fila2.Cell(j) != null ? fila2.Cell(j).GetValue<string>() : null;
                    if (!string.IsNullOrEmpty(valor)) Clientes.Add(valor);

                }
            }

            IXLRow fila3 = HojaExcel.Row((columnaTanque + 2));
            if (fila3 != null)
            {
                for (int j = 3; j <= 12; j++)
                {
                    string? valor = fila3.Cell(j) != null ? fila3.Cell(j).GetValue<string>() : null;
                    if (!string.IsNullOrEmpty(valor)) Placas.Add(valor);

                }
            }

            IXLRow fila4 = HojaExcel.Row((columnaTanque + 3));
            if (fila4 != null)
            {
                for (int j = 3; j <= 12; j++)
                {
                    string? valor = fila4.Cell(j) != null ? fila4.Cell(j).GetValue<string>() : null;
                    double valorVolumen = 0;
                    if (double.TryParse(valor, out valorVolumen)) Volumenes.Add(valorVolumen);

                }
            }

            for (var i = 0; i < Tanques.Count; i++)
            {
                if (Tanques[i].Equals("0"))
                {
                    continue;
                }
                var entidad = new VolumenDespacho()
                {
                    Tanque = Tanques[i],
                    Cliente = Clientes.Count > i ? Clientes[i] : null,
                    Placa = Placas.Count > i ? Placas[i] : null,
                    Volumen = Volumenes.Count > i ? Volumenes[i] : new double?(),
                    IdRegistroSupervisor = idRegistroSupervisor,
                    Tipo = tipo,
                    Actualizado = DateTime.UtcNow
                };
                await _datoDeltaVRepositorio.GuardarVolumenDeDespachoAsync(entidad);

            }




        }



        //DESPACHO GLP ENVASADO
        private async Task GuardarDespachoGlpEnvasadoAsync(ISheet hoja, long idRegistroSupervisor)
        {

            await _datoDeltaVRepositorio.EliminarDespachoGlpEnvasadoAsync(idRegistroSupervisor);

            for (int i = 40; i <= 41; i++)
            {
                IRow fila3 = hoja.GetRow(i);

                if (fila3 == null)
                {
                    continue;
                }

                var entidad = new DespachoGlpEnvasado();
                entidad.IdRegistroSupervisor = idRegistroSupervisor;
                entidad.Nombre = fila3.GetCell(1, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(1, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString() : "";

                var envasado = fila3.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";
                var granel = fila3.GetCell(3, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila3.GetCell(3, MissingCellPolicy.RETURN_NULL_AND_BLANK).NumericCellValue.ToString() : "";

                double valorEnvasado = 0;
                if (double.TryParse(envasado, out valorEnvasado)) entidad.Envasado = valorEnvasado;

                double valorGranel = 0;
                if (double.TryParse(granel, out valorGranel)) entidad.Granel = valorGranel;

                await _datoDeltaVRepositorio.GuardarDespachoGlpEnvasadoAsync(entidad);
            }
        }



        public async Task<OperacionDto<CargaSupervisorPgtDto>> ObtenerAsync()
        {

            var entidad = await _registroSupervisorRepositorio.BuscarPorFechaAsync(FechasUtilitario.ObtenerDiaOperativo());
            if (entidad == null)
            {
                return new OperacionDto<CargaSupervisorPgtDto>(CodigosOperacionDto.NoExiste, "No existe registro para el día seleccionado");
            }

            var dto = new CargaSupervisorPgtDto();

            var datoDeltaV = await _datoDeltaVRepositorio.BuscarDatosDeltaVAsync(entidad.IdRegistroSupervisor);
            dto.DatoDeltaV = datoDeltaV.Select(e => new DatoDeltaV()
            {
                Id = e.Id,
                Tanque = e.Tanque,
                Nivel = e.Nivel,
                Pres = e.Pres,
                Temp = e.Temp,
                Api = e.Api
            }).ToList();

            var volumenDeltaV = await _datoDeltaVRepositorio.BuscarVolumenDeltaVAsync(entidad.IdRegistroSupervisor);
            dto.VolumenDeltaV = volumenDeltaV.Select(e => new VolumenDeltaVDto()
            {
                Id = e.Id,
                NombreLote = e.NombreLote,
                Volumen = e.Volumen,
            }).ToList();


            var volumenMsPcBrutoGns = await _datoDeltaVRepositorio.BuscarGnsVolumeMsYPcBrutoAsync(entidad.IdRegistroSupervisor, TiposTablasSupervisorPgt.VolumenMsGnsAgpsa);
            dto.VolumenMsPcBrutoGns = volumenMsPcBrutoGns.Select(e => new GnsVolumeMsYPcBrutoDto()
            {
                Id = e.Id,
                Nombre = e.Nombre,
                VolumeMs = e.VolumeMs,
                PcBrutoRepCroma = e.PcBrutoRepCroma
            }).ToList();

            var volumenMsPcBrutoVol = await _datoDeltaVRepositorio.BuscarGnsVolumeMsYPcBrutoAsync(entidad.IdRegistroSupervisor, TiposTablasSupervisorPgt.VolumenVolLimaGas);
            dto.VolumenMsPcBrutoVol = volumenMsPcBrutoVol.Select(e => new GnsVolumeMsYPcBrutoDto()
            {
                Id = e.Id,
                Nombre = e.Nombre,
                VolumeMs = e.VolumeMs,
                PcBrutoRepCroma = e.PcBrutoRepCroma
            }).ToList();

            var almacenamientoLimaGas = await _datoDeltaVRepositorio.BuscarGnsVolumeMsYPcBrutoAsync(entidad.IdRegistroSupervisor, TiposTablasSupervisorPgt.AlmacenamientoLimaGas);
            if(almacenamientoLimaGas != null && almacenamientoLimaGas.Count > 0)
            {
                dto.AlmacenamientoLimaGasBbl = almacenamientoLimaGas.First().VolumeMs;
            }

            var produccionDiariaMs = await _datoDeltaVRepositorio.BuscarProduccionDiariaMsAsync(entidad.IdRegistroSupervisor);
            dto.ProduccionDiariaMs = produccionDiariaMs.Select(e => new ProduccionDiariaMsDto()
            {
                Id = e.Id,
                Producto = e.Producto,
                MedidoresMasicos = e.MedidoresMasicos
            }).ToList();

            var datoCgn = await _datoDeltaVRepositorio.BuscarDatosCgnAsync(entidad.IdRegistroSupervisor);
            dto.DatoCgn = datoCgn.Select(e => new DatoCgnDto()
            {
                Id = e.Id,
                Volumen = e.Volumen,
                Centaje = e.Centaje,
                Tanque = e.Tanque
            }).ToList();


            var despachoGlpEnvasado = await _datoDeltaVRepositorio.BuscarDespachoGlpEnvasadoAsync(entidad.IdRegistroSupervisor);
            dto.DespachoGlpEnvasado = despachoGlpEnvasado.Select(e => new DespachoGlpEnvasadoDto()
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Envasado = e.Envasado,
                Granel = e.Granel
            }).ToList();

            var volumenDespachoGlp = await _datoDeltaVRepositorio.BuscarVolumenDeDespachoAsync(entidad.IdRegistroSupervisor, TiposTablasSupervisorPgt.DespachoGlp);
            dto.VolumenDespachoGlp = volumenDespachoGlp.Select(e => new VolumenDespachoDto()
            {
                Id = e.Id,
                Tanque = e.Tanque,
                Cliente = e.Cliente,
                Placa = e.Placa,
                Volumen = e.Volumen,
            }).ToList();

            var volumenDespachoCgn = await _datoDeltaVRepositorio.BuscarVolumenDeDespachoAsync(entidad.IdRegistroSupervisor, TiposTablasSupervisorPgt.DespachoCgn);
            dto.VolumenDespachoCgn = volumenDespachoCgn.Select(e => new VolumenDespachoDto()
            {
                Id = e.Id,
                Tanque = e.Tanque,
                Cliente = e.Cliente,
                Placa = e.Placa,
                Volumen = e.Volumen,
            }).ToList();


            return new OperacionDto<CargaSupervisorPgtDto>(dto);
        }

    }
}
