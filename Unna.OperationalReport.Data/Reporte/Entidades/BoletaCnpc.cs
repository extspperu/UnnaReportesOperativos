using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class BoletaCnpc
    {
        public DateTime Fecha { get; set; }
        public double? GasMpcd { get; set; }
        public double? GlpBls { get; set; }
        public double? CgnBls { get; set; }
        public double? GnsMpc { get; set; }
        public double? GcMpc { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }


        public int? Id { get; set; }
    }
}
