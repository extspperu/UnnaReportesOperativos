using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaSuministroGNSdelLoteIVaEnel.Dtos
{
    public class BoletaSuministroGNSdelLoteIVaEnelDto
    {

        public string? Periodo { get; set; }
        public List<BoletaSuministroGNSdelLoteIVaEnelDetDto>? BoletaSuministroGNSdelLoteIVaEnelDet { get; set; }

        public double TotalVolumenMPC { get; set; }
        public double TotalPCBTUPC { get; set; }
        public double TotalEnergiaMMBTU { get; set; }
        public double TotalEnergiaVolTransferidoMMBTU { get; set; }

        public string? Comentarios { get; set; }
    }
}
