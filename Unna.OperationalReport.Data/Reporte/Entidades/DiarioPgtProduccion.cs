using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class DiarioPgtProduccion
    {
        public DateTime Fecha {  get; set; }
        public int IdLote {  get; set; }
        public double? Glp {  get; set; }
        public double? Cgn {  get; set; }
    }
}
