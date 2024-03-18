using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos
{
    public class BoletadeValorizacionPetroperuLoteIDetDto
    {
        public int Dia { get; set; }
        public double? GasNaturalLoteIGNAMPCSD { get; set; }
        public double? GasNaturalLoteIPCBTUPCSD { get; set; }
        public double? GasNaturalLoteIEnergiaMMBTU { get; set; }
        public double? GasNaturalLoteIRiquezaGALMPC { get; set; }
        public double? GasNaturalLoteIRiquezaBLMMPC { get; set; }
        public double? GasNaturalLoteILGNRecupBBL { get; set; }

       
        public double? GasNaturalEficienciaPGT_Porcentaje { get; set; }
       

        public double? GasSecoMS9215GNSLoteIMCSD { get; set; }
        public double? GasSecoMS9215PCBTUPCSD { get; set; }
        public double? GasSecoMS9215EnergiaMMBTU { get; set; }

        public double? PrecioGLPESinIGVSolesKG { get; set; }
        public double? PrecioGLPGSinIGVSolesKG { get; set; }
        public double? PrecioRefGLPSinIGVSolesKG { get; set; }
        public double? PrecioGLPSinIGVUSBL { get; set; }
        public double? TipodeCambioSoles_US { get; set; }
        public double? PrecioCGNUSBL { get; set; }
        public double? ValorLiquidosUS { get; set; }
        public double? CostoUnitMaquilaUSMMBTU { get; set; }
        public double? CostoMaquilaUS { get; set; }
    }
}
