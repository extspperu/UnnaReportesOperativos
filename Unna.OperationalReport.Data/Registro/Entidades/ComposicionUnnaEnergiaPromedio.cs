using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class ComposicionUnnaEnergiaPromedio
    {
        public DateTime? Fecha { get; set; }
        public double? C6 { get; set; }
        public double? C3 { get; set; }
        public double? IC4 { get; set; }
        public double? NC4 { get; set; }
        public double? NEOC5 { get; set; }
        public double? IC5 { get; set; }
        public double? NC5 { get; set; }
        public double? N2 { get; set; }
        public double? C1 { get; set; }
        public double? CO2 { get; set; }
        public double? C2 { get; set; }
        public string? Observacion { get; set; }

        public string? Simbolo { get; set; }
        public string? Suministrador { get; set; }
        public double? PromedioComponente { get; set; }
    }
}
