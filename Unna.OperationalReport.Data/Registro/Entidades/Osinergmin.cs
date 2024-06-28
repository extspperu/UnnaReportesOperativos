using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class Osinergmin
    {
        public DateTime? Fecha { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public double? NacionalizadaM3 { get; set; }
        public double? NacionalizadaTn { get; set; }
        public double? NacionalizadaBbl { get; set; }
        public long? IdArchivo { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdUsuario { get; set; }

        public Osinergmin()
        {
            Creado = DateTime.UtcNow;
        }

    }
}
