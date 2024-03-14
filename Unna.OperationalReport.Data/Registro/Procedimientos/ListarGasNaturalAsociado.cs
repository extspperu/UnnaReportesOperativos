using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Procedimientos
{
    public class ListarGasNaturalAsociado
    {
        public int? IdLote { get; set; }
        public string? Lote { get; set; }
        public double? Volumen { get; set; }
        public double? Calorifico { get; set; }
        public double? Riqueza { get; set; }
        public double? RiquezaBls { get; set; }
        public double? EnergiaDiaria { get; set; }
        public double? VolumenPromedio { get; set; }
    }
}
