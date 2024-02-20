using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Dtos
{
    public class ReporteOperacionUnnaDto
    {
        public string? ReporteNro { get; set; }
        public string? EmpresaNombre { get; set; }
        public string? FechaEmision { get; set; }
        public string? DiaOperativo { get; set; }
        public ReporteOperacionUnnaPlantaSepGasNatDto? PlantaSepGasNat { get; set; }
        public ReporteOperacionUnnaPlantaFracLiqGasNatDto? PlantaFracLiqGasNat { get; set; }
        

    }
}
