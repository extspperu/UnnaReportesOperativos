using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class ResumenBalanceEnergiaLGNResult
    {
        public Int64 IdImprimir { get; set; }
        public Int64 IdConfiguracion { get; set; }
        public DateTime Fecha { get; set; }
        public string Datos { get; set; }
    }


}
