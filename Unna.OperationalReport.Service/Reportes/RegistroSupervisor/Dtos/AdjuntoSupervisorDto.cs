using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;
using Unna.OperationalReport.Service.Reportes.Adjuntos.Dtos;

namespace Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Dtos
{
    public class AdjuntoSupervisorDto
    {
        public string? Id { get; set; }
        public bool? EsConciliado { get; set; }
        public int? IdAdjunto { get; set; }
        public string? IdArchivo { get; set; }
        public string? Color { get; set; }
        public string? IdRegistroSupervisor { get; set; }
        public AdjuntoDto? Adjunto { get; set; }
        public ArchivoRespuestaDto? Archivo { get; set; }
    }
}
