using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Propiedad.Procedimientos
{
    public class BuscarComposicionGnaPromedio
    {
        public string? Lote { get; set; }
        public int? IdSuministrador { get; set; }
        public string? Suministrador { get; set; }
        public DateTime? Fecha { get; set; }
        public double? Porcentaje { get; set; }
    }
}
