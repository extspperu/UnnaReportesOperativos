using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Calculos.Dtos
{
    public class DeterminacionFactorConvertirVolumenLgnDto
    {
        public double Eficiencia { get; set; }
        public double VolumenGasEntrada { get; set; }
        public double PoderCalorificoGlp { get; set; }
        public double PoderCalorificoCgn { get; set; }
        public double PoderCalorificoLgn { get; set; }
        public double DensidadGlpLbGal { get; set; }
        public double DensidadGlpKgLb { get; set; }

        public double TotalMolar { get; set; }
        public double TotalVolumenBl { get; set; }
        public double? TotalLiquidoVolumenBl { get; set; }
        public double? TotalLiquidoVolumenPcsd { get; set; }
        public double TotalProductoGlpBl { get; set; }
        public double TotalProductoGlpVol { get; set; }
        public double TotalProductoCgnBl { get; set; }
        public double TotalProductoCgnVol { get; set; }


        public double Factor { get; set; }
        public double FactorCalculo { get; set; }


    }

    public class ComponentesComposicionGnaDto
    {
        public string? Simbolo { get; set; }
        public string? Componente { get; set; }
        public double? Molar { get; set; }
        public double? GnaComponente { get; set; }
        public double? GnaVolumen { get; set; }
        public double? EficienciaRecuperacion { get; set; }
        public double? LiquidoVolumenBl { get; set; }
        public double? LiquidoVolumenPcsd { get; set; }
        public double? ProductoGlpBl { get; set; }
        public double? ProductoGlpVol { get; set; }
        public double? ProductoCgnBl { get; set; }
        public double? ProductoCgnVol { get; set; }
    }


}
