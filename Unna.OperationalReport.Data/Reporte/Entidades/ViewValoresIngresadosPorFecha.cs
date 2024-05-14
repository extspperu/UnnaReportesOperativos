using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class ViewValoresIngresadosPorFecha
    {

        public DateTime Fecha { get; set; }
        public int IdLote { get; set; }
        public string? Lote { get; set; }
        public double? Volumen { get; set; }
        public double? Calorifico { get; set; }
        public double? Riqueza { get; set; }
    }
}
