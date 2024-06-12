using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Dtos
{
    public class Osinergmin1Dto
    {
        public string? Periodo { get; set; }
        
        // Segunda página
        // Tabla 1
        public RecepcionGasNaturalAsociadoDto? RecepcionGasNaturalAsociado { get; set; }
        // Tabla 2
        public UsoGasDto? UsoGas { get; set; }
        // Tabla 3
        public ProduccionLiquidosGasNaturalDto? ProduccionLiquidosGasNatural { get; set; }
        public string? Observacion { get; set; }
    }
}
