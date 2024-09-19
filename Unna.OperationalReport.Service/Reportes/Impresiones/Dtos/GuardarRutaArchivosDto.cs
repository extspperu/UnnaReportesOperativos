using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.Impresiones.Dtos
{
    public class GuardarRutaArchivosDto
    {
        [JsonIgnore]
        public long IdImprimir { get; set; }
        public int IdReporte { get; set; }
        public string? RutaPdf { get; set; }
        public string? RutaExcel { get; set; }
    }
}
