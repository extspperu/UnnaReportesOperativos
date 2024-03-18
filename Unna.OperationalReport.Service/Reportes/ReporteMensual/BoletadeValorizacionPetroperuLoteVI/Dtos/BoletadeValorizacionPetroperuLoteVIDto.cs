using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteI.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Dtos
{
    public class BoletadeValorizacionPetroperuLoteVIDto
    {
        public string? Fecha { get; set; }
        public List<BoletadeValorizacionPetroperuLoteVIDetDto>? BoletadeValorizacionPetroperuLoteVIDet { get; set; }


        public double TotalGasNaturalLoteVIGNAMPCSD { get; set; }
        public double TotalGasNaturalLoteVIEnergiaMMBTU { get; set; }
        public double TotalGasNaturalLoteVILGNRecupBBL { get; set; }


        public double TotalGasNaturalEficienciaPGT { get; set; }

        public double TotalGasSecoMS9215GNSLoteVIMCSD { get; set; }

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
