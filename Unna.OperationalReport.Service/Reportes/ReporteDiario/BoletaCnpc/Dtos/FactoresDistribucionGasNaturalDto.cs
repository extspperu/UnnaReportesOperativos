using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaCnpc.Dtos
{
    public class FactoresDistribucionGasNaturalDto
    {
        public int? Item { get; set; }
        public string? Suministrador { get; set; }
        public double? Volumen { get; set; }
        public double? ConcentracionC1 { get; set; }        
        public double? VolumenC1 { get; set; }                
        public double? FactoresDistribucion { get; set; }
        public double? AsignacionGns { get; set; }

        public double? VolumenConcentracionC1
        {
            get
            {
                return Volumen * ConcentracionC1;
            }
        }
    }
}
