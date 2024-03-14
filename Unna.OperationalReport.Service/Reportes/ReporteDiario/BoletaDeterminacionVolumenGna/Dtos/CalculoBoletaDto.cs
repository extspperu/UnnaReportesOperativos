using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos
{
    public class CalculoBoletaDto
    {
        public List<PgtVolumenDto>? PgtVolumen { get; set; }
    }

    public class PgtVolumenDto
    {
        public string? Pgt { get; set; }
        public string? Vol { get; set; }
    }
}
