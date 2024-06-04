using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class BalanceEnergiaDiaria
    {
        public DateTime Fecha { get; set; }
        public double? ProduccionGlp { get; set; }
        public double? ProduccionCgn { get; set; }
    }
}
