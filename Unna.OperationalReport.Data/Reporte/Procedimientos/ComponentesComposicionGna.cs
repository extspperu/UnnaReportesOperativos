using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Procedimientos
{
    public class ComponentesComposicionGna
    {
        public string? Simbolo { get; set; }
        public string? Componente { get; set; }
        public double? Molar { get; set; }
        public double? GnaComponente { get; set; }
        public double? GnaVolumen { get; set; }
        public double? EficienciaRecuperacion { get; set; }
        public double? LiquidoVolumenBl { get; set; }
        public double? LiquidoVolumenPcsd { get; set; }
        public double? ProductoGlpBl { get; set; }
        public double? ProductoGlpVol { get; set; }
        public double? ProductoCgnBl { get; set; }
        public double? ProductoCgnVol { get; set; }
    }
}
