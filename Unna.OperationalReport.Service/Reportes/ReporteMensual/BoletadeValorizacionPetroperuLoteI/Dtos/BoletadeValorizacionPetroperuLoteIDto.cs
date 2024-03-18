using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos
{
    public class BoletadeValorizacionPetroperuLoteIDto
    {
        public string? Fecha { get; set; }
        public List<BoletadeValorizacionPetroperuLoteIDetDto>? BoletadeValorizacionPetroperuLoteIDet { get; set; }


        public double TotalGasNaturalLoteIGNAMPCSD { get; set; }
        public double TotalGasNaturalLoteIEnergiaMMBTU { get; set; }
        public double TotalGasNaturalLoteILGNRecupBBL { get; set; }

        
        public double TotalGasNaturalEficienciaPGT { get; set; }

        public double TotalGasSecoMS9215GNSLoteIMCSD { get; set; }

        public double TotalGasSecoMS9215EnergiaMMBTU { get; set; }

        public double TotalValorLiquidosUS { get; set; }
        public double TotalCostoUnitMaquilaUSMMBTU { get; set; }
        public double TotalCostoMaquilaUS { get; set; }

        public double TotalDensidadGLPPromMesAnt { get; set; }
        public double TotalMontoFacturarporUnnaE { get; set; }
        public double TotalMontoFacturarporPetroperu { get; set; }

        public double Observacion1 { get; set; }
        public double Observacion2 { get; set; }
        public double Observacion3 { get; set; }
    }
}
