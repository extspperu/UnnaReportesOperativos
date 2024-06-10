using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteQuincenal.ResBalanceEnergLIV.Dtos
{
    public class ResBalanceEnergLIVPost
    {
        public long IdUsuario { get; set; }
        public string Mes { get; set; }
        public string Anio { get; set; }
        public List<DiaDatosDto> DatosDiarios { get; set; } = new List<DiaDatosDto>();
        public ParametrosLGN ParametrosLGN { get; set; }
        public ResumenGNSEnergia ResumenGNSEnergia { get; set; }
    }
    public class DiaDatosDto
    {
        public string Dia { get; set; }
        public List<MedicionDto> Mediciones { get; set; } = new List<MedicionDto>();
    }

    public class MedicionDto
    {
        public string ID { get; set; }
        public string Valor { get; set; }
    }
    public class ParametrosLGN
    {
        public double? DensidadGLPKgBl { get; set; }
        public double? PCGLPMMBtuBl60F { get; set; }
        public double? PCCGNMMBtuBl60F { get; set; }
        public double? PCLGNMMBtuBl60F { get; set; }
        public double? FactorConversionSCFDGal { get; set; }
        public double? EnergiaMMBTUQ1GLP { get; set; }
        public double? EnergiaMMBTUQ1CGN { get; set; }
        public double? DensidadGLPKgBlQ2 { get; set; }
        public double? PCGLPMMBtuBl60FQ2 { get; set; }
        public double? PCCGNMMBtuBl60FQ2 { get; set; }
        public double? PCLGNMMBtuBl60FQ2 { get; set; }
        public double? FactorConversionSCFDGalQ2 { get; set; }
        public double? EnergiaMMBTUQ2GLP { get; set; }
        public double? EnergiaMMBTUQ2CGN { get; set; }
    }

    public class ResumenGNSEnergia
    {
        public double? GNSEnergia1Q { get; set; }
        public double? GNSEnergia2Q { get; set; }
    }
}
