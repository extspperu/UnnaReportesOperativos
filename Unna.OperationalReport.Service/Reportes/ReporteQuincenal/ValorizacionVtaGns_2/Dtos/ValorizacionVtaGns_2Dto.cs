using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ValorizacionVtaGns_2.Dtos
{
    public class ValorizacionVtaGns_2Dto
    {
        public string? Periodo { get; set; }
        public string? PuntoFiscal { get; set; }
        public double? TotalVolumen { get; set; }
        public double? TotalPoderCal { get; set; }
        public double? TotalEnergia { get; set; }
        public double? PromPrecio { get; set; }
        public double? TotalCosto { get; set; }
        public double? EnerVolTransM { get; set; }
        public double? SubTotalFact { get; set; }
        public double? Igv { get; set; }
        public double? TotalFact { get; set; }
        public string? Comentario { get; set; }

        public List<ValorizacionVtaGns_2DetDto>? ValorizacionVtaGns_2Det { get; set; }
    }
}
