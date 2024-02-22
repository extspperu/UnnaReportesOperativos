using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.Generales.Dtos
{
    public class ReporteDto
    {
        public string? NombreReporte { get; set; }
        public string? Nombre { get; set; }
        public string? Version { get; set; }
        public string? PreparadoPör { get; set; }
        public string? AprobadoPor { get; set; }
        public string? Detalle { get; set; }
        public string? Grupo { get; set; }
        public string? UrlFirma { get; set; }
        public string? Fecha { get; set; }

    }
}
