using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Mensual.Entidades
{
    public class PeriodoPrecioGlp
    {
        public long IdPeriodoPrecioGlp { get; set; }
        public DateTime Periodo { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public double? PrecioKg { get; set; }
        public int? NroDia { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public PeriodoPrecioGlp()
        {
            Creado = DateTime.UtcNow;
            Actualizado = DateTime.UtcNow;
        }
    }
}
