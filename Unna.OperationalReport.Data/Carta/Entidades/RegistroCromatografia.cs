using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Carta.Entidades
{
    public class RegistroCromatografia
    {
        public long Id { get; set; }
        public DateTime? Periodo { get; set; }
        public TimeSpan? HoraMuestreo { get; set; }
        public string? Tipo { get; set; }
        public int? IdLote { get; set; }
        public string? Tanque { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public bool EstaBorrado { get; set; }
        public long? IdUsuario { get; set; }

        public RegistroCromatografia()
        {
            Creado = DateTime.UtcNow;
            EstaBorrado = false;
        }

    }
}
