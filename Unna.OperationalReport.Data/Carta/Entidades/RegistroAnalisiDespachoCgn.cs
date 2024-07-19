using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Carta.Entidades
{
    public class RegistroAnalisiDespachoCgn
    {

        public DateTime Fecha { get; set; }
        public long? IdRegistroCromatografia { get; set; }
        public double? Pinic { get; set; }
        public double? P5 { get; set; }
        public double? P10 { get; set; }
        public double? P30 { get; set; }
        public double? P50 { get; set; }
        public double? P70 { get; set; }
        public double? P90 { get; set; }
        public double? P95 { get; set; }
        public double? Pfin { get; set; }
        public double? Api { get; set; }
        public double? Gesp { get; set; }
        public double? Rvp { get; set; }
        public int? NroDespacho { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdUsuario { get; set; }
    }
}
