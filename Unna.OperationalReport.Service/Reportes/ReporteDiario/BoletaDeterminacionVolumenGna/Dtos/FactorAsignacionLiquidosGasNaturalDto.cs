using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Reportes.ReporteDiario.BoletaDeterminacionVolumenGna.Dtos
{
    public class FactorAsignacionLiquidosGasNaturalDto
    {
        public int? Item { get; set; }
        public string? Suministrador { get; set; }
        public double Volumen { get; set; }
        public double Riqueza { get; set; }
        public double Contenido { get; set; }
        public double FactorAsignacion { get; set; }
        public double Asignacion { get; set; }

        public double VolumenRiqueza
        {
            get
            {
                return Volumen * Riqueza;
            }
        }
    }
}
