using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos
{
    public class BoletaCnpcDto
    {
        public ReporteDto? General { get; set; }

        public string? Fecha { get; set; }
        public BoletaCnpcTabla1Dto? Tabla1 { get; set; }
        public double? VolumenTotalGnsEnMs { get; set; }
        public double? VolumenTotalGns { get; set; }
        public double? FlareGna { get; set; }
        public List<FactoresDistribucionGasNaturalDto>? FactoresDistribucionGasNaturalSeco { get; set; }
    }
}
