using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Laboratorio.GLP.Dto
{
    public class GLPDataDto
    {
        public int Day { get; set; }
        public decimal C1 { get; set; } = 0;
        public decimal C2 { get; set; } = 0;
        public decimal C3 { get; set; } = 0;
        public decimal IC4 { get; set; } = 0;
        public decimal NC4 { get; set; } = 0;
        public decimal NeoC5 { get; set; } = 0;
        public decimal IC5 { get; set; } = 0;
        public decimal NC5 { get; set; } = 0;
        public decimal C6Plus { get; set; } = 0;
        public decimal DRel { get; set; } = 0;
        public decimal PresionVapor { get; set; } = 0;
        public decimal T95 { get; set; } = 0;
        public decimal PorcentajeMolarTotal { get; set; } = 0;
        public int TK { get; set; } = 0;
        public int Despachos { get; set; } = 0;
    }
}
