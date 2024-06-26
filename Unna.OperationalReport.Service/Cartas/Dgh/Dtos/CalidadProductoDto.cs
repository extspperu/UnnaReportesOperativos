using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Dtos
{
    public class CalidadProductoDto
    {

        public string? Periodo { get; set; }
        public List<ComposicionMolarGasDto>? ComposicionMolar { get; set; }
        public double? TotalComposicionMolarGasAsociado { get; set; }
        public double? TotalComposicionMolarGasResidual { get; set; }
        public List<ComposicionMolarGasDto>? ComposicionMolarPromedio { get; set; }


        public List<ComposicionMolarMetodoDto>? ComposicionMolarGlp { get; set; }
        public double? TotalComposicionMolarGlp { get; set; }
        public List<ComposicionMolarMetodoDto>? ComposicionMolarGlpPromedio { get; set; }

        public List<PropiedadesDestilacionDto>? PropiedadesDestilacion { get; set; }
        public string? PreparadoPor { get; set; }
        public string? Aprobado { get; set; }
    }


    public class ComposicionMolarGasDto
    {
        public int? Item { get; set; }
        public string? Propiedad { get; set; }
        public double? GasAsociado { get; set; }
        public double? GasResidual { get; set; }
    }
    public class ComposicionMolarMetodoDto
    {
        public int? Item { get; set; }
        public string? Propiedad { get; set; }
        public double? Metodo { get; set; }
        public double? Glp { get; set; }
    }


    public class PropiedadesDestilacionDto
    {
        public int? Item { get; set; }
        public string? Propiedad { get; set; }
        public string? Metodo { get; set; }
        public double? Cgn { get; set; }
    }

}
