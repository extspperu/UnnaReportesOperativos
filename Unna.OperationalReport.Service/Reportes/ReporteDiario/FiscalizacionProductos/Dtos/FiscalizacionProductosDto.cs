using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Dtos
{
    public class FiscalizacionProductosDto
    {
        public ReporteDto? General { get; set; }
        public string? Fecha { get; set; }
        public List<FiscalizacionProductoTanqueDto>? ProductoParaReproceso { get; set; }
        public List<FiscalizacionProductoTanqueDto>? ProductoGlp { get; set; }
        public List<FiscalizacionProductoTanqueDto>? ProductoCgn { get; set; }
        public List<FiscalizacionProductoTanqueDto>? ProductoGlpCgn { get; set; }
        public string? Observacion { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }
    }
}
