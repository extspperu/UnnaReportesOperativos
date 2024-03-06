using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class DespachoGlpEnvasado
    {
        public long Id { get; set; }
        public string? Nombre { get; set; }
        public double? Envasado { get; set; }
        public double? Granel { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdRegistroSupervisor { get; set; }
    }
}
