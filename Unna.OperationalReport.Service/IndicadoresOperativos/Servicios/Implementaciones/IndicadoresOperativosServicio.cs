using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Configuracion.Enums;
using Unna.OperationalReport.Data.Mantenimiento.Entidades;
using Unna.OperationalReport.Data.Mensual.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.IndicadoresOperativos.Dtos;
using Unna.OperationalReport.Service.IndicadoresOperativos.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Mantenimientos.TipoCambios.Dtos;
using Unna.OperationalReport.Service.Usuarios.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.IndicadoresOperativos.Servicios.Implementaciones
{
    public class IndicadoresOperativosServicio : IIndicadoresOperativosServicio
    {

        private readonly IMensualRepositorio _mensualRepositorio;
        public IndicadoresOperativosServicio(IMensualRepositorio mensualRepositorio)
        {
            _mensualRepositorio = mensualRepositorio;
        }

        public OperacionDto<List<PeriodoIndicadoresDto>> ListarPeriodosAsync()
        {

            DateTime fechaActual = FechasUtilitario.ObtenerFechaSegunZonaHoraria(DateTime.UtcNow);

            var dto = new List<PeriodoIndicadoresDto>
            {
                new PeriodoIndicadoresDto
                {
                    Periodo = $"{fechaActual.Month}_{fechaActual.Year}",
                    Nombre = $"{fechaActual.Year} - {FechasUtilitario.ObtenerNombreMes(fechaActual)}"
                }
            };

            for (var i = 1; i < 12; i++)
            {
                DateTime fecha = fechaActual.AddMonths(-i);
                dto.Add(new PeriodoIndicadoresDto
                {
                    Periodo = $"{fecha.Month}_{fecha.Year}",
                    Nombre = $"{fecha.Year} - {FechasUtilitario.ObtenerNombreMes(fecha)}"
                });
            }
            return new OperacionDto<List<PeriodoIndicadoresDto>>(dto);
        }


        public async Task<OperacionDto<List<IndicadoresOperativosDto>>> BusquedaIndicadoresAsync(string periodo)
        {

            string[] cadenas = periodo.Split('_');
            if (cadenas.Length != 2)
            {
                return new OperacionDto<List<IndicadoresOperativosDto>>(CodigosOperacionDto.UsuarioIncorrecto, "Seleccione un periodo valido");
            }
            int numeroMes = int.Parse(cadenas[0]);
            int numeroAnio = int.Parse(cadenas[1]);

            DateTime fecha = new DateTime(numeroAnio, numeroMes, 1);


            var indicadores = await _mensualRepositorio.BuscarIndicadoresOperativosAsync(fecha);
            if (indicadores == null)
            {
                return new OperacionDto<List<IndicadoresOperativosDto>>(CodigosOperacionDto.UsuarioIncorrecto, "No se pudo obtener registros del periodo seleccionado");
            }

            var dto = indicadores.Select(e => new IndicadoresOperativosDto
            {
                Dia = e.Fecha.HasValue ? e.Fecha.Value.ToString("dd/MM/yyyy") : null,
                Eficiencia = e.Eficiencia,
                Glp = e.Glp,
                Gna = e.Gna,
                Lgn = e.Lgn
            }).ToList();

            return new OperacionDto<List<IndicadoresOperativosDto>>(dto);
        }
    }
}
