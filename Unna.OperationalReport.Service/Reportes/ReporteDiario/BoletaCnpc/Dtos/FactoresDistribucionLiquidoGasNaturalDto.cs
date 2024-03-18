using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos
{
    public class FactoresDistribucionLiquidoGasNaturalDto
    {
        public int? Item { get; set; }
        public string? Suministrador { get; set; }
        public double Volumen { get; set; }
        public double Riqueza { get; set; }
        public double Contenido { get; set; }
        public double FactoresDistribucion { get; set; }
        public double AsignacionGns { get; set; }

        public double VolumenRiqueza
        {
            get
            {
                return Volumen * Riqueza;
            }
        }
    }
}
