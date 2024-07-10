using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos;

namespace Unna.OperationalReport.Service.Laboratorio.GNSGNA.Dtos
{
    public class GNSGNADto
    {
        public long IdUsuario { get; set; }
        public List<GNSGNA>? Data { get; set; } = new List<GNSGNA>();
    }

    public class GNSGNA
    {
        public int Day { get; set; }
        public double C6 { get; set; }
        public double C3 { get; set; }
        public double IC4 { get; set; }
        public double NC4 { get; set; }
        public double NeoC5 { get; set; }
        public double IC5 { get; set; }
        public double NC5 { get; set; }
        public double NITROG { get; set; }
        public double C1 { get; set; }
        public double CO2 { get; set; }
        public double C2 { get; set; }
        public double O2 { get; set; }
        public double TOTAL { get; set; }
        public double GRAV { get; set; }
        public double BTU { get; set; }
        public double LGN { get; set; }
        public double LGNRPTE { get; set; }
        public bool Conciliado { get; set; }
        public string Comentarios { get; set; }
    }
}
