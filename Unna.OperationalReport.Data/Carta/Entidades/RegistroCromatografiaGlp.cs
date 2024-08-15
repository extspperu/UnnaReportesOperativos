using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Carta.Entidades
{
    public class RegistroCromatografiaGlp
    {
        public long Id { get; set; }
      public DateTime  Fecha { get; set; }
        public double? C1 { get; set; }
        public double? C2 { get; set; }
        public double? C3 { get; set; }
        public double? Ic4 { get; set; }
        public double? Nc4 { get; set; }
        public double? NeoC5 { get; set; }
        public double? Ic5 { get; set; }
        public double? Nc5 { get; set; }
        public double? C6 { get; set; }
        public double? Drel { get; set; }
        public double? PresionVapor { get; set; }
        public double? T95 { get; set; }
        public double? MolarTotal { get; set; }
        public string? Tk { get; set; }
        public int? NroDespacho { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdRegistroCromatografia { get; set; }
        public long? IdUsuario { get; set; }
    }

}
