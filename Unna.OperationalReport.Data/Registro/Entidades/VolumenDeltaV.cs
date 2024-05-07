using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class VolumenDeltaV
    {
        public long Id { get; set; }
        public string? NombreLote { get; set; }
        public double? Volumen { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdRegistroSupervisor { get; set; }
        public int? IdLote { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
