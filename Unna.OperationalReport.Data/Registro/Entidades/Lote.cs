using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Entidades
{
    public class Lote
    {
        public int IdLote { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<DiaOperativo>? DiaOperativos { get; set; }
    }
}
