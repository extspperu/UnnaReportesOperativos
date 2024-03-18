using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Dtos
{
    public class BoletadeValorizacionPetroperuLoteZ69DetDto
    {
        public int Dia { get; set; }
        public double? GasNaturalLoteZ69GNAMPCSD { get; set; }
        public double? GasNaturalLoteZ69PCBTUPCSD { get; set; }
        public double? GasNaturalLoteZ69EnergiaMMBTU { get; set; }
        public double? GasNaturalLoteZ69RiquezaGALMPC { get; set; }
        public double? GasNaturalLoteZ69RiquezaBLMMPC { get; set; }
        public double? GasNaturalLoteZ69LGNRecupBBL { get; set; }


        public double? GasNaturalEficienciaPGT_Porcentaje { get; set; }


        public double? GasSecoMS9215GNSLoteZ69MCSD { get; set; }
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
