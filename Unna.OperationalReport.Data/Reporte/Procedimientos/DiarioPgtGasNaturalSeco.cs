using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Procedimientos
{
    public class DiarioPgtGasNaturalSeco
    {

        public int Item { get; set; }
        public string? Distribucion { get; set; }
        public double? Volumen { get; set; }
        public double? PoderCalorifico { get; set; }
        public double? VolumenPromedio { get; set; }
        public double? EnergiaDiaria { get; set; }
    }
}
