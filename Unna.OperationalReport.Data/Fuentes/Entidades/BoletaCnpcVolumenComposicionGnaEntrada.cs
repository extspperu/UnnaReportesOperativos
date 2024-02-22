using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Fuentes.Entidades
{
    public class BoletaCnpcVolumenComposicionGnaEntrada
    {
        public int Id { get; set; }
        public string? Corriente { get; set; }
        public string? Medidor { get; set; }
        public double? Volumen { get; set; }
        public double? Riqueza { get; set; }
        public double? C6 { get; set; }
        public double? C3 { get; set; }
        public double? Ic4 { get; set; }
        public double? Nc4 { get; set; }
        public double? NeoC5 { get; set; }
        public double? Ic5 { get; set; }
        public double? Nc5 { get; set; }
        public double? N2 { get; set; }
        public double? C1 { get; set; }
        public double? Co2 { get; set; }
        public double? C2 { get; set; }
        public double? O2 { get; set; }
        public double? Total { get; set; }
        public double? ConcentracionN2HastaO2 { get; set; }
    }
}
