using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Procedimientos
{
    public class ObtenerLiquidosBarriles
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public double Enel { get; set; }
        public double BlsdTotal { get; set; }
    }
}
