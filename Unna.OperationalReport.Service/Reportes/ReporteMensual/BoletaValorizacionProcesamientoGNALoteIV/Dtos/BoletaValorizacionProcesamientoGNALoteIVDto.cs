using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaValorizacionProcesamientoGNALoteIV.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletaValorizacionProcesamientoGNALoteIV.Dtos
{
    public class BoletaValorizacionProcesamientoGNALoteIVDto
    {
        public string? Año { get; set; }
        public string? Mes { get; set; }

        public List<BoletaValorizacionProcesamientoGNALoteIVDetDto>? BoletaValorizacionProcesamientoGNALoteIVDet { get; set; }

        public double? TotalVolumenMPC { get; set; }
        public double? TotalPCBTUPC { get; set; }
        public double? TotalEnergiaMMBTU { get; set; }
        public double? TotalEnergiaVolProcesadoMMBTU { get; set; }

        public double? PrecioUSDMMBTU { get; set; }

        public double? SubTotalAFacturarUSD { get; set; }
        public double? IGV18Porcentaje { get; set; }

        public double? TotalAFacturarUSD { get; set; }

    }
}
