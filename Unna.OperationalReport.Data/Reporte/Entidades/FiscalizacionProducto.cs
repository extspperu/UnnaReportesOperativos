using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class FiscalizacionProducto
    {
        public DateTime Fecha { get; set; }
        public string? Producto { get; set; }
        public string? Tanque { get; set; }
        public double? Nivel { get; set; }
        public double? Inventario { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdImprimir { get; set; }
    }
}
