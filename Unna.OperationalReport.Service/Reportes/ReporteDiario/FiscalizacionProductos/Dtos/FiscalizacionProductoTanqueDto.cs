using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.FiscalizacionProductos.Dtos
{
    public class FiscalizacionProductoTanqueDto
    {
        public int Item { get; set; } 
        public string? Producto { get; set; } 
        public string? Tanque { get; set; } 
        public double? Nivel { get; set; } 
        public double Inventario { get; set; } 
    }
}
