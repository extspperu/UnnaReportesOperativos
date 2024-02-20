using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ReporteOperacionUnna.Dtos
{
    public class ReporteOperacionUnnaPlantaSepGasNatDto
    {
        public double? CapacidadDisPlanta { get; set; }
        public double? VolumenGasNatHumedo { get; set; }
        public double? VolumenGasNatSecoReinyFlare { get; set; }
        public double? VolumenGasNatSecoVentas { get; set; }
        public double? ProcGasNatSecoTotal { get; set; }
        public double? VolumenLgnProducidoPlanta { get; set; }
    }
}
