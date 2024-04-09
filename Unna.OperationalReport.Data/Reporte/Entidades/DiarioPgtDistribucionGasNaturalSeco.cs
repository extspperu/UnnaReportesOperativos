using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Entidades
{
    public class DiarioPgtDistribucionGasNaturalSeco
    {

        public DateTime Fecha { get; set; }
        public int Id { get; set; }
        public string? Distribucion { get; set; }
        public double? VolumenDiaria { get; set; }
        public double? PoderCalorifico { get; set; }
        public double? PromedioMensual { get; set; }
        public double? EnergiaDiaria { get; set; }
    }
}
