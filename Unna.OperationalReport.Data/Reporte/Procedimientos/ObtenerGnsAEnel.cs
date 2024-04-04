using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Procedimientos
{
    public class ObtenerGnsAEnel
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Distribucion { get; set; }
        public double Volumen { get; set; }
        public double PoderCalorifico { get; set; }
        public double? Energia { get; set; }
    }
}
