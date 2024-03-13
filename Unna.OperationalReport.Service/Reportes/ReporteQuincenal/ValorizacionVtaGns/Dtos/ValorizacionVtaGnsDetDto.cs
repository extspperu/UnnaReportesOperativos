using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns.Dtos
{
    public class ValorizacionVtaGnsDetDto
    {
        public string? Fecha { get; set; }
        public double? Volumen { get; set; }
        public double? PoderCal { get; set; }
        public double? Energia { get; set; }
        public double? Precio { get; set; }
        public double? Costo { get; set; }

    }
}
