using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos
{
    public class DistribucionGasNaturalAsociadoDto
    {

        public int? Item { get; set; }
        public string? Suministrador { get; set; }
        public double? VolumenGnsd { get; set; }
        public double? GasCombustible { get; set; }
        public double? VolumenGns { get; set; }
        public double? VolumenGna { get; set; }


    }
}
