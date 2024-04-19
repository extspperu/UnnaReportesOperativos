using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos
{
    public class ComposicionUnnaEnergiaLIVDto
    {
        public string? Fecha { get; set; }
        public double? C6 { get; set; }
        public double? C3 { get; set; }
        public double? Ic4 { get; set; }
        public double? Nc4 { get; set; }
        public double? NeoC5 { get; set; }
        public double? Ic5 { get; set; }
        public double? Nc5 { get; set; }
        public double? Nitrog { get; set; }
        public double? C1 { get; set; }
        public double? Co2 { get; set; }
        public double? C2 { get; set; }
        public string? Observacion { get; set; }
    }
}
