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



        public bool? EsValidado { get; set; }

        [JsonIgnore]
        public DateTime? FechaValidado { get; set; }
        public bool? EsObservado { get; set; }

        [JsonIgnore]
        public DateTime? FechaObservado { get; set; }



        [JsonIgnore]
        public long? IdUsuario { get; set; }
        public List<AdjuntoSupervisorDto>? Adjuntos { get; set; }
        public ArchivoRespuestaDto? Archivo { get; set; }


        [JsonProperty(PropertyName = "fechaObservado")]
        public string? FechaObservadoCadena
        {
            get
            {
                return FechaObservado.HasValue ? FechaObservado.Value.ToString("dd/MM/yyyy HH:mm:ss") : null;
            }
        }

        [JsonProperty(PropertyName = "fechaValidado")]
        public string? FechaValidadoCadena
        {
            get
            {
                return FechaValidado.HasValue ? FechaValidado.Value.ToString("dd/MM/yyyy HH:mm:ss") : null;
            }
        }


    }
}
