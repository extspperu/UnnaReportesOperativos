using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.ResDiarioFiscalProd.Dtos
{
    public class ResDiarioFiscalProdDetDto
    {
        public string? Producto { get; set; }
        public string? Tanques { get; set; }
        public double? Niveles { get; set; }
        public double? Inventario { get; set; }
    }
}
