using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ComposicionGnaLIV.Dtos
{
    public class ComposicionGnaLIVDetComposicionDto
    {
        public string? CompGnaDia { get; set; }
        public double? CompGnaC6 { get; set; }
        public double? CompGnaC3 { get; set; }
        public double? CompGnaIc4 { get; set; }
        public double? CompGnaNc4 { get; set; }
        public double? CompGnaNeoC5 { get; set; }
        public double? CompGnaIc5 { get; set; }
        public double? CompGnaNc5 { get; set; }
        public double? CompGnaNitrog { get; set; }
        public double? CompGnaC1 { get; set; }
        public double? CompGnaCo2 { get; set; }
        public double? CompGnaC2 { get; set; }
        public double? CompGnaTotal { get; set; }
        public double? CompGnaVol { get; set; }
        public double? CompGnaPCalorifico { get; set; }
        public string? CompGnaObservacion { get; set; }

       
    }
}
