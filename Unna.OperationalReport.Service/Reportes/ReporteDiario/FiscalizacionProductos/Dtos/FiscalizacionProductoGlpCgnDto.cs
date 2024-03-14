using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Dtos
{
    public class FiscalizacionProductoGlpCgnDto
    {
        public string? Producto { get; set; }
        public double? Produccion { get; set; }
        public double? Despacho { get; set; }
        public double? Inventario { get; set; }
    }
}
