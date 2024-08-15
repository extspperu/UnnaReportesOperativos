using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Cromatografia.Dtos
{
    public class RegistroGlpDto
    {
        public DateTime? Fecha { get; set; }
        public double? C1 { get; set; }
        public double? C2 { get; set; }
        public double? C3 { get; set; }
        public double? IC4 { get; set; }
        public double? NC4 { get; set; }
        public double? NeoC5 { get; set; }
        public double? IC5 { get; set; }
        public double? NC5 { get; set; }
        public double? C6Plus { get; set; }
        public double? DRel { get; set; }
        public double? PresionVapor { get; set; }
        public double? T95 { get; set; }
        public double? PorcentajeMolarTotal { get; set; }
        public string? Tk { get; set; }
        public int? Despachos { get; set; }

        public int? Day { get; set; }
    }
}
