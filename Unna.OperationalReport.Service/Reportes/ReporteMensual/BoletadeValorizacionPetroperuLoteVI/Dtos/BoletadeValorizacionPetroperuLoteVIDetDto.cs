using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Dtos
{
    public class BoletadeValorizacionPetroperuLoteVIDetDto
    {
        public int Dia { get; set; }
        public double? GasNaturalLoteVIGNAMPCSD { get; set; }
        public double? GasNaturalLoteVIPCBTUPCSD { get; set; }
        public double? GasNaturalLoteVIEnergiaMMBTU { get; set; }
        public double? GasNaturalLoteVIRiquezaGALMPC { get; set; }
        public double? GasNaturalLoteVIRiquezaBLMMPC { get; set; }
        public double? GasNaturalLoteVILGNRecupBBL { get; set; }


        public double? GasNaturalEficienciaPGT_Porcentaje { get; set; }


        public double? GasSecoMS9215GNSLoteVIMCSD { get; set; }
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
