using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.BoletaFiscalizacion.Dtos
{
    public class FirstTableDataFiscalizacion
    {
        public int Item { get; set; }
        public string Supplier { get; set; }
        public decimal VolumeGNA { get; set; }
        public decimal Richness { get; set; }
        public decimal LGNContent { get; set; }
        public decimal AssignmentFactor { get; set; }
        public decimal LGNAssignment { get; set; }
    }
}
