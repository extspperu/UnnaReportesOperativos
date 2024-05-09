using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Propiedad.Entidades
{
    public class Fisicas
    {

        public int Id { get; set; }
        public string? Grupo { get; set; }
        public double? PoderCalorifico { get; set; }
        public double? RelacionVolumen { get; set; }
        public double? PesoMolecular { get; set; }
        public double? DensidadLiquido { get; set; }
    }
}
