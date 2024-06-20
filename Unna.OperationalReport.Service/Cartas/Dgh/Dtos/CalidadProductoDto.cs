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
        public List<ComposicionMolarGasDto>? ComposicionMolarPromedio { get; set; }
        public List<ComposicionMolarMetodoDto>? ComposicionMolarMetodo { get; set; }
        public List<ComposicionMolarMetodoDto>? ComposicionMolarMetodoPromedio { get; set; }
        public List<PropiedadesDestilacionDto>? PropiedadesDestilacion { get; set; }
        public string? PreparadoPor { get; set; }
        public string? Aprobado { get; set; }
    }


    public class ComposicionMolarGasDto
    {
        public string? Propiedad { get; set; }
        public double? GasAsociado { get; set; }
        public double? GasResidual { get; set; }
    }
    public class ComposicionMolarMetodoDto
    {
        public string? Propiedad { get; set; }
        public double? Metodo { get; set; }
        public double? Glp { get; set; }
    }


    public class PropiedadesDestilacionDto
    {
        public string? Destilacion { get; set; }
        public string? Metodo { get; set; }
        public double? Cgn { get; set; }
    }

}
