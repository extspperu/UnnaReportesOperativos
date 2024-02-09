using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Configuracion.Entidades
{
    public class Error
    {
        public long Id { get; set; }
        public string? Mensaje { get; set; }
        public string? Traza { get; set; }
        public string? Adicionales { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? CreadoLocal { get; set; }
    }
}
