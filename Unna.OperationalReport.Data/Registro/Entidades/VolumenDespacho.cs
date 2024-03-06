using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class VolumenDespacho
    {
        public long Id { get; set; }
        public string? Tanque { get; set; }
        public string? Cliente { get; set; }
        public string? Placa { get; set; }
        public double? Volumen { get; set; }
        public string? Tipo { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdRegistroSupervisor { get; set; }
    }
}
