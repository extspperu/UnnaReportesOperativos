using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Dtos
{
    public class UsoGasDto
    {
        public double GasNaturalRestituido { get; set; }
        public double ConsumoPropio { get; set; }
        public double ConvertidoEnLgn { get; set; }
        public double Total { get; set; }
    }
}
