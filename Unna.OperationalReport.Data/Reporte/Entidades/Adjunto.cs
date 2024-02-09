using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class Adjunto
    {
        public int Id { get; set; }        
        public string? Grupo { get; set; }
        public string? Nomenclatura { get; set; }
        public string? Extension { get; set; }
        public virtual ICollection<AdjuntoSupervisor>? AdjuntoSupervisores { get; set; }

    }
}
