using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Mensual.Procedimientos
{
    public class BoletaSuministroGnsDeLoteIvAEnel
    {
        public DateTime? Fecha { get; set; }
        public double Volumen { get; set; }
        public double PoderCalorifico { get; set; }
        public double Energia { get; set; }
    }
}
