using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Carta.Repositorios.Abstracciones;
using Unna.OperationalReport.Data.Seguimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Dtos;
using Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Abstracciones;

namespace Unna.OperationalReport.Service.Seguimiento.BalanceDiario.Servicios.Implementaciones
{
    public class SeguimientoBalanceDiarioServicio : ISeguimientoBalanceDiarioServicio
    {
        private readonly ISeguimientoRepositorio _seguimientoRepositorio;
        public SeguimientoBalanceDiarioServicio(ISeguimientoRepositorio seguimientoRepositorio)
        {
            _seguimientoRepositorio = seguimientoRepositorio;
        }

        public async Task<List<ColumnaDto>> ObtenerDatosSeguimiento(int IdModuloSeguimiento)
        {
            var datosDelDia = await _seguimientoRepositorio.ListarPorFechaAsync(IdModuloSeguimiento);

            // Agrupar los datos por columna (NombreColumna)
            var columnasAgrupadas = datosDelDia
                .GroupBy(d => d.NombreColumna)
                .Select(g => new ColumnaDto(
                    g.Key,
                    g.Select(d => new SeguimientoBoxDto(
                        d.Titulo,
                        d.IdEstadoColor,
                        d.Color,
                        d.ColorTexto,
                        d.EsVisible,
                        d.Orden
                    )).ToList()
                )).ToList();

            return columnasAgrupadas;
        }
        public async Task<bool> ActualizarEstadoSeguimientoDiarioAsync(int idSeguimientoDiario, int idEstadoColor)
        {
            return await _seguimientoRepositorio.ActualizarEstadoSeguimientoDiarioAsync(idSeguimientoDiario, idEstadoColor);
        }
    }
}
