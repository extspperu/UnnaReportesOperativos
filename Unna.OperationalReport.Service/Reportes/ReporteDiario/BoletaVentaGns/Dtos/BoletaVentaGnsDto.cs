using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaVentaGns.Dtos
{
    public class BoletaVentaGnsDto
    {
        public string? Fecha { get; set; }
        public double Mpcs { get; set; }
        public double BtuPcs { get; set; }
        public double Mmbtu { get; set; }
        public string? UrlFirma { get; set; }
    }
}
