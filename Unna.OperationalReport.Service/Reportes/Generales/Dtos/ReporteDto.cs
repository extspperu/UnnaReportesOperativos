using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.Generales.Dtos
{
    public class ReporteDto
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "NombreReporte es requerido")]
        public string? NombreReporte { get; set; }
        public string? Nombre { get; set; }
        public string? Version { get; set; }
        public string? PreparadoPor { get; set; }
        public string? AprobadoPor { get; set; }
        public string? Detalle { get; set; }
        public string? Grupo { get; set; }
        public string? UrlFirma { get; set; }

        [JsonIgnore]
        public string? RutaFirma { get; set; }

        
        public string? FechaCadena
        {
            get
            {
                return Fecha.HasValue ? Fecha.Value.ToString("dd-MM-yy") : null;
            }
        }

        
        public DateTime? Fecha { get; set; }


    }
}
