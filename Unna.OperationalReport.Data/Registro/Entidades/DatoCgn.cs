using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class DatoCgn
    {

        public long Id { get; set; }
        public string? Tanque { get; set; }
        public double? Centaje { get; set; }
        public double? Volumen { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdRegistroSupervisor { get; set; }
    }
}
