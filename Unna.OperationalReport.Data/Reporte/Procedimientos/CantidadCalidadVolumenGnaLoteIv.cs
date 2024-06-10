using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Procedimientos
{
    public class CantidadCalidadVolumenGnaLoteIv
    {
        public int IdLote { get; set; } 
        public string? Lote { get; set; } 
        public double Volumen { get; set; } 
        public double Calorifico { get; set; } 
        public double Riqueza { get; set; } 
        public string? Corriente { get; set; } 
        public string? Medidor { get; set; } 
    }
}
