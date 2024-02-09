using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Configuracion.Entidades
{
    public class TipoArchivo
    {
        public int Id { get; set; }

        public string? Tipo { get; set; }

        public string? Extension { get; set; }

        public string? TypeMime { get; set; }

        public virtual ICollection<Archivo>? Archivos { get; set; }
    }
}
