using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.BoletaFiscalizacion.Dtos
{
    public class SecondTableDataFiscalizacion
    {
        public int Item { get; set; }
        public string Supplier { get; set; }
        public decimal VolumeGNA { get; set; }
        public decimal CalorificValue { get; set; }
        public decimal VolumeGNS { get; set; }
        public decimal VolumeGNSd { get; set; }
    }
}
