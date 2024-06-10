using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Mantenimiento.Entidades
{
    public class Carta
    {
        public int IdCarta { get;set; }
        public string? Sumilla { get;set; }
        public string? Destinatario { get;set; }
        public string? Asunto { get;set; }
        public string? Cuerpo { get;set; }
        public string? Pie { get;set; }
        public DateTime Creado { get;set; }
        public DateTime? Actualizado { get;set; }
    }
}
