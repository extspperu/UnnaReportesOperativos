using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Mensual.Procedimientos
{
    public class ResumenEntrega
    {
        public int Id { get; set; }
        public string? Fecha { get; set; }
        public double? Cnpc { get; set; }
        public double? Total { get; set; }
        public double? NoProcesado { get; set; }
        public double? PlantaFs { get; set; }
        public double? AjustePlantaFs { get; set; }
        public double? Procesado { get; set; }
        public double? ProcesadoHoras { get; set; }
    }
}
