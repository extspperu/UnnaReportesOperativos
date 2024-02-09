using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Configuraciones.Archivos.Dtos;

namespace Unna.OperationalReport.Service.Reportes.RegistroSupervisor.Dtos
{
    public class RegistroSupervisorDto
    {
        public string? Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Comentario { get; set; }
        public string? IdArchivo { get; set; }
        

        [JsonIgnore]
        public long? IdUsuario { get; set; }
        public List<AdjuntoSupervisorDto>? Adjuntos { get; set; }
        public ArchivoRespuestaDto? Archivo { get; set; }
    }
}
