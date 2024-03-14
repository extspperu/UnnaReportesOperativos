using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.Impresiones.Dtos
{
    public class ImpresionDto
    {
        public string? Id { get; set; }
        public string? IdConfiguracion { get; set; }
        public DateTime Fecha { get; set; }
        public string? Datos { get; set; }
        public bool EsEditado { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }
        
    }
}
