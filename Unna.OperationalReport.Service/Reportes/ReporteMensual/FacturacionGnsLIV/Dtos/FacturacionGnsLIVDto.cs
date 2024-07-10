using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos
{
    public class FacturacionGnsLIVDto
    {
        public string? NombreReporte { get; set; }        
        public string? Periodo { get; set; }
        public string? Concepto { get; set; }
        public double? Mpc { get; set; }
        public double? Mmbtu { get; set; }
        public double? PrecioUs { get; set; }
        public double? ImporteUs { get; set; }
        public double? TotalMpc { get; set; }
        public double? TotalMmbtu { get; set; }
        public double? TotalPrecioUs { get; set; }
        public double? TotalImporteUs { get; set; }
        public string? UrlFirma { get; set; }

        [JsonIgnore]
        public string? RutaFirma { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }
    }
}
