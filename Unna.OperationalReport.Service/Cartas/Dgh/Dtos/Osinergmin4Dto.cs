using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Dtos
{
    public class Osinergmin4Dto
    {
        public string Periodo {  get; set; }
        public ProduccionLiquidosGasNaturalDto? ProduccionLiquidosGasNatural { get; set; }
        public VentaLiquidosGasNaturalDto? VentaLiquidoGasNatural { get; set; }

        public List<VolumenVendieronProductosDto>? InventarioLiquidoGasNatural { get; set; }
    }
}
