using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Calculos.Dtos
{
    public class CantidadCalidadDto
    {
        public string? Fecha { get; set; }
        public List<GnaEntradaPlantaPariniasDto>? GnaEntradaPlantaParinias { get; set; }
        public double GasCombustible { get; set; }
        public double Glp { get; set; }
        public double C5 { get; set; }
        public List<VolumenGasNaturalVendidoPorClienteDto>? VolumenGasVendidoPorCliente { get; set; }
    }

    public class GnaEntradaPlantaPariniasDto
    {
        public string? Corriente { get; set; }
        public string? Medidor { get; set; }
        public double VolumenFiscalizado { get; set; }
        public double PoderCalorifico { get; set; }
        public double Riqueza { get; set; }
    }

    public class VolumenGasNaturalVendidoPorClienteDto
    {
        public string? Cliente { get; set; }
        public double Volumen { get; set; }
    }

}
