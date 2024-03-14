using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Procedimientos
{
    public class ReporteDiarioLiquidoGasNatural
    {
        public string? Producto { get; set; }
        public double? ProduccionDiaria { get; set; }
        public double? ProduccionMensual { get; set; }
        public double? VentaDiaria { get; set; }
        public double? VentaMensual { get; set; }
    }
}
