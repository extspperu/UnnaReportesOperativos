using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Laboratorio.GNSGNA.Dtos
{
    public class GNSGNAData
    {
        public int Day { get; set; }
        public decimal C6 { get; set; }
        public decimal C3 { get; set; }
        public decimal IC4 { get; set; }
        public decimal NC4 { get; set; }
        public decimal NeoC5 { get; set; }
        public decimal IC5 { get; set; }
        public decimal NC5 { get; set; }
        public decimal NITROG { get; set; }
        public decimal C1 { get; set; }
        public decimal CO2 { get; set; }
        public decimal C2 { get; set; }
        public decimal O2 { get; set; }
        public decimal TOTAL { get; set; }
        public decimal GRAV { get; set; }
        public decimal BTU { get; set; }
        public decimal LGN { get; set; }
        public decimal LGNRPTE { get; set; }
        public bool Conciliado { get; set; }
        public string Comentarios { get; set; }
    }
}
