using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro
{
    public class ProduccionDiariaMs
    {
        public long Id { get; set; }
        public string? Producto { get; set; }
        public double? MedidoresMasicos { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public long? IdRegistroSupervisor { get; set; }
    }
}
