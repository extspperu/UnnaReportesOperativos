using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Data.Mantenimiento.Repositorios.Abstracciones;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Dtos;
using Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Abstracciones;
using Unna.OperationalReport.Service.Registros.DiaOperativos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios;

namespace Unna.OperationalReport.Service.Mantenimientos.ValoresDefectoReportes.Servicios.Implementaciones
{
    public class ValoresDefectoReporteServicio: IValoresDefectoReporteServicio
    {

        private readonly IValoresDefectoReporteRepositorio _valoresDefectoReporteRepositorio;
        public ValoresDefectoReporteServicio(IValoresDefectoReporteRepositorio valoresDefectoReporteRepositorio)
        {
            _valoresDefectoReporteRepositorio = valoresDefectoReporteRepositorio;
        }

        public async Task<double?> ObtenerValorAsync(string? llave)
        {
            if (string.IsNullOrWhiteSpace(llave))
            {
                return new double?();
            }
            var entidad = await _valoresDefectoReporteRepositorio.BuscarPorLlaveAsync(llave);
            if (entidad == null)
            {
                return new double?();
            }

            return entidad.Valor;
        }


    }
}
