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
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Registro;
using Unna.OperationalReport.Data.Registro.Entidades;
using Unna.OperationalReport.Data.Registro.Enums;
using Unna.OperationalReport.Data.Registro.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Reporte.Entidades;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Dtos;
using Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Registros.CargaSupervisorPgt.Servicios.Implementaciones
{
    public class CargaSupervisorPgtServicio : ICargaSupervisorPgtServicio
    {
        private readonly IArchivoServicio _archivoServicio;
        private readonly IDatoDeltaVRepositorio _datoDeltaVRepositorio;
        public CargaSupervisorPgtServicio(
            IArchivoServicio archivoServicio,
            IDatoDeltaVRepositorio datoDeltaVRepositorio
            )
        {
            _archivoServicio = archivoServicio;
            _datoDeltaVRepositorio = datoDeltaVRepositorio;
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

                double valorNivel = 0;
                if (double.TryParse(nivel, out valorNivel)) entidad.Nivel = valorNivel;

                double valorPres = 0;
                if (double.TryParse(pres, out valorPres)) entidad.Pres = valorPres;

                double valorTemp = 0;
                if (double.TryParse(temp, out valorTemp)) entidad.Temp = valorTemp;

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
                    if (!string.IsNullOrEmpty(valor)) Tanques.Add(valor);
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
            long IdRegistroSupervisor = 0;

            var dto = new CargaSupervisorPgtDto();

            var entidadDatoDeltaV = await _datoDeltaVRepositorio.BuscarDatosDeltaVAsync(IdRegistroSupervisor);
            var datoDeltaV = entidadDatoDeltaV.Select(e => new DatoDeltaV()
            {
                Id = e.Id,
                Tanque = e.Tanque,
                Nivel = e.Nivel,
                Pres = e.Pres,
                Temp = e.Temp
            }).ToList();
            dto.DatoDeltaV = datoDeltaV;

            var entidadVolumenDeltaV = await _datoDeltaVRepositorio.BuscarVolumenDeltaVAsync(IdRegistroSupervisor);
            dto.VolumenDeltaV = entidadVolumenDeltaV.Select(e => new VolumenDeltaVDto()
            {
                Id = e.Id,
                NombreLote = e.NombreLote,
                Volumen = e.Volumen,
            }).ToList();


            return new OperacionDto<CargaSupervisorPgtDto>(dto);
        }

    }
}
