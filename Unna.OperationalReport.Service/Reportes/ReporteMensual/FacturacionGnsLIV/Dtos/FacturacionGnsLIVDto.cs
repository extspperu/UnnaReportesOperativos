using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.FacturacionGnsLIV.Dtos
{
    public class FacturacionGnsLIVDto
    {
        public string? MesAnio { get; set; }
        public string? Periodo { get; set; }
        public List<FacturacionGnsLIVDetDto>? FacturacionGnsLIVDet { get; set; }
        public double? MpcTotal { get; set; }
        public double? MmbtuTotal { get; set; }
        public double? PrecioUsTotal { get; set; }
        public double? ImporteUsTotal { get; set; }
    }
}
