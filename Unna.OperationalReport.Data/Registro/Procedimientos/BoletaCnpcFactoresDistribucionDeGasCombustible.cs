using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Registro.Procedimientos
{
    public class BoletaCnpcFactoresDistribucionDeGasCombustible
    {
        public int IdLote { get; set; }
        public string? Lote { get; set; }
        public double Volumen { get; set; }
        public double ConcentracionC1 { get; set; }
        public double VolumenC1 { get; set; }      
        public double FactoresDistribucion { get; set; }
        public double AsignacionGns { get; set; }
        public double Riqueza { get; set; }
    }
}
