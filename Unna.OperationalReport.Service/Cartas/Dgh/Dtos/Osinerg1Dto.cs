using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Dtos
{
    public class Osinerg1Dto
    {
        public string? Periodo { get; set; }
        public string? PlantaDestilacion { get; set; }
        public string? PlantaAbsorcion { get; set; }



        public List<GasNaturalPrincipalDto>? RecepcionGasNatural { get; set; }
        public List<GasNaturalPrincipalDto>? UsoGas { get; set; }
        public List<GasNaturalPrincipalDto>? ProduccionLiquidos { get; set; }
        public string? Observacion { get; set; }
    }
}
