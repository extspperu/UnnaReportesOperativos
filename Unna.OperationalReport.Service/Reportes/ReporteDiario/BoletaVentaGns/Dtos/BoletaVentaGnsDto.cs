using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos
{
    public class BoletaVentaGnsDto
    {
        public ReporteDto? General { get; set; }
        public string? Fecha { get; set; }
        public double Mpcs { get; set; }
        public double BtuPcs { get; set; }
        public double Mmbtu { get; set; }
        public string? Empresa { get; set; }
        public string? Abreviatura { get; set; }

        [JsonIgnore]
        public long? IdUsuario { get; set; }
    }
}
