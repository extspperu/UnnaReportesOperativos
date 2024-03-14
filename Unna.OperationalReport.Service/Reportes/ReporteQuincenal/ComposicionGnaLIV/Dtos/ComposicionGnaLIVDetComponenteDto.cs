using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos
{
    public class ComposicionGnaLIVDetComponenteDto
    {
        public string? CompSimbolo { get; set; }
        public string? CompDescripcion { get; set; }
        public double? CompMolPorc { get; set; }
        public double? CompUnna { get; set; }
        public double? CompDif { get; set; }
    }
}
