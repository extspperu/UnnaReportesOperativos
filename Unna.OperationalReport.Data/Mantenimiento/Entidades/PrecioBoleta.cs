using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Mantenimiento.Entidades
{
    public  class PrecioBoleta
    {
        public DateTime Fecha { get; set; }
        public int? IdTipoPrecio { get; set; }
        public double? Precio { get; set; }
        public DateTime Creado { get; set; }
        public DateTime? Actualizado { get; set; }
        public bool EstaHabilitado { get; set; }
        public long? IdUsuario { get; set; }
    }
}
