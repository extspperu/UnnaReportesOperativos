using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Dtos
{
    public class InventarioLiquidoGasNaturalDto
    {
        public double Glp { get; set; }
        public double PropanoSaturado { get; set; }
        public double ButanoSaturado { get; set; }
        public double Hexano { get; set; }
        public double Condensados { get; set; }
        public double PromedioLiquidos { get; set; }
    }
}
