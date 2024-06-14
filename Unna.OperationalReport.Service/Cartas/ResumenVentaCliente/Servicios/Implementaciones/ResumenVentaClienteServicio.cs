using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Entidades;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Mensual.Entidades;
using Unna.OperationalReport.Service.Cartas.ResumenVentaCliente.Dtos;
using Unna.OperationalReport.Service.Cartas.ResumenVentaCliente.Servicios.Abstracciones;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;
using Unna.OperationalReport.Tools.Seguridad.Servicios.General.Dtos;

namespace Unna.OperationalReport.Service.Cartas.ResumenVentaCliente.Servicios.Implementaciones
{
    public class ResumenVentaClienteServicio : IResumenVentaClienteServicio
    {
        private readonly IVentaPorClienteDetalleRepositorio _ventaPorClienteDetalleRepositorio;
        private readonly ICargaInventarioRepositorio _cargaInventarioRepositorio;
        private readonly IVentaPorClienteRepositorio _ventaPorClienteRepositorio;
        private readonly GeneralDto _general;
        public ResumenVentaClienteServicio(
            IVentaPorClienteDetalleRepositorio ventaPorClienteDetalleRepositorio,
            ICargaInventarioRepositorio cargaInventarioRepositorio,
            IVentaPorClienteRepositorio ventaPorClienteRepositorio,
            GeneralDto general
            )
        {
            _ventaPorClienteDetalleRepositorio = ventaPorClienteDetalleRepositorio;
            _cargaInventarioRepositorio = cargaInventarioRepositorio;
            _ventaPorClienteRepositorio = ventaPorClienteRepositorio;
            _general = general;
        }

        public List<string> ProductosAsync()
        {
            List<string> productos = new List<string>
            {
                "INVENTARIO",
                "RESUMEN"
            };
            return productos;
        }

        public List<string> TiposAsync()
        {
            List<string> tipos = new List<string>
            {
                "LOTE IV",
                "PGT"
            };
            return tipos;
        }

        public async Task<OperacionDto<RespuestaSimpleDto<bool>>> ProcesarArchivoAsync(ProcesarArchivoCartaDto peticion)
        {
            var operacionValidacion = ValidacionUtilitario.ValidarModelo<RespuestaSimpleDto<bool>>(peticion);
            if (!operacionValidacion.Completado)
            {
                return operacionValidacion;
            }

            if (ProductosAsync().Where(e => e.Equals(peticion.Tipo)).FirstOrDefault() == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.Invalido, "El tipo de archivo seleccionado no es valido");
            }
            if (TiposAsync().Where(e => e.Equals(peticion.Producto)).FirstOrDefault() == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.Invalido, "El producto no es valido");
            }

            switch (peticion.Tipo)
            {
                case "INVENTARIO":
                case "RESUMEN":
                    break;
                default:
                    return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.Invalido, "Seleccione correctamente el tipo de documento a cargar");
            }

            var extension = Path.GetExtension(peticion.File.FileName);

            var nameArchivo = $"{Guid.NewGuid()}{extension}";

            var rutaArchivo = $"{_general.RutaArchivos}{nameArchivo}";

            using (FileStream filestream = System.IO.File.Create($"{rutaArchivo}"))
            {
                await peticion.File.CopyToAsync(filestream);
                filestream.Flush();
            }

            DateTime inicio = FechasUtilitario.ObtenerDiaOperativo().AddDays(1).AddMonths(-1);


            XLWorkbook archivoExcel = new XLWorkbook(rutaArchivo);
            if (archivoExcel == null)
            {
                return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.Invalido, "No se puede abrir archivo excel");
            }


            switch (peticion.Tipo)
            {
                case "INVENTARIO":
                    await InsertarInventarioDetalleAsync(archivoExcel, inicio, peticion.Producto);
                    break;
                case "RESUMEN":
                    await InsertarVentasAsync(archivoExcel, peticion.Producto, inicio);
                    break;
                default:
                    return new OperacionDto<RespuestaSimpleDto<bool>>(CodigosOperacionDto.Invalido, "Seleccione correctamente el tipo de documento a cargar");
            }


            return new OperacionDto<RespuestaSimpleDto<bool>>(
               new RespuestaSimpleDto<bool>()
               {
                   Id = true,
                   Mensaje = "Se guardo correctamente"
               }
               );
        }


        private async Task InsertarVentasAsync(XLWorkbook archivoExcel, string? producto, DateTime fecha)
        {

            var HojaExcel = archivoExcel.Worksheet(1);

            IXLRow fila = HojaExcel.Row(3);

            string? periodo = fila.Cell(1) != null ? fila.Cell(1).GetValue<string>() : null;

            var entidad = await _ventaPorClienteRepositorio.BuscarPorFechaYProductoAsync(fecha, producto);
            if (entidad == null)
            {
                entidad = new VentaPorCliente();
            }
            entidad.Producto = producto;
            entidad.Periodo = !string.IsNullOrWhiteSpace(periodo) ? periodo.Replace("periodo:", "").Trim() : null;
            entidad.Fecha = fecha;
            entidad.Creado = DateTime.UtcNow;
            if (entidad.IdVentaPorCliente > 0)
            {
                await _ventaPorClienteRepositorio.EditarAsync(entidad);
            }
            else
            {
                await _ventaPorClienteRepositorio.InsertarAsync(entidad);
            }

            var venta = await _ventaPorClienteRepositorio.BuscarPorFechaYProductoAsync(fecha, producto);
            if (venta != null)
            {
                await InsertarVentasDetalleAsync(archivoExcel, venta.IdVentaPorCliente);
            }

        }



        private async Task InsertarVentasDetalleAsync(XLWorkbook archivoExcel, long idVentaPorCliente)
        {
            await _ventaPorClienteDetalleRepositorio.EliminarAsync(new VentaPorClienteDetalle
            {
                IdVentaPorCliente = idVentaPorCliente
            });

            var HojaExcel = archivoExcel.Worksheet(1);

            string? producto = "CGN";
            for (int i = 12; i <= 24; i++)
            {

                IXLRow fila = HojaExcel.Row(i);

                var venta = new VentaPorClienteDetalle()
                {
                    IdVentaPorCliente = idVentaPorCliente,
                    Creado = DateTime.UtcNow,
                    Producto = producto
                };

                string? productoFila = fila.Cell(1) != null ? fila.Cell(1).GetValue<string>() : null;

                if (!string.IsNullOrWhiteSpace(productoFila))
                {
                    if (productoFila.Trim() == "GAS LICUADO DE PETROLEO - GLP")
                    {
                        producto = "GLP";
                        venta.Producto = producto;
                    }
                }

                string? cliente = fila.Cell(2) != null ? fila.Cell(2).GetValue<string>() : null;
                if (string.IsNullOrWhiteSpace(cliente))
                {
                    return;
                }
                venta.Cliente = cliente;
                venta.Uom = fila.Cell(3) != null ? fila.Cell(3).GetValue<string>() : null;

                var volumen = fila.Cell(4) != null ? fila.Cell(4).GetValue<string>() : null;
                double volumenValor = 0;
                bool canValumen = double.TryParse(volumen, out volumenValor);
                if (canValumen) venta.Volumen = volumenValor;

                var centaje = fila.Cell(5) != null ? fila.Cell(5).GetValue<string>() : null;
                double centajeValor = 0;
                bool canCentaje = double.TryParse(centaje, out centajeValor);
                if (canCentaje) venta.Volumen = centajeValor;

                var brl = fila.Cell(7) != null ? fila.Cell(7).GetValue<string>() : null;
                double brlValor = 0;
                bool canBrl = double.TryParse(brl, out brlValor);
                if (canBrl) venta.Brl = brlValor;

                await _ventaPorClienteDetalleRepositorio.InsertarAsync(venta);

            }

        }


        private async Task InsertarInventarioDetalleAsync(XLWorkbook archivoExcel, DateTime periodo, string tipo)
        {
            await _cargaInventarioRepositorio.EliminarAsync(new CargaInventario
            {
                Tipo = tipo,
                Periodo = periodo
            });

            var HojaExcel = archivoExcel.Worksheet(1);
            IXLRow fila9 = HojaExcel.Row(9);// Linea 9           
            await InsertarPorFilaInventarioAsync(fila9, "GLP", periodo, tipo);

            IXLRow fila13 = HojaExcel.Row(13);// Linea 13
            await InsertarPorFilaInventarioAsync(fila9, "HAS", periodo, tipo);
        }

        private async Task InsertarPorFilaInventarioAsync(IXLRow fila, string clase, DateTime periodo, string tipo)
        {
            var venta = new CargaInventario
            {
                Clase = clase,
                Periodo = periodo,
                Tipo = tipo
            };
            string? producto = fila.Cell(2) != null ? fila.Cell(2).GetValue<string>() : null;
            if (string.IsNullOrWhiteSpace(producto))
            {
                return;
            }
            venta.Producto = producto;
            venta.Almacen = fila.Cell(3) != null ? fila.Cell(3).GetValue<string>() : null;
            venta.Uom = fila.Cell(4) != null ? fila.Cell(4).GetValue<string>() : null;
            venta.Inventario = fila.Cell(5) != null ? fila.Cell(5).GetValue<double>() : 0;
            await _cargaInventarioRepositorio.InsertarAsync(venta);

        }

    }


}
