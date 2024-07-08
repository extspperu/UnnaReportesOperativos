using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Laboratorio.CGN.Dtos
{
    public class CGNDataDto
    {
        public int Day { get; set; }
        public decimal PInicial { get; set; } = 0;
        public decimal Valor5 { get; set; } = 0;
        public decimal Valor10 { get; set; } = 0;
        public decimal Valor30 { get; set; } = 0;
        public decimal Valor50 { get; set; } = 0;
        public decimal Valor70 { get; set; } = 0;
        public decimal Valor90 { get; set; } = 0;
        public decimal Valor95 { get; set; } = 0;
        public decimal PFinal { get; set; } = 0;
        public decimal API { get; set; } = 0;
        public decimal GEsp { get; set; } = 0;
        public decimal RVP { get; set; } = 0;
        public int NDespachos { get; set; } = 0;
    }
}
