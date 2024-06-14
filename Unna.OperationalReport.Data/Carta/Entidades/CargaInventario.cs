using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Carta.Entidades
{
    public class CargaInventario
    {

        public long IdCargaInventario { get; set; }
        public DateTime? Periodo { get; set; }
        public string? Tipo { get; set; }
        public string? Clase { get; set; }
        public string? Producto { get; set; }
        public string? Almacen { get; set; }
        public string? Uom { get; set; }
        public double? Inventario { get; set; }
        public DateTime Creado { get; set; }
        public long? IdArchivo { get; set; }
        public CargaInventario()
        {
            Creado = DateTime.UtcNow;
        }
    }
}
