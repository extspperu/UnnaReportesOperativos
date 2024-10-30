using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Correos.Dtos
{
    public class EnviarCorreoDto
    {
        public string? IdReporte { get; set; }
        public List<string>? Destinatario { get; set; }
        public List<string>? Cc { get; set; }
        public string? Asunto { get; set; }
        public string? Cuerpo { get; set; }
        public bool AdjuntaReporte { get; set; }
        public bool TieneArchivoPdf { get; set; }
        public bool TieneArchivoExcel { get; set; }
        public bool FueEnviado { get; set; }
        public bool ReporteFueGenerado { get; set; }
        public DateTime? DiaOperativo { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }

        public List<AdjuntoCorreoDto>? Adjuntos { get; set; }
    }
}
