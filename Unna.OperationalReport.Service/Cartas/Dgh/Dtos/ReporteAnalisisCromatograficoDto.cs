using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Cartas.Dgh.Dtos
{
    public class ReporteAnalisisCromatograficoDto
    {
        public string? Periodo { get; set; }
        public List<CompoisicionModalDto>? Componente { get; set; }
        public List<CompoisicionModalDto>? ComponentePromedio { get; set; }
        public string? Observaciones { get; set; }
        public string? PreparadoPor { get; set; }
        public string? AprobadoPor { get; set; }
    }

    public class CompoisicionModalDto
    {
        public int? Item { get; set; }
        public string? Componente { get; set; }
        public string? MetodoAstm { get; set; }
        public double? LoteZ69 { get; set; }
        public double? LoteX { get; set; }
        public double? LoteVi { get; set; }
        public double? LoteI { get; set; }
        public double? LoteIv { get; set; }

    }

}
