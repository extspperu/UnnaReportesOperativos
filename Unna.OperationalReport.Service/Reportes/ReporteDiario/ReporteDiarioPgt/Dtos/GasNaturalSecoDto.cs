using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos
{
    public class GasNaturalSecoDto
    {
        public int? Item { get;set; }
        public string? Distribucion { get;set; }
        public double? Volumen { get;set; }
        public double? Calorifico { get;set; }
        public double? VolumenPromedio { get;set; }
        public double? EnergiaDiaria { get;set; }
    }
}
