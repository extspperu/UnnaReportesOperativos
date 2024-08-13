using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class ResumenBalanceEnergiaLGNCalculos
    {
        public int NumeroQuincena {get; set;}
        public double DensidadGlpKgBl { get; set; }
        public double PcGlp { get; set; }
        public double PcCgn { get; set; }
        public double PcLgn { get; set; }
        public double AvgFactorCoversion { get; set; }

    }
}
