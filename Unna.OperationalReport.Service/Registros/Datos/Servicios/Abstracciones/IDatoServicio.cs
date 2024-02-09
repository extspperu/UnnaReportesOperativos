using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Registros.Datos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Registros.Datos.Servicios.Abstracciones
{
    public interface IDatoServicio
    {
        Task<OperacionDto<List<DatoDto>?>> ListarPorTipoAsync(string? tipo);
    }
}
