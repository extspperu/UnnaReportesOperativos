using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Dtos
{
    public class ProcesamientoVolumenDto
    {
        public int Item { get; set; }
        public string? Nombre { get; set; }
        public double? Volumen { get; set; }
    }
}
