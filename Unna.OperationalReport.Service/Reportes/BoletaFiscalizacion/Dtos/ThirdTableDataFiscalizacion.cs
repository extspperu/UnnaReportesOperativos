using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.BoletaFiscalizacion.Dtos
{
    public class ThirdTableDataFiscalizacion
    {
        public int Item { get; set; }
        public string Supplier { get; set; }
        public decimal VolumeGNS { get; set; }
        public decimal FlareVolume { get; set; }
        public decimal TransferredGNSVolume { get; set; }
    }
}
