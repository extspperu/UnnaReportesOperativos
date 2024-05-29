using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Data.Reporte.Procedimientos
{
    public class ParametrosQuincenalLGN
    {
        public int Id { get; set; }
        public double DensidadGLPKgBl { get; set; }
        public double PCGLPMMBtuBl60F { get; set; }
        public double PCCGNMMBtuBl60F { get; set; }
        public double PCLGNMMBtuBl60F { get; set; }
        public double FactorConversionSCFDGal { get; set; }
        public int Quincena {  get; set; }
        public DateTime Fecha { get; set; }
    }

}
