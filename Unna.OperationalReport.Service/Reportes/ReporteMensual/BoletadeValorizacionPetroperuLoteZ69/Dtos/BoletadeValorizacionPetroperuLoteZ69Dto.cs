using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Unna.OperationalReport.Service.Reportes.Generales.Dtos;
using Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteVI.Dtos;

namespace Unna.OperationalReport.Service.Reportes.ReporteMensual.BoletadeValorizacionPetroperuLoteZ69.Dtos
{
    public class BoletadeValorizacionPetroperuLoteZ69Dto
    {
        public string? Fecha { get; set; }
        public List<BoletadeValorizacionPetroperuLoteZ69DetDto>? BoletadeValorizacionPetroperuLoteZ69Det { get; set; }


        public double TotalGasNaturalLoteZ69GNAMPCSD { get; set; }
        public double TotalGasNaturalLoteZ69EnergiaMMBTU { get; set; }
        public double TotalGasNaturalLoteZ69LGNRecupBBL { get; set; }


        public double TotalGasNaturalEficienciaPGT { get; set; }

        public double TotalGasSecoMS9215GNSLoteZ69MCSD { get; set; }

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

        [JsonIgnore]
        public long? IdUsuario { get; set; }

        public ReporteDto? General { get; set; }
    }
}
