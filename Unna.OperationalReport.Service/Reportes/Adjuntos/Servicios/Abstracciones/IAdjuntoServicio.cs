using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Dtos;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Service.Reportes.Adjuntos.Servicios.Abstracciones
{
    public interface IAdjuntoServicio
    {
        Task<OperacionDto<List<AdjuntoDto>>> ListarPorGrupoAsync(string? grupo);
    }
}
