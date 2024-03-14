using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaBalanceEnergia.Dtos
{
    public class BalanceDto
    {

        public string? Balance { get; set; }
        public double? Mpcsd { get; set; }
        public double? Barriles { get; set; }
        public double? Energia { get; set; }
    }
}
