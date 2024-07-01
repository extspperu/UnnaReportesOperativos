using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Propiedad.Procedimientos
{
    public class ListarPropiedadesFisicas
    {

        public int Id { get; set; }
        public string? Componente { get; set; }
        public double? PoderCalorifico { get; set; }
        public double? RelacionVolumen { get; set; }
        public double? PesoMolecular { get; set; }
        public double? DensidadLiquido { get; set; }
        public double? PoderCalorificoBruto { get; set; }

    }
}
