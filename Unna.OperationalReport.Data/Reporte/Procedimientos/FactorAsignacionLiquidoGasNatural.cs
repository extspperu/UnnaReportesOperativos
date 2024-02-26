using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Procedimientos
{
    public class FactorAsignacionLiquidoGasNatural
    {
        public int? Item { get; set; }
        public string? Suministrador { get; set; }
        public double Volumen { get; set; }
        public double Riqueza { get; set; }
        public double Calorifico { get; set; }
        public double Contenido { get; set; }
        public double Factor { get; set; }
        public double Asignacion { get; set; }
    }
}
