using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteDiarioPgt.Dtos
{
    public class ReporteDiarioPgtDetGnaDto
    {
        public string? Suministrador { get; set; }
        public double? VolumenDiario { get; set; }
        public double? PoderCalorifico { get; set; }
        public double? RiquezaGM { get; set; }
        public double? RiquezaBM { get; set; }
        public double? EnergiaDiaria { get; set; }
        public double? VolumenPromMes { get; set; }
    }
}
