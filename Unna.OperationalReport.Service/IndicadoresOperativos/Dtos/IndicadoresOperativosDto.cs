using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.IndicadoresOperativos.Dtos
{
    public class IndicadoresOperativosDto
    {
        public string? Dia { get; set; }
        public double? Gna { get; set; }
        public double? Eficiencia { get; set; }
        public double? Glp { get; set; }
        public double? Lgn { get; set; }
    }
}
