using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos
{
    public class ComposicionGnaLIVDetComponenteDto
    {
        public int? Item { get; set; }
        public string? Simbolo { get; set; }
        public string? Descripcion { get; set; }
        public double? MolPorc { get; set; }
        //public double? CompUnna { get; set; }
        //public double? CompDif { get; set; }
    }
}
