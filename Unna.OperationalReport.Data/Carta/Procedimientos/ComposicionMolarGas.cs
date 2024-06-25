using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Carta.Procedimientos
{
    public class ComposicionMolarGas
    {
        public int? Id { get; set; }
        public string? Propiedad { get; set; }
        public double? GasAsociado { get; set; }
        public double? GasResidual { get; set; }
        public string? Grupo { get; set; }
    }
}
